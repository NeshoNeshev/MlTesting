
using ConsoleApp2;
using System.Net;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter;
using UglyToad.PdfPig.DocumentLayoutAnalysis.ReadingOrderDetector;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;

//using (PdfDocument document = PdfDocument.Open(@"C:\Users\nnesh\source\repos\Web\MlTesting\ConsoleApp2\act_2022_8_1. (1).pdf"))
//{
//    foreach (Page page in document.GetPages())
//    {
//        IReadOnlyList<Letter> letters = page.Letters;

//        string example = string.Join(string.Empty, letters.Select(x => x.Value));
//        Console.WriteLine(example);
//        IEnumerable<Word> words = page.GetWords();
//    }
//}
//using (var document = PdfDocument.Open(@"C:\Users\nnesh\source\repos\Web\MlTesting\ConsoleApp2\act_2022_8_1. (1).pdf"))
//{
//    for (var i = 0; i < document.NumberOfPages; i++)
//    {
//        var page = document.GetPage(i + 1);

//        var words = page.GetWords(NearestNeighbourWordExtractor.Instance).ToArray();
//        var sb = new List<string>();
//        foreach (var word in words)
//        {
//            if (String.IsNullOrWhiteSpace(word.ToString()))
//            {
//                continue;
//            }
//            sb.Add(word.ToString());
//        }
//        Console.WriteLine(sb[0]);
//    }
//}

//using (var document = PdfDocument.Open(@"C:\Users\nnesh\source\repos\Web\MlTesting\ConsoleApp2\act_2022_8_1. (1).pdf"))
//{
//    string[] punctuation = new[] { ".", "," , " "};
//    string[] cannotStartWord = new[] { ")", "]", "!", "?", ";", ":" };
//    string[] cannotEndWord = new[] { "(", "[", };

//    for (var i = 0; i < document.NumberOfPages; i++)
//    {
//        var page = document.GetPage(i + 1);

//        var nnOptions = new NearestNeighbourWordExtractor.NearestNeighbourWordExtractorOptions()
//        {
//            // Ignore the letters that are space or belong to 'punctuation' array
//            // These letters will be put in a single word
//            FilterPivot = letter => !string.IsNullOrWhiteSpace(letter.Value) &&
//                !punctuation.Contains(letter.Value),

//            Filter = (pivot, candidate) =>
//            {
//                if (string.IsNullOrWhiteSpace(candidate.Value) ||
//                    cannotEndWord.Contains(candidate.Value))
//                {
//                    // start new word if the candidate neighbour is 
//                    // a space or belongs to 'cannotEndWord' array
//                    return false;
//                }
//                else if (cannotStartWord.Contains(pivot.Value))
//                {
//                    // end word if pivot belongs to 'cannotStartWord' array
//                    return false;
//                }
//                return true;
//            }
//        };

//        var nnWordExtracor = new NearestNeighbourWordExtractor(nnOptions);

//        var words = nnWordExtracor.GetWords(page.Letters);

//        Console.WriteLine(String.Join(" ", words));
//    }
//}


var cases = new List<string>();
var parser = new HtmlParser();



string reshenie = "5001";
string prisada = "5003";
string sporazumenie = "5004";
string url = "https://burgas-rs.justice.bg/bg/5335?from={0}&to={1}&actkindcode={2}&casenumber=&caseyear=&casetype=";

var result = await parser.JudjeParseAsync("10.08.2022","10.08.2022",reshenie, url);
var dir = @"C:\Users\nnesh\source\repos\Web\MlTesting\ConsoleApp2\pdfs\";
using (var client = new WebClient())
{
    for (int i = 0; i < result.Count; i++)
    {
        client.DownloadFile($"{result[i]}", dir + $"{i}.pdf");
	}
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "pdfs");
	
}

using (var document = PdfDocument.Open(@"C:\Users\nnesh\source\repos\Web\MlTesting\ConsoleApp2\1702-2022.pdf"))
{
	for (var i = 0; i < document.NumberOfPages; i++)
	{
		var page = document.GetPage(i + 1);

		var words = page.GetWords(NearestNeighbourWordExtractor.Instance);
		var blocks = DocstrumBoundingBoxes.Instance.GetBlocks(words);

		var unsupervisedReadingOrderDetector = new UnsupervisedReadingOrderDetector(5);
		var orderedBlocks = unsupervisedReadingOrderDetector.Get(blocks);
		var vount = 0;
		var all = new List<string>();
		foreach (var block in orderedBlocks)
		{
			all.Add(block.ToString());
		}

        foreach (var item in all)
        {
            Console.WriteLine(item);
            Console.WriteLine("------", 60);
        }
    }
}