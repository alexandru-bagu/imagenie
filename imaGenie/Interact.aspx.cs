using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace imaGenie
{
    public partial class Interact : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var id = Request.Params["id"];
                if (string.IsNullOrEmpty(id)) return;
                genieImage.ImageId = id;

                var approved = $"and (approved = 1 or username=@user)";
                if (genieImage.HasRights())
                    approved = "";

                DataSet ds = new DataSet();
                using (var connection = DatabaseManager.Connection)
                using (var cmd = new SqlCommand($"select id from comment where imageid=@id {approved}" , connection))
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("user", User.Identity.Name);
                    adapter.Fill(ds);
                }
                genieComments.DataSource = ds;
                genieComments.DataBind();
            }
        }

        protected void commentButton_Click(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated) return;
            var msgBox = (TextBox)newComment.FindControl("commentBox");
            using (var connection = DatabaseManager.Connection)
            using (var cmd = new SqlCommand("insert into comment (imageid, username, comment, date, approved) values (@imageId, @username, @message, @date, @approved)", connection))
            {
                cmd.Parameters.AddWithValue("imageid", genieImage.ImageId);
                cmd.Parameters.AddWithValue("username", User.Identity.Name);
                cmd.Parameters.AddWithValue("message", msgBox.Text);
                cmd.Parameters.AddWithValue("date", DateTime.Now);
                if (genieImage.HasRights()) cmd.Parameters.AddWithValue("approved", 1);
                else cmd.Parameters.AddWithValue("approved", 0);
                cmd.ExecuteNonQuery();
            }
            Response.Redirect(Request.RawUrl);
        }
    }
}