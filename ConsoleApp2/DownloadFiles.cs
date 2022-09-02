using Data;
using System.Net;

namespace ConsoleApp2
{
    public class DownloadFiles
    {
        public DownloadFiles()
        {

        }
        public async Task<Queue<string>> Download(string url)
        {
            var context = new ApplicationDbContext();
            var cases = new List<string>();
            var parser = new HtmlParser(context);

            var result = await parser.GetHrefAsync(url);
            var dir = @"C:\Users\nnesh\source\repos\Web\MlTesting\ConsoleApp2\pdfs\";
            var directories = new List<string>();
            var queue = new Queue<string>();

            using (var client = new WebClient())
            {
                for (int i = 0; i < result.Count; i++)
                {
                    byte[] fileBytes = client.DownloadData(result[i]);
                    string fileType = client.ResponseHeaders[HttpResponseHeader.ContentType];
                    if (fileType == null)
                    {
                        continue;
                    }
                    if (fileType.Contains("html"))
                    {
                        client.DownloadFile($"{result[i]}", dir + $"{i}.html");
                        queue.Enqueue(dir + $"{i}.html");
                       
                    }
                    else
                    {
                        client.DownloadFile($"{result[i]}", dir + $"{i}.pdf");
                        queue.Enqueue(dir + $"{i}.pdf");
                       
                    }                  
                } 
            }
            return queue;
        }
    }
}
