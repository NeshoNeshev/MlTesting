namespace ConsoleApp2
{
    using Microsoft.Extensions.Primitives;
    using System.Text;
    using System.Text.RegularExpressions;
    using UglyToad.PdfPig;
    using UglyToad.PdfPig.Content;
    using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;

    public static class PdfExtractor
    {

        public static string GetFullText(string url)
        {

            var example = String.Empty;
            var builder = new StringBuilder();
            using (var document = PdfDocument.Open(url))
            {
                for (var i = 0; i < document.NumberOfPages; i++)
                {
                    var page = document.GetPage(i + 1);

                    var words = page.GetWords(NearestNeighbourWordExtractor.Instance);

                    foreach (var word in words)
                    {
                        builder.Append(word.ToString().Trim());
                        builder.Append(' ');
                    }
                }
            }
           
            example = builder.ToString();
            example = ReplaceText(example);


            return example;

        }
        public static string ReplaceText(string text)
        {
       
            var result = text;
            result = Regex.Replace(result, @"[&nbsp]", " ");
            result = Regex.Replace(result, "[\\r\\n]", " ");
            result = Regex.Replace(result, @"[{ЕГН{IsCyrillic}]+[ ]+[*]{2,}", " ");
            result = Regex.Replace(result, @"[Е{IsCyrillic}][Г{IsCyrillic}][Н{IsCyrillic}]", " ");
            result = Regex.Replace(result, @"[{А-Я}{IsCyrillic}]+[.]+[ ][{А-Я}{IsCyrillic}]+[.]+[ ][{А-Я}{IsCyrillic}]+[.]", " ");
            result = Regex.Replace(result, @"[.]{2,}", " ");
            result = Regex.Replace(result, @"[*]{2,}", " ");
            result = Regex.Replace(result, @"[_]{2,}", " ");
            result = Regex.Replace(result, @"[ ]{2,}", " ");
            //result = Regex.Replace(result, @"[Р]+[ ]+[Е]+[ ]+[Ш]+[ ]+[И]+[ ]+[:]", "РЕШИ:");
            return result;
        }
    }

}
