
using System;
using System.Collections.Generic;
using System.IO;
using ParadoxHelper;

public enum BlockSize
{
    OneK = 1,
    TwoK = 2,
    ThreeK = 3,
    FourK = 4
}

public enum TableType
{
    Keyed = 0,
    Unkeyed = 2
}

public class ParadoxFileHeader
{
    private BlockSize blockSize;

    public int BlockSizeBytes => (int)blockSize * 1024;

    public TableType TableType { get; private set; }

    public int RecordBufferSize { get; private set; }

    public int HeaderBlockSize { get; private set; }

    public long NumberOfRecords { get; private set; }

    public int NumberOfFields { get; private set; }

    public int NumberOfKeyFields { get; private set; }

    public int DataBlockSizeCode { get; private set; }

    public int BlocksInUse { get; private set; }

    public int TotalBlocksInFile { get; private set; }

    public int FirstDataBlock { get; private set; }

    public int LastDataBlock { get; private set; }

    public int FirstFreeBlock { get; private set; }

    public string EmbeddedFilename { get; private set; }
    /**
     * fields
     */
    public List<TableField> Fields { get; private set; }

    /**
     * figure out the total records in a block
     */

    public int CalculateRecordsPerBlock()
    {
        return BlockSizeBytes / RecordBufferSize;
    }

    public void Read(BinaryReader inputStream)
    {
        try
        {
            RecordBufferSize = inputStream.ReadUInt16();
            HeaderBlockSize = inputStream.ReadUInt16();

            var tableType = inputStream.ReadByte();
            if (tableType == 0)
            {
                TableType = TableType.Keyed;
            }
            else if (tableType == 2)
            {
                TableType = TableType.Unkeyed;
            }
            else
            {
                throw new Exception("Unknown table type '" + tableType + "'");
            }

            var dataBlockSizeCode = inputStream.ReadByte();
            if (dataBlockSizeCode == 1)
            {
                blockSize = BlockSize.OneK;
            }
            else if (dataBlockSizeCode == 2)
            {
                blockSize = BlockSize.TwoK;
            }
            else if (dataBlockSizeCode == 3)
            {
                blockSize = BlockSize.ThreeK;
            }
            else if (dataBlockSizeCode == 4)
            {
                blockSize = BlockSize.FourK;
            }
            else
            {
                throw new Exception("Unknown block size code '" + dataBlockSizeCode + "'");
            }

            NumberOfRecords = inputStream.ReadInt32();
            BlocksInUse = inputStream.ReadUInt16();
            TotalBlocksInFile = inputStream.ReadUInt16();
            FirstDataBlock = inputStream.ReadUInt16();
            LastDataBlock = inputStream.ReadUInt16();
            inputStream.ReadBytes(15);
            // byte 0x21
            NumberOfFields = inputStream.ReadByte();
            // byte 0x22
            inputStream.ReadByte();
            // byte 0x23
            NumberOfKeyFields = inputStream.ReadByte();
            inputStream.ReadBytes(0x34);
            // byte 0x58
            ReadFieldTypesAndSizes(inputStream);

            var toSkip = 0x53 + 4 * NumberOfFields;

            // name
            for (int i = 0; i < toSkip; i++)
            {
                var c = inputStream.ReadByte();
            }
            //EmbeddedFilename = inputStream.ReadNullTerminatedString();

            ReadFieldNames(inputStream);
        }
        catch (Exception ex)
        {
            throw new Exception("Exception in read", ex);
        }
    }

    private void ReadFieldNames(BinaryReader inputStream)
    {
        try
        {
            foreach (var pdxTableField in Fields)
            {
                pdxTableField.ReadFieldName(inputStream);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Exception in readFields", ex);
        }
    }

    /**
 * read the field descriptions
 */

    private void ReadFieldTypesAndSizes(BinaryReader inputStream)
    {
        try
        {
            Fields = new List<TableField>();
            for (int i = 0; i < NumberOfFields; i++)
            {
                var pdxTableField = new TableField();
                pdxTableField.ReadFieldTypeAndSize(inputStream);
                Fields.Add(pdxTableField);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Exception in readFields", ex);
        }
    }

    public void Report()
    {
        foreach (var pdxTableField in Fields)
        {
            Console.WriteLine("Field '" + pdxTableField.Name + "' type '" + pdxTableField.FieldType + "'");
        }
    }
}
