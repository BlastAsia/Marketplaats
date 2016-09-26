using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

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
    }
}
