using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace imaGenie
{
    public partial class Album : Page
    {
        public string AlbumId { get { return Request.Params["id"]; } }
        public string Name, Date, Owner;
        
        protected void load()
        {
            if (!IsPostBack)
            {
                using (var connection = DatabaseManager.Connection)
                using (var command = new SqlCommand("select * from album where id=@param", connection))
                {
                    command.Parameters.AddWithValue("param", AlbumId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Name = reader["name"].ToString();
                            Owner = reader["username"].ToString();
                            Date = reader["date"].ToString();
                        }
                    }
                }
                
                DataSet ds = new DataSet();
                using (var connection = DatabaseManager.Connection)
                using (var cmd = new SqlCommand("SELECT a.id as id FROM [IMAGE] a, [IMAGEALBUM] b where b.ImageId = a.Id and b.AlbumId=@param order by a.date desc", connection))
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("param", AlbumId);
                    adapter.Fill(ds);
                }
                imagesRepeater.DataSource = ds;
                imagesRepeater.DataBind();

                if (Owner == User.Identity.Name)
                {
                    privateImages.Visible = true;
                    ds = new DataSet();
                    using (var connection = DatabaseManager.Connection)
                    using (var cmd = new SqlCommand("SELECT id as imageid from image where username=@param order by date desc", connection))
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        cmd.Parameters.AddWithValue("param", User.Identity.Name);
                        adapter.Fill(ds);
                    }
                    imageRepeater2.DataSource = ds;
                    imageRepeater2.DataBind();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            load();
            albumName.Text = Name;
            albumDate.Text = Date;
        }

        protected void imageRepeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var checkBox  = (CheckBox)e.Item.FindControl("imageIsSelected");
            var view = (DataRowView)e.Item.DataItem;
            var id = view[0];

            using (var connection = DatabaseManager.Connection)
            using (var cmd = new SqlCommand("SELECT id from imagealbum where imageid=@param and albumid=@param2", connection))
            {
                cmd.Parameters.AddWithValue("param", id);
                cmd.Parameters.AddWithValue("param2", AlbumId);
                using (var reader = cmd.ExecuteReader())
                    if (reader.Read())
                        checkBox.Checked = true;
            }
        }

        protected void imageIsSelected_CheckedChanged(object sender, EventArgs e)
        {
            var cb = (CheckBox)sender;
            using (var connection = DatabaseManager.Connection)
            {
                if (cb.Checked)
                {
                    using (var cmd = new SqlCommand("insert into imagealbum (imageid, albumid) values (@param1, @param2)", connection))
                    {
                        cmd.Parameters.AddWithValue("param1", cb.ValidationGroup);
                        cmd.Parameters.AddWithValue("param2", AlbumId);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (var cmd = new SqlCommand("delete imagealbum where imageid=@param1 and albumid = @param2", connection))
                    {
                        cmd.Parameters.AddWithValue("param1", cb.ValidationGroup);
                        cmd.Parameters.AddWithValue("param2", AlbumId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            Response.Redirect(Request.RawUrl);
        }
    }
}