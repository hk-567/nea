using NEA.classes;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

namespace NEA
{
    public partial class Invoice : System.Web.UI.Page
    {
   private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private Service serviceRates = new Service();
        private int lastServiceNo = 0;

protected void Page_Load(object sender, EventArgs e)
  {
      if (!IsPostBack)
            {          
               // loop through the Rates dictionary and add items to dropdown list
                int userId = (int)Session["UserId"];
                string employeeType = userManager.GetUserType(userId);
                serviceRates.EmployeeType = employeeType;

                // loop through the services and their rates and add items to dropdown list
                foreach (KeyValuePair<string, decimal[]> item in serviceRates.Rates)
                {
                    string listItemText = item.Key + " ($" + serviceRates.GetRate(item.Key) + ")";
                    string listItemValue = serviceRates.GetRate(item.Key).ToString();
                    System.Web.UI.WebControls.ListItem listItem = new System.Web.UI.WebControls.ListItem(listItemText, listItemValue);
                    ddlService.Items.Add(listItem);
                }
       }
}

 protected void AutofillClients(object sender, EventArgs e)
{
    if (ddlClients.SelectedValue != "")
    {
        int clientId;
        if (int.TryParse(ddlClients.SelectedValue, out clientId))
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Name, Gender, EmailAddress, MobileNo, HomeAddress FROM ClientsTbl WHERE ClientId = @ClientId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClientId", clientId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtfName.Text = reader["Name"].ToString();
                                txtEmail.Text = reader["EmailAddress"].ToString();
                                txtMob.Text = reader["MobileNo"].ToString();
                                txtAddress.Text = reader["HomeAddress"].ToString();
                            }
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this.Page, "An unexpected error has occurred" + ex.Message);
            }
        }
        else
        {
            MessageHelper.ShowErrorMessage(this.Page, "Invalid Client ID");
        }
    }
    else
    {
        // Reset the text boxes to empty
        txtfName.Text = "";
        txtEmail.Text = "";
        txtMob.Text = "";
        txtAddress.Text = "";
    }
}

protected void AutofillServices(object sender, EventArgs e)
{
    try
    {
        // Get the selected item from the dropdown list
        System.Web.UI.WebControls.ListItem selectedItem = ddlService.SelectedItem;

        // Set the Text property of the service and rate textboxes
        txtService.Text = selectedItem.Text;
        txtRate.Text = selectedItem.Value;

        // Display success message
        MessageHelper.ShowSuccessMessage(this.Page, "Service details have been successfully updated.");
    }
    catch (Exception ex)
    {
        // Display error message
        MessageHelper.ShowErrorMessage(this.Page, "An error occurred while updating the service details. Please try again later.");   
    }
}

private void AddClientToInvoice()
{
    try
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = connection.CreateCommand())
        {
            command.CommandText = "INSERT INTO InvoicesTbl (InvoiceDate, ClientId, UserId) VALUES (@InvoiceDate, @ClientId, @UserId)";
            command.Parameters.AddWithValue("@InvoiceDate", DateTime.Now);
            command.Parameters.AddWithValue("@ClientId", ddlClients.SelectedValue);
            command.Parameters.AddWithValue("@UserId", Session["UserID"]);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                MessageHelper.ShowErrorMessage(this.Page, "Failed to add client to invoice.");
            }
            else
            {
                MessageHelper.ShowSuccessMessage(this.Page, "Client added to invoice successfully.");
            }
        }
    }
    catch (Exception ex)
    {
        MessageHelper.ShowErrorMessage(this.Page, "An error occurred while adding the client to invoice: " + ex.Message);
    }
}

protected void GenerateInvoicePDF(object sender, EventArgs e)
{
    try
    {
        // Add the client to the invoice table
        AddClientToInvoice();

        // Validate that the necessary data and resources are available
        if (String.IsNullOrEmpty(txtfName.Text))
        {
            MessageHelper.ShowErrorMessage(this.Page,"Client name is required.");
        }

        if (serviceRates == null || serviceRates.dt == null || serviceRates.dt.Rows.Count == 0)
        {
            MessageHelper.ShowErrorMessage(this.Page,"No services selected for invoice.");
        }

        // Generate the invoice PDF
        string companyName = "JW & Co";
        string clientName = txtfName.Text;
        int invoiceNo = UserManager.GetInvoiceId();

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                StringBuilder sb = new StringBuilder();

                // Generate Invoice (Bill) Header.
                sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b><h1>Invoice</h1></b></td></tr>");
                sb.Append("<tr><td colspan = '2'></td></tr>");
                sb.Append("<tr><td><b>Invoice No: </b>");
                sb.Append(invoiceNo);
                sb.Append("</td><td align = 'right'><b>Invoice Date: </b>");
                sb.Append(DateTime.Now);
                sb.Append(" </td></tr>");
                sb.Append("<tr><td colspan = '2'><b>Company Name: </b>");
                sb.Append(companyName);
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan = '2'><b>Billed To: </b>");
                sb.Append(clientName);
                sb.Append("</td></tr>");

                sb.Append("</table>");
                sb.Append("<br />");

                // Generate Invoice (Bill) Items Grid.
                sb.Append("<table border = '1'>");
                sb.Append("<tr>");
                sb.Append("<th>Service No</th>");
                sb.Append("<th>Service</th>");
                sb.Append("<th>Description</th>");
                sb.Append("<th>Rate</th>");
                sb.Append("<th>Quantity</th>");
                sb.Append("<th>Amount</th>");
                sb.Append("</tr>");

                decimal totalAmount = 0;
                foreach (DataRow row in serviceRates.dt.Rows)
                {
                    sb.Append("<tr>");
                    sb.Append("<td>" + row["ServiceNo"] + "</td>");
                    sb.Append("<td>" + row["Service"] + "</td>");
                    sb.Append("<td>" + row["Description"] + "</td>");
                    sb.Append("<td>" + row["Hourly Rate"] + "</td>");
                    sb.Append("<td>" + row["Quantity"] + "</td>");
                    decimal amount = Convert.ToDecimal(row["Hourly Rate"]) * Convert.ToInt32(row["Quantity"]);
                    totalAmount += amount;
                    sb.Append("<td>" + amount + "</td>");
                    sb.Append("</tr>");
                }

                sb.Append("<tr><td align = 'right' colspan = '");
                sb.Append(serviceRates.dt.Columns.Count - 1);
                sb.Append("'>Total</td>");
                sb.Append("<td>");
                sb.Append(totalAmount);
                sb.Append("</td>");
                sb.Append("</tr></table>");

                // Export HTML String as PDF.
StringReader sr = new StringReader(sb.ToString());
Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
pdfDoc.Open();
htmlparser.Parse(sr);
pdfDoc.Close();            // Set response headers and output PDF
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Invoice_" + invoiceNo + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
        }
    }
}
catch (Exception ex)
{
    // Display error message to user
    MessageHelper.ShowErrorMessage(this.Page, "An unexpected error has occurred " + ex.Message);
}

 protected void AddService(object sender, EventArgs e)
{
    try
    {
        // Retrieve the values from the user input fields and add a row to the DataTable for each service
        // Increment the service number and retrieve the values from the user input fields
        lastServiceNo++;
        int serviceNo = lastServiceNo;
        decimal rate;
        int quantity;

        // Validate user input
        if (!decimal.TryParse(txtRate.Text, out rate))
        {
            MessageHelper.ShowErrorMessage(this.Page,"Invalid rate value.");
        }

        if (!int.TryParse(txtHours.Text, out quantity))
        {
            MessageHelper.ShowErrorMessage(this.Page, "Invalid quantity value.");
        }

        if (string.IsNullOrEmpty(txtService.Text))
        {
            MessageHelper.ShowErrorMessage(this.Page,"Service name is required.");
        }

        // Calculate the amount based on the rate and quantity
        decimal amount = rate * quantity;

        // Add a row to the DataTable with the service information
        DataRow row = serviceRates.dt.NewRow();
        row["ServiceNo"] = serviceNo;
        row["Service"] = txtService.Text;
        row["Description"] = txtDescription.Text;
        row["Hourly Rate"] = rate;
        row["Quantity"] = quantity;
        row["Amount"] = amount;
        serviceRates.dt.Rows.Add(row);
    }
    catch (Exception ex)
    {
        // Handle any errors that occur and display an error message to the user
       MessageHelper.ShowErrorMessage(this.Page, "An unexpected error has occurred " + ex.Message);
    }
}

    }
}

