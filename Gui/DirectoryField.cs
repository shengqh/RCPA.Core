using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using RCPA.Gui.FileArgument;

namespace RCPA.Gui
{
  public partial class DirectoryField : UserControl, IDependentRcpaComponent
  {
    public CheckBox PreCondition
    {
      get
      {
        return Field.PreCondition;
      }
      set
      {
        Field.PreCondition = value;
      }
    }

    private RcpaDirectoryField _field;

    private RcpaDirectoryField Field
    {
      get
      {
        if (_field == null)
        {
          GenerateField();
        }
        return _field;
      }
    }

    public DirectoryField()
    {
      InitializeComponent();
      _required = true;
    }

    [Localizable(true)]
    [Category("Directory"), DescriptionAttribute("Gets or sets the Load Button visible"), DefaultValue(false)]
    public bool LoadButtonVisible
    {
      get { return btnLoad.Visible; }
      set { btnLoad.Visible = value; }
    }

    [Localizable(true)]
    [Category("Directory"), DescriptionAttribute("Gets or sets the Open Button width")]
    public int OpenButtonWidth
    {
      get { return btnOpen.Width; }
      set { btnOpen.Width = value; }
    }

    [Localizable(true)]
    [Category("Directory"), DescriptionAttribute("Gets or sets the Open Button Text")]
    public string OpenButtonText
    {
      get { return btnOpen.Text; }
      set { btnOpen.Text = value; }
    }

    private string _key = string.Empty;
    private string _description = string.Empty;

    [Localizable(true)]
    [Category("Directory"), DescriptionAttribute("Gets or sets the key stored to config file")]
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
        GenerateField();
      }
    }

    private bool _required;

    [Localizable(true)]
    [Category("Directory"), DescriptionAttribute("Gets or sets the directory requirement"), DefaultValue(true)]
    public bool Required
    {
      get
      {
        return _required;
      }
      set
      {
        _required = value;
        GenerateField();
      }
    }

    public void SetDirectoryArgument(string key, string description)
    {
      this._key = key;
      this._description = description;
      GenerateField();
    }

    private void GenerateField()
    {
      if (null != _field)
      {
        _field.RemoveClickEvent();
      }

      _field = new RcpaDirectoryField(btnOpen, txtFile, _key, _description, Required);
    }

    #region IRcpaComponent Members

    public void ValidateComponent()
    {
      Field.ValidateComponent();
    }

    #endregion

    #region IOptionFile Members

    public void RemoveFromXml(System.Xml.Linq.XElement option)
    {
      Field.RemoveFromXml(option);
    }

    public void LoadFromXml(System.Xml.Linq.XElement option)
    {
      Field.LoadFromXml(option);
    }

    public void SaveToXml(System.Xml.Linq.XElement option)
    {
      Field.SaveToXml(option);
    }

    #endregion

    public string FullName
    {
      get { return Field.FullName; }
      set { Field.FullName = value; }
    }

    public bool Exists
    {
      get
      {
        return Directory.Exists(FullName);
      }
    }
  }
}
