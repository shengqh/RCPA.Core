using System;
using System.Collections.Generic;
using System.Linq;

namespace RCPA
{
  public class ItemInfo
  {
    public ItemInfo()
    {
      SubItems = new List<string>();

      Selected = false;
    }

    public List<string> SubItems { get; set; }

    public bool Selected { get; set; }
  }

  public class ItemInfoList : List<ItemInfo>
  {
    public ItemInfoList() { }

    public ItemInfoList(IEnumerable<ItemInfo> collection)
      : base(collection)
    { }

    public ItemInfoList(IEnumerable<string> collections)
    {
      AddRange(
        from c in collections
        select new ItemInfo()
        {
          Selected = false,
          SubItems = new List<string>(new[] { c })
        });
    }

    public string[] GetAllItems()
    {
      return (from item in this
              select item.SubItems[0]).ToArray();
    }

    public string[] GetSelectedItems()
    {
      return (from item in this
              where item.Selected
              select item.SubItems[0]).ToArray();
    }

    private Dictionary<string, string> GetMap(Func<ItemInfo, bool> valid)
    {
      Dictionary<string, string> result = new Dictionary<string, string>();
      foreach (var m in this)
      {
        if (valid(m))
        {
          if (m.SubItems.Count > 1)
          {
            result[m.SubItems[0]] = m.SubItems[1];
          }
          else
          {
            result[m.SubItems[0]] = string.Empty;
          }
        }
      }
      return result;
    }

    public Dictionary<string, string> GetAllItemMap()
    {
      return GetMap(m => true);
    }

    public Dictionary<string, string> GetSelectItemMap()
    {
      return GetMap(m => m.Selected);
    }
  }

  public class SimpleItemInfos : IItemInfos
  {
    public ItemInfoList Items { get; set; }
  }
}
