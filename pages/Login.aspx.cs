using NEA.classes;
using System;

namespace NEA
{
    public partial class Login : System.Web.UI.Page
    {
        private UserManager _userManager;
protected void Page_Load(object sender, EventArgs e)
        {
            _userManager = new UserManager();
        }

protected void Login(object sender, EventArgs e)
{
        string username = txtLoginUsername.Text;
        string password = txtLoginPassword.Text;
    try
    {
        
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            MessageHelper.ShowErrorMessage(this.Page, "Please enter both username and password.");
            return;
        }
// verifies login details
        if (_userManager.VerifyLogin(username, password))
        {
            int userId = _userManager.GetUserId(username, password);
            if (userId > 0)
            {
                Session["UserId"] = userId; // stores the session variable 
                Response.Redirect("Menu.aspx");
            }
            else
            {
                MessageHelper.ShowErrorMessage(this.Page, "An unexpected error occurred. Please try again later.");
            }
        }
        else
        {
            MessageHelperShowErrorMessage(this.Page,"Invalid username or password. Please try again.");
        }
    }
    catch (Exception ex)
    {
        // show a generic error message to the user
      
        MessageHelper.ShowErrorMessage(this.Page, "An unexpected error occurred. Please try again later.");
    }
}


