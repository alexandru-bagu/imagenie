using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;

namespace imaGenie
{
    public partial class Edit : Page
    {        
        private string ImageId { get { return Request.Params["id"]; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                image.ImageUrl = $"Image.aspx?id={ImageId}";
                using (var connection = DatabaseManager.Connection)
                using (var cmd = new SqlCommand("select width,height from image where id=@id", connection))
                {
                    cmd.Parameters.AddWithValue("id", ImageId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            Response.Redirect("Index.aspx");//oups, the image is missing?
                        }
                        else
                        {
                            widthResize.Text = reader["width"].ToString();
                            heightResize.Text = reader["height"].ToString();
                            endX.Text = reader["width"].ToString();
                            endY.Text = reader["height"].ToString();
                        }
                    }
                }
            }
        }

        protected void pResizeBtn_Click(object sender, EventArgs e)
        {
            var text = pResize.Text;
            text = text.Replace("%", "");
            float k = float.Parse(text);
            k /= 100;
            if (k == 1) return;
            Bitmap resizedImg = null;
            using (var connection = DatabaseManager.Connection)
            {
                using (var cmd = new SqlCommand("select data from image where id=@id", connection))
                {
                    cmd.Parameters.AddWithValue("id", ImageId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var bytes = (byte[])reader["data"];
                            using (var ms = new MemoryStream(bytes))
                            {
                                Bitmap bmp = new Bitmap(ms);
                                resizedImg = ImageHelper.ResizeImage(bmp, (int)(bmp.Width * k), (int)(bmp.Height * k));
                            }
                        }
                    }
                }
                if(resizedImg != null)
                {
                    byte[] data = ImageHelper.Convert(resizedImg, ImageFormat.Png);
                    using (var cmd2 = new SqlCommand("update image set data=@data, thumbnail=@tn, width=@width, height=@height where id=@id", connection))
                    {
                        cmd2.Parameters.AddWithValue("id", ImageId);
                        cmd2.Parameters.AddWithValue("data", data);
                        cmd2.Parameters.AddWithValue("tn", ImageHelper.Convert(ImageHelper.Thumbnail(resizedImg), ImageFormat.Png));
                        cmd2.Parameters.AddWithValue("width", resizedImg.Width);
                        cmd2.Parameters.AddWithValue("height", resizedImg.Height);
                        cmd2.ExecuteNonQuery();
                    }
                }
            }
            Response.Redirect(Request.RawUrl);
        }

        protected void wxhResize_Click(object sender, EventArgs e)
        {
            var widthText = widthResize.Text;
            var heightText = heightResize.Text;
            int width, height;
            if (int.TryParse(widthText, out width) && int.TryParse(heightText, out height))
            {
                Bitmap resizedImg = null;
                using (var connection = DatabaseManager.Connection)
                {
                    using (var cmd = new SqlCommand("select data from image where id=@id", connection))
                    {
                        cmd.Parameters.AddWithValue("id", ImageId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var bytes = (byte[])reader["data"];
                                using (var ms = new MemoryStream(bytes))
                                {
                                    Bitmap bmp = new Bitmap(ms);
                                    if (bmp.Width == width && bmp.Height == height) return;
                                    resizedImg = ImageHelper.ResizeImage(bmp, width, height);
                                }
                            }
                        }
                    }
                    if (resizedImg != null)
                    {
                        byte[] data = ImageHelper.Convert(resizedImg, ImageFormat.Png);
                        using (var cmd2 = new SqlCommand("update image set data=@data, thumbnail=@tn, width=@width, height=@height where id=@id", connection))
                        {
                            cmd2.Parameters.AddWithValue("id", ImageId);
                            cmd2.Parameters.AddWithValue("data", data);
                            cmd2.Parameters.AddWithValue("tn", ImageHelper.Convert(ImageHelper.Thumbnail(resizedImg), ImageFormat.Png));
                            cmd2.Parameters.AddWithValue("width", resizedImg.Width);
                            cmd2.Parameters.AddWithValue("height", resizedImg.Height);
                            cmd2.ExecuteNonQuery();
                        }
                    }
                }
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                (Master as Default).ShowPopup("Error", "Values for width and height must be integers.");
            }
        }

        protected void cropBtn_Click(object sender, EventArgs e)
        {
            var startXText = initialX.Text;
            var startYText = initialY.Text;
            var endXText = endX.Text;
            var endYText = endY.Text;
            int sX, sY, eX, eY;
            if (int.TryParse(startXText, out sX) && int.TryParse(startYText, out sY) && int.TryParse(endXText, out eX) && int.TryParse(endYText, out eY))
            {
                if (eX - sX < 0 || eY - sY < 0)
                {
                    (Master as Default).ShowPopup("Error", "You cannot use those values.");
                    return;
                }
                Bitmap croppedImg = null;
                using (var connection = DatabaseManager.Connection)
                {
                    using (var cmd = new SqlCommand("select data from image where id=@id", connection))
                    {
                        cmd.Parameters.AddWithValue("id", ImageId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var bytes = (byte[])reader["data"];
                                using (var ms = new MemoryStream(bytes))
                                {
                                    Bitmap bmp = new Bitmap(ms);
                                    croppedImg = ImageHelper.Crop(bmp, sX, sY, eX, eY);
                                }
                            }
                        }
                    }
                    if (croppedImg != null)
                    {
                        byte[] data = ImageHelper.Convert(croppedImg, ImageFormat.Png);
                        using (var cmd2 = new SqlCommand("update image set data=@data, thumbnail=@tn, width=@width, height=@height where id=@id", connection))
                        {
                            cmd2.Parameters.AddWithValue("id", ImageId);
                            cmd2.Parameters.AddWithValue("data", data);
                            cmd2.Parameters.AddWithValue("tn", ImageHelper.Convert(ImageHelper.Thumbnail(croppedImg), ImageFormat.Png));
                            cmd2.Parameters.AddWithValue("width", croppedImg.Width);
                            cmd2.Parameters.AddWithValue("height", croppedImg.Height);
                            cmd2.ExecuteNonQuery();
                        }
                    }
                }
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                (Master as Default).ShowPopup("Error", "Values for width and height must be integers.");
            }
        }

        protected void cv1_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            var text = args.Value;
            text = text.Replace("%", "");
            float res;
            if (float.TryParse(text, out res))
            {
                if (res >= 0 && res <= 100)
                {
                    args.IsValid = true;
                }
                else
                {
                    args.IsValid = false;
                }
            }
            else
                args.IsValid = false;
        }
    }
}