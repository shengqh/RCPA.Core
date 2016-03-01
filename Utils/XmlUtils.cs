using System.Text;
using System.Xml;

namespace RCPA.Utils
{
  public static class XmlUtils
  {
    public static XmlTextWriter CreateWriter(string fileName, Encoding encoding = null)
    {
      var enc = null == encoding ? new UTF8Encoding(false) : encoding;
      var result = new XmlTextWriter(fileName, enc);
      result.Formatting = Formatting.Indented;
      return result;
    }

    public static string ToXml(string value)
    {
      if (null == value)
      {
        return string.Empty;
      }
      return value;
    }
  }
}
