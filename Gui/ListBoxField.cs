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
  public partial class ListBoxField : UserControl, IItemInfos, IRcpaComponent
  {
    public ListBoxField()
    {
      InitializeComponent();

      adaptor = new ItemInfosListBoxAdaptor(lbFiles);

      handlers = new ListBoxFileEventHandlers(lbFiles, null);

      btnAdd.Click += handlers.AddEvent;

      btnRemove.Click += handlers.RemoveEvent;

      btnClear.Click += handlers.ClearEvent;

      btnLoad.Click += handlers.LoadEvent;

      btnSave.Click += handlers.SaveEvent;
    }

    private ItemInfosListBoxAdaptor adaptor;

    private ListBoxFileEventHandlers handlers;

    public OpenFileArgument FileArgument
    {
      get
      {
        return handlers.FileArgument;
      }
      set
      {
        if (value == null)
        {
          btnAdd.Visible = false;
        }

        btnAdd.Visible = true;
        handlers.FileArgument = value;
      }
    }

    [Localizable(true)]
    [Category("LoadButtonVisible"), DescriptionAttribute("Gets or sets the Load Button visible"), DefaultValue(true)]
    public bool LoadButtonVisible
    {
      get { return btnLoad.Visible; }
      set { btnLoad.Visible = value; }
    }

    [Localizable(true)]
    [Category("SaveButtonVisible"), DescriptionAttribute("Gets or sets the Save Button visible"), DefaultValue(true)]
    public bool SaveButtonVisible
    {
      get { return btnSave.Visible; }
      set { btnSave.Visible = value; }
    }

    [Localizable(true)]
    [Category("SelectionMode"), DescriptionAttribute("Gets or sets the SelectionMode of ListBox"), DefaultValue(true)]
    public SelectionMode SelectionMode
    {
      get { return lbFiles.SelectionMode; }
      set { lbFiles.SelectionMode = value; }
    }

    [Localizable(true)]
    [Category("Description"), DescriptionAttribute("Gets or sets the Description value"), DefaultValue("")]
    public string Description
    {
      get { return groupBox.Text; }
      set { groupBox.Text = value; }
    }

    [Localizable(true)]
    [Category("ListBox"), DescriptionAttribute("Gets the ListBox")]
    public System.Windows.Forms.ListBox ListBox
    {
      get { return this.lbFiles; }
    }

    public object SelectedItem
    {
      get { return ListBox.SelectedItem; }
      set { ListBox.SelectedItem = value; }
    }

    public int SelectedIndex
    {
      get { return ListBox.SelectedIndex; }
      set { ListBox.SelectedIndex = value; }
    }

    public string[] FileNames
    {
      get { return adaptor.Items.GetAllItems(); }
      set { adaptor.Items = new ItemInfoList(value); }
    }

    public string[] SelectedFileNames
    {
      get { return adaptor.Items.GetSelectedItems(); }
    }

    public IItemInfos GetItemInfos()
    {
      return adaptor;
    }

    #region IItemInfos Members
    public ItemInfoList Items
    {
      get { return adaptor.Items; }
      set { adaptor.Items = value; }
    }

    #endregion

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
  }
}
