using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RCPA.Gui.Event;
using RCPA.Gui.FileArgument;

namespace RCPA.Gui
{
  public partial class ListViewField : UserControl, IRcpaComponent, IProgress
  {
    private RcpaListViewField _field;

    public ListViewField()
    {
      InitializeComponent();

      _field = new RcpaListViewField(btnRemove, btnLoad, btnSave, btnUp, btnDown, lbFiles, string.Empty, string.Empty);
    }

    public OpenFileArgument FileArgument { get; set; }

    public bool FileMode { get; set; }

    [Localizable(true)]
    [Category("RcpaComponent"), DescriptionAttribute("Gets or sets the Load Button visible"), DefaultValue(true)]
    public bool LoadButtonVisible
    {
      get { return btnLoad.Visible; }
      set { btnLoad.Visible = value; }
    }

    [Localizable(true)]
    [Category("RcpaComponent"), DescriptionAttribute("Gets or sets the Save Button visible"), DefaultValue(true)]
    public bool SaveButtonVisible
    {
      get { return btnSave.Visible; }
      set { btnSave.Visible = value; }
    }

    [Localizable(true)]
    [Category("RcpaComponent"), DescriptionAttribute("Gets or sets the SelectionMode of ListBox"), DefaultValue(true)]
    public SelectionMode SelectionMode { get; set; }

    [Localizable(true)]
    [Category("RcpaComponent"), DescriptionAttribute("Gets or sets the Description value"), DefaultValue("")]
    public string Description
    {
      get { return groupBox.Text; }
      set { groupBox.Text = value; }
    }

    [Localizable(true)]
    [Category("RcpaComponent"), DescriptionAttribute("Gets the ListView")]
    public System.Windows.Forms.ListView View
    {
      get { return this.lbFiles; }
    }

    #region IRcpaComponent Members

    public void ValidateComponent()
    {
      throw new NotImplementedException();
    }

    #endregion

    #region IOptionFile Members

    public void RemoveFromXml(System.Xml.Linq.XElement option)
    {
      throw new NotImplementedException();
    }

    public void LoadFromXml(System.Xml.Linq.XElement option)
    {
      throw new NotImplementedException();
    }

    public void SaveToXml(System.Xml.Linq.XElement option)
    {
      throw new NotImplementedException();
    }

    #endregion

    #region IProgress Members

    public Utils.IProgressCallback Progress
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    #endregion
  }
}
