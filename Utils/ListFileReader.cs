using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace RCPA.Utils
{
  /// <summary>
  /// Read string list from *.lst file, no matter the key
  /// </summary>
  public class ListFileReader : IFileReader<List<string>>
  {
    private static string OldKey = "appSettings";

    #region IFileReader<List<string>> Members

    public List<string> ReadFromFile(string fileName)
    {
      if (!File.Exists(fileName))
      {
        throw new FileNotFoundException(MyConvert.Format("List file {0} not exists", fileName));
      }

      XElement root = XElement.Load(fileName);

      XElement keyNode = root.Elements().FirstOrDefault();

      List<string> result;

      string xname = keyNode.Name.LocalName;

      if (xname.Equals(OldKey))
      {
        var countElement =
          (from add in keyNode.Descendants()
           where add.Attribute("key").Value.EndsWith("_count")
           select add).First();

        var count = Convert.ToInt32(countElement.Attribute("value").Value);

        var key_count = countElement.Attribute("key").Value;

        var key = key_count.Substring(0, key_count.Length - 6);

        Regex keyRegex = new Regex(key + @"_\d+$");

        result =
          (from add in keyNode.Descendants()
           let curkey = add.Attribute("key").Value
           where keyRegex.Match(curkey).Success
           select add.Attribute("value").Value).ToList();
      }
      else
      {
        SimpleItemInfos infos = new SimpleItemInfos();

        OptionFileItemInfosAdaptor adaptor = new OptionFileItemInfosAdaptor(infos, xname);

        adaptor.LoadFromXml(XElement.Load(fileName));

        result = infos.Items.GetAllItems().ToList();
      }

      return result;
    }

    #endregion
  }
}
