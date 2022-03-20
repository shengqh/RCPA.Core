using System.Collections.Generic;
using System.Windows.Forms;

namespace RCPA
{
  public static class ComponentExtension
  {
    public static List<ListViewItem> GetSelectedItemList(this ListView lv)
    {
      List<ListViewItem> result = new List<ListViewItem>();

      for (int i = 0; i < lv.SelectedItems.Count; i++)
      {
        result.Add(lv.SelectedItems[i]);
      }

      return result;
    }

    public static List<ColumnHeader> GetColumnList(this ListView lv)
    {
      List<ColumnHeader> result = new List<ColumnHeader>();

      foreach (ColumnHeader ch in lv.Columns)
      {
        result.Add(ch);
      }

      return result;
    }
  }
}
