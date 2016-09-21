using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Marketplaats.Winforms.Helper;

namespace Marketplaats.Winforms.Services
{
    public class HtmlParsersService
    {

        public HtmlParsersService()
        {
            
        }
        public List<Advertishments> StartParsing()
        {

            var html = new HtmlDocument();
            var ads = new List<Advertishments>();

            html.LoadHtml(new WebClient().DownloadString("http://www.marktplaats.nl/z/auto-s.html?startDateFrom=today&categoryId=91")); // load a string
            var section = html.DocumentNode.SelectNodes("//section[@class='search-results-table table']");
            
            for (int i = 0; i < 1; i++)
            {


                var sections = section.Descendants()
                    .Where(n => n.GetAttributeValue("class", "")
                        .Equals($"row search-result defaultSnippet group-{i} listing-aurora"));

                foreach (var child in sections)
                {
                    string build = "";

                    var link = child.Attributes["data-url"].Value;
                    

                    var title = child
                                .Descendants()
                                .Single(n => n.GetAttributeValue("class", "")
                                .Equals("mp-listing-title"))
                                .InnerText;

                    var price = Convert.ToDouble(
                                 child.Descendants()
                                .Single(n => n.GetAttributeValue("class", "")
                                .Equals("price-new ellipsis"))
                                .InnerText
                                .Replace("&euro;&nbsp;", string.Empty).Trim()
                                .Replace(".", string.Empty).Replace(",", "."));

                    var built = child
                                .Descendants()
                                .FirstOrDefault(n => n.GetAttributeValue("class", "")
                                .Equals("mp-listing-attributes"));



                    if (built != null)
                    {
                        build = built.InnerText;
                    }


                    ads.Add(new Advertishments() { Type_ = title, Build = build,Price =price, PhoneNumber = "Make a call" ,Link = link});
                }
            }
            GC.Collect();
            
            return ads;
        }

        public string GetPhoneNumber(string link)
        {


            var html = new HtmlDocument();
            

            html.LoadHtml(new WebClient().DownloadString(link)); 
            var section = html.DocumentNode.SelectNodes("//div[@class='phone-link alternative']");

            string number = "";
            if (section != null)
            {
                number = section.Single().InnerText;
            }
            GC.Collect();
            return number.ToSkypeFormat();
        }

    }
}
