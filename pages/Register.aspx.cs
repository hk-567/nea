using System;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using HabibaKhatun_NEA.classes;

namespace NEA
{
    public partial class Register : System.Web.UI.Page
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString; // from web.config file
protected void Page_Load(object sender, EventArgs e)
        {            
             if (!IsPostBack)
            {
                Clear();
            }  if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {  int userID = Convert.ToInt32(Request.QueryString["id"]);
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    try
                    { // Stored procedure explained in queries section 
                        sqlConn.Open();
                        SqlDataAdapter sqlDa = new SqlDataAdapter("View_User_By_ID", sqlConn);
                        sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                        sqlDa.SelectCommand.Parameters.AddWithValue("@UserId", userID);
                        DataTable dtbl = new DataTable();
                        sqlDa.Fill(dtbl);

                        hfUserID.Value = userID.ToString();
                        txtUsername.Text = dtbl.Rows[0][1].ToString();
                        txtPassword.Text = dtbl.Rows[0][2].ToString();
              txtPassword.Attributes.Add("value", dtbl.Rows[0][2].ToString());
              txtConfirmPassword.Text = dtbl.Rows[0][2].ToString();
               txtPassword.Attributes.Add("value", dtbl.Rows[0][2].ToString());
                        txtEmail.Text = dtbl.Rows[0][3].ToString();
                    }
                    catch (SqlException ex)
                    {
                        MessageHelper.ShowErrorMessage("Error connecting to database. Please try again later." + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageHelper.ShowErrorMessage(this.Page,"An unexpected error occurred. Please try again later." + ex.Message);
                    }
                    finally
                    {
                        sqlConn.Close();
                    }
                }
            }
        }

private void Clear() // method to clear textboxes and controls
        {
            txtUsername.Text = txtPassword.Text = txtConfirmPassword.Text = txtEmail.Text = ""; 
            hfUserID.Value = "";        
        }

private bool CheckUserCredentials(string email)
        {
            // regular expression pattern for validating the email address
            string pattern = @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$";
            // checks if the email address matches the pattern and the password meets the length and confirmation requirements
            if (Regex.IsMatch(email, pattern))
            {
                if  (txtPassword.Text == txtConfirmPassword.Text)
                {
                    if (txtPassword.Text.Length <= 8) {
                        MessageHelper.ShowErrorMessage(this.Page, "Password needs to be more than 8 characters");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }            
                else
                {
                    MessageHelper.ShowErrorMessage(this.Page, "Password do not match");
                    return false;
                }
            }
            else            
            // set the error message and return false
            {
                MessageHelper.ShowErrorMessage(this.Page, "Invalid email address");
                return false;
            }
        }

   protected void RegisterUser(object sender, EventArgs e)
        {
            UserManager userManager = new UserManager();
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string email = txtEmail.Text;
            string type = ddlType.SelectedValue;
            bool userExists = userManager.UserExists(username);

            try
            {   // If the user credentials are valid, store the password and display a success message
                if (!userExists)
                {
                    if (CheckUserCredentials(email))
                    {
                        PasswordHelper.StorePassword(username, email, password, type); // stores user details to db
                        Clear();  
                        int userId = userManager.GetUserId(username, password);      
                                MessageHelper.ShowSuccessMessage(this.Page, "Registration successful. Activation email has been sent.");
                                SendActivationEmail(userId);    
                        ClientScript.RegisterStartupScript(GetType(), "alert", "alert('" + "');", true);
                    }
                }
                else
                {
                    MessageHelper.ShowErrorMessage(this.Page, "User with this username already exists.");
                }
            }
            catch (SqlException ex)
            {
                Clear();
               MessageHelper.ShowErrorMessage(this.Page, "Error connecting to database. Please try again later." + ex.Message);
            }
            catch (Exception ex)
            {
                Clear();
               MessageHelper.ShowErrorMessage(this.Page, "An unexpected error occurred. Please try again later." + ex.Message);
            }
        }

 private void SendActivationEmail(int userId)
{        // generates activation code 
    try { string activationCode = Guid.NewGuid().ToString();
        using (SqlConnection con = new SqlConnection(connectionString))
        { string query = "INSERT INTO UserActivationTbl VALUES(@UserId, @ActivationCode)";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ActivationCode", activationCode);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            } // builds the template of the email 
        } using (MailMessage mm = new MailMessage("jwco@gmail.com", txtEmail.Text))
        {   // firm email, user email
            mm.Subject = "Account Activation";
            string body = "Hello " + txtUsername.Text.Trim() + ",";
            body += "<br /><br />Please click the following link to activate your account";
            body += "<br /><a href = '" + Request.Url.AbsoluteUri.Replace("Activate.aspx", "CS_Activation.aspx?ActivationCode=" + activationCode) + "'>Click here to activate your account.</a>";
            body += "<br /><br />Thanks";
            mm.Body = body;
            mm.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient(); // uses SMTP
            smtp.Host = "smtp.gmail.com"; 
            smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential("jwco@gmail.com", "*********");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mm);
        }
    }
catch (SqlException ex)
    {
        MessageHelper.ShowErrorMessage(this.Page, "Error connecting to database. Please try again later." + ex.Message);
    }
    catch (SmtpException ex)
    {
       MessageHelper.ShowErrorMessage(this.Page, "Error sending email. Please try again later." + ex.Message);
    }
    catch (Exception ex)
    {
       MessageHelper.ShowErrorMessage(this.Page, "An unexpected error occurred. Please try again later." + ex.Message);
    }
}

protected void GoToLogin(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

    }
}

