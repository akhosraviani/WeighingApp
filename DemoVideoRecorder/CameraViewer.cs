using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _03_Onvif_Network_Video_Recorder
{
    public partial class CameraViewer : Form
    {
        public Image PreviewImage
        {
            get { return this.pictureBox1.Image; }
            set { this.pictureBox1.Image = value; }
        }
        public CameraViewer()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
