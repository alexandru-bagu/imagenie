using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace imaGenie
{
    public partial class Default : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                profile.NavigateUrl = "~/Profile.aspx?u=" + Server.UrlEncode(Page.User.Identity.Name);
            }
        }
       
        public void ShowPopup(string caption, string text)
        {
            (popupHeader.Controls[0] as LiteralControl).Text = caption;
            popupBodyText.Text = text;
            modalPopupExtender.Show();
            Session["popupOpen"] = true;
            Timer1.Enabled = true;
        }
        
        protected void login_LoginError(object sender, EventArgs e)
        {
            ShowPopup("Error!", "Invalid username or password. Please try again!");
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            if ((bool)Session["popupOpen"] == true)
            {
                modalPopupExtender.Hide();
                Session["popupOpen"] = false;
            }
            Timer1.Enabled = false;
        }

        protected void registerBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }

        protected void searchBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect($"Search.aspx?q={Server.UrlEncode(searchText.Text)}");
        }
    }
}