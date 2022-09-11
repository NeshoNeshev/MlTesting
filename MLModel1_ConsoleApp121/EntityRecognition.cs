using Catalyst;
using Catalyst.Models;
using Mosaik.Core;
using Version = Mosaik.Core.Version;
using P = Catalyst.PatternUnitPrototype;

namespace MLModel1_ConsoleApp121
{

    public static class EntityRecognition
    {

        public static async Task AveragePerceptronEntityRecognizerAndPatternSpotterSample()
        {
            Catalyst.Models.Bulgarian.Register();
            Catalyst.Models.English.Register(); //You need to pre-register each language (and install the respective NuGet Packages)

            // За обучение на AveragePerceptronModel проверете изходния код тук: https://github.com/curiosity-ai/catalyst/blob/master/Catalyst.Training/src/TrainWikiNER.cs
            // Този пример използва предварително обучен модел WikiNER, обучен върху данните, предоставени от статията „Изучаване на многоезично разпознаване на имена от Wikipedia“, Изкуствен интелект 194 (DOI: 10.1016/j.artint.2012.03.006)
            // Данните за обучение са получени от следното хранилище: https://github.com/dice-group/FOX/tree/master/input/Wikiner

            //Конфигурира хранилището на модела да използва онлайн хранилището, поддържано от локалната папка ./catalyst-models/

            //Create a new pipeline for the english language, and add the WikiNER model to it
            Console.WriteLine("Loading models... This might take a bit longer the first time you run this sample, as the models have to be downloaded from the online repository");
            var nlp = await Pipeline.ForAsync(Language.English);
            nlp.Add(await AveragePerceptronEntityRecognizer.FromStoreAsync(language: Language.English, version: Version.Latest,tag: "WikiNER"));

            //Друг наличен модел за NER е PatternSpotter, който е концептуалният еквивалент на RegEx върху необработен текст, но работи върху токенизираната форма извън текста.
            //Добавя персонализиран модел за наблюдение на шаблона: единичен("е" / глагол) + множествен(СЪЩ./AUX/PROPN/AUX/DET/ADJ)
            var isApattern = new PatternSpotter(Language.English, 0, tag: "is-a-pattern", captureTag: "IsA");
            isApattern.NewPattern(
                "Is+Noun",
                mp => mp.Add(
                    new PatternUnit(P.Single().WithToken("is").WithPOS(PartOfSpeech.VERB)),
                    new PatternUnit(P.Multiple().WithPOS(PartOfSpeech.NOUN, PartOfSpeech.PROPN, PartOfSpeech.AUX, PartOfSpeech.DET, PartOfSpeech.ADJ))
            ));
            nlp.Add(isApattern);


            //За обработка на един документ можете да извикате nlp.ProcessSingle
            var doc = new Document(Data.Sample_1, Language.English);
            nlp.ProcessSingle(doc);

            //За паралелна обработка на множество документи (т.е. многопоточност), можете да извикате nlp.Process на IEnumerable<IDocument> enumerable
            var docs = nlp.Process(MultipleDocuments());

            //Това ще отпечата всички разпознати обекти. Можете също да видите как моделът WikiNER прави грешка при разпознаването на Amazon като местоположение в Data.Sample_1
            PrintDocumentEntities(doc);
            foreach (var d in docs) { PrintDocumentEntities(d); }

            //За коригиране на грешки при разпознаване на обекти можете да използвате класа Neuralyzer.
            //Този клас използва класа за разпознаване на обекти за съвпадение на шаблони, за да извърши "забравяне на обект" и "добавяне на обект"
            //предава документа, след като е бил обработен от всички други процеси в NLP конвейера
            var neuralizer = new Neuralyzer(Language.English, 0, "WikiNER-sample-fixes");

            //Научете класа Neuralyzer да забрави съвпадението за един токен „Amazon“ с тип обект „Местоположение“
            neuralizer.TeachForgetPattern("Location", "Amazon", mp => mp.Add(new PatternUnit(P.Single().WithToken("Amazon").WithEntityType("Location"))));

            //Научете класа Neuralyzer да добавя типа организация за съвпадение за единичния токен „Amazon“
            neuralizer.TeachAddPattern("Organization", "Amazon", mp => mp.Add(new PatternUnit(P.Single().WithToken("Amazon"))));

            //Add the Neuralyzer to the pipeline
            nlp.UseNeuralyzer(neuralizer);

            //Сега можете да видите, че „Amazon“ е правилно разпознат като тип обект „Организация“
            var doc2 = new Document(Data.Sample_1, Language.English);
            nlp.ProcessSingle(doc2);
            PrintDocumentEntities(doc2);
        }
        public static void SpotterSample()
        {
            Catalyst.Models.Bulgarian.Register();
            Catalyst.Models.English.Register(); //You need to pre-register each language (and install the respective NuGet Packages)
            //Друг начин за извършване на разпознаване на обект е да се използва модел, подобен на справочник. Например, ето един за улавяне на набор от езици за програмиране
            var spotter = new Spotter(Language.Any, 0, "Numbers", "number");
            spotter.Data.IgnoreCase = true; //В някои случаи може да е по - добре да го зададете на false и да добавяте изключения само за главни / малки букви, ако е необходимо

            //spotter.AddEntry("C#");
            //spotter.AddEntry("Python");
            spotter.AddEntry("1999.23"); //записите могат да имат повече от една дума и ще бъдат автоматично токенизирани на интервал
            spotter.AddEntry("155.55");
            spotter.AddEntry("123.00");
            spotter.AddEntry("455.52");

            var nlp = Pipeline.TokenizerFor(Language.Bulgarian);
            nlp.Add(spotter); //При добавяне на модел за наблюдение, моделът разпространява всички изключения при токенизиране към токенизатора на конвейера
            string asdas = "Берлин е столицата и най-големият град на Германия както по площ, така и";
            var docAboutProgramming = new Document(Data.SampleProgramming, Language.Bulgarian);

            nlp.ProcessSingle(docAboutProgramming);
            

            PrintDocumentEntities(docAboutProgramming);
        }

        static void PrintDocumentEntities(IDocument doc)
        {
            Console.WriteLine($"Input text:\n\t'{doc.Value}'\n\nTokenized Value:\n\t'{doc.TokenizedValue(mergeEntities: true)}'\n\nEntities: \n{string.Join("\n", doc.SelectMany(span => span.GetEntities()).Select(e => $"\t{e.Value} [{e.EntityType.Type}]"))}");
        }

        static IEnumerable<IDocument> MultipleDocuments()
        {
            yield return new Document(Data.Sample_2, Language.English);
            yield return new Document(Data.Sample_3, Language.English);
            yield return new Document(Data.Sample_4, Language.English);
        }
        public static IEnumerable<IDocument> GetDocs()
        {
            yield return new Document(Data.Sample_6, Language.Bulgarian);
            yield return new Document(Data.Sample_7, Language.Bulgarian);
            yield return new Document(Data.Sample_8, Language.Bulgarian);
            yield return new Document(Data.Sample_9, Language.Bulgarian); ;
        }
    }
}
