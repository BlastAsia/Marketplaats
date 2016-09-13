﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using Marketplaats.Winforms.Model;
using Marketplaats.Winforms.Services;
using static Marketplaats.Winforms.Properties.Settings;

namespace Marketplaats.Winforms
{
    public partial class frmMainDev : Form
    {
        public frmMainDev()
        {
            InitializeComponent();
            lblStatus.Text = "";
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

            RestSharpService restSharp = new RestSharpService();
            try
            {
              //TODO: Testing of expire tokens
             //throw new Exception("Expire Token");

                // If no token yet
                if (string.IsNullOrEmpty(Default.AccessToken))
                {
                    if (restSharp.Authentication())
                    {
                        BoxUser user = restSharp.GetBoxUser(restSharp.AccessToken.access_token);
                        MessageBox.Show($"{user.name} {user.login}");
                    }
                }
                else
                {
                        BoxUser user = restSharp.GetBoxUser(Default.AccessToken);
                        MessageBox.Show($"{user.name} {user.login}");
                }

            }
            catch (Exception ex)
            {
                if (ex.Message == "Expire Token")
                {
                    restSharp.RefreshToken();
                    MakeResquest();
                }
            }

        }

 

        private void DisplayToListView()
        { 
            var cultureInfo = CultureInfo.GetCultureInfo("da-DK");
            
            List<Advertishments> ads = new List<Advertishments>()
            {
                new Advertishments() { Type_="HYBRID",Build="2016",Price =20000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="HYBRID",Build="2016",Price =20000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =20000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="+639987900777" },
                new Advertishments() { Type_="SEDAN",Build="2016",Price =20000.00,PhoneNumber="+639987900617" }

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
        }

        private void repositoryItemHyperLinkEdit1_Click(object sender, EventArgs e)
        {
            string phoneNumber = gridView1.GetFocusedDisplayText();
            DialogResult dialogResult = MessageBox.Show("Do you want to call this seller using Skype.", $"Skype call to ({phoneNumber})", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(dialogResult == DialogResult.Yes)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "skype.exe";
                startInfo.Arguments = $"/callto:{phoneNumber}";
                Process.Start(startInfo);
            }
        }

        private void gridView1_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            ColumnView view = sender as ColumnView;
            if (e.Column.FieldName == "Price" && e.ListSourceRowIndex != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
            {
                decimal price = Convert.ToDecimal(e.Value);
                var cultureInfo = CultureInfo.GetCultureInfo("da-DK");
                e.DisplayText = String.Format(cultureInfo, "{0:C}", price);
               
            }
        }
    }
    
}
