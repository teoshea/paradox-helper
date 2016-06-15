using System;
using System.IO;
using System.Text;

namespace ParadoxHelper
{
    public static class BinaryReaderExtensions
    {
        public static string ReadNullTerminatedString(this BinaryReader reader)
        {
            var stringBuilder = new StringBuilder();
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                var nextByte = reader.ReadByte();
                if (nextByte == 0x0)
                {
                    break;
                }

                stringBuilder.Append((char)nextByte);
            }

            return stringBuilder.ToString().Trim();
        }
    }
}