using System;
using System.ComponentModel;
using System.Windows.Forms;

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

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Bindable(true)]
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

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Bindable(true)]
    [Category("Design"), DescriptionAttribute("Gets or sets ReadOnly stored to config file")]
    public bool ReadOnly
    {
      get
      {
        return this.TextEdit.ReadOnly;
      }
      set
      {
        this.TextEdit.ReadOnly = value;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Bindable(true)]
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
    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Bindable(true)]
    [Category("File"), DescriptionAttribute("Gets or sets the caption width")]
    public int CaptionWidth
    {
      get
      {
        return lblCaption.Width;
      }
      set
      {
        lblCaption.Width = value;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Bindable(true)]
    [Category("File"), DescriptionAttribute("Gets or sets the description")]
    public string Description
    {
      get
      {
        return lblDescription.Text;
      }
      set
      {
        lblDescription.Text = value;
        if (!string.IsNullOrEmpty(value))
        {
          lblDescription.Visible = true;
          TextEdit.Dock = DockStyle.Left;
          lblDescription.Dock = DockStyle.Fill;
          lblDescription.BringToFront();
        }
        else
        {
          lblDescription.Visible = false;
          lblDescription.Dock = DockStyle.Right;
          TextEdit.Dock = DockStyle.Fill;
        }
      }
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Bindable(true)]
    [Category("File"), DescriptionAttribute("Gets or sets the text width")]
    public int TextWidth
    {
      get
      {
        return TextEdit.Width;
      }
      set
      {
        TextEdit.Width = value;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Bindable(true)]
    [Category("File"), DescriptionAttribute("Gets or sets the text requirement"), DefaultValue(true)]
    public bool Required { get; set; }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Bindable(true)]
    [Category("File"), DescriptionAttribute("Gets or sets default value")]
    public string DefaultValue
    {
      get { return TextEdit.Text; }
      set { TextEdit.Text = value; }
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

    public TextBox Box
    {
      get
      {
        return TextEdit;
      }
    }
  }
}