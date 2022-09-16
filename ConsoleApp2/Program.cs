using ConsoleApp2;
using Data;

var context = new ApplicationDbContext();
//ResetDatabase(context, shouldDropDatabase: true);





var parser = new HtmlParser(context);

var url = "C:\\Users\\nnesh\\source\\repos\\Web\\MlTesting\\ConsoleApp2\\pdfs\\act_2021_7_27..html";
parser.HtmlParse(url);

//string reshenie = "5001";
//string prisada = "5003";
//string sporazumenie = "5004";
//string url = "https://burgas-rs.justice.bg/bg/5335?from={0}&to={1}&actkindcode={2}&casenumber=&caseyear=&casetype=";



//await parser.JudjeParseAsync("01.07.2022", "29.07.2022", reshenie, url);


var test = context.Cases.Where(x=>x.TypeOfCase == "НАХД").Select(x=>x.Answer).Where(x=>x.ToLower().Contains("глоба"));
Console.WriteLine(test.Count());
//RemuveStopWords.RemuveCasesStopWords(context);
//var search = "предаване на стоката или на документите";
//var words = search?.Split(' ').Select(x => x.Trim())
//                .Where(x => !string.IsNullOrWhiteSpace(x) && x.Length >= 2).ToList();

//var result = context.Cases.Where(x=>x.Id != null);

//foreach (var word in words)
//{
//    result = context.Cases.Where(x => EF.Functions.FreeText(x.Content, word));
//    //// query = query.Where(x => x.SearchText.Contains(word));
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
