using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using Marketplaats.Winforms.Model;
using Marketplaats.Winforms.Services;
using static Marketplaats.Winforms.Properties.Settings;
using System.Reflection;


namespace Marketplaats.Winforms
{
    public partial class frmMain : XtraForm
    {

        List<Advertishments> _ads;

        public frmMain()
        {
            InitializeComponent();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Text = Text + " v" + version.Major + "." + version.Minor;
            lblStatus.Text = "";

        }
        
        private  void btnFetch_Click(object sender, EventArgs e)
        {
            Fetch();
        }

        private async void Fetch()
        {
            try
            {    
                start_progress();
            
                //Load and parse
                HtmlParsersService htmlpack = new HtmlParsersService();
                _ads =   await Task.Run(() => htmlpack.StartParsing());
            
                //Load data to grid
                DisplayToListView();

                stop_progress();
                lblStatus.Text = $"Last reload { DateTime.Now.ToShortTimeString()}";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      
        private void DisplayToListView()
        {
            gridView1.ClearColumnsFilter();
            grd.DataSource = _ads;


            ColumnView view = grd.MainView as ColumnView;
            foreach (GridColumn column in view.Columns)
            {
                column.OptionsFilter.FilterPopupMode = FilterPopupMode.CheckedList;
            }
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

        private async void repositoryItemHyperLinkEdit1_Click(object sender, EventArgs e)
        {
            try
            {

    
                Cursor = Cursors.WaitCursor;

                var link = gridView1.GetRowCellValue(gridView1.FocusedRowHandle,"Link").ToString();

                HtmlParsersService htmlpack = new HtmlParsersService();
            
                var phoneNumber = await Task.Run(() => htmlpack.GetPhoneNumber(link));

                Cursor = Cursors.Default;

                DialogResult dialogResult = MessageBox.Show("Call this seller using Skype.", $"Skype call to ({phoneNumber})", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if(dialogResult == DialogResult.Yes)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "skype.exe";
                    startInfo.Arguments = $"/callto:{phoneNumber}";
                    Process.Start(startInfo);
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
