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
            spotter.Data.IgnoreCase = ignorCase; //In some cases, it might be better to set it to false, and only add upper/lower-case exceptions as required
            foreach (var entry in entrys)
            {
                spotter.AddEntry(entry);
            }

            var nlp = Pipeline.TokenizerFor(Language.Bulgarian);
            nlp.Add(spotter); //When adding a spotter model, the model propagates any exceptions on tokenization to the pipeline's tokenizer

            var doc = new Document(document, Language.Bulgarian);

            nlp.ProcessSingle(doc);
            await nlp.StoreAsync();

            return doc;
        }
    }
}
