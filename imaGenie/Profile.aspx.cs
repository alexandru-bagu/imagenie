using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace imaGenie
{
    public partial class Profile : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var username = Request.Params["u"];
                DataSet ds = new DataSet();
                using (var connection = DatabaseManager.Connection)
                using (var cmd = new SqlCommand("SELECT id,name from album where username=@param", connection))
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("param", username);
                    adapter.Fill(ds);
                }
                albumRepeater.DataSource = ds;
                albumRepeater.DataBind();

                if (username == User.Identity.Name)
                {
                    ds = new DataSet();
                    using (var connection = DatabaseManager.Connection)
                    using (var cmd = new SqlCommand("SELECT id as imageid from image where username=@param order by date desc", connection))
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        cmd.Parameters.AddWithValue("param", username);
                        adapter.Fill(ds);
                    }
                    imageRepeater2.DataSource = ds;
                    imageRepeater2.DataBind();
                    privateImages.Visible = true;
                    newAlbum.Visible = true;
                }
            }
        }

        protected void albumRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var username = Request.Params["u"];
            if (username == User.Identity.Name)
            {
                var placeHolder = e.Item.FindControl("editNamePlaceHolder");
                placeHolder.Visible = true;
            }
            var repeater = (Repeater)e.Item.FindControl("imageRepeater");
            DataRowView view = (DataRowView)e.Item.DataItem;
            var id = view[0];
            DataSet ds = new DataSet();
            using (var connection = DatabaseManager.Connection)
            using (var cmd = new SqlCommand("select top 3 a.imageid as imageid from imagealbum a, image b where a.albumid = @param and b.id = a.imageid", connection))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("param", id);
                adapter.Fill(ds);
            }
            repeater.DataSource = ds;
            repeater.DataBind();
        }

        protected void albumSave_Click(object sender, EventArgs e)
        {
            var control = (Control)sender;
            var parent = (RepeaterItem)control.Parent.Parent;
            var albumNameLink = parent.FindControl("albumNameLink");
            var tbPlaceHolder = parent.FindControl("tbPlaceHolder");
            var albumNameText = (Label)parent.FindControl("albumNameText");
            var albumName = (TextBox)parent.FindControl("albumName");
            var idHolder = (HiddenField)parent.FindControl("albumIdHolder");
            albumNameLink.Visible = true;
            tbPlaceHolder.Visible = false;
            albumNameText.Text = albumName.Text;
            
            using (var connection = DatabaseManager.Connection)
            using (var cmd = new SqlCommand("update album set name=@param where id=@param2", connection))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("param", albumName.Text);
                cmd.Parameters.AddWithValue("param2", idHolder.Value);
                cmd.ExecuteNonQuery();
            }
        }

        protected void editAlbumBtn_Click(object sender, EventArgs e)
        {
            var control = (Control)sender;
            var parent = (RepeaterItem)control.Parent.Parent.Parent;
            var albumNameLink = parent.FindControl("albumNameLink");
            var tbPlaceHolder = parent.FindControl("tbPlaceHolder");
            albumNameLink.Visible = false;
            tbPlaceHolder.Visible = true;
        }

        protected void albumAdd_Click(object sender, EventArgs e)
        {
            using (var connection = DatabaseManager.Connection)
            using (var cmd = new SqlCommand("insert into album (name, username, date) values (@name, @username, @date)", connection))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("name", newAlbumName.Text);
                cmd.Parameters.AddWithValue("username", User.Identity.Name);
                cmd.Parameters.AddWithValue("date", albumDate.SelectedDate);
                cmd.ExecuteNonQuery();
            }
            Response.Redirect(Request.RawUrl);
        }
    }
}