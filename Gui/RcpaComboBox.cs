using System;
using System.Linq;
using System.Configuration;
using System.Windows.Forms;
using RCPA.Utils;
using System.Xml.Linq;

namespace RCPA.Gui
{
  public class RcpaComboBox<T> : AbstractRcpaComponent
  {
    private readonly ComboBox cb;
    private readonly int defaultIndex;
    private readonly bool required;
    private readonly string key;
    private readonly string description;

    public T[] Items { get; private set; }

    public RcpaComboBox(ComboBox cb, String key, T[] values, int defaultIndex, bool required=false, string description = "")
      : this(cb, key, values, (from v in values select v.ToString()).ToArray(), defaultIndex, required, description)
    { }

    public RcpaComboBox(ComboBox cb, String key, T[] values, string[] displayValues, int defaultIndex, bool required = false, string description = "")
    {
      if (values.Length != displayValues.Length)
      {
        throw new ArgumentException(
          MyConvert.Format("The length of values ({0}) is not equals to the length of displayValues ({1})", values.Length,
                        displayValues.Length));
      }

      this.cb = cb;
      this.required = required;
      this.key = key;
      this.description = string.IsNullOrWhiteSpace(description) ? key : description;

      ResetItems(values, displayValues);

      this.defaultIndex = defaultIndex;

      if (cb.Items.Count > defaultIndex)
      {
        cb.SelectedIndex = defaultIndex;
      }

      Adaptor = new OptionFileComboBoxAdaptor(cb, key, defaultIndex);

      Childrens.Add(cb);
    }

    public void ResetItems(T[] newValues)
    {
      ResetItems(newValues, (from v in newValues select v.ToString()).ToArray());
    }

    public void ResetItems(T[] newValues, string[] displayValues)
    {
      this.Items = newValues;
      cb.Items.Clear();
      cb.Items.AddRange(displayValues);
    }

    public int SelectedIndex
    {
      get { return this.cb.SelectedIndex; }
      set { this.cb.SelectedIndex = value; }
    }

    public string Text
    {
      get { return this.cb.Text; }
      set { this.cb.Text = value; }
    }

    public T SelectedItem
    {
      get { return this.Items[this.cb.SelectedIndex]; }
      set
      {
        for (int i = 0; i < this.Items.Length; i++)
        {
          if (this.Items[i].Equals(value))
          {
            this.cb.SelectedIndex = i;
            return;
          }
        }
      }
    }

    public override bool Enabled
    {
      get
      {
        return cb.Enabled;
      }
      set
      {
        cb.Enabled = value;
      }
    }

    public override void ValidateComponent()
    {
      base.ValidateComponent();

      if (required && this.SelectedIndex == -1)
      {
        Error.SetError(cb, "Required");
        throw new Exception(description + " is required!");
      }
    }
  }
}