using System;
using System.Linq;
using System.Configuration;
using System.Windows.Forms;
using RCPA.Utils;
using System.Collections.Generic;
using RCPA.Gui.Event;
using System.Xml.Linq;

namespace RCPA.Gui
{
  public class RcpaListViewField : AbstractRcpaComponent
  {
    protected string description;

    protected ListView lvItems;

    protected ItemInfosListViewAdaptor infoAdaptor;

    public RcpaListViewField(Button btnRemove, Button btnLoad, Button btnSave, Button btnUp, Button btnDown,
                             ListView lvItems, string key, string description)
    {
      this.description = description;

      this.lvItems = lvItems;

      ListViewEventHandlers handlers = new ListViewEventHandlers(lvItems);

      infoAdaptor = new ItemInfosListViewAdaptor(lvItems);

      lvItems.KeyDown += new KeyEventHandler(lvItems_KeyDown);

      Childrens.Add(lvItems);

      if (btnRemove != null)
      {
        btnRemove.Text = "Remove";
        btnRemove.Click += handlers.RemoveEvent;
        Childrens.Add(btnRemove);
      }

      if (btnLoad != null)
      {
        btnLoad.Text = "Load";
        btnLoad.Click += handlers.LoadEvent;
        Childrens.Add(btnLoad);
      }

      if (btnSave != null)
      {
        btnSave.Text = "Save";
        btnSave.Click += handlers.SaveEvent;
        Childrens.Add(btnSave);
      }

      if (btnUp != null)
      {
        btnUp.Text = "Up";
        btnUp.Click += handlers.UpEvent;
        Childrens.Add(btnUp);
      }

      if (btnDown != null)
      {
        btnDown.Text = "Down";
        btnDown.Click += handlers.DownEvent;
        Childrens.Add(btnDown);
      }

      Adaptor = new OptionFileItemInfosAdaptor(infoAdaptor, key);
    }

    void lvItems_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Control && e.KeyCode == Keys.A)
      {
        foreach (ListViewItem item in lvItems.Items)
        {
          item.Selected = true;
        }
      }
    }

    public RcpaListViewField(Button btnRemove, Button btnLoad, Button btnSave, ListView lvItems, string key,
                             string description)
      : this(btnRemove, btnLoad, btnSave, null, null, lvItems, key, description)
    { }

    public void ClearItems()
    {
      lvItems.Items.Clear();
    }

    public string[] GetAllItems()
    {
      return infoAdaptor.Items.GetAllItems();
    }

    public string[] GetSelectedItems()
    {
      return infoAdaptor.Items.GetSelectedItems();
    }

    public void AddItems(string[] items)
    {
      var allItems = GetAllItems();

      var newItems =
        from item in items
        where !allItems.Contains(item)
        select item;

      newItems.ToList().ForEach(m => lvItems.Items.Add(m));
    }

    public IItemInfos GetItemInfos()
    {
      return infoAdaptor;
    }
  }
}