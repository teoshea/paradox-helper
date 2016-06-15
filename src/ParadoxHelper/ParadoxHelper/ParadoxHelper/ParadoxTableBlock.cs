using System.Collections.Generic;

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

    public ParadoxTableBlockHeader getDbTableBlockHeader()
    {
        return DBTableBlockHeader;
    }
    
    public List<DBTableRecord> getRecords()
    {
        return records;
    }

    public int getRecordsPerBlock()
    {
        return RecordsPerBlock;
    }

    /**
     * read data. This assumes that the inputStream is on byte 0 from the start of the block
     */
    public void read(PDXReaderListener pdxReaderListener, InputStream inputStream) throws Exception
    {
      try {
            records = new ArrayList<DBTableRecord>();
            /*
             * read the header
             */
            final LittleEndianDataInputStream littleEndianDataInputStream = new LittleEndianDataInputStream(inputStream);
            readHeader(littleEndianDataInputStream);
            /*
             * read the records
             */
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

    /**
     * read header
     */
    private void readHeader(LittleEndianDataInputStream littleEndianDataInputStream) throws Exception
    {
      try {
            dbTableBlockHeader = new DBTableBlockHeader();
            dbTableBlockHeader.read(littleEndianDataInputStream);
        } catch (final Exception e) {
            throw new Exception("Exception in readHeader", e);
        }
    }

    public void setDbTableBlockHeader(DBTableBlockHeader dbTableBlockHeader)
    {
        this.dbTableBlockHeader = dbTableBlockHeader;
    }

    public void setPdxFileBlockHeader(DBTableBlockHeader pdxFileBlockHeader)
    {
        dbTableBlockHeader = pdxFileBlockHeader;
    }

    public void setRecords(List<DBTableRecord> records)
    {
        this.records = records;
    }
}

Status API Training Shop Blog About

    © 2016 GitHub, Inc.Terms Privacy Security Contact Help

