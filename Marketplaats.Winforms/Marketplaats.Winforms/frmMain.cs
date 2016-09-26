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
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Layout.Modes;
using Marketplaats.Winforms.Helper;


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

        public int ResultPerPage
        {
            get { return Convert.ToInt32(dropdownpage.Text); }
            set { dropdownpage.Text = value.ToString(); }
        }

        private void btnFetch_Click(object sender, EventArgs e)
        {
            _currentpage = 1;
            Fetch(_currentpage, ResultPerPage);
        }

        private async void Fetch(int page, int resultperpage)
        {
            try
            {




                start_progress();
                var netconnection = Utilities.CheckForInternetConnection();
                if (!string.IsNullOrEmpty(netconnection) &&
                    !netconnection.Equals("An exception occurred during a Ping request."))
                {
                    throw new Exception("Can't connect to remote server. Please check your internet connection.");
                }


                //Load and parse
                HtmlParsersService htmlpack = new HtmlParsersService();

                int maxpage = 0;
                var task = Task.Run(() => htmlpack.StartParsing(page, resultperpage, ref maxpage));

                if (await Task.WhenAny(task, Task.Delay(_timeout)) == task)
                {

                    if (task.IsFaulted)
                    {
                        throw task.Exception;
                    }
                    else
                    {
                        _ads = task.Result;


                        if (maxpage == 1)
                        {
                            btnForward.Enabled = false;
                            btnBack.Enabled = false;
                        }
                        else
                        {
                            btnBack.Enabled = true;
                            btnForward.Enabled = true;

                            if (_currentpage == maxpage)
                            {
                                btnForward.Enabled = false;
                            }
                            else if (_currentpage == 1)
                            {
                                btnBack.Enabled = false;
                            }

                        }


                        lblPage.Text = _currentpage.ToString();
                        lblPageMax.Text = maxpage.ToString();
                    }


                }
                else
                {
                    throw new Exception("Connection Time-Out");
                }


                //Load data to grid
                DisplayToListView();

                stop_progress();
                lblStatus.Text = $"Last reload {DateTime.Now.ToShortTimeString()}";

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

        void start_progress()
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
            _currentpage = 1;
            Fetch(_currentpage, ResultPerPage);
        }

        private async void repositoryItemHyperLinkEdit1_Click(object sender, EventArgs e)
        {
            try
            {

                Cursor = Cursors.WaitCursor;

                var link = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Link").ToString();

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

                DialogResult dialogResult = MessageBox.Show("Call this seller using Skype.",
                    $"Skype call to ({phoneNumber})", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
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
                if (ex.Message.Equals("The system cannot find the file specified"))
                {
                    MessageBox.Show(ex.Message + "\nPlease make sure Skype is installed.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (ex.Message.Equals("Connection Time-Out"))
                {
                    MessageBox.Show(ex.Message + ". Please check your internet connection.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void gridView1_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                ColumnView view = sender as ColumnView;
                if (e.Column.FieldName == "Price" &&
                    e.ListSourceRowIndex != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
                {
                    var cultureInfo = CultureInfo.GetCultureInfo("nl-NL");
                    e.DisplayText = String.Format(cultureInfo, "{0:C2}", e.Value);

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
                Fetch(_currentpage, ResultPerPage);

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
            ResultPerPage = Convert.ToInt32(item.Caption);
            _currentpage = 1;
            Fetch(_currentpage, ResultPerPage);
        }

        private void dropdownpage_Click(object sender, EventArgs e)
        {
            dropdownpage.ShowDropDown();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Process.Start("Marktplaats Car Ads.pdf");
        }

        private void gridView1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            GridView view = sender as GridView;
            // Check whether a row is right-clicked.
            if (e.MenuType == DevExpress.XtraGrid.Views.Grid.GridMenuType.Row)
            {
                int rowHandle = e.HitInfo.RowHandle;

                DXMenuItem item = CreateMergingEnabledMenuItem(view, rowHandle);
                item.BeginGroup = false;
                e.Menu.Items.Add(item);

            }
        }

        // Create a check menu item that triggers the Boolean AllowCellMerge option.
        DXMenuItem CreateMergingEnabledMenuItem(GridView view, int rowHandle)
        {
            DXMenuItem item = new DXMenuItem("View in Browser", new EventHandler(onClick));


            return item;
        }

        //The handler for the DeleteRow menu item
        void onClick(object sender, EventArgs e)
        {
            var link = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Link").ToString();
            Process.Start(link);

        }

        private void toolTipController1_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl != grd) return;

            ToolTipControlInfo info = null;
            //Get the view at the current mouse position
            GridView view = grd.GetViewAt(e.ControlMousePosition) as GridView;
            if (view == null) return;
            //Get the view's element information that resides at the current position
            GridHitInfo hi = view.CalcHitInfo(e.ControlMousePosition);
            //Display a hint for row indicator cells
            if (hi.HitTest == GridHitTest.RowCell)
            {
                //An object that uniquely identifies a row indicator cell
                if(hi.Column.GetCaption().Equals("Price"))
                {
                    int price = Convert.ToInt32( gridView1.GetRowCellValue(hi.RowHandle, "Price"));

                    if (price == 0)
                    {
                        object o = hi.HitTest.ToString() + hi.RowHandle;
                        string priceDesc = gridView1.GetRowCellValue(hi.RowHandle, "PriceDesc").ToString();
                        info = new ToolTipControlInfo(o, priceDesc);
                    }
                }
            }
            //Supply tooltip information
            e.Info = info;
        }
    }
}
