using System;
using System.Configuration;
using System.Windows.Forms;
using RCPA.Utils;

namespace RCPA.Gui
{
  public class RcpaStringComboBox : AbstractRcpaComponent
  {
    private readonly ComboBox cb;
    
    public RcpaStringComboBox(ComboBox cb, String key, string[] values, string defaultValue)
    {
      this.cb = cb;
      cb.Items.Clear();
      cb.Items.AddRange(values);
      cb.SelectedText = defaultValue;

      Adaptor = new OptionFileStringComboBoxAdaptor(cb, key, defaultValue);

      Childrens.Add(cb);
    }

    public int SelectedIndex
    {
      get { return this.cb.SelectedIndex; }
      set { this.cb.SelectedIndex = value; }
    }

    public string SelectedItem
    {
      get { return this.cb.Text; }
      set { this.cb.Text = value; }
    }
  }
}