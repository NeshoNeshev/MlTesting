using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using HtmlAgilityPack;
using System.Net;
using System.Text;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

var leters = new List<string>() { "а" };
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
var parser = new HtmlParser();
var webClient = new WebClient { Encoding = Encoding.GetEncoding("windows-1251") };
foreach (var item in leters)
{
    var url = $"https://slovored.com/search/pravopisen-rechnik/{item}";
    string html = null;
    html = webClient.DownloadString(url);

    var document = parser.ParseDocument(html);
    var anchors = document.GetElementsByTagName("a");
    for (var i = 0; i < anchors.Length; i++)
    {
        var text = anchors[i].TextContent;
    }
}

//var lines = new StringBuilder();
//foreach (var item in td)
//{
//    var text = item.TextContent;
//    lines.AppendLine(text);
//}
//await File.WriteAllTextAsync("WriteText.txt", lines.ToString());
//Console.WriteLine("read");