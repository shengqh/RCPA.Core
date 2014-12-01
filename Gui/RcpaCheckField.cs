using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public partial class RcpaCheckField : CheckBox, IRcpaComponent
  {
    public RcpaCheckField()
    {
      InitializeComponent();

      _key = string.Empty;
    }

    private string _key;

    [Localizable(true)]
    [Category("RcpaComponent"), DescriptionAttribute("Gets or sets the key stored to config file")]
    public string Key
    {
      get
      {
        if (string.IsNullOrEmpty(_key))
        {
          return this.Name;
        }
        else
        {
          return _key;
        }
      }
      set
      {
        _key = value;
      }
    }

    private CheckBox _preCondition;

    protected virtual void DoPreConditionChanged(object sender, EventArgs e)
    {
      this.Enabled = _preCondition.Enabled && _preCondition.Checked;
    }

    public CheckBox PreCondition
    {
      get
      {
        return _preCondition;
      }
      set
      {
        if (_preCondition != null)
        {
          _preCondition.CheckedChanged -= DoPreConditionChanged;
          _preCondition.EnabledChanged -= DoPreConditionChanged;
        }

        _preCondition = value;

        if (_preCondition != null)
        {
          _preCondition.CheckedChanged += DoPreConditionChanged;
          _preCondition.EnabledChanged += DoPreConditionChanged;
        }
      }
    }

    public bool EnabledAndChecked
    {
      get
      {
        if (_preCondition != null)
        {
          return _preCondition.Checked && this.Enabled && this.Checked;
        }
        else
        {
          return this.Enabled && this.Checked;
        }
      }
    }

    #region IRcpaComponent Members

    public void ValidateComponent()
    { }

    #endregion

    #region IOptionFile Members

    public void RemoveFromXml(System.Xml.Linq.XElement option)
    {
      new RcpaCheckBox(this, Key, Checked).RemoveFromXml(option);
    }

    public void LoadFromXml(System.Xml.Linq.XElement option)
    {
      new RcpaCheckBox(this, Key, Checked).LoadFromXml(option);
    }

    public void SaveToXml(System.Xml.Linq.XElement option)
    {
      new RcpaCheckBox(this, Key, Checked).SaveToXml(option);
    }

    #endregion
  }
}
