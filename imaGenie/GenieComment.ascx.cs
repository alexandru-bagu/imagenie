using System;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI;

namespace imaGenie
{
    public partial class GenieComment : UserControl
    {
        public string CommentId
        {
            get { return (string)ViewState["commentId"]; }
            set { ViewState["commentId"] = value;
                load(value);
            }
        }

        public string ImageOwner
        {
            get { return (string)ViewState["imageowner"]; }
            set { ViewState["imageowner"] = value; }
        }

        public string Owner
        {
            get { return (string)ViewState["owner"]; }
            set { ViewState["owner"] = value; }
        }

        public string Message
        {
            get { return (string)ViewState["message"]; }
            set { ViewState["message"] = value; }
        }

        public string Date
        {
            get { return (string)ViewState["date"]; }
            set { ViewState["date"] = value; }
        }

        public bool Approved
        {
            get { return (bool)ViewState["approved"]; }
            set { ViewState["approved"] = value; }
        }
        
        private void load(string value)
        {
            using (var connection = DatabaseManager.Connection)
            using (var cmd = new SqlCommand("select username, comment, date, approved from comment where id=@id", connection))
            {
                cmd.Parameters.AddWithValue("id", value);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Owner = reader["username"].ToString();
                        Message = reader["comment"].ToString();
                        Date = reader["date"].ToString();
                        Approved = reader["approved"].ToString() == "1";
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Moderator") || Owner == Page.User.Identity.Name)
            {
                approveCb.Enabled = false;
                approveCb.Visible = true;
                editBtn.Visible = true;
                deleteBtn.Visible = true;
            }
            if (Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Moderator") || ImageOwner == Page.User.Identity.Name)
            {
                approveCb.Visible = true;
                approveCb.Enabled = true;
            }
        }

        protected void editBtn_Click(object sender, EventArgs e)
        {
            editHolder.Visible ^= true;
            textHolder.Visible ^= true;
        }

        protected void deleteBtn_Click(object sender, EventArgs e)
        {
            using (var connection = DatabaseManager.Connection)
            using (var cmd = new SqlCommand("delete comment where id=@id;", connection))
            {
                cmd.Parameters.AddWithValue("id", CommentId);
                cmd.ExecuteNonQuery();
            }
            Response.Redirect(Request.RawUrl);
        }

        protected void commentSave_Click(object sender, EventArgs e)
        {
            using (var connection = DatabaseManager.Connection)
            using (var cmd = new SqlCommand("update comment set comment=@message where id=@id;", connection))
            {
                cmd.Parameters.AddWithValue("id", CommentId);
                cmd.Parameters.AddWithValue("message", commentText.Text);
                cmd.ExecuteNonQuery();
            }
            labelText.Text = commentText.Text;
            editBtn_Click(sender, e);
        }

        protected void approveCb_CheckedChanged(object sender, EventArgs e)
        {
            if (Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Moderator") || ImageOwner == Page.User.Identity.Name)
            {
                using (var connection = DatabaseManager.Connection)
                using (var cmd = new SqlCommand("update comment set approved=@approved where id=@id;", connection))
                {
                    cmd.Parameters.AddWithValue("id", CommentId);
                    cmd.Parameters.AddWithValue("approved", approveCb.Checked ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}