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
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Views.Grid;


namespace Marketplaats.Winforms
{
    public partial class frmMain : XtraForm
    {

        List<Advertishments> _ads;
        int _timeout = 30000;
        int _currentpage = 1;

        public frmMain()
        {
            InitializeComponent();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Text = Text + " v" + version.Major + "." + version.Minor;
            lblStatus.Text = "";
            dropdownpage.DropDownControl = CreateDXPopupMenu();
            Paging(false);

        }

        public int  ResultPerPage
        {
            get { return Convert.ToInt32(dropdownpage.Text); }
            set { dropdownpage.Text = value.ToString(); }
        }

        private  void btnFetch_Click(object sender, EventArgs e)
        {
            Fetch(1, ResultPerPage);
        }

        private async void Fetch(int page,int resultperpage)
        {
            try
            {    
                start_progress();
            
                //Load and parse
                HtmlParsersService htmlpack = new HtmlParsersService();
               
                var task = Task.Run(() => htmlpack.StartParsing(page, resultperpage));

                if (await Task.WhenAny(task, Task.Delay(_timeout)) == task)
                {
                    _ads = task.Result;

                    if (_currentpage == 100)
                    {
                        btnForward.Enabled = false;
                    }
                    else
                    {
                        btnBack.Enabled = true;
                    }

                    if (_currentpage == 1)
                    {
                        btnBack.Enabled = false;
                    }
                    else
                    {
                        btnForward.Enabled = true;
                    }
                    lblPage.Text = _currentpage.ToString();

                }
                else
                {
                    throw new Exception("Connection Time-Out");
                }

            
                //Load data to grid
                DisplayToListView();

                stop_progress();
                lblStatus.Text = $"Last reload { DateTime.Now.ToShortTimeString()}";

            }
            catch (Exception ex)
            {
                stop_progress();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      
        private void DisplayToListView()
        {
            gridView1.ClearColumnsFilter();
            grd.DataSource = _ads;
            Paging(true);

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

            Cursor = Cursors.WaitCursor;
            progress.Visible = true;
            progress.Style = ProgressBarStyle.Marquee;
        }

        void stop_progress()
        {
            Cursor = Cursors.Default;
            progress.Visible = false;
            progress.Style = ProgressBarStyle.Continuous;
        }
        
        private void frmMain_Load(object sender, EventArgs e)
        {
            Fetch(1,30);    
        }

        private async void repositoryItemHyperLinkEdit1_Click(object sender, EventArgs e)
        {
            try
            {
                
                Cursor = Cursors.WaitCursor;

                var link = gridView1.GetRowCellValue(gridView1.FocusedRowHandle,"Link").ToString();

                HtmlParsersService htmlpack = new HtmlParsersService();
                
                string phoneNumber = string.Empty;

                var task = Task.Run(() => htmlpack.GetPhoneNumber(link));

                if (await Task.WhenAny(task, Task.Delay(_timeout)) == task)
                {
                    phoneNumber = task.Result;
                }
                else
                {
                    throw new Exception("Connection Time-Out");
                }
                
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

        private void grd_Layout(object sender, LayoutEventArgs e)
        {
           
            for (int i = 0; i < gridView1.Columns.Count; i++)
            {
                gridView1.Columns[i].OptionsFilter.AllowFilter = gridView1.Columns[i].Visible;
            }
        }

        void Paging(bool state)
        {
            panelPage.Visible = state;
            panelPage.Enabled = state;
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            try
            {
                _currentpage++;
                Fetch(_currentpage,ResultPerPage);
                
            }
            catch (Exception)
            {
                _currentpage--;


            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            try
            {

                _currentpage--;
                Fetch(_currentpage, ResultPerPage);
            }
            catch (Exception)
            {

                _currentpage++;
            }
            
            
        }

        
        private DXPopupMenu CreateDXPopupMenu()
        {
            var menu = new DXPopupMenu();
            menu.Items.Add(new DXMenuItem("30", OnItemClick));
            menu.Items.Add(new DXMenuItem("50", OnItemClick));
            menu.Items.Add(new DXMenuItem("100", OnItemClick));
            
            return menu;
        }



        private void OnItemClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            ResultPerPage = Convert.ToInt32( item.Caption);
            Fetch(_currentpage, ResultPerPage);
        }

        private void dropdownpage_Click(object sender, EventArgs e)
        {
            dropdownpage.ShowDropDown();
        }
    }
    
}
