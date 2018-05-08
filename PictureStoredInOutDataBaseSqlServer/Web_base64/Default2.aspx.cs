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

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strconn = @"server=PC-20160528TLMD\SQLEXPRESS;database=jwgl;Integrated Security=true";
        using (SqlConnection connection = new SqlConnection(strconn))
        {
            Stopwatch time1 = Stopwatch.StartNew();
            connection.Open();
            SqlDataAdapter myda = new SqlDataAdapter(" select * from myimage3 ", strconn);
            DataSet myds = new DataSet();
            myda.Fill(myds);
            string base64 = (myds.Tables[0].Rows[0]["base"]).ToString();
            byte[] bytes = Convert.FromBase64String(base64);
            this.Response.BinaryWrite(bytes);
            connection.Close();
            time1.Stop();
            long milliSec = time1.ElapsedMilliseconds; //计算整个操作所需时
            Application["time"] = milliSec;
        }
    }
}