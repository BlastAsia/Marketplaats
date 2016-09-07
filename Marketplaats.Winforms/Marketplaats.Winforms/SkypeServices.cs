using SKYPE4COMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplaats.Winforms
{
    public class SkypeServices
    {
        public SkypeServices()
        {
            Skype skype;
            skype = new SKYPE4COMLib.Skype();
            string SkypeID = "jetganzon";
            Call call = skype.PlaceCall(SkypeID);
            
        }
    }
}
