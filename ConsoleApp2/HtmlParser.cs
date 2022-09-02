﻿using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Data;
using HtmlAgilityPack;
using System.Text;

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
                        var judje = this.context1.Judjes.FirstOrDefault(x => x.Name.ToLower() == test[5].Trim().ToLower());
                        if (curent.Contains("html"))
                        {
                            text = HtmlParse(curent);

                            int index = text.IndexOf("РЕШИ:");
                            if (index != -1)
                            {
                                if (judje == null)
                                {
                                    judje = new Judje()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        Name = test[5].ToUpper()
                                    };
                                    await this.context1.Judjes.AddAsync(judje);
                                }

                                var content = text?.Substring(0, index).Trim();
                                var answer = text?.Substring(index).Trim();
                                var newCase = new Case()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    TypeOfCase = test[1],
                                    CaseNumber = test[2],
                                    TypeOfAct = test[6],
                                    Content = content,
                                    Answer = answer,
                                    JudjeId = judje.Id
                                };
                                counter = 0;
                                await this.context1.Cases.AddAsync(newCase);
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

                                if (judje == null)
                                {
                                    judje = new Judje()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        Name = test[5].ToUpper()
                                    };
                                    await this.context1.Judjes.AddAsync(judje);
                                }

                                var content = text?.Substring(0, index).Trim();
                                var answer = text?.Substring(index).Trim();
                                var newCase = new Case()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    TypeOfCase = test[1],
                                    CaseNumber = test[2],
                                    TypeOfAct = test[6],
                                    Content = content,
                                    Answer = answer,
                                    JudjeId = judje.Id,
                                    Decision = FindText(answer)
                                };

                                counter = 0;
                                await this.context1.Cases.AddAsync(newCase);
                                await this.context1.SaveChangesAsync();
                                File.Delete(curent);
                            }
                            else
                            {
                                counter = 0;
                                File.Delete(curent);
                            }
                        }
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
                
            }
            var text = PdfExtractor.ReplaceText(builder.ToString());
            return text;
        }
        private string FindText(string text)
        {
            var result = String.Empty;
            var array = text.Split(" ");

            if (text.Contains("ПРИЕМА ЗА УСТАНОВЕНО", StringComparison.OrdinalIgnoreCase))
            {
                result = "ПРИЕМА ЗА УСТАНОВЕНО";
            }
            else if (text.Contains("НАЛАГА мерки за защита", StringComparison.OrdinalIgnoreCase))
            {
                result = "НАЛАГА мерки за защита";
            }
            else if (text.Contains("ДОПУСКА РАЗВОД", StringComparison.OrdinalIgnoreCase))
            {
                result = "ДОПУСКА РАЗВОД";
            }
            else if (text.Contains("ОТХВЪРЛЯ иск", StringComparison.OrdinalIgnoreCase))
            {
                result = "ОТХВЪРЛЯ ИСКА";
            }
            else if (text.Contains("ПРЕДОСТАВЯ упражняването на родителските права", StringComparison.OrdinalIgnoreCase))
            {
                result = "ПРЕДОСТАВЯ упражняването на родителските права";
            }
            else if (text.Contains("ПРЕКРАТЯВА", StringComparison.OrdinalIgnoreCase))
            {
                result = "ПРЕКРАТЯВА";
            }
            else if (text.Contains("ОТМЕНЯ", StringComparison.OrdinalIgnoreCase))
            {
                result = "ОТМЕНЯ";
            }
            else if (text.Contains("ПОТВЪРЖДАВА", StringComparison.OrdinalIgnoreCase))
            {
                result = "ПОТВЪРЖДАВА";
            }
            else if (text.Contains("ИЗМЕНЯ", StringComparison.OrdinalIgnoreCase))
            {
                result = "ИЗМЕНЯ";
            }

            else if (text.Contains("ОСЪЖДА", StringComparison.OrdinalIgnoreCase))
            {
                result = "ОСЪЖДА";
            }
            else if (text.Contains("ОТХВЪРЛЯ", StringComparison.OrdinalIgnoreCase))
            {
                result = "ОТХВЪРЛЯ";
            }
            else if (text.Contains("ОТМЕНЯВА", StringComparison.OrdinalIgnoreCase))
            {
                result = "ОТМЕНЯ";
            }
            else if (text.Contains("УТВЪРЖДАВА", StringComparison.OrdinalIgnoreCase))
            {
                result = "УТВЪРЖДАВА";
            }
            else if (text.Contains("ПРИЗНАВА", StringComparison.OrdinalIgnoreCase))
            {
                result = "ПРИЗНАВА";
            }
            else if (text.Contains("ЗАДЪЛЖАВА", StringComparison.OrdinalIgnoreCase))
            {
                result = "ЗАДЪЛЖАВА";
            }
            else if (text.Contains("НАСТАНЯВА", StringComparison.OrdinalIgnoreCase))
            {
                result = "НАСТАНЯВА";
            }
            else if (text.Contains("ДОПУСКА", StringComparison.OrdinalIgnoreCase))
            {
                result = "ДОПУСКА";
            }
            else
            {
                result = "ПРОМЕНИ!!";
            }
            return result;
        }
    }
}
