using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using RCPA.Utils;

namespace RCPA.Gui
{
  public class OptionFileRadioButtonAdaptor : AbstractOptionFileAdaptor
  {
    private RadioButton cbValue;

    private string key;

    private bool defaultValue;

    public OptionFileRadioButtonAdaptor(RadioButton cbValue, string key, bool defaultValue)
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
