using System.Windows.Forms;
using System.Xml.Linq;

namespace RCPA.Gui
{
  public class OptionFileCheckBoxAdaptor : AbstractOptionFileAdaptor
  {
    private CheckBox cbValue;

    private string key;

    private bool defaultValue;

    public OptionFileCheckBoxAdaptor(CheckBox cbValue, string key, bool defaultValue)
      : base(key)
    {
      this.cbValue = cbValue;
      this.key = key;
      this.defaultValue = defaultValue;
    }

    public override void LoadFromXml(XElement option)
    {
      cbValue.Checked = option.GetChildValue(key, defaultValue);
    }

    public override void SaveToXml(XElement option)
    {
      option.Add(new XElement(key, cbValue.Checked.ToString()));
    }
  }
}
