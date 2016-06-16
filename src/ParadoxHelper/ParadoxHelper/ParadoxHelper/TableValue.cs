using System;
using System.IO;

namespace ParadoxHelper
{
    public class FieldValue
    {
        public FieldValue(TableField field)
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            Type = field.FieldType;
            Length = field.Length;
        }

        
        public string Value { get; private set; }
        /**
     * the type
     */
        public FieldType Type { get; private set; }

        public int Length { get; private set; }

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
        public void Read(BinaryReader reader)
        {
            try
            {
                /*
             * get the data
             */
                byte[] data = new byte[Length];
                reader.Read(data, 0, Length);
                /*
             * convert to type
             */
                switch (Type)
                {
                    case FieldType.A:
                        Value = System.Text.Encoding.ASCII.GetString(data).ToString();
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
                        Value = BitConverter.ToInt64(data, 0).ToString();
                        break;
                    case FieldType.B:
                        throw new Exception("B Not yet supported");
                    case FieldType.Auto:
                        Value = BitConverter.ToInt16(data, 0).ToString();
                        break;
                    //final short auto = ByteBuffer.wrap(data).order(ByteOrder.LITTLE_ENDIAN).getShort();
                    //value = Short.toString(auto);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in read", ex);
            }
        }
    }
}
