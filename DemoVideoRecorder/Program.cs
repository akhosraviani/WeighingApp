/************************************************************************************************************ 
 * OZEKI CAMERA SDK
 * http://www.camera-sdk.com/
 * 
 * Example project
 * Title: Video recorder
 * Description: This example demonstrates how you can start and stop a recording
 * and take a snapshot on multiple cameras into a given directory. 
 * 
 * Documentation:
 * http://www.camera-sdk.com/p_20-onvif.html
 * 
 * License:
 * This example can be freely used, distributed and modified according to the
 * license agreement at the following webpage: http://camera-sdk.com/p_241-license.html 
 * *********************************************************************************************************/
using System;
using System.Windows.Forms;

namespace _03_Onvif_Network_Video_Recorder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DialogResult result;
            using (var loginForm = new LoginForm())
                result = loginForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                // login was successful
                Application.Run(new App());
            }
            
        }
    }
}
