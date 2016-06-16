using System;
using System.Linq;
using System.Threading.Tasks;

namespace ParadoxHelper
{
    class Program
    {
        /*
                 * init the array
                 */
        


        static void Main(string[] args)
        {
           var reader = new ParadoxFileReader();
            reader.Read("RECORDS.DB");
            reader.Report("RECORDS.CSV");

            Console.ReadKey();
        }
    }
}
