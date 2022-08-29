
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Data;
using HtmlAgilityPack;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace ConsoleApp2
{
    public class HtmlParser
    {
        private readonly ApplicationDbContext context1;
        public HtmlParser(ApplicationDbContext context1)
        {
            this.context1 = context1;
        }
        public async Task<List<string>> GetHrefAsync(string url)
        {
            List<string> cases = new List<string>();
            string html = "";

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        html = await content.ReadAsStringAsync();
                    }
                }
            }
            using var context = BrowsingContext.New(Configuration.Default);
            using var doc = await context.OpenAsync(req => req.Content(html));
            
            var anchors = doc.GetElementsByTagName("a");
           
            var urls = new List<string>();
            for (var i = 0; i < anchors.Length; i++)
            {     
                var anchor = anchors[i] as IHtmlAnchorElement;
                
                if (anchor.Href != null && anchor.Href.Contains("https://portalextensions"))
                {
                    var right = anchor.TextContent;
                    var motiv = "Мотиви";
                    if (right.ToLower() != motiv.ToLower())
                    {
                        Console.WriteLine(anchor.TextContent);
                        urls.Add(anchor.Href);
                    }                  
                }
            }
            return urls;
        }
        public async Task JudjeParseAsync(string startDate, string endDate, string code, string urlAddress)
        {
            var files = new DownloadFiles();
            var url = String.Format(urlAddress, startDate, endDate, code);
            var result = await files.Download(url);
           
            string html = "";
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        html = await content.ReadAsStringAsync();
                    }
                }
            }
            using var context = BrowsingContext.New(Configuration.Default);
            using var doc = await context.OpenAsync(req => req.Content(html));
            var td = doc.QuerySelectorAll("td");
            byte counter = 0;
            var test = new List<string>();

            foreach (var item in td)
            {
                counter++;
                test.Add(item.Text().Trim());

                if (counter == 10)
                {
                    if (!test[8].Contains("Свали"))
                    {

                        test = new List<string>();
                        counter = 0;
                    }
                    else
                    {
                        var curent = result.Dequeue();
                        var text = String.Empty;
                        if (curent.Contains("html"))
                        {
                            text = HtmlParse(curent);

                            int index = text.IndexOf("РЕШИ:");
                            if (index != -1)
                            {
                                var newCase = new Case()
                                {
                                    Id = Guid.NewGuid().ToString(),

                                    TypeOfCase = test[1],
                                    CaseNumber = test[2],
                                    TypeOfAct = test[6],
                                    Content = text?.Substring(0, index).Trim(),
                                    Answer = text?.Substring(index).Trim()
                                };
                                counter = 0;
                                this.context1.Cases.Add(newCase);
                                await this.context1.SaveChangesAsync();
                                File.Delete(curent);
                            }
                            else
                            {
                                counter = 0;
                                File.Delete(curent);
                            }
                            
                        }
                        else
                        {
                            text = PdfExtractor.GetFullText(curent);

                            int index = text.IndexOf("РЕШИ:");
                            if (index != -1)
                            {
                                var newCase = new Case()
                                {
                                    Id = Guid.NewGuid().ToString(),

                                    TypeOfCase = test[1],
                                    CaseNumber = test[2],
                                    TypeOfAct = test[6],
                                    Content = text?.Substring(0, index).Trim(),
                                    Answer = text?.Substring(index).Trim()
                                };
                                //йкхйк
                                counter = 0;
                                this.context1.Cases.Add(newCase);
                                await this.context1.SaveChangesAsync();
                                File.Delete(curent);
                            }
                            else
                            {
                                counter = 0;
                                File.Delete(curent);
                            }
                           
                        }
                        //var existJudje = test[5];
                    }
                    test = new List<string>();
                }
                else
                {
                    continue;
                }
            }
        }

        public string HtmlParse(string url)
        {
            var builder = new StringBuilder();
            HtmlDocument document = new HtmlDocument();
            string htmlCode = "";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding("windows-1251");
            document.Load(url, encoding);
            foreach (HtmlNode paragraph in document.DocumentNode.SelectNodes("//p"))
            {
                htmlCode = paragraph.InnerText;
               
                builder.Append(htmlCode.Trim());
                Console.WriteLine(htmlCode);
            }
            var text = PdfExtractor.ReplaceText(builder.ToString());
            return text;
        }      
    }
}
