using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
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
            string base64 = Convert.ToBase64String(FileData);
            string strInsert = "insert into myimage3 (base) values (@base)";
            SqlCommand cmd = new SqlCommand(strInsert, connection);
            cmd.Parameters.Add("@base", SqlDbType.VarChar);    //以参数化形式写入数据库
            cmd.Parameters["@base"].Value = base64;
            cmd.ExecuteNonQuery();
            time1.Stop();
            connection.Close();
            long milliSec = time1.ElapsedMilliseconds; //计算整个操作所需时间
            Label1.Text = milliSec.ToString() + "ms";
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Image1.ImageUrl = "Default2.aspx";
        Label2.Text = Application["time"].ToString() + "ms";
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        string connStr = @"server=PC-20160528TLMD\SQLEXPRESS;database=jwgl;Integrated Security=true";
        string sqlStr = "delete from myimage3";
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