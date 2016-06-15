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
    public List<TableField> Fields { get; private set; }
    /**
     * records
     */
    //private List<DBTableRecord> records;
    /**
     * records per block
     */
    public int RecordsPerBlock { get; private set; }

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
     /*
    public void read(PDXReaderListener pdxReaderListener, InputStream inputStream) throws Exception
    {
      try {
            records = new ArrayList<DBTableRecord>();
            
            final LittleEndianDataInputStream littleEndianDataInputStream = new LittleEndianDataInputStream(inputStream);
            readHeader(littleEndianDataInputStream);
           
            for (int i = 0; i < RecordsPerBlock; i++)
            {
                final DBTableRecord pdxTableRecord = new DBTableRecord();
                pdxTableRecord.read(pdxReaderListener, Fields, inputStream);
                records.add(pdxTableRecord);
            }
        } catch (final Exception e) {
            throw new Exception("Exception in read", e);
        }
    }
*/
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
