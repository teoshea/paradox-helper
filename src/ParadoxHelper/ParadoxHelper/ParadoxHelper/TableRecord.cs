
using System;
using System.Collections.Generic;
using System.IO;
using ParadoxHelper;

public class TableRecord
{
    public readonly List<FieldValue> Values = new List<FieldValue>();

    public void Read(List<TableField> fields, BinaryReader reader)
    {
        try
        {
            
            foreach (var pdxTableField in fields)
            {
                var value = new FieldValue(pdxTableField);
                value.Read(reader);
                Values.Add(value);
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Exception in read", ex);
        }
    }
}
