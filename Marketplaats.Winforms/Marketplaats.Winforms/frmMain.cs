using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Marketplaats.Winforms
{
    public partial class frmMain : Form
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
        }

        private void DisplayToListView()
        {

           
            var cultureInfo = CultureInfo.GetCultureInfo("da-DK");
            int i = 1;
            listView.Items.Clear();
            List<Advertishments> ads = new List<Advertishments>()
            {
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
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SUV",Build="2016",Price =10000.00,PhoneNumber="09987900777" },
                new Advertishments() { Type_="SEDAN",Build="2016",Price =20000.00,PhoneNumber="09987900617" }

            };

            foreach (var ad in ads)
            {
                ListViewItem item = new ListViewItem(ad.Type_);
                item.SubItems.Add(ad.Build);
                item.SubItems.Add(String.Format(cultureInfo, "{0:C}", ad.Price));
                item.SubItems.Add(ad.PhoneNumber);
                listView.Items.Add(item);

                item.UseItemStyleForSubItems = false;
                ChangeToUnderline(ref listView, 3, Color.Blue);
            }

           

        }
        private void ChangeToUnderline(ref ListView lv, int ColumnIndex, Color color)
        {
            foreach (ListViewItem lvi in lv.Items)
            {
                lvi.SubItems[ColumnIndex].Font = new Font("Microsoft Sans Serif", 8, FontStyle.Underline);
                lvi.SubItems[ColumnIndex].ForeColor = color;
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

        //protected override void WndProc(ref Message message)
        //{
        //    const int WM_PAINT = 0xf;

        //    // if the control is in details view mode and columns
        //    // have been added, then intercept the WM_PAINT message
        //    // and reset the last column width to fill the list view
        //    switch (message.Msg)
        //    {
        //        case WM_PAINT:
        //            if (listView.View == View.Details && listView.Columns.Count > 0)
        //                listView.Columns[listView.Columns.Count - 1].Width = -2;
        //            break;
        //    }

        //    // pass messages on to the base control for processing
        //    base.WndProc(ref message);
        //}

        private void listView_MouseMove(object sender, MouseEventArgs e)
        {
            var hit = listView.HitTest(e.Location);
            if (hit.SubItem != null && hit.SubItem == hit.Item.SubItems[3]) listView.Cursor = Cursors.Hand;
            else listView.Cursor = Cursors.Default;
        }

        private void listView_MouseUp(object sender, MouseEventArgs e)
        {
            var hit = listView.HitTest(e.Location);
            if (hit.SubItem != null && hit.SubItem == hit.Item.SubItems[3])
            {
                var phone = hit.SubItem.Text;
                MessageBox.Show("Do you want to call this seller using Skype","Skype Call",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                // etc..
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Fetch();
        }
    }
}
