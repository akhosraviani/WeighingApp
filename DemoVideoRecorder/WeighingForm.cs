using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Text;

namespace _03_Onvif_Network_Video_Recorder
{
    public partial class WeighingForm : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private PrivateFontCollection fonts = new PrivateFontCollection();

        Font myFont;
        public WeighingForm()
        {
            InitializeComponent();
            this.Load += WeighingForm_Load;

            Initialize();

            
        }

        private void Initialize()
        {
            var pos1 = this.PointToScreen(cameraIndicator1.Location);
            pos1 = imgCamera1.PointToClient(pos1);
            cameraIndicator1.Parent = imgCamera1;
            cameraIndicator1.Location = pos1;
            cameraIndicator1.BackColor = Color.Transparent;

            var pos2 = this.PointToScreen(cameraIndicator2.Location);
            pos2 = imgCamera2.PointToClient(pos2);
            cameraIndicator2.Parent = imgCamera2;
            cameraIndicator2.Location = pos2;
            cameraIndicator2.BackColor = Color.Transparent;
                
            var pos3 = this.PointToScreen(cameraIndicator3.Location);
            pos3 = imgCamera3.PointToClient(pos3);
            cameraIndicator3.Parent = imgCamera3;
            cameraIndicator3.Location = pos3;
            cameraIndicator3.BackColor = Color.Transparent;

            var pos4 = this.PointToScreen(cameraIndicator4.Location);
            pos4 = imgCamera4.PointToClient(pos4);
            cameraIndicator4.Parent = imgCamera4;
            cameraIndicator4.Location = pos4;
            cameraIndicator4.BackColor = Color.Transparent;

            byte[] fontData = Properties.Resources.IRANSans_FaNum_;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.IRANSans_FaNum_.Length);
            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.IRANSans_FaNum_.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

            myFont = new Font(fonts.Families[0], 8.5F);
        }

        void WeighingForm_Load(object sender, EventArgs e)
        {
            this.Font = myFont;
        }
    }
}
