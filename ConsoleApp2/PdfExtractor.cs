namespace ConsoleApp2
{
    using Microsoft.Extensions.Primitives;
    using Nest;
    using System.Text;
    using System.Text.RegularExpressions;
    using UglyToad.PdfPig;
    using UglyToad.PdfPig.Content;
    using UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter;
    using UglyToad.PdfPig.DocumentLayoutAnalysis.ReadingOrderDetector;
    using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;
    using Page = UglyToad.PdfPig.Content.Page;

    public static class PdfExtractor
    {
        public static List<string> GetPdfBloks(string url)
        {
            var allPdfBlocks = new List<string>();
            using (var document = PdfDocument.Open(url))
            {
                for (var i = 0; i < document.NumberOfPages; i++)
                {
                    var page = document.GetPage(i + 1);

                    var words = page.GetWords(NearestNeighbourWordExtractor.Instance);
                    var blocks = DocstrumBoundingBoxes.Instance.GetBlocks(words);

                    var unsupervisedReadingOrderDetector = new UnsupervisedReadingOrderDetector(5);
                    var orderedBlocks = unsupervisedReadingOrderDetector.Get(blocks);

                    foreach (var block in orderedBlocks)
                    {
                        allPdfBlocks.Add(block.ToString());
                    }
                }
            }
            return allPdfBlocks;
        }

        //get leters and create string text
        public static string ExtractLetters(string url)
        {
            var example = String.Empty;
            using (PdfDocument document = PdfDocument.Open(url))
            {
                //var example = new List<string>();
                foreach (Page page in document.GetPages())
                {
                    IReadOnlyList<Letter> letters = page.Letters;

                    example = string.Join(string.Empty, letters.Select(x => x.Value));

        
                }
            }
            return example;

        }
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
            //example = ReplaceText(example);


            return example;

        }
        public static string RemuveHtmlNewLine(string text)
        {
            var result = text;
            result = Regex.Replace(result, @"[&nbsp]", " ");
            result = Regex.Replace(result, "[\\r\\n]", " ");
            result = Regex.Replace(result, @"[рР][  ]{1,}[eЕ][  ]{1,}[шШ][  ]{1,}[иИ]", "РЕШИ");
            result = Regex.Replace(result, @"[ ]{2,}", " ");
            result = Regex.Replace(result, @"[   ]{2,}", " ");
            return result;
        }
        public static string ReplaceText(string text)
        {
       
            var result = text;
            result = Regex.Replace(result, @"[Р][ ]{0,}[Е][ ]{0,}[Ш][ ]{0,}[Е][ ]{0,}[Н][ ]{0,}[И][ ]{0,}[Е][ ]{0,}", " ");
            result = Regex.Replace(result, @"[0-3][0-9][.][0-1][0-9][.][0-9]{4}", " ");
            result = Regex.Replace(result, @"[№][ ]{1,}[0-9]{1,}", " ");
            result = Regex.Replace(result, @"[{ЕГН{IsCyrillic}]+[ ]+[*]{2,}", " ");
            result = Regex.Replace(result, @"[Е{IsCyrillic}][Г{IsCyrillic}][Н{IsCyrillic}]", " ");
            result = Regex.Replace(result, @"[{А-Я}{IsCyrillic}]+[.]+[ ][{А-Я}{IsCyrillic}]+[.]+[ ][{А-Я}{IsCyrillic}]+[.]", " ");
            result = Regex.Replace(result, @"[{А-Я}{IsCyrillic}][.][{А-Я}{IsCyrillic}][.][{А-Я}{IsCyrillic}][.]", " ");
            result = Regex.Replace(result, @"[.]{2,}", " ");
            result = Regex.Replace(result, @"[*]{2,}", " ");
            result = Regex.Replace(result, @"[_]{2,}", " ");
            result = Regex.Replace(result, @"[ ]{2,}", " ");
            result = Regex.Replace(result, @"[   ]{2,}", " ");
            result = Regex.Replace(result, @"[,]+[ ]+[,]", " ");
            
            //result = Regex.Replace(result, @"[Р]+[ ]+[Е]+[ ]+[Ш]+[ ]+[И]+[ ]+[:]", "РЕШИ:");
            return result;
        }
    }

}
