using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace imaGenie
{
    public partial class Search : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var searchText = Request.Params["q"];
                if (!string.IsNullOrEmpty(searchText)) searchText = Server.UrlDecode(searchText);
                else
                {
                    Response.Redirect("Index.aspx");
                    return;
                }
                
                string[] parts = searchText.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                DataSet ds = new DataSet();
                using (var connection = DatabaseManager.Connection)
                {
                    for (int i = 0; i < parts.Length; i++)
                    {
                        using (var cmd = new SqlCommand($"SELECT top 20 id FROM image where text like @param order by date desc", connection))
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            cmd.Parameters.AddWithValue($"param", $"%{parts[i]}%");
                            adapter.Fill(ds);
                        }
                    }

                }
                var distinctDs = new DataSet();
                var table = ds.Tables[0];
                var view = table.DefaultView;
                table = view.ToTable(true, "id");
                view = table.DefaultView;
                while (view.Count > 20) view.Delete(20);
                distinctDs.Tables.Add(view.ToTable());
                imagesRepeater.DataSource = distinctDs;
                imagesRepeater.DataBind();
            }
        }
    }
}