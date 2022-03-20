using System;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public class RcpaCheckBox : AbstractRcpaComponent
  {
    private CheckBox cbValue;

    public RcpaCheckBox(CheckBox cbValue, String key, bool defaultValue)
    {
      this.cbValue = cbValue;
      this.cbValue.Checked = defaultValue;

      Adaptor = new OptionFileCheckBoxAdaptor(cbValue, key, defaultValue);

      Childrens.Add(cbValue);
    }

    public bool Checked
    {
      get { return this.cbValue.Checked; }
      set { this.cbValue.Checked = value; }
    }

    protected override void DoPreConditionChanged(object sender, EventArgs e)
    {
      if (sender is CheckBox)
      {
        var cb = sender as CheckBox;
        cbValue.Enabled = cb.Enabled && cb.Checked;
      }
    }
  }
}