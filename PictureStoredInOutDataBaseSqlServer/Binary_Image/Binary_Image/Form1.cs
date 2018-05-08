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

namespace Binary_Image
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string pictureName; //图片名称
        private void LiuLan_Btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"E:\";
            openFileDialog.Filter = "图像图片*.*jpg|*.JPG|*.bmp|*.BMP|*.png|*.PNG";
            openFileDialog.FilterIndex = 1;
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureName=openFileDialog.SafeFileName;
                    Image img = Image.FromFile(openFileDialog.FileName);
                    pictureBox1.Image = img;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            string strconn = @"server=PC-20160528TLMD\SQLEXPRESS;database=jwgl;Integrated Security=true";
            using (SqlConnection connection = new SqlConnection(strconn))
            {
                //计时开始
                Stopwatch time1 = Stopwatch.StartNew();
                connection.Open();
                string fullpath = openFileDialog.FileName;//获取文件对话框中选定的文件名的字符串，包括文件路径 
                FileStream fs = new FileStream(fullpath, FileMode.Open, FileAccess.Read);
                byte[] imagebytes = new byte[fs.Length];//fs.Length文件流的长度，用字节表示 
                BinaryReader br = new BinaryReader(fs);//二进制文件读取器 
                imagebytes = br.ReadBytes(Convert.ToInt32(fs.Length));//从当前流中将count个字节读入字节数组中
                string strInsert = "insert into myimage (name,photo) values (@pictureName,@imgdata)";
                SqlCommand cmd = new SqlCommand(strInsert, connection);
                cmd.Parameters.Add("@pictureName", SqlDbType.VarChar);    //以参数化形式写入数据库
                cmd.Parameters["@pictureName"].Value = pictureName;
                cmd.Parameters.Add("@imgdata",SqlDbType.VarBinary);
                cmd.Parameters["@imgdata"].Value = imagebytes;
                cmd.ExecuteNonQuery();
                connection.Close();
                time1.Stop();
                long milliSec = time1.ElapsedMilliseconds; //计算整个操作所需时间
                label1.Text = milliSec.ToString()+"ms";
            }
        }

        private void DuQu_Btn_Click(object sender, EventArgs e)
        {
            string connStr = @"server=PC-20160528TLMD\SQLEXPRESS;database=jwgl;Integrated Security=true";
            string sqlStr = "select * from myimage";
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
                    MemoryStream ms = new MemoryStream((byte[])ds.Tables[0].Rows[0]["photo"]);
                    Bitmap image = new Bitmap(ms);
                    this.pictureBox2.Image = image;
                }
                connection.Close();
                time1.Stop();
                long milliSec = time1.ElapsedMilliseconds; //计算整个操作所需时间
                label2.Text = milliSec.ToString() + "ms";
            } 
        }
        private void Delete_Btn_Click(object sender, EventArgs e)
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
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
