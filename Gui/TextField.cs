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
  public partial class TextField : UserControl, IDependentRcpaComponent
  {
    public TextField()
    {
      InitializeComponent();
    }

    public CheckBox PreCondition { get; set; }

    protected Func<string, bool> ValidateFunc { get; set; }

    private string _key = string.Empty;

    [Localizable(true)]
    [Category("File"), DescriptionAttribute("Gets or sets the key stored to config file")]
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

    [Localizable(true)]
    [Category("File"), DescriptionAttribute("Gets or sets the caption")]
    public string Caption
    {
      get
      {
        return lblCaption.Text;
      }
      set
      {
        lblCaption.Text = value;
      }
    }

    [Localizable(true)]
    [Category("File"), DescriptionAttribute("Gets or sets the text requirement"), DefaultValue(true)]
    public bool Required { get; set; }

    [Localizable(true)]
    [Category("File"), DescriptionAttribute("Gets or sets default value")]
    public string DefaultValue
    {
      get { return TextEdit.Text; }
      set { TextEdit.Text = value; }
    }

    [Localizable(true)]
    [Category("File"), DescriptionAttribute("Gets or sets the caption size")]
    public Size CaptionSize
    {
      get
      {
        return lblCaption.Size;
      }
      set
      {
        lblCaption.Size = value;
      }
    }

    protected virtual IRcpaComponent Field
    {
      get
      {
        var result = new RcpaTextField(TextEdit, Key, Caption, DefaultValue, Required);
        result.ValidateFunc = this.ValidateFunc;
        result.PreCondition = PreCondition;
        return result;
      }
    }

    #region IRcpaComponent Members

    public virtual void ValidateComponent()
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

    public override string Text
    {
      get { return TextEdit.Text; }
      set { TextEdit.Text = value; }
    }
  }
}