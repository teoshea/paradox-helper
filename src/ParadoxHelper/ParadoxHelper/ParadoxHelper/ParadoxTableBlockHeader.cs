using System;
using System.IO;

public class ParadoxTableBlockHeader
{
    public int NextBlockNumber { get; private set; }
    public int PreviousBlockNumber { get; private set; }
    public int OffsetLastRecord { get; private set; }

    /**
     * block header, 6 bytes
     */

    public void Read(BinaryReader reader)
    {
        try
        {
            NextBlockNumber = reader.ReadUInt16();
            PreviousBlockNumber = reader.ReadUInt16();
            OffsetLastRecord = reader.ReadUInt16();
        }
        catch (Exception ex)
        {
            throw new Exception("Exception in read", ex);
        }
    }
}