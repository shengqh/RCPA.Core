using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RCPA.Gui
{
  public class OptionFileTextBoxAdaptor : AbstractOptionFileAdaptor
  {
    private TextBox txtValue;

    private string key;

    private string defaultValue;

    public OptionFileTextBoxAdaptor(TextBox txtValue, string key, string defaultValue)
      : base(key)
    {
      this.txtValue = txtValue;
      this.key = key;
      this.defaultValue = defaultValue;
    }

    public override void LoadFromXml(XElement option)
    {
      var result =
        (from item in option.Descendants(key)
         select item).FirstOrDefault();

      if (null == result)
      {
        txtValue.Text = defaultValue;
      }
      else
      {
        txtValue.Text = result.Value;
      }
    }

    public override void SaveToXml(XElement option)
    {
      option.Add(new XElement(key, txtValue.Text));
    }

  }
}
