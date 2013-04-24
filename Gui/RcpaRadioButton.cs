using System;
using System.Configuration;
using System.Windows.Forms;
using RCPA.Utils;
using System.Xml.Linq;

namespace RCPA.Gui
{
  public class RcpaRadioButton : AbstractRcpaComponent
  {
    private RadioButton cbValue;

    public RcpaRadioButton(RadioButton cbValue, String key, bool defaultValue)
    {
      this.cbValue = cbValue;
      this.cbValue.Checked = defaultValue;

      Adaptor = new OptionFileRadioButtonAdaptor(cbValue, key, defaultValue);

      Childrens.Add(cbValue);
    }

    public bool Checked
    {
      get { return this.cbValue.Checked; }
      set { this.cbValue.Checked = value; }
    }
  }
}