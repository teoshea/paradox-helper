using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ParadoxHelper
{
    public class ParadoxFileReader
    {
        private readonly Dictionary<int, ParadoxTableBlock> blocks = new Dictionary<int, ParadoxTableBlock>();

        public void Read(string fileName)
        {
            using (var fileStream = File.OpenRead(fileName))
            {
                using (var reader = new BinaryReader(fileStream, Encoding.Default))
                {
                    var header = new ParadoxFileHeader();
                    header.Read(reader);

                    ReadBlocks(header, reader);
                }
            }
        }

        private void ReadBlocks(ParadoxFileHeader header, BinaryReader reader)
        {
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);

            try
            {
                /*
                 * skip to the first block
                 */
                reader.ReadBytes(header.BlockSizeBytes);
                /*
                 * walk blocks
                 */
                int blocksInUse = header.BlocksInUse;
                for (int i = 0; i < blocksInUse; i++)
                {
                    /*
                     * block
                     */
                    var pdxTableBlock = new ParadoxTableBlock(i + 1, header.CalculateRecordsPerBlock(), header.Fields);

                    var streamPos = reader.BaseStream.Position;
                    /*
                     * read the block data
                     */
                    pdxTableBlock.Read(reader);
                    /*
                     * store it. blocks are numbered from 1, not from 0.
                     */
                    blocks.Add(pdxTableBlock.BlockNumber, pdxTableBlock);
                    /*
                     * reset to the start of the block
                     */
                    reader.BaseStream.Seek(streamPos, SeekOrigin.Begin);
                    /*
                     * skip ahead to next block
                     */
                    reader.ReadBytes(header.BlockSizeBytes);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in readBlocks", ex);
            }
        }

        public void Report(string outputFileName)
        {

            using (var fileStream = File.Open(outputFileName, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(fileStream))
                {
                    if (blocks.Any())
                    {
                        var firstBlock = blocks.First();
                        var fieldCount = firstBlock.Value.Fields.Count;

                        for (int fieldIndex = 0; fieldIndex < fieldCount; fieldIndex++)
                        {
                            var field = firstBlock.Value.Fields[fieldIndex];
                            var comma = fieldIndex < fieldCount - 1 ? "," : string.Empty;
                            var formatString = $"{{0, -{field.Length}}}" + comma;
                            writer.Write(formatString, field.Name);
                        }
                        writer.WriteLine();

                        foreach (var blockId in blocks.Keys)
                        {
                            var block = blocks[blockId];

                            foreach (var record in block.Records)
                            {
                                for (int valueIndex = 0; valueIndex < fieldCount; valueIndex++)
                                {
                                    var value = record.Values[valueIndex];
                                    var field = block.Fields[valueIndex];
                                    var comma = valueIndex < fieldCount - 1 ? "," : string.Empty;

                                    var formatString = field.FieldType == FieldType.A ? $"\"{{0, -{field.Length}}}\"" : $"{{0, -{field.Length}}}";
                                    formatString += comma;

                                    writer.Write(formatString, value.Value);
                                }
                                writer.WriteLine();
                            }
                        }
                    }
                }
            }
        }
    }
}