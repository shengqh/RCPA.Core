using System.Text;
using System.Xml;

namespace RCPA.Utils
{
  public static class XmlUtils
  {
    public static XmlTextWriter CreateWriter(string fileName, Encoding encoding)
    {
      XmlTextWriter result;
      if (null == encoding)
      {
        result = CreateWriter(fileName);
      }
      else
      {
        result = new XmlTextWriter(fileName, encoding);
      }
      result.Formatting = Formatting.Indented;
      return result;
    }

    public static XmlTextWriter CreateWriter(string fileName)
    {
      var result = new XmlTextWriter(fileName, new UTF8Encoding(false));
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
