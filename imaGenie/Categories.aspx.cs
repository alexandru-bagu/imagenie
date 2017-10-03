using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace imaGenie
{
    public partial class Categories : System.Web.UI.Page
    {
        protected bool bound = false;
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void categoryRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var repeater = (Repeater)e.Item.FindControl("imageRepeater");
            DataRowView view =(DataRowView) e.Item.DataItem;
            var id = view[0];
            DataSet ds = new DataSet();
            using (var connection = DatabaseManager.Connection)
            using (var cmd = new SqlCommand("select top 3 a.imageid as imageid from imagecategory a, image b where a.categoryid = @param and b.id = a.imageid and b.ispublic=1  order by b.date desc", connection))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("param", id);
                adapter.Fill(ds);
            }
            repeater.DataSource = ds;
            repeater.DataBind();
        }
        
        protected void categoryName_TextChanged(object sender, EventArgs e)
        {
            using (var conn = DatabaseManager.Connection)
            using (var cmd = new SqlCommand("update category set name=@name where id=@id", conn))
            {
                TextBox box = (TextBox)sender;
                if (box.Text.Length == 0) return;
                cmd.Parameters.AddWithValue("name", box.Text);
                cmd.Parameters.AddWithValue("id", box.ValidationGroup);
                cmd.ExecuteNonQuery();
                Response.Redirect(Request.RawUrl);
            }
        }

        protected void categoryDelete_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            using (var conn = DatabaseManager.Connection)
            {
                using (var cmd = new SqlCommand("delete category where id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id", button.ValidationGroup);
                    cmd.ExecuteNonQuery();
                }
                using (var cmd = new SqlCommand("delete imagecategory where categoryid=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id", button.ValidationGroup);
                    cmd.ExecuteNonQuery();
                }
            }
            Response.Redirect(Request.RawUrl);
        }

        protected void categoryInsert_Click(object sender, EventArgs e)
        {
            var master = (Master as Default);
            try
            {
                using (var conn = DatabaseManager.Connection)
                using (var cmd = new SqlCommand("insert into category (name) values (@name)", conn))
                {
                    TextBox box = (TextBox)newCategoryView.FindControl("newCategoryName");
                    if (box.Text.Length == 0) return;
                    cmd.Parameters.AddWithValue("name", box.Text);
                    cmd.ExecuteNonQuery();
                    Response.Redirect(Request.RawUrl);
                }
            }
            catch
            {
                master.ShowPopup("Error", "Category name already used. Try another one.");
            }
        }
    }
}