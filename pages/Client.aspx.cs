using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI.WebControls;
using NEA.classes;

namespace NEA
{
    public partial class Client : System.Web.UI.Page
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private readonly ClientRepository _clientRepository;
public Client()
        {
            _clientRepository = new ClientRepository(_connectionString);
        }

protected void Page_Load(object sender, EventArgs e)
{
    try
    {
        if (!IsPostBack)
        { // Check if this is the first time loading the page (not a postback)
            if (Session["UserId"] == null) // Check if user session has expired
            {
                MessageHelper.ShowErrorMessage(this.Page, "Session expired or invalid. Please login again.");
            }
            int userId = Convert.ToInt32(Session["UserId"]);
            var clients = _clientRepository.GetClientsByUserId(userId); // Retrieve clients for the user

            if (clients != null && clients.Any()) // Check if there are any clients for the user
            {
                var clientTableHtml = BuildClientTableHtml(clients); // Generate HTML table for the clients
                if (!string.IsNullOrEmpty(clientTableHtml))
                {
                    phClient.Controls.Add(new Literal { Text = clientTableHtml }); // Add the HTML table to the placeholder
                }
                else
                {
                     MessageHelper.ShowErrorMessage(this.Page,"Failed to generate client table HTML.");
                }
            }
            else
            {
                MessageHelper.ShowErrorMessage(this.Page,"No clients found for user.");
            }
        }
    }
    catch (Exception ex)
    {
        var errorMessage = "An error occurred while loading the page: " + ex.Message;
        phClient.Controls.Add(new Literal { Text = errorMessage }); // Add error message to the placeholder
    }
}

private string BuildClientTableHtml(DataTable dt)
{
    var html = new StringBuilder(); // StringBuilder is used to efficiently concatenate strings

    try // Exception handling
    {
        // Check if DataTable is null or empty
        if (dt == null || dt.Rows.Count == 0)
        {
            throw new Exception("The client table is empty."); // Throw an exception with a message if DataTable is null or empty
        }

        // Generate HTML table header with column names
        html.Append("<table border='1'>");
        html.Append("<tr>");
        foreach (DataColumn column in dt.Columns)
        {
            html.Append("<th>");
            html.Append(column.ColumnName);
            html.Append("</th>");
        }
        html.Append("</tr>");

        // Generate HTML table rows with data
        foreach (DataRow row in dt.Rows)
        {
            html.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                html.Append("<td>");
                html.Append(row[column.ColumnName]);
                html.Append("</td>");
            }
            html.Append("</tr>");
        }

        html.Append("</table>"); // Close the HTML table
    }
    catch (Exception ex) // Handle any exceptions that may occur during execution
    {
        html.Append("<p style='color:red'>An error occurred: ");
        html.Append(ex.Message);
        html.Append("</p>");
    }

    return html.ToString(); // Return the generated HTML table as a string
}

protected void AddClients(object sender, EventArgs e)
{
    try // Exception handling
    {
        int userId;

        // Check if the user is logged in and get the user ID
        if (Session["UserId"] != null && int.TryParse(Session["UserId"].ToString(), out userId))
        {
            // Validate input fields
            if (string.IsNullOrEmpty(txtfName.Text) || string.IsNullOrEmpty(txtlName.Text) || string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtMob.Text) || string.IsNullOrEmpty(txtAddress.Text))
            {
                MessageHelper.ShowErrorMessage(this.Page, "Please fill out all fields.");
                return;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                MessageHelper.ShowErrorMessage(this.Page, "Please enter a valid email address.");
                return;
            }

            if (!IsValidPhoneNumber(txtMob.Text))
            {
                MessageHelper.ShowErrorMessage(this.Page, "Please enter a valid phone number.");
                return;
            }

            // Create a new client object with the input data
            var client = new ClientModel
            {
                Name = txtfName.Text + txtmName.Text + txtlName.Text,
                Gender = rdGender.SelectedValue,
                EmailAddress = txtEmail.Text.Trim(),
                MobileNo = txtMob.Text.Trim(),
                HomeAddress = txtAddress.Text.Trim(),
                UserId = userId
            };

            // Add the new client to the database
            _clientRepository.AddClient(client);

            MessageHelper.ShowSuccessMessage(this.Page,"Client added successfully!");
        }
        else
        {
            MessageHelper.ShowErrorMessage(this.Page, "Invalid user ID. Please log in again.");
        }
    }
    catch (SqlException ex) // Handle any SQL exceptions that may occur during execution
    {
        MessageHelper.ShowErrorMessage(this.Page,  "An error occurred while adding the client. Please try again later.");
    }
    catch (Exception ex) // Handle any other exceptions that may occur during execution
    {
        MessageHelper.ShowErrorMessage(this.Page,  "An unknown error has occurred. Please contact the developer.");
    }
}

private bool IsValidPhoneNumber(string number)
{
    return Regex.Match(number, @"^[0-9]+$").Success;
    
}
private bool IsValidEmail(string email)
{
    try
    {
        var addr = new System.Net.Mail.MailAddress(email);
        return addr.Address == email;
    }
    catch
    {
        return false;
    }
}





     }
} 

