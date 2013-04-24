using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RCPA.Gui
{
  public class OptionFileStringComboBoxAdaptor : AbstractOptionFileAdaptor
  {
    private ComboBox cbValue;

    private string key;

    private string defaultValue;

    public OptionFileStringComboBoxAdaptor(ComboBox cbValue, string key, string defaultValue)
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
        cbValue.Text = defaultValue;
      }
      else
      {
        cbValue.Text = result.Value;
      }
    }

    public override void SaveToXml(XElement option)
    {
      option.Add(new XElement(key, cbValue.Text));
    }
  }
}
