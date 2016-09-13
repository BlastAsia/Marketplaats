using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplaats.Winforms.Model
{

    /// <summary>
    /// Box.com user
    /// </summary>
    public class BoxUser
    {
        public string type { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string login { get; set; }
        public DateTime created_at { get; set; }
        public DateTime modified_at { get; set; }
        public string language { get; set; }
        public long space_amount { get; set; }
        public long space_used { get; set; }
        public long max_upload_size { get; set; }
        public string status { get; set; }
        public string job_title { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string avatar_url { get; set; }


    }
}
