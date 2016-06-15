using System;
using System.IO;
using ParadoxHelper;

public enum FieldType
{
    A = 1,
    D = 2,
    S = 3,
    Dollar = 5,
    N = 6,
    M = 0x0c,
    B = 0x0d,
    Auto = 22
}

public class TableField
{
    /**
 * field type
 */
    public FieldType FieldType { get; private set; }
    /**
 * field type
 */
    public int Type { get; private set; }
    /**
     * field length
     */
    public int Length { get; private set; }
    /**
     * name
     */
    public string Name { get; private set; }

    /**
     * names
     */

    public void ReadFieldName(BinaryReader inputStream)
    {
        try
        {
            Name = inputStream.ReadNullTerminatedString();
        }
        catch (Exception ex)
        {
            throw new Exception("Exception in read", ex);
        }
    }
    
    public void ReadFieldTypeAndSize(BinaryReader inputStream)
    {
        try
        {
            Type = inputStream.ReadByte();
            Length = inputStream.ReadByte();
            switch (Type)
            {
                case 1:
                    FieldType = FieldType.A;
                    break;
                case 2:
                    FieldType = FieldType.D;
                    if (Length != 4)
                    {
                        throw new Exception("Invalid field length '" + Length + "' for type '" + Type + "'");
                    }
                    break;
                case 03:
                    FieldType = FieldType.S;
                    if (Length != 2)
                    {
                        throw new Exception("Invalid field length '" + Length + "' for type '" + Type + "'");
                    }
                    break;
                case 05:
                    FieldType = FieldType.Dollar;
                    if (Length != 8)
                    {
                        throw new Exception("Invalid field length '" + Length + "' for type '" + Type + "'");
                    }
                    break;
                case 06:
                    FieldType = FieldType.N;
                    if (Length != 8)
                    {
                        throw new Exception("Invalid field length '" + Length + "' for type '" + Type + "'");
                    }
                    break;
                case 0xC:
                    FieldType = FieldType.M;
                    break;
                case 0xD:
                    FieldType = FieldType.B;
                    break;
                case 22:
                    FieldType = FieldType.Auto;
                    break;
                default:
                    FieldType = FieldType.Auto;
                    break;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Exception in read", ex);
        }
    }
}