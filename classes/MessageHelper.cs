using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace NEA.classes
{
    public static  class MessageHelper
    {

        public static void RegisterSuccessMessage(Page page, string message)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "SuccessMessage", $"showSuccessMessage('{message}');", true);
        }
        public static void ShowErrorMessage(Page page, string message)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "ErrorMessage", $"alert('{message}');", true);
        }

        public static void RegisterSuccessMessageScript(Page page)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "SuccessMessageScript", SuccessMessageScript, true);
        }

    }
}
