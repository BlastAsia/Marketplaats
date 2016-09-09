using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;


namespace Marketplaats.Winforms
{
    public partial class frmMainDev : Form
    {
        public frmMainDev()
        {
            InitializeComponent();
            lblStatus.Text = "";
            webBrowser1.ObjectForScripting = new ScriptManager(this);

        }
        
        private  void btnFetch_Click(object sender, EventArgs e)
        {
            Fetch();
        }

        private async void Fetch()
        {
            start_progress();
           

            // Invoke http resquest
            await Task.Run(() => MakeResquest());
            
            // Load data to grid
            DisplayToListView();

            stop_progress();
            lblStatus.Text = $"Last reload { DateTime.Now.ToShortTimeString()}";
        }

        private void MakeResquest()
        {
            Thread.Sleep(1000);
        }

        private void DisplayToListView()
        {

            
            var cultureInfo = CultureInfo.GetCultureInfo("da-DK");
            int i = 1;
            
            List<Advertishments> ads = new List<Advertishments>()
            {
                new Advertishments() { Type_="HYBRID",Build="2016",Price =20000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="HYBRID",Build="2016",Price =20000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =20000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },

                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SEDAN",Build="2016",Price =20000.00,PhoneNumber="09987900617" }

            };

            gridView1.ClearColumnsFilter();
            grd.DataSource = ads;


            ColumnView view = grd.MainView as ColumnView;
            foreach (GridColumn column in view.Columns)
            {
                column.OptionsFilter.FilterPopupMode = FilterPopupMode.CheckedList;
            }
        }

      

        void  start_progress()
        {
            
            progress.Visible = true;
            progress.Style = ProgressBarStyle.Marquee;
        }

        void stop_progress()
        {
            progress.Visible = false;
            progress.Style = ProgressBarStyle.Continuous;
        }
        

        private void frmMain_Load(object sender, EventArgs e)
        {
            Fetch();

            this.webBrowser1.Navigate(@"C:\temp\html.html");

            //ProcessStartInfo startInfo = new ProcessStartInfo();
            //startInfo.FileName = "skype.exe";
            //startInfo.Arguments = "/secondary /callto:echo123"; 
            //Process.Start(startInfo);
        }

        private void repositoryItemHyperLinkEdit1_Click(object sender, EventArgs e)
        {
            string cellValue = gridView1.GetFocusedDisplayText();
            MessageBox.Show("Do you want to call this seller using Skype.", $"Skype Call ({cellValue})", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }


    [ComVisible(true)]
    public class ScriptManager
    {
        frmMainDev _form;
        public ScriptManager(frmMainDev form)
        {
            _form = form;
        }

        public void ShowMessage(object obj)
        {
            MessageBox.Show(obj.ToString());
        }
    }
}
