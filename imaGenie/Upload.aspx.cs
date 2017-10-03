using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace imaGenie
{
    public partial class Upload : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("Register.aspx");
            if (!IsPostBack)
            {
                DataSet ds = new DataSet();
                using (var connection = DatabaseManager.Connection)
                using (var cmd = new SqlCommand("select id,name from album where username=@param", connection))
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("param", User.Identity.Name);
                    adapter.Fill(ds);
                }

                albumList.DataSource = ds;
                albumList.DataBind();
            }
        }
        
        protected void uploadButton_Click(object sender, EventArgs e)
        {
            var master = (Master as Default);
            
            try
            {
                if (fileUpload.HasFile)
                {
                    var ext = Path.GetExtension(fileUpload.FileName);
                    if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".gif" && ext != ".bmp")
                    {
                        master.ShowPopup("Warning", "You can only upload images.");
                        return;
                    }
                    if (fileUpload.FileBytes.Length > 20971520)
                    {
                        master.ShowPopup("Warning", "You can only upload images of 20MB at most.");
                        return;
                    }

                    Bitmap image;
                    using (var stream = new MemoryStream(fileUpload.FileBytes))
                        image = new Bitmap(stream);

                    var thumbnail = ImageHelper.Thumbnail(image);
                    int id;
                    using (var conn = DatabaseManager.Connection)
                    {
                        using (var cmd = new SqlCommand("insert into image (username, data, width, height, thumbnail, extension, text, date, ispublic) values (@user, @data, @width, @height, @thumbnail, @extension, @text, @date, @public);SELECT SCOPE_IDENTITY();", conn))
                        {
                            cmd.Parameters.AddWithValue("user", User.Identity.Name);
                            cmd.Parameters.AddWithValue("data", fileUpload.FileBytes);
                            cmd.Parameters.AddWithValue("width", image.Width);
                            cmd.Parameters.AddWithValue("height", image.Height);
                            cmd.Parameters.AddWithValue("thumbnail", ImageHelper.Convert(thumbnail, ImageFormat.Png));
                            cmd.Parameters.AddWithValue("extension", ext.Replace(".", ""));
                            cmd.Parameters.AddWithValue("text", messageInput.Text);
                            cmd.Parameters.AddWithValue("date", DateTime.Now);
                            cmd.Parameters.AddWithValue("public", publicImage.Checked ? 1 : 0);
                            id = int.Parse(cmd.ExecuteScalar().ToString());
                        }
                        foreach (ListItem item in categoryList.Items)
                        {
                            if (item.Selected)
                            {
                                using (var cmd = new SqlCommand("insert into imagecategory (imageid, categoryid) values (@imageid, @categoryid)", conn))
                                {
                                    cmd.Parameters.AddWithValue("imageid", id);
                                    cmd.Parameters.AddWithValue("categoryid", item.Value);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        foreach (ListItem item in albumList.Items)
                        {
                            if (item.Selected)
                            {
                                using (var cmd = new SqlCommand("insert into imagealbum (imageid, albumid) values (@imageid, @albumid)", conn))
                                {
                                    cmd.Parameters.AddWithValue("imageid", id);
                                    cmd.Parameters.AddWithValue("albumid", item.Value);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    Response.Redirect("Index.aspx");
                }
            }
            catch(Exception ex)
            {
                master.ShowPopup("Error", ex.ToString());
            }
        }

        protected void cv1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int selected = 0;
            foreach (ListItem item in categoryList.Items)
                if (item.Selected)
                    selected++;
            args.IsValid = selected > 0;
        }
    }
}