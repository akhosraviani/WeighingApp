using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace AshaWeighing
{
    public partial class LoginForm : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private PrivateFontCollection fonts = new PrivateFontCollection();

        private Font myFont;
        public LoginForm()
        {
            InitializeComponent();
            this.Load += LoginForm_Load;
            Initialize();
        }

        void LoginForm_Load(object sender, EventArgs e)
        {
            label1.Font = myFont;
            label2.Font = myFont;
            button1.Font = myFont;
            button2.Font = myFont;
            txtUsername.Font = myFont;
            txtPassword.Font = myFont;
        }

        private void Initialize()
        {
            byte[] fontData = AshaWeighing.Properties.Resources.IRANSans_FaNum_;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, AshaWeighing.Properties.Resources.IRANSans_FaNum_.Length);
            AddFontMemResourceEx(fontPtr, (uint)AshaWeighing.Properties.Resources.IRANSans_FaNum_.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

            myFont = new Font(fonts.Families[0], 10F);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        public static string EncryptString(string s)
        {
            string sKey = "3xp10r3r";
            MemoryStream ms = new MemoryStream();

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = DES.CreateEncryptor();

            CryptoStream cryptoStream = new CryptoStream(ms, desencrypt, CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(s);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (DoLogin())
                Close();
        }

        private bool DoLogin()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var pwd = EncryptString(txtPassword.Text + txtUsername.Text.Length.ToString() + txtUsername.Text.ToLower());

                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                             ConnectionStrings["AshaDbContext"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT a.Code, b.PersonnelCode, b.Title FROM SIScr_User AS a INNER JOIN HREA_Personnel as b ON a.Code = b.UserCode " +
                                                           " WHERE a.Title = '" + txtUsername.Text + "' AND a.Password = '" + pwd + "'"
                                                            , con))
                    {
                        con.Open();
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            Globals.UserCode = dt.Rows[0].ItemArray[0].ToString();
                            Globals.PersonnelCode = dt.Rows[0].ItemArray[1].ToString();
                            Globals.UserName = dt.Rows[0].ItemArray[2].ToString();
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("نام کاربری یا رمز عبور اشتباه است. لطفا مجددا تلاش نمایید", "پیغام", MessageBoxButtons.OK,
                                MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                            txtUsername.Focus();
                            txtUsername.SelectAll();
                            return false;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("ارتباط با پایگاه داده برقرار نشد. لطفا مجددا تلاش نمایید", "پیغام", MessageBoxButtons.OK,
                                MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                txtUsername.Focus();
                txtUsername.SelectAll();
                return false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if(DoLogin())
                    Close();
        }
    }
}
