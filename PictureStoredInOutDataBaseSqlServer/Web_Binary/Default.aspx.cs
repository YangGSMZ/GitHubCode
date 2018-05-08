using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    string Img;//获取图片信息
    string ImgName;//图片文件名
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            Img = FileUpload1.PostedFile.FileName;    //获取FileUpload控件上的内容
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('错误！')</script>");
        }
        ImgName = Img.Substring(Img.LastIndexOf("\\") + 1);  //获取图片文件名
        string strconn = @"server=PC-20160528TLMD\SQLEXPRESS;database=jwgl;Integrated Security=true";
        using (SqlConnection connection = new SqlConnection(strconn))
        {
            connection.Open();

            //计时开始
            Stopwatch time1 = Stopwatch.StartNew();
            int FileLen = FileUpload1.PostedFile.ContentLength;
            Byte[] FileData = new Byte[FileLen];
            HttpPostedFile hp = FileUpload1.PostedFile;
            Stream sr = hp.InputStream;     //创建文件流
            sr.Read(FileData, 0, FileLen);
            string strInsert = "insert into myimage (name,photo) values (@pictureName,@imgdata)";
            SqlCommand cmd = new SqlCommand(strInsert, connection);
            cmd.Parameters.Add("@pictureName", SqlDbType.VarChar);    //以参数化形式写入数据库
            cmd.Parameters["@pictureName"].Value = ImgName;
            cmd.Parameters.Add("@imgdata", SqlDbType.VarBinary);
            cmd.Parameters["@imgdata"].Value = FileData;
            cmd.ExecuteNonQuery();
            time1.Stop();
            connection.Close();
            long milliSec = time1.ElapsedMilliseconds; //计算整个操作所需时间
            Label1.Text = milliSec.ToString() + "ms";
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        //显示图片  
        Image1.ImageUrl = "Default2.aspx";
        Label2.Text = Application["time"].ToString()+"ms";
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        string connStr = @"server=PC-20160528TLMD\SQLEXPRESS;database=jwgl;Integrated Security=true";
        string sqlStr = "delete from myimage";
        SqlCommand comm;
        using (SqlConnection connection = new SqlConnection(connStr))
        {
            comm = new SqlCommand(sqlStr, connection);
            comm.Connection.Open();
            comm.ExecuteNonQuery();
            connection.Close();
        }
    }
}