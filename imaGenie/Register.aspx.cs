using System;
using System.Web.Security;
using System.Web.UI;

namespace imaGenie
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
        {
            Roles.AddUserToRole(CreateUserWizard1.UserName, "Normal");
            Response.Redirect("Index.aspx");
        }
    }
}