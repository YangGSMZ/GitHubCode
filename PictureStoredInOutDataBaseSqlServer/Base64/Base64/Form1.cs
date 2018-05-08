using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Base64
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CunFang_Btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"E:\";
            openFileDialog.Filter = "图像图片*.*jpg|*.JPG|*.bmp|*.BMP|*.png|*.PNG";
            openFileDialog.FilterIndex = 1;
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Image img = Image.FromFile(openFileDialog.FileName);
                    pictureBox1.Image = img;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            string strconn = @"server=PC-20160528TLMD\SQLEXPRESS;database=jwgl;Integrated Security=true";
            using (SqlConnection connection = new SqlConnection(strconn))
            {
                //计时开始
                Stopwatch time1 = Stopwatch.StartNew();
                connection.Open();
                Image img = this.pictureBox1.Image;
                BinaryFormatter binFormatter = new BinaryFormatter();
                MemoryStream memStream = new MemoryStream();
                binFormatter.Serialize(memStream, img);
                byte[] bytes = memStream.GetBuffer();
                string base64 = Convert.ToBase64String(bytes);
                string strInsert = "insert into myimage3 (base) values (@base)";
                SqlCommand cmd = new SqlCommand(strInsert, connection);
                cmd.Parameters.Add("@base", SqlDbType.VarChar);    //以参数化形式写入数据库
                cmd.Parameters["@base"].Value = base64;
                cmd.ExecuteNonQuery();
                connection.Close();
                time1.Stop();
                long milliSec = time1.ElapsedMilliseconds; //计算整个操作所需时间
                label1.Text = milliSec.ToString() + "ms";
            }
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void DuQu_Btn_Click(object sender, EventArgs e)
        {
            string connStr = @"server=PC-20160528TLMD\SQLEXPRESS;database=jwgl;Integrated Security=true";
            string sqlStr = "select * from myimage3";
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

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string base64 = (ds.Tables[0].Rows[0]["base"]).ToString();
                    byte[] bytes = Convert.FromBase64String(base64);
                    MemoryStream mem = new MemoryStream(bytes);
                    BinaryFormatter binForm = new BinaryFormatter();
                    Image img = (Image)binForm.Deserialize(mem);
                    this.pictureBox2.Image = img;
                }
                connection.Close();
                time1.Stop();
                long milliSec = time1.ElapsedMilliseconds; //计算整个操作所需时间
                label2.Text = milliSec.ToString() + "ms";
            } 
        }
    }
}
