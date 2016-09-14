using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Marketplaats.Winforms.Properties.Settings;

namespace Marketplaats.Winforms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            if (string.IsNullOrEmpty(Default.ClientID) || string.IsNullOrEmpty(Default.ClientSecret))
            {
                MessageBox.Show("Client ID and Client Secret are required.", "Marktplaats API", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }



            
        }
    }
}
