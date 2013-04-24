using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RCPA.Gui;
using System.Collections.ObjectModel;

namespace RCPA
{
  public partial class RcpaListBox : UserControl, IRcpaComponent
  {
    private ItemInfosListBoxAdaptor listBoxAdaptor;

    public RcpaListBox()
    {
      InitializeComponent();
      listBoxAdaptor = new ItemInfosListBoxAdaptor(cbListBox);
    }

    public string Key { get; set; }

    public string Title
    {
      get
      {
        return lblTitle.Text;
      }
      set
      {
        lblTitle.Text = value;
      }
    }

    public List<object> Items
    {
      get
      {
        List<object> result = new List<object>();
        foreach (var obj in Box.Items)
        {
          result.Add(obj);
        }
        return result;
      }
      set
      {
        try
        {
          Box.BeginUpdate();
          Box.Items.Clear();
          foreach (var v in value)
          {
            Box.Items.Add(v);
          }
        }
        finally
        {
          Box.EndUpdate();
        }
      }
    }

    public List<object> CheckedItems
    {
      get
      {
        List<object> result = new List<object>();
        foreach (var obj in Box.CheckedItems)
        {
          result.Add(obj);
        }
        return result;
      }
      set
      {
        for (int i = 0; i < Box.Items.Count; i++)
        {
          Box.SetItemChecked(i, value.Contains(Box.Items[i]));
        }
      }
    }

    public List<object> SelectedItems
    {
      get
      {
        List<object> result = new List<object>();
        foreach (var obj in Box.SelectedItems)
        {
          result.Add(obj);
        }
        return result;
      }
      set
      {
        for (int i = 0; i < Box.Items.Count; i++)
        {
          Box.SetSelected(i, value.Contains(Box.Items[i]));
        }
      }
    }

    public CheckedListBox Box
    {
      get { return this.cbListBox; }
    }

    public Button UpButton
    {
      get { return this.btnUp; }
    }

    public Button DownButton
    {
      get { return this.btnDown; }
    }

    private void SwitchItem(int i, int j)
    {
      object objI = cbListBox.Items[i];
      bool selectI = cbListBox.GetSelected(i);
      bool checkedI = cbListBox.GetItemChecked(i);

      cbListBox.Items[i] = cbListBox.Items[j];
      cbListBox.SetSelected(i, cbListBox.GetSelected(j));
      cbListBox.SetItemChecked(i, cbListBox.GetItemChecked(j));

      cbListBox.Items[j] = objI;
      cbListBox.SetSelected(j, selectI);
      cbListBox.SetItemChecked(j, checkedI);
    }

    private void btnUp_Click(object sender, EventArgs e)
    {
      for (int i = 1; i < cbListBox.Items.Count; i++)
      {
        if (cbListBox.GetSelected(i))
        {
          if (cbListBox.GetSelected(i - 1))
          {
            continue;
          }

          SwitchItem(i, i - 1);
        }
      }
    }

    private void btnDown_Click(object sender, EventArgs e)
    {
      for (int i = cbListBox.Items.Count - 2; i >= 0; i--)
      {
        if (cbListBox.GetSelected(i))
        {
          if (cbListBox.GetSelected(i + 1))
          {
            continue;
          }

          SwitchItem(i, i + 1);
        }
      }
    }

    public CheckedListBox.ObjectCollection ListBoxItems
    {
      get
      {
        return cbListBox.Items;
      }
    }

    public void RemoveSelectItems()
    {
      var selected = SelectedItems;
      foreach (var item in selected)
      {
        cbListBox.Items.Remove(item);
      }
    }

    #region IRcpaComponent Members

    public virtual void ValidateComponent()
    { 
      return;
    }

    #endregion

    #region IOptionFile Members

    public void RemoveFromXml(System.Xml.Linq.XElement option)
    {
      var adaptor = new OptionFileItemInfosAdaptor(listBoxAdaptor, Key);
      adaptor.RemoveFromXml(option);
    }

    public void LoadFromXml(System.Xml.Linq.XElement option)
    {
      var adaptor = new OptionFileItemInfosAdaptor(listBoxAdaptor, Key);
      adaptor.LoadFromXml(option);
    }

    public void SaveToXml(System.Xml.Linq.XElement option)
    {
      var adaptor = new OptionFileItemInfosAdaptor(listBoxAdaptor, Key);
      adaptor.SaveToXml(option);
    }

    #endregion
  }
}
