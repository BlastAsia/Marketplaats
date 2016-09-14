using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using Marketplaats.Winforms.Helper;
using Marketplaats.Winforms.Model;
using Marketplaats.Winforms.Services;
using static Marketplaats.Winforms.Properties.Settings;

namespace Marketplaats.Winforms
{
    public partial class frmMain : XtraForm
    {
        public frmMain()
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
             
             //throw new Exception("Expire Token");

                // If no token yet
                if (string.IsNullOrEmpty(Default.AccessToken))
                {
                    if (restSharp.Authentication())
                    {
                        BoxUser user = restSharp.GetBoxUser(restSharp.AccessToken.access_token);
                        //MessageBox.Show($"{user.name} {user.login}");
                    }
                }
                else
                {
                        BoxUser user = restSharp.GetBoxUser(Default.AccessToken);
                        //MessageBox.Show($"{user.name} {user.login}");
                }

            }
            catch (Exception ex)
            {
                if (ex.Message == "Unauthorized")
                {
                    restSharp.RefreshToken();
                    MakeResquest();
                }
            }

        }

 

        private void DisplayToListView()
        { 
            
            
            List<Advertishments> ads = new List<Advertishments>()
            {
               
                new Advertishments() { Type_="Hatchback",Build="2013",Price =200.00,PhoneNumber="0597647062".ToSkypeFormat() },
                new Advertishments() { Type_="Hatchback",Build="2012",Price =189.00,PhoneNumber="06-62962395".ToSkypeFormat() },
                new Advertishments() { Type_="Hatchback",Build="2012",Price =170.00,PhoneNumber="06-64002124".ToSkypeFormat() },
                new Advertishments() { Type_="Hatchback",Build="2011",Price =110.00,PhoneNumber="06-53021733".ToSkypeFormat() },
                new Advertishments() { Type_="Hatchback",Build="2010",Price ="Ask",PhoneNumber="06-83336206".ToSkypeFormat() },

                new Advertishments() { Type_="Sedan",Build="2014",Price =210.00,PhoneNumber="06-42681333".ToSkypeFormat() },
                new Advertishments() { Type_="Sedan",Build="2013",Price =190.00,PhoneNumber="06-82015867".ToSkypeFormat() },
                new Advertishments() { Type_="Sedan",Build="2012",Price =159.00,PhoneNumber="06-43052640".ToSkypeFormat() },
                new Advertishments() { Type_="Sedan",Build="2011",Price =110.00,PhoneNumber="06-31340367".ToSkypeFormat() },
                new Advertishments() { Type_="Sedan",Build="2010",Price ="Ask",PhoneNumber= "06-31340367".ToSkypeFormat() },

                new Advertishments() { Type_="SUV",Build="2014",Price =299.00,PhoneNumber= "06-31340367".ToSkypeFormat() },
                new Advertishments() { Type_="SUV",Build="2013",Price =198.00,PhoneNumber= "06-31340367".ToSkypeFormat() },
                new Advertishments() { Type_="SUV",Build="2015",Price =300.00,PhoneNumber= "06-31340367".ToSkypeFormat() },
                new Advertishments() { Type_="SUV",Build="2009",Price ="Ask",PhoneNumber= "06-57325659".ToSkypeFormat() },
                new Advertishments() { Type_="SUV",Build="2008",Price ="Ask",PhoneNumber= "06-31340367".ToSkypeFormat() },

                new Advertishments() { Type_="stationwagon",Build="2010",Price =10000.00,PhoneNumber= "06-56736840".ToSkypeFormat() },
                new Advertishments() { Type_="stationwagon",Build="2008",Price =3000.00,PhoneNumber= "06-56736840".ToSkypeFormat() },
                new Advertishments() { Type_="stationwagon",Build="2015",Price =20000.00,PhoneNumber= "06-56736840".ToSkypeFormat() },
                new Advertishments() { Type_="stationwagon",Build="2009",Price ="See Description",PhoneNumber= "06-38809780".ToSkypeFormat() },

                new Advertishments() { Type_="cabriolet",Build="2000",Price =2940.00,PhoneNumber= "06-56736840".ToSkypeFormat() },
                new Advertishments() { Type_="cabriolet",Build="2001",Price =9400,PhoneNumber= "06-56736840".ToSkypeFormat() },
                new Advertishments() { Type_="cabriolet",Build="1994",Price =400,PhoneNumber= "06-56736840".ToSkypeFormat() },
                new Advertishments() { Type_="cabriolet",Build="2004",Price =8550,PhoneNumber= "06-56736840".ToSkypeFormat() },
                new Advertishments() { Type_="cabriolet",Build="2011",Price =30950,PhoneNumber= "06-56736840".ToSkypeFormat() },




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
            try
            {

            
            ColumnView view = sender as ColumnView;
            if (e.Column.FieldName == "Price" && e.ListSourceRowIndex != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
            {
                decimal price = Convert.ToDecimal(e.Value);
                var cultureInfo = CultureInfo.GetCultureInfo("da-DE");
                e.DisplayText = String.Format(cultureInfo, "{0:C}", price);
               
            }

            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("Input string was not in a correct format."))
                {
                    e.DisplayText = e.Value.ToString();
                }
                
            }
        }
    }
    
}
