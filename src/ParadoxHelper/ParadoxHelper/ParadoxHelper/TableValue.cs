
using System;
using System.IO;

public class FieldValue
{
    /**
     * the value
     */
    private string value;
    /**
     * the type
     */
    private FieldType type;

    /**
     * <p>
     * $ and N are double precision floating point numbers
     * </p>
     * <p>
     * D are signed long ints (they are dates). Number of days since January 1, 1 AD.
     * </p>
     * <p>
     * S are signed shorts
     * </p>
     * <p>
     * M, B and U are BLOBS.
     * </p>
     * <p>
     * A are null terminated, fixed length strings
     * </p>
     */
    public void Read(TableField pdxTableField, BinaryReader reader)
    {
      try {
            /*
             * get the data
             */
            byte[] data = new byte[pdxTableField.Length];
            reader.Read(data, 0, pdxTableField.Length);
            /*
             * convert to type
             */
            switch (pdxTableField.FieldType)
            {
                case FieldType.A:
                    value = StringUtil.readString(data);
                    break;
                /*case D:
                    final long d = ByteBuffer.wrap(data).order(ByteOrder.LITTLE_ENDIAN).getShort();
                    value = Long.toString(d);
                    break;
                case S:
                    final long s = ByteBuffer.wrap(data).order(ByteOrder.LITTLE_ENDIAN).getShort();
                    value = Long.toString(s);
                    break;
                case $:
               final double dollars = ByteBuffer.wrap(data).order(ByteOrder.BIG_ENDIAN).getDouble();
                    value = Double.toString(dollars);
                    break;
                case M:
                    throw new Exception("M Not yet supported");*/
                case FieldType.N:
                     
                    value = BitConverter.ToInt64(data, 0);
                    break;
                case B:
                    throw new Exception("B Not yet supported");
                case Auto:
                    final short auto = ByteBuffer.wrap(data).order(ByteOrder.LITTLE_ENDIAN).getShort();
                    value = Short.toString(auto);
            }
        } catch (final Exception e) {
            throw new Exception("Exception in read", e);
        }
    }

    public void setType(DBTableField.FieldType type)
    {
        this.type = type;
    }

    public void setValue(String value)
    {
        this.value = value;
    }
}

Status API Training Shop Blog About

    © 2016 GitHub, Inc.Terms Privacy Security Contact Help

