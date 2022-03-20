using System;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public class RcpaDoubleField : RcpaTextField
  {
    public double MinValue { get; set; }
    public double MaxValue { get; set; }
    public RcpaDoubleField(TextBox txtValue, string key, string title, double defaultValue, bool required)
      : base(txtValue, key, title, MyConvert.Format(defaultValue), required)
    {
      base.ValidateFunc = DoValidate;
      MinValue = double.MinValue;
      MaxValue = double.MaxValue;
    }

    private bool DoValidate(string text)
    {
      double result;
      if (!MyConvert.TryParse(text, out result))
      {
        return false;
      }

      if (result < MinValue || result > MaxValue)
      {
        throw new Exception(MyConvert.Format("Value {0} must be in range [{1} - {2}]", text, MinValue, MaxValue));
      }

      return true;
    }

    public double Value
    {
      get { return MyConvert.ToDouble(Text); }
      set { Text = MyConvert.Format(value); }
    }
  }
}