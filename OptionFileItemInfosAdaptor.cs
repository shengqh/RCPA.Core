using System;
using System.Linq;
using System.Xml.Linq;

namespace RCPA
{
  public class OptionFileItemInfosAdaptor : AbstractOptionFileAdaptor
  {
    private IItemInfos items;

    private string key;

    public OptionFileItemInfosAdaptor(IItemInfos items, string key)
      : base(key)
    {
      this.items = items;
      this.key = key;
    }

    public override void LoadFromXml(XElement option)
    {
      if (option == null)
      {
        throw new ArgumentNullException("option");
      }

      items.Items = new ItemInfoListNewReader(key).ReadFromXml(option);

      if (items.Items.Count == 0 && option.Element("appSettings") != null)
      {
        items.Items = new ItemInfoListOldReader(key).ReadFromXml(option);
      }
    }

    public override void SaveToXml(XElement option)
    {
      option.Add(new XElement(key,
                              from item in items.Items
                              select new XElement("Item", new XAttribute("Selected", item.Selected.ToString()),
                                                  from subitem in item.SubItems
                                                  select new XElement("SubItem", subitem)
                                                  )
                              )
                 );
    }
  }
}
