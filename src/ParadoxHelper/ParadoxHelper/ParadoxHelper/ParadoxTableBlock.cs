using System;
using System.Collections.Generic;
using System.IO;

public class ParadoxTableBlock
{
    /**
     * header
     */
    public ParadoxTableBlockHeader Header { get; private set; }
    /**
     * block number
     */
    /**
     * fields
     */
    public List<TableField> Fields { get; }
    /**
     * records
     */
    public List<TableRecord> Records { get; private set; }
    /**
     * records per block
     */
    public int RecordsPerBlock { get; }

    public ParadoxTableBlock(int blockNumber, int recordsPerBlock, List<TableField> fields)
    {
        BlockNumber = blockNumber;
        RecordsPerBlock = recordsPerBlock;
        Fields = fields;
    }

    public int BlockNumber { get; private set; }
    
    /**
     * read data. This assumes that the inputStream is on byte 0 from the start of the block
     */
     
    public void Read(BinaryReader reader)
    {
        try
        {
            Records = new List<TableRecord>();

            ReadHeader(reader);

            for (int i = 0; i < RecordsPerBlock; i++)
            {
                var record = new TableRecord();
                record.Read(Fields, reader);
                Records.Add(record);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Exception in read", ex);
        }
    }

    private void ReadHeader(BinaryReader reader)
    {
        try
        {
            Header = new ParadoxTableBlockHeader();
            Header.Read(reader);
        }
        catch (Exception ex)
        {
            throw new Exception("Exception in readHeader", ex);
        }
    }
}
