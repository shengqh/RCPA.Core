using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RCPA.Gui
{
  public class OptionFileComboBoxAdaptor : AbstractOptionFileAdaptor
  {
    private ComboBox cbValue;

    private string key;

    private int defaultValue;

    public OptionFileComboBoxAdaptor(ComboBox cbValue, string key, int defaultValue)
      : base(key)
    {
      this.cbValue = cbValue;
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
        cbValue.SelectedIndex = defaultValue;
      }
      else
      {
        cbValue.SelectedIndex = Convert.ToInt32(result.Value);
      }
    }

    public override void SaveToXml(XElement option)
    {
      option.Add(new XElement(key, cbValue.SelectedIndex.ToString()));
    }
  }
}
