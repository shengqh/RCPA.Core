using System.Linq;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public class ItemInfosListBoxAdaptor : IItemInfos
  {
    private ListBox lbFiles;

    public ItemInfosListBoxAdaptor(ListBox lbFiles)
    {
      this.lbFiles = lbFiles;
    }

    #region IItemInfos Members

    public ItemInfoList Items
    {
      get
      {
        ItemInfoList result = new ItemInfoList();

        var selected = lbFiles.SelectedIndices.Cast<int>().ToList();
        for (int i = 0; i < lbFiles.Items.Count; i++)
        {
          ItemInfo item = new ItemInfo();
          item.SubItems.Add(lbFiles.Items[i].ToString());
          item.Selected = selected.Contains(i);
          result.Add(item);
        }

        return result;
      }
      set
      {
        lbFiles.BeginUpdate();
        try
        {
          lbFiles.Items.Clear();
          for (int i = 0; i < value.Count; i++)
          {
            lbFiles.Items.Add(value[i].SubItems[0]);
            lbFiles.SetSelected(i, value[i].Selected);
          }
        }
        finally
        {
          lbFiles.EndUpdate();
        }
      }
    }

    #endregion
  }
}
