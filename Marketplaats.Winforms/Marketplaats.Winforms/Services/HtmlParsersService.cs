﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

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

                    var link = child.Descendants("a");
                    var title = child
                        .Descendants()
                        .Single(n => n.GetAttributeValue("class", "")
                            .Equals("mp-listing-title"))
                        .InnerText;

                    var price = child.Descendants()
                        .Single(n => n.GetAttributeValue("class", "")
                            .Equals("price-new ellipsis"))
                        .InnerText
                        .Replace("&euro;&nbsp;", "€ ").Trim();

                    var built = child
                        .Descendants()
                        .FirstOrDefault(n => n.GetAttributeValue("class", "")
                            .Equals("mp-listing-attributes"));



                    if (built != null)
                    {
                        build = built.InnerText;
                    }


                    ads.Add(new Advertishments() { Type_ = title, Build = build, Price = price, PhoneNumber = "Make a call" });
                }
            }

            return ads;
        }

    }
}