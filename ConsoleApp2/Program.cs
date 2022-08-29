using ConsoleApp2;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter;
using UglyToad.PdfPig.DocumentLayoutAnalysis.ReadingOrderDetector;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;

var context = new ApplicationDbContext();
//ResetDatabase(context, shouldDropDatabase: true);
//var files = new DownloadFiles();
//var result = await files.Download();
//string criteria = "нормативни актове";  
//var asa = context.Cases.Where(x=> EF.Functions.Contains(x.Content, criteria)).ToList();
//var sdasd = ";";
//using (var document = PdfDocument.Open(@"C:\Users\nnesh\source\repos\Web\MlTesting\ConsoleApp2\act_2022_8_1. (1).pdf"))
//{
//    for (var i = 0; i < document.NumberOfPages; i++)
//    {
//        var page = document.GetPage(i + 1);

//        var words = page.GetWords(NearestNeighbourWordExtractor.Instance);

//        foreach (var word in words)
//        {
//            Console.WriteLine(word);
//        }
//    }
//}


//get leters and create string text
//using (PdfDocument document = PdfDocument.Open(@"C:\Users\nnesh\source\repos\Web\MlTesting\ConsoleApp2\2604-2022.pdf"))
//{
//    //var example = new List<string>();
//    foreach (Page page in document.GetPages())
//    {
//        IReadOnlyList<Letter> letters = page.Letters;

//        var example = string.Join(string.Empty, letters.Select(x => x.Value));

//        Console.WriteLine(example);
//        //Console.WriteLine("...........................................");
//        IEnumerable<Word> words = page.GetWords();
//    }
//    Console.WriteLine("hkjjjkhj");
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
//var wordss = new List<string>() { "УТВЪРЖДАВА", "ОСЪЖДА", "ОТМЕНЯ", "ПОТВЪРЖДАВА", "НАЛАГА", "ДОПУСКА РАЗВОД" };
//var sasa = "test.pdf";
//var filePath = @$"C:\Users\nnesh\source\repos\Web\MlTesting\ConsoleApp2\pdfs\{sasa}";
//using (var document = PdfDocument.Open(filePath))
//{
//    var builder = new StringBuilder();
//    var list = new List<string>();
//    for (var i = 0; i < document.NumberOfPages; i++)
//    {
//        var page = document.GetPage(i + 1);

//        var words = page.GetWords(NearestNeighbourWordExtractor.Instance);

//        foreach (var word in words)
//        {
//            if (String.IsNullOrWhiteSpace(word.ToString()))
//            {
//                continue;
//            }
//            list.Add(word.ToString());
//            builder.Append(word);
//            builder.Append(" ");
//        }
//    }
//    Console.WriteLine(builder);

//}


ResetDatabase(context, shouldDropDatabase: true);

//var cases = new List<string>();
var parser = new HtmlParser(context);
string reshenie = "5001";
string prisada = "5003";
string sporazumenie = "5004";
string url = "https://burgas-rs.justice.bg/bg/5335?from={0}&to={1}&actkindcode={2}&casenumber=&caseyear=&casetype=";

await parser.JudjeParseAsync("27.07.2021", "27.07.2021", reshenie, url);

 



//var allPdfBlocks = new List<string>();
//using (var document = PdfDocument.Open(@"C:\Users\nnesh\source\repos\Web\MlTesting\ConsoleApp2\2604-2022.pdf"))
//{
//    for (var i = 0; i < document.NumberOfPages; i++)
//    {
//        var page = document.GetPage(i + 1);

//        var words = page.GetWords(NearestNeighbourWordExtractor.Instance);
//        var blocks = DocstrumBoundingBoxes.Instance.GetBlocks(words);

//        var unsupervisedReadingOrderDetector = new UnsupervisedReadingOrderDetector(5);
//        var orderedBlocks = unsupervisedReadingOrderDetector.Get(blocks);

//        foreach (var block in orderedBlocks)
//        {
//            allPdfBlocks.Add(block.ToString());
//        }

//        foreach (var item in allPdfBlocks)
//        {
//            Console.WriteLine(item);
//            Console.WriteLine("------", 60);
//        }

//    }
//}

void ResetDatabase(ApplicationDbContext context, bool shouldDropDatabase = false)
{
    if (shouldDropDatabase)
    {
        context.Database.EnsureDeleted();
    }

    if (context.Database.EnsureCreated())
    {
        return;
    }

    
}