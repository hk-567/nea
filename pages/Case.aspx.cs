using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NEA
{
    public partial class Case : System.Web.UI.Page
{ 
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
  protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridview(); // calls method
            }
        }

private void BindGridview()
{
    try
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
string query = "SELECT CaseId, Name, Type, Status, DateOpened FROM CasesTbl";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    sda.Fill(dt);
                    gvCases.DataSource = dt;
                    gvCases.DataBind(); // bind datatable to gridview
                }
            }
        }
    }
    catch (SqlException ex)
    {
        // Handle SQL exceptions here
        MessageHelper.ShowErrorMessage(this.Page,"An error occurred while retrieving the cases. Please try again later.");     
    }
    catch (Exception ex)
    {
        // Handle other exceptions here
       MessageHelper.ShowErrorMessage(this.Page,"An unexpected error occurred. Please try again later.");  
    }
}

protected void AddCase(object sender, EventArgs e)
{
    // Check if a user is logged in
    if (Session["UserId"] != null && int.TryParse(Session["UserId"].ToString(), out int userId))
    {
        try
        {
            // Get the case name from the input textbox and the client ID associated with the user
            string caseName = txtCaseName.Text.Trim();
            int clientId = Convert.ToInt32(ClientRepository.GetClientsByUserId(userId));

            // Validate the input to ensure that the case name is not empty or null
            if (string.IsNullOrEmpty(caseName))
            {
                MessageHelper.ShowErrorMessage(this.Page,"Please enter a valid case name.");
                return;
            }

            // Insert a new record into the "CasesTbl" table in the database with the case name, type, status, date opened, client ID, and user ID
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO CasesTbl (Name, Type, Status, DateOpened, ClientId, UserId) VALUES (@Name, @Type, @Status, @DateOpened, @ClientId, @UserId)", con))
                {
                    cmd.Parameters.AddWithValue("@Name", caseName);
                    cmd.Parameters.AddWithValue("@Type", ddlCaseType.SelectedValue);
                    cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@DateOpened", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ClientId", clientId);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException ex)
        {
            // Show an error message if an error occurs while adding the case to the database
            MessageHelper.ShowErrorMessage(this.Page, "An error occurred while adding the case: " + ex.Message);
        }
    }
}       

