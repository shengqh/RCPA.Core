using System;
using System.Linq;
using System.Windows.Forms;

namespace RCPA.Gui.Event
{
  public class ListBoxEventHandlers : ItemInfosEventHandlers
  {
    protected ListBox lbFiles;

    public ListBoxEventHandlers(ListBox listBox)
      : base(new ItemInfosListBoxAdaptor(listBox))
    {
      this.lbFiles = listBox;
    }

    public void RemoveEvent(object sender, EventArgs e)
    {
      lbFiles.BeginUpdate();
      try
      {
        var selected = lbFiles.SelectedIndices.Cast<int>().ToList();
        for (int i = selected.Count - 1; i >= 0; i--)
        {
          lbFiles.Items.RemoveAt(selected[i]);
        }
      }
      finally
      {
        lbFiles.EndUpdate();
      }
    }

    public void ClearEvent(object sender, EventArgs e)
    {
      lbFiles.BeginUpdate();
      try
      {
        lbFiles.Items.Clear();
      }
      finally
      {
        lbFiles.EndUpdate();
      }
    }
  }
}
