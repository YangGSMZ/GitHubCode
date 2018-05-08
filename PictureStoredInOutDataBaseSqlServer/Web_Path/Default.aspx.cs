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
    string Img;
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            Img = FileUpload1.PostedFile.FileName;
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('错误！')</script>");
        }
        string filepath = "~/photo/";
        string filefullname = filepath + Img;
        string strconn = @"server=PC-20160528TLMD\SQLEXPRESS;database=jwgl;Integrated Security=true";
        using (SqlConnection connection = new SqlConnection(strconn))
        {
            //计时开始
            Stopwatch time1 = Stopwatch.StartNew();
            connection.Open();
            string strInsert = "insert into myimage2 (picturePath) values (@picturePath)";
            SqlCommand cmd = new SqlCommand(strInsert, connection);
            cmd.Parameters.Add("@picturePath", SqlDbType.VarChar);    //以参数化形式写入数据库
            cmd.Parameters["@picturePath"].Value = filefullname;
            cmd.ExecuteNonQuery();
            string uppath = Server.MapPath("~/Photo/") + Img;
            FileUpload1.PostedFile.SaveAs(uppath);
            connection.Close();
            time1.Stop();
            long milliSec = time1.ElapsedMilliseconds; //计算整个操作所需时间
            Label1.Text = milliSec.ToString() + "ms";
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string connStr = @"server=PC-20160528TLMD\SQLEXPRESS;database=jwgl;Integrated Security=true";
        string sqlStr = "select * from myimage2";
        SqlDataAdapter sda;
        SqlCommand comm;
        DataSet ds;
        comm = new SqlCommand();
        using (SqlConnection connection = new SqlConnection(connStr))
        {
            //计时开始
            Stopwatch time1 = Stopwatch.StartNew(); 
            comm.Connection = connection;
            comm.CommandText = sqlStr;
            comm.CommandType = CommandType.Text;
            comm.Connection.Open(); 
            ds = new DataSet();
            sda = new SqlDataAdapter(comm);
            sda.Fill(ds);
            string pathfile = ds.Tables[0].Rows[0][0].ToString();
            Image1.ImageUrl = pathfile;
            connection.Close();
            time1.Stop();
            long milliSec = time1.ElapsedMilliseconds; //计算整个操作所需时间
            Label2.Text = milliSec.ToString() + "ms";
        }        
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        string connStr = @"server=PC-20160528TLMD\SQLEXPRESS;database=jwgl;Integrated Security=true";
        string sqlStr = "delete from myimage2";
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