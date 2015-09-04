using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Windows.Forms
{
  public static class WinFormsExtensions
  {
    public static void AppendLine(this TextBoxBase box, string line)
    {
      box.AppendText(line + Environment.NewLine);
    }

    public static void AppendLine(this TextBoxBase box, string format, params object[] values)
    {
      AppendLine(box, string.Format(format, values));
    }

    public static void AppendColorLine(this RichTextBox box, Color color, string line)
    {
      box.SelectionStart = box.TextLength;
      box.SelectionLength = 0;

      box.SelectionColor = color;
      box.AppendText(line + Environment.NewLine);
      box.SelectionColor = box.ForeColor;
    }

    public static void AppendColorLine(this RichTextBox box, Color color, string format, params object[] values)
    {
      AppendLine(box, string.Format(format, values), color);
    }

    public static void DoubleBuffered(this Control control, bool enable)
    {
      var doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
      doubleBufferPropertyInfo.SetValue(control, enable, null);
    }
  }
}
