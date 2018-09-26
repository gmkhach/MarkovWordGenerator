using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkovText
{
    class Program
    {
        static void Main(string[] args)
        {
            var filepath = Filepath();

            // From https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/file-system/how-to-read-from-a-text-file
            string[] lines = File.ReadAllLines(filepath);

            var model = new MarkovModel();
            model.AddWords(lines);

            string input;

            double real = 0;
            double total = 0;

            do
            {
                string word = model.GenerateWord(3, 10);
                bool realWord = lines.Contains(word);

                Console.WriteLine(word + (realWord ? " (real)" : ""));

                input = Console.ReadLine();

                total++;
                real += realWord ? 1 : 0;
            } while (input == "");

            Console.WriteLine($"Percentage real: {real / total}");
        }

        static string Filepath()
        {
            // The path three directories above the executable directory in VS
            // From https://stackoverflow.com/a/14549805
            string wanted_path =
                Path.GetDirectoryName(Path.GetDirectoryName(
                    Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));

            string filepath = $"{wanted_path}\\words_alpha.txt";

            return filepath;
        }
    }
}