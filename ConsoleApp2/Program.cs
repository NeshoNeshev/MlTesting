﻿using ConsoleApp2;
using Data;
using HtmlAgilityPack;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;

var context = new ApplicationDbContext();
//ResetDatabase(context, shouldDropDatabase: true);


//Dictionary<string, int> wordCounts = new Dictionary<string, int>();
//var dbs = context.Cases.Select(x => x.Content).ToList();

//foreach (var item in dbs)
//{
//    var words = item.Split(" ").ToList();
//    foreach (var word in words)
//    {
//        if (!wordCounts.ContainsKey(word))
//        {
//            wordCounts.Add(word, 1);
//        }
//        else
//        {
//            wordCounts[word]++;
//        }
//    }
//}

//foreach (var kvp in wordCounts.OrderByDescending(x => x.Value).Where(x => x.Value > 50))
//{
//    //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
//    Console.WriteLine($"\"{kvp.Key}\","+$"value: {kvp.Value}");

//}

//var cases = new List<string>();
var parser = new HtmlParser(context);

//var url = "C:\\Users\\nnesh\\source\\repos\\Web\\MlTesting\\ConsoleApp2\\pdfs\\act_2021_4_13. (6).html";
//parser.HtmlParse(url);

//string reshenie = "5001";
//string prisada = "5003";
//string sporazumenie = "5004";
//string url = "https://burgas-rs.justice.bg/bg/5335?from={0}&to={1}&actkindcode={2}&casenumber=&caseyear=&casetype=";



//await parser.JudjeParseAsync("13.04.2021", "13.04.2021", reshenie, url);

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
