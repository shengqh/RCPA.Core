using System.Windows.Forms;

namespace RCPA.Gui
{
  public class RcpaIntegerField : RcpaTextField
  {
    public RcpaIntegerField(TextBox txtValue, string key, string title, int defaultValue, bool required)
      : base(txtValue, key, title, defaultValue.ToString(), required)
    {
      base.ValidateFunc = DoValidate;
    }

    private bool DoValidate(string text)
    {
      int result;
      return int.TryParse(text, out result);
    }

    /// <summary>
    /// Get and set integer value
    /// </summary>
    public int Value
    {
      get { return int.Parse(Text); }
      set { Text = value.ToString(); }
    }
  }
}