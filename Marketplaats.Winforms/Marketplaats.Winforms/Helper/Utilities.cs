using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Marketplaats.Winforms.Properties;

namespace Marketplaats.Winforms.Helper
{
    public static class Utilities
    {
        public static string CheckForInternetConnection()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "www.google.com";
                byte[] buffer = new byte[32];
                int timeout = 2000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                if (reply.Status == IPStatus.TimedOut)
                {
                    return "TimeOut";
                }
                else
                {
                    return "Connected";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public static Font CurrentFont(int size)
        {
            Settings.Default.FontSize = size;
            Settings.Default.Save();
            return    new Font("Tahoma", size, FontStyle.Regular);
            
        }
    }
}
