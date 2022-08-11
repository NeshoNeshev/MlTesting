
using AngleSharp;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace ConsoleApp2
{
    public class HtmlParser 
    {
        
        public HtmlParser()
        {
            
        }
        public async Task<List<string>> JudjeParseAsync(string startDate, string endDate, string code, string urlAddress)
        {

            List<string> cases = new List<string>();
            
            var url = String.Format(urlAddress,startDate,endDate,code);
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
                    urls.Add(anchor.Href);
                }
                
            }

            return urls;
        }
    }
}
