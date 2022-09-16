using Catalyst;
using Catalyst.Models;
using Mosaik.Core;



namespace MLModel1_ConsoleApp121
{
    public static class SpotterExtension
    {
        public static async Task<Document> SpotterSampleDocument(string tag, string captureTag, string entry, string document, bool ignorCase)
        {
            var spotter = new Spotter(Language.Any, 0, tag, captureTag);
            spotter.Data.IgnoreCase = ignorCase; //In some cases, it might be better to set it to false, and only add upper/lower-case exceptions as required

            spotter.AddEntry(entry);

            var nlp = Pipeline.TokenizerFor(Language.Bulgarian);
            nlp.Add(spotter); //When adding a spotter model, the model propagates any exceptions on tokenization to the pipeline's tokenizer

            var doc = new Document(document, Language.Bulgarian);

            nlp.ProcessSingle(doc);
            await nlp.StoreAsync();

            return doc;
        }
        public static async Task<Document> SpotterMultipleEntrys(string tag, string captureTag, IEnumerable<string> entrys, string document, bool ignorCase)
        {
            var spotter = new Spotter(Language.Any, 0, tag, captureTag);
            spotter.Data.IgnoreCase = ignorCase;
            foreach (var entry in entrys)
            {
                spotter.AddEntry(entry);
            }

            var nlp = Pipeline.TokenizerFor(Language.Bulgarian);
            nlp.Add(spotter);

            var doc = new Document(document, Language.Bulgarian);

            nlp.ProcessSingle(doc);
            await nlp.StoreAsync();

            return doc;
        }
        public static async Task SaveSpotterPatternAsync(string tag, string captureTag, string fileName)
        {
            if (!File.Exists("ngvr.bin"))
            {
                var isApattern = new Spotter(Language.Bulgarian, 0, tag: tag, captureTag: captureTag);
                using (var f = File.OpenWrite($"{fileName}.bin"))
                {
                    await isApattern.StoreAsync(f);
                }
            }
            else
            {
                throw new InvalidOperationException("File not exist");
            }
        }
        public static async Task<Spotter> LoadSpotterPattern(string tag, string captureTag, string fileName)
        {
            Spotter isApattern2 = new Spotter(Language.Bulgarian, 0, tag: tag, captureTag: captureTag);

            using (var f = File.OpenRead($"{fileName}.bin"))
            {
                await isApattern2.LoadAsync(f);
            }

            return isApattern2;
        }

        public static async Task<Document> FindDocuments(string text,string document)
        {
            var entrys = text.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
            var spotter = new Spotter(Language.Any, 0, "newTag", "newCaptureTag");
            spotter.Data.IgnoreCase = true;
            foreach (var entry in entrys)
            {
                spotter.AddEntry(entry);
            }

            var nlp = Pipeline.TokenizerFor(Language.Bulgarian);
            nlp.Add(spotter);

            var doc = new Document(document, Language.Bulgarian);

            nlp.ProcessSingle(doc);
            //await nlp.StoreAsync();

            return doc;
        }
    }
}
