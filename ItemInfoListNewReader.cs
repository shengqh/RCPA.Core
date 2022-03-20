using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace RCPA
{
  public class ItemInfoListNewReader : IFileReader<ItemInfoList>
  {
    private string key;

    public ItemInfoListNewReader(string key)
    {
      this.key = key;
    }

    public ItemInfoListNewReader()
    {
      this.key = null;
    }

    #region IFileReader<ItemInfoList> Members

    public ItemInfoList ReadFromFile(string fileName)
    {
      if (!File.Exists(fileName))
      {
        throw new FileNotFoundException(MyConvert.Format("File not found {0}", fileName));
      }

      XElement option = XElement.Load(fileName, LoadOptions.SetBaseUri);

      return ReadFromXml(option);
    }

    public ItemInfoList ReadFromXml(XElement option)
    {
      XElement keyElement;
      if (key == null || key == string.Empty)
      {
        keyElement = option.Elements().FirstOrDefault();
      }
      else
      {
        keyElement = option.Element(key);
      }

      if (keyElement == null)
      {
        return new ItemInfoList();
      }

      return
        new ItemInfoList((from item in keyElement.Descendants("Item")
                          select new ItemInfo()
                          {
                            Selected = Convert.ToBoolean(item.Attribute("Selected").Value),
                            SubItems =
                            (from subitem in item.Descendants("SubItem")
                             select subitem.Value).ToList()
                          }).ToList());
    }

    #endregion
  }
}
