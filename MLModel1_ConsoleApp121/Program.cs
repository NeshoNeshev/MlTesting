using Catalyst;
using Catalyst.Models;
using Mosaik.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Version = Mosaik.Core.Version;
using P = Catalyst.PatternUnitPrototype;
using System.Text;
using Microsoft.Extensions.Logging;
using MLModel1_ConsoleApp121;

Catalyst.Models.Bulgarian.Register();



Console.OutputEncoding = Encoding.UTF8; ApplicationLogging.SetLoggerFactory(LoggerFactory.Create(lb => lb.AddConsole()));

//// В момента Catalyst поддържа 3 различни типа модели за разпознаване на именувани обекти (NER):
//// - Подобно на Gazetteer (т.е. [Spotter](https://github.com/curiosity-ai/catalyst/blob/master/Catalyst/src/Models/EntityRecognition/Spotter.cs))
//// - Подобно на Regex(т.е. [PatternSpotter](https://github.com/curiosity-ai/catalyst/blob/master/Catalyst/src/Models/EntityRecognition/PatternSpotter.cs))
//// - Perceptron (т.е. [AveragePerceptronEntityRecognizer](https://github.com/curiosity-ai/catalyst/blob/master/Catalyst/src/Models/EntityRecognition/AveragePerceptronEntityRecognizer.cs))

Storage.Current = new DiskStorage("catalyst-models");

await EntityRecognition.AveragePerceptronEntityRecognizerAndPatternSpotterSample();

//var nlp = await Pipeline.ForAsync(Language.English);
//var isApattern = new PatternSpotter(Language.English, 0, tag: "is-a-pattern", captureTag: "IsA");
//isApattern.NewPattern(
//    "Is+Noun",
//    mp => mp.Add(
//        new PatternUnit(P.Single().WithToken("да").WithPOS(PartOfSpeech.VERB)),
//        new PatternUnit(P.Multiple().WithPOS(PartOfSpeech.NOUN, PartOfSpeech.PROPN, PartOfSpeech.AUX, PartOfSpeech.DET, PartOfSpeech.ADJ))
//));
//nlp.Add(isApattern);
//var doc = new Document(Data.Sample_2, Language.English);
//nlp.ProcessSingle(doc);

//nlp.ProcessSingle(doc);
//PrintDocumentEntities(doc);


//Catalyst.Models.English.Register(); //You need to pre-register each language (and install the respective NuGet Packages)

//Storage.Current = new DiskStorage("catalyst-models");

//var doc = new Document("The quick 100 brown fox jumps over the lazy dog", Language.English);
//nlp.ProcessSingle(doc);
//Console.WriteLine(doc.ToJson());

//var nlp = await Pipeline.ForAsync(Language.English);
//var doc2 = new Document(Data.SampleProgramming, Language.Bulgarian);
//nlp.Process(GetDocs());

//for (int i = 100; i < 1000; i++)
//{


//    var ft = new FastText(Language.Bulgarian, 0, "wiki-word2vec");
//    ft.Data.Type = FastText.ModelType.CBow;
//    ft.Data.Loss = FastText.LossType.NegativeSampling;
//    ft.Train(nlp.Process(EntityRecognition.GetDocs()));
//     ft.StoreAsync();
//}


//static IEnumerable<IDocument> GetDocs()
//{
//    yield return new Document(Data.Sample_1, Language.English);
//    yield return new Document(Data.Sample_2, Language.English);
//    yield return new Document(Data.Sample_3, Language.English);
//}


//PrintDocumentEntities(doc2);

//var docs = nlp.Process(MultipleDocuments());

////This will print all recognized entities. You can also see how the WikiNER model makes a mistake on recognizing Amazon as a location on Data.Sample_1

//foreach (var d in docs) { PrintDocumentEntities(d); }



static IEnumerable<IDocument> MultipleDocuments()
{
    yield return new Document(Data.Sample_2, Language.English);
    yield return new Document(Data.Sample_3, Language.English);
    yield return new Document(Data.Sample_4, Language.English);
}
void PrintDocumentEntities(IDocument doc)
{
    Console.WriteLine($"Input text:\n\t'{doc.Value}'\n\nTokenized Value:\n\t'{doc.TokenizedValue(mergeEntities: true)}'\n\nEntities: \n{string.Join("\n", doc.SelectMany(span => span.GetEntities()).Select(e => $"\t{e.Value} [{e.EntityType.Type}]"))}");
}