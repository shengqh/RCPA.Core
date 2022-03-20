using System.Collections.Generic;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public class AnnotationCell : DataGridViewTextBoxCell
  {
    public AnnotationCell()
      : base()
    { }

    public string Key
    {
      get;
      set;
    }

    protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, System.ComponentModel.TypeConverter valueTypeConverter, System.ComponentModel.TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
    {
      if (value is Dictionary<string, object>)
      {
        var ann = (Dictionary<string, object>)value;
        if (ann.ContainsKey(Key))
        {
          return base.GetFormattedValue(ann[Key].ToString(), rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
        }
        else
        {
          return string.Empty;
        }
      }
      else
      {
        return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
      }
    }

    /// <summary>
    /// Clones a AnnotationCell cell, copies all the custom properties.
    /// </summary>
    public override object Clone()
    {
      AnnotationCell result = base.Clone() as AnnotationCell;
      if (result != null)
      {
        result.Key = this.Key;
      }
      return result;
    }

  }
}
