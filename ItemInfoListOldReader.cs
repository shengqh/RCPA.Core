using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Configuration;
using RCPA.Utils;

namespace RCPA
{
  public class ItemInfoListOldReader : IFileReader<ItemInfoList>
  {
    private string key;

    public ItemInfoListOldReader(string key)
    {
      this.key = key;
    }

    public ItemInfoListOldReader()
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

      return ReadFromXml(XElement.Load(fileName, LoadOptions.SetBaseUri));
    }

    #endregion

    public ItemInfoList ReadFromXml(XElement option)
    {
      if (key == null || key == string.Empty)
      {
        key = FindKey(option);
      }

      if (key == null || key == string.Empty)
      {
        return new ItemInfoList();
      }

      XElement keyNode = option.Element("appSettings");

      var result = new ItemInfoList();

      int count = GetValue(keyNode, key + "_count", 0);
      for (int i = 0; i < count; i++)
      {
        ItemInfo item = new ItemInfo();

        string curKey = key + "_" + i;

        item.SubItems.Add(GetValue(keyNode, curKey));

        int subCount = GetValue(keyNode, curKey + "_count", 0);
        for (int j = 1; j <= subCount; j++)
        {
          item.SubItems.Add(GetValue(keyNode, curKey + "_" + j, ""));
        }

        item.Selected = GetValue(keyNode, curKey + "_selected", false);

        result.Add(item);
      }

      return result;
    }

    private string FindKey(XElement option)
    {
      XElement keyNode = option.Element("appSettings");
      if (keyNode == null)
      {
        return null;
      }
      else
      {
        return
          (from add in keyNode.Descendants()
           let curkey = add.Attribute("key").Value
           where curkey.EndsWith("_count")
           select curkey.Substring(0, curkey.Length - 6)).FirstOrDefault();
      }
    }

    private static string GetValue(XElement keyNode, string curKey)
    {
      return (from add in keyNode.Descendants()
              let key = add.Attribute("key").Value
              where key == curKey
              select add.Attribute("value").Value).FirstOrDefault();
    }

    private static string GetValue(XElement keyNode, string curKey, string defaultValue)
    {
      var result = GetValue(keyNode, curKey);

      if (result == null)
      {
        return defaultValue;
      }

      return result;
    }

    private static int GetValue(XElement keyNode, string curKey, int defaultValue)
    {
      return Convert.ToInt32(GetValue(keyNode, curKey, defaultValue.ToString()));
    }

    private static bool GetValue(XElement keyNode, string curKey, bool defaultValue)
    {
      return Convert.ToBoolean(GetValue(keyNode, curKey, defaultValue.ToString()));
    }

  }
}
