using System;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace imaGenie
{
    public partial class imaGenieControl : UserControl
    {
        public imaGenieControl()
        {
            Height = "100%";
            Width = "100%";
            Thumbnail = false;
            IsPublic = true;
            IsAnchor = true;
        }

        public string ImageId
        {
            get { return ViewState["imageId"] as string; }
            set
            {
                ViewState["imageId"] = value;
                load(ImageId);
            }
        }

        public string Text
        {
            get { return ViewState["text"] as string; }
            set { ViewState["text"] = value; }
        }

        public bool Thumbnail
        {
            get { return (bool)ViewState["thumbnail"]; }
            set { ViewState["thumbnail"] = value; }
        }

        public string Width
        {
            get { return ViewState["width"] as string; }
            set { ViewState["width"] = value; }
        }

        public string Height
        {
            get { return ViewState["height"] as string; }
            set { ViewState["height"] = value; }
        }

        public string Owner
        {
            get { return ViewState["owner"] as string; }
            set { ViewState["owner"] = value; }
        }

        public string Date
        {
            get { return ViewState["date"] as string; }
            set { ViewState["date"] = value; }
        }

        public bool IsPublic
        {
            get { return (bool)ViewState["ispublic"]; }
            set { ViewState["ispublic"] = value; }
        }

        public bool IsAnchor
        {
            get { return (bool)ViewState["isAnchor"]; }
            set { ViewState["isAnchor"] = value; }
        }
        
        private void load(string value)
        {
            using (var connection = DatabaseManager.Connection)
            using (var cmd = new SqlCommand("select text,username,date,ispublic from image where id=@id", connection))
            {
                cmd.Parameters.AddWithValue("id", value);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Text = reader["text"].ToString();
                        Owner = reader["username"].ToString();
                        Date = reader["date"].ToString();
                        IsPublic = (int)reader["ispublic"] == 1;

                        if (!IsPostBack)
                        {
                            isPublicCb.Visible = false;
                            if (Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Moderator") || Owner == Page.User.Identity.Name)
                            {
                                removeImage.Visible = true;
                                isPublicCb.Visible = true;
                                editBtn.Visible = true;
                            }
                            isPublicCb.Checked = IsPublic;
                        }
                    }
                }
            }
        }

        protected void removeAll_Click(object sender, EventArgs e)
        {
            using (var connection = DatabaseManager.Connection)
            {
                using (var cmd = new SqlCommand("select id from image where username=@owner;", connection))
                {
                    cmd.Parameters.AddWithValue("owner", Owner);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader["id"].ToString();
                            deleteImage(id, connection);
                        }
                    }
                }
            }
            Response.Redirect(Request.RawUrl);
        }

        protected void removeImage_Click(object sender, EventArgs e)
        {
            if (Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Moderator") || Owner == Page.User.Identity.Name)
            {
                using (var connection = DatabaseManager.Connection)
                {
                    deleteImage(ImageId, connection);
                    Response.Redirect(Request.RawUrl);
                }
            }
        }

        private void deleteImage(string id, SqlConnection connection)
        {
            using (var cmd = new SqlCommand("delete from image where id=@id;", connection))
            {
                cmd.Parameters.AddWithValue("id", ImageId);
                cmd.ExecuteNonQuery();
            }
            using (var cmd = new SqlCommand("delete from imagecategory where imageid=@id;", connection))
            {
                cmd.Parameters.AddWithValue("id", ImageId);
                cmd.ExecuteNonQuery();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public bool HasRights()
        {
            return Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Moderator") || Owner == Page.User.Identity.Name;
        }

        protected void isPublicCb_CheckedChanged(object sender, EventArgs e)
        {
            var cbox = (CheckBox)sender;
            if (cbox.Visible && cbox.ValidationGroup == ImageId)
            {
                IsPublic = cbox.Checked;
                using (var connection = DatabaseManager.Connection)
                using (var cmd = new SqlCommand("update image set ispublic=@param where id=@id;", connection))
                {
                    cmd.Parameters.AddWithValue("id", ImageId);
                    cmd.Parameters.AddWithValue("param", cbox.Checked ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected void editBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect($"Edit.aspx?id={ImageId}");
        }
    }
}