using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;

namespace Path_Image
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string picturePath;
        Image img;
        private void CunFang_Btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = @"E:\";
            openFileDialog.Filter = "图像图片*.*jpg|*.JPG|*.bmp|*.BMP|*.png|*.PNG";
            openFileDialog.FilterIndex = 1;
            try
            {
                FolderBrowserDialog folderBrowerDialog=new FolderBrowserDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    img = Image.FromFile(openFileDialog.FileName);
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
                string strInsert = "insert into myimage2 (picturePath) values (@picturePath)";
                SqlCommand cmd = new SqlCommand(strInsert, connection);
                cmd.Parameters.Add("@picturePath", SqlDbType.VarChar);    //以参数化形式写入数据库
                //string path = openFileDialog.FileName;
                //string path = "~/photo/" + openFileDialog.SafeFileName;
                //picturePath = path;
                //img.Save(picturePath);
                string picturePath = Application.StartupPath+"/" + openFileDialog.SafeFileName;
                img.Save(openFileDialog.SafeFileName);
                //picturePath = openFileDialog.FileName;//获取文件对话框中选定的文件名的字符串，包括文件路径 
                cmd.Parameters["@picturePath"].Value = picturePath;
                cmd.ExecuteNonQuery();
                connection.Close();
                time1.Stop();
                long milliSec = time1.ElapsedMilliseconds; //计算整个操作所需时间
                label1.Text = milliSec.ToString() + "ms";
            }
        }

        private void Delete_Btn_Click(object sender, EventArgs e)
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

        private void DuQu_Btn_Click(object sender, EventArgs e)
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

                if (ds.Tables[0].Rows.Count > 0)
                {
                    this.pictureBox2.Image = Bitmap.FromFile(ds.Tables[0].Rows[0][0].ToString());
                }
                connection.Close();
                time1.Stop();
                long milliSec = time1.ElapsedMilliseconds; //计算整个操作所需时间
                label2.Text = milliSec.ToString() + "ms";
            } 
        }
    }
}
