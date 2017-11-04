using AshaWeighing.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AshaWeighing
{
    public partial class AddSettingForm : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private PrivateFontCollection fonts = new PrivateFontCollection();
        private string _text = "";
        private Font myFont, myFontBig;

        public string Text
        {
            set
            {
                _text = value;
                txtName.Text = _text;
            }
            get { return _text; }
        }
        public AddSettingForm()
        {
            InitializeComponent();
            Load += AddSettingForm_Load;
        }

        private void AddSettingForm_Load(object sender, EventArgs e)
        {
            InitializeFont();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _text = txtName.Text;
        }

        private void InitializeFont()
        {
            byte[] fontData = AshaWeighing.Properties.Resources.IRANSans_FaNum_;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Resources.IRANSans_FaNum_.Length);
            AddFontMemResourceEx(fontPtr, (uint)Resources.IRANSans_FaNum_.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

            myFont = new Font(fonts.Families[0], 8.5F);
            myFontBig = new Font(fonts.Families[0], 14F);

            Font = myFont;
        }
    }
}
