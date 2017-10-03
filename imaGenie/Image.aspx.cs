using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace imaGenie
{
    public partial class Image : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Request.Params["id"];
            if (string.IsNullOrEmpty(id)) return;
            var tn = Request.Params["tn"];
            if (string.IsNullOrEmpty(tn)) tn = "0";

            using (var conn = DatabaseManager.Connection)
            using (var cmd = new SqlCommand("select * from image where id=@id", conn))
            {
                cmd.Parameters.AddWithValue("id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        byte[] bytes;
                        if (tn[0] == '1') bytes = (byte[])reader["thumbnail"];
                        else bytes = (byte[])reader["data"];
                        var ext = (string)reader["extension"];

                        Response.ContentType = "image/" + ext;
                        Response.BinaryWrite(bytes);
                    }
                }
            }
        }
    }
}