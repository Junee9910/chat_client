using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Chat_Client
{
    public partial class frm_Login : Form
    {
        public frm_Login()
        {
            InitializeComponent();
        }

        public static string UserName;
        public static string showUser
        {
            get { return UserName; }
            set { UserName = value; }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btDangNhap_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=DINHNHUNG\SQLEXPRESS;Initial Catalog=ListClient;Integrated Security=True;");
            string sql = "select * from ClientAccount where TenDangNhap = '" + tbUser.Text + "' and MatKhau = '" + tbPass.Text + "'";
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            try
            {
                if (dt.Rows.Count > 0)
                {
                    showUser = tbUser.Text;
                    frm_Client fcl = new frm_Client();
                    this.Hide();
                    fcl.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoăc mật khẩu sai", "Xin lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối");
            }
        }
    }
}
