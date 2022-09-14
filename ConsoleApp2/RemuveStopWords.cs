using Data;
using System.Text;

namespace ConsoleApp2
{
    public static class RemuveStopWords
    {

        public static void RemuveCasesStopWords(ApplicationDbContext context)
        {
            Dictionary<string, int> wordCounts = new Dictionary<string, int>();
            var dbs = context.Cases.Select(x => x.Content).ToList();

            foreach (var item in dbs)
            {

                var words = item.Split(" ").ToList();
                foreach (var word in words)
                {
                    if (!wordCounts.ContainsKey(word))
                    {
                        wordCounts.Add(word, 1);
                    }
                    else
                    {
                        wordCounts[word]++;
                    }
                }
            }
            foreach (var item in wordCounts.OrderByDescending(x => x.Value).Where(x => x.Value > 40))
            {
                Console.WriteLine(item.Key + " " + item.Value);
            }
            var list = wordCounts.Where(x => x.Value > 0).Select(x => x.Key).ToList();

            var res = context.Cases.ToList();
            //var tr = res.Content.Split(" ").ToList();

            foreach (var item1 in res)
            {
                var builder = new StringBuilder();
                var caser = item1.Content.Split(" ").ToList();
                foreach (var item in list)
                {


                    caser.RemoveAll(x => x.Equals(item));


                }
                foreach (var item in caser)
                {
                    builder.Append(item);
                    builder.Append(" ");
                }
                item1.Content = builder.ToString();
                context.Cases.Update(item1);
                context.SaveChanges();
            }
        }
    }
}
