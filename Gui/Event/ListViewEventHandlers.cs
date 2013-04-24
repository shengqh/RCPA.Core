using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RCPA.Gui.Event
{
  public class ListViewEventHandlers : ItemInfosEventHandlers
  {
    private ListView lvItems;

    public ListViewEventHandlers(ListView listBox)
      : base(new ItemInfosListViewAdaptor(listBox))
    {
      this.lvItems = listBox;
    }

    public void RemoveEvent(object sender, EventArgs e)
    {
      lvItems.BeginUpdate();
      try
      {
        List<ListViewItem> selected = lvItems.GetSelectedItemList();
        selected.ForEach(m => m.Remove());
      }
      finally
      {
        lvItems.EndUpdate();
      }
    }

    public void ClearEvent(object sender, EventArgs e)
    {
      lvItems.BeginUpdate();
      try
      {
        lvItems.Items.Clear();
      }
      finally
      {
        lvItems.EndUpdate();
      }
    }

    public void UpEvent(object sender, EventArgs e)
    {
      if (this.lvItems.Items.Count == 1)
      {
        return;
      }

      if (this.lvItems.SelectedItems.Count == 0)
      {
        return;
      }

      this.lvItems.BeginUpdate();
      try
      {
        for (int i = 0; i < this.lvItems.Items.Count; i++)
        {
          if (this.lvItems.Items[i].Selected)
          {
            if (i == 0)
            {
              return;
            }

            ListViewItem item = this.lvItems.Items[i];
            this.lvItems.Items.RemoveAt(i);
            this.lvItems.Items.Insert(i - 1, item);
            return;
          }
        }
      }
      finally
      {
        this.lvItems.EndUpdate();
      }
    }

    public void DownEvent(object sender, EventArgs e)
    {
      if (this.lvItems.Items.Count == 1)
      {
        return;
      }

      if (this.lvItems.SelectedItems.Count == 0)
      {
        return;
      }

      this.lvItems.BeginUpdate();
      try
      {
        for (int i = 0; i < this.lvItems.Items.Count - 1; i++)
        {
          if (this.lvItems.Items[i].Selected)
          {
            ListViewItem item = this.lvItems.Items[i];
            this.lvItems.Items.RemoveAt(i);
            this.lvItems.Items.Insert(i + 1, item);
            return;
          }
        }
      }
      finally
      {
        this.lvItems.EndUpdate();
      }
    }
  }
}
