using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParadoxHelper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var reader = new ParadoxFileReader();

            var inFile = args[0];
            if (inFile == null)
            {
                Console.WriteLine("Usage: ParadoxHelper dbFileName");
                return;
            }

            var outFile = Path.ChangeExtension(inFile, "csv");

            reader.Read(inFile);
            reader.Report(outFile);
        }
    }
}
