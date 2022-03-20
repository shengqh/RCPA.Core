using System.Linq;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public class ItemInfosListViewAdaptor : IItemInfos
  {
    private ListView lvItems;

    public ItemInfosListViewAdaptor(ListView lvItems)
    {
      this.lvItems = lvItems;
    }

    #region IItemInfos Members

    public ItemInfoList Items
    {
      get
      {
        var items = lvItems.Items.Cast<ListViewItem>().ToList();

        var lstItems = from item in items
                       let subitems = item.SubItems.Cast<ListViewItem.ListViewSubItem>()
                       select new ItemInfo()
                       {
                         Selected = item.Selected,
                         SubItems = (from subitem in subitems
                                     select subitem.Text).ToList()
                       };

        ItemInfoList result = new ItemInfoList();
        result.AddRange(lstItems);
        return result;
      }
      set
      {
        lvItems.BeginUpdate();
        try
        {
          lvItems.Items.Clear();
          value.ForEach(m =>
          {
            ListViewItem item = new ListViewItem();
            item.Selected = m.Selected;
            if (m.SubItems.Count > 0)
            {
              item.Text = m.SubItems[0];
              for (int i = 1; i < m.SubItems.Count; i++)
              {
                item.SubItems.Add(m.SubItems[i]);
              }
            }
            lvItems.Items.Add(item);
          });
        }
        finally
        {
          lvItems.EndUpdate();
        }
      }
    }

    #endregion
  }
}
