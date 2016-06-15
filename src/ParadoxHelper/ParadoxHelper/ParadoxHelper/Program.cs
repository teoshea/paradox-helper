using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var fileStream = File.OpenRead("Records.Db"))
            {
                using (var reader = new BinaryReader(fileStream, Encoding.Default))
                {
                    var header = new ParadoxFileHeader();
                    header.Read(reader);
                    header.Report();
                }
            }
        }
    }
}
