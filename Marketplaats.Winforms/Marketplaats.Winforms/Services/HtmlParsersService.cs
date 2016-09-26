using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Marketplaats.Winforms.Helper;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace Marketplaats.Winforms.Services
{
    public class HtmlParsersService
    {

        public HtmlParsersService()
        {
            
        }
        public List<Advertishments> StartParsing(int page,int resultperpage, ref int maxpage)
        {
            string link = "";
            try
            {

                var html = new HtmlDocument();
                var ads = new List<Advertishments>();

                var url = $"http://www.marktplaats.nl/z/auto-s.html?startDateFrom=today&categoryId=91&currentPage={page}&numberOfResultsPerPage={resultperpage}";
                html.LoadHtml(new WebClient().DownloadString(url)); 
                var section = html.DocumentNode.SelectNodes("//section[@class='search-results-table table']");

                maxpage = GetMaxPage(html);
                


                for (int i = 0; i < 2; i++)
                {


                    var sections = section.Descendants()
                                    .Where(n => n.GetAttributeValue("class", "")
                                    .Equals($"row search-result defaultSnippet group-{i} listing-aurora"));

                    foreach (var child in sections)
                    {
                        string build = "";

                        link = child.Attributes["data-url"].Value;
                            
                        var title = child
                                    .Descendants()
                                    .Single(n => n.GetAttributeValue("class", "")
                                    .Equals("mp-listing-title"))
                                    .InnerText;

                         string priceRaw = child.Descendants()
                                    .Single(n => n.GetAttributeValue("class", "")
                                    .Equals("price-new ellipsis"))
                                    .InnerText
                                    .Replace("&euro;&nbsp;", string.Empty)
                                    .Trim();

                        var price = priceRaw.ToDecimal();

                        string priceDesc = "";
                    if (price == 0)
                        {
                            priceDesc = child.Descendants()
                                    .Single(n => n.GetAttributeValue("class", "")
                                    .Equals("price-new ellipsis"))
                                    .InnerText
                                    .Replace("&euro;&nbsp;", string.Empty).Trim();
                        }

                        var built = child
                                    .Descendants()
                                    .FirstOrDefault(n => n.GetAttributeValue("class", "")
                                    .Equals("mp-listing-attributes"));

                    
                        if (built != null)
                        {
                            build = built.InnerText;
                        }


                        ads.Add(new Advertishments() { Type_ = title, Build = build, Price = price,
                                                        PhoneNumber = "Make a call" ,
                                                        PriceDesc = priceDesc, Link = link,
                                                        });
                    }
                }

            

                GC.Collect();
            
                return ads;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + link);
            }
        }

        public string GetPhoneNumber(string link)
        {

            try
            {
                var html = new HtmlDocument();
            
                html.LoadHtml(new WebClient().DownloadString(link));
                var section = html.DocumentNode.SelectNodes("//div[@class='phone-link alternative']");

                string number = "";
                if (section != null)
                {
                    number = section.Single().InnerText;
                }

                return number.ToSkypeFormat();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public int GetMaxPage(HtmlDocument html)
        {
            
            var pagination = html.DocumentNode.SelectNodes("//div[@class='pagination']");

            int number = 1;
            if (pagination != null)
            {
                var maxpage = pagination
                            .Descendants()
                            .Single(n => n.GetAttributeValue("class", "")
                            .Equals("last"))
                            .InnerText;

                return Convert.ToInt32(maxpage);
            }

            return number;
        }


    }
}
