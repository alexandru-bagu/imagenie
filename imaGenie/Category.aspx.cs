using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace imaGenie
{
    public partial class Category : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var id = Request.Params["id"];
                DataSet ds = new DataSet();
                using (var connection = DatabaseManager.Connection)
                using (var cmd = new SqlCommand("SELECT top 20 a.id as id FROM [IMAGE] a, [IMAGECATEGORY] b where a.IsPublic=1 and b.ImageId = a.Id and b.CategoryId=@param  order by date a.desc", connection))
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("param", id);
                    adapter.Fill(ds);
                }
                imagesRepeater.DataSource = ds;
                imagesRepeater.DataBind();
            }
        }
    }
}