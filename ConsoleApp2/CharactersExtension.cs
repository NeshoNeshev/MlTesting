using Data;
using System.Linq;
using System.Text;
using static iTextSharp.text.pdf.AcroFields;

namespace ConsoleApp2
{
    public static class CharactersExtension
    {
        
        public static string RemuveSpecialCharacters(string text)
        {
            //"(", ")"
            List<string> testWords = new List<string>() { ";", ":", "/", "-","_","." };
           
            var words = text.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
            StringBuilder builder = new StringBuilder();
            if (text.Contains("РЕШЕНИЕ", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("yes");
            }
            foreach (var word in words)
            {
                
                if (testWords.Contains(word))
                {
                    continue;
                }
             
                builder.Append(word);
                builder.Append(" ");
            }
            return builder.ToString();
        }
        public static string RemuveSpecialCharactersFroSingleWord(string word)
        {
            
            List<char> testChars = new List<char>() { '.', ',', '/', '?', '(', ')', ':', '-', '"', '№' };

            var chars = word.ToList();
            StringBuilder builder = new StringBuilder();
            foreach (var ch in testChars)
            {

                chars.Remove(ch);
                var myString = new string(chars.ToArray());
                builder.Append(myString);
                builder.Append(" ");
            }
            return builder.ToString();
        }
    }
}
