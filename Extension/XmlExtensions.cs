using RCPA;
using System.Text;

namespace System.Xml
{
  public static class XmlExtensions
  {
    public static bool MoveToElement(this XmlReader xr, string localName)
    {
      if (!xr.IsStartElement(localName))
      {
        return xr.ReadToFollowing(localName);
      }

      return true;
    }

    public static void WriteElement(this XmlWriter xw, string localName, object value)
    {
      xw.WriteStartElement(localName);
      xw.WriteValue(value);
      xw.WriteEndElement();
    }

    public static void WriteElementFormat(this XmlWriter xw, string localName, string format, params object[] values)
    {
      WriteElement(xw, localName, MyConvert.Format(format, values));
    }

    public static void WriteAttribute(this XmlWriter xw, string localName, object value)
    {
      xw.WriteStartAttribute(localName);
      xw.WriteValue(value.ToString());
      xw.WriteEndAttribute();
    }

    public static void WriteAttributeFormat(this XmlWriter xw, string localName, string format, params object[] values)
    {
      WriteAttribute(xw, localName, MyConvert.Format(format, values));
    }

    public static int ReadElementAsInt(this XmlReader xw, string localName)
    {
      xw.ReadStartElement(localName);
      var result = xw.ReadContentAsInt();
      xw.ReadEndElement();
      return result;
    }

    public static double ReadElementAsDouble(this XmlReader xw, string localName)
    {
      xw.ReadStartElement(localName);
      var result = xw.ReadContentAsDouble();
      xw.ReadEndElement();
      return result;
    }

    public static bool ReadElementAsBoolean(this XmlReader xw, string localName)
    {
      xw.ReadStartElement(localName);
      var result = xw.ReadContentAsBoolean();
      xw.ReadEndElement();
      return result;
    }

    public static string ReadElementAsString(this XmlReader xw, string localName)
    {
      xw.ReadStartElement(localName);
      var result = xw.ReadContentAsString();
      xw.ReadEndElement();
      return result;
    }

    public static int ReadAttributeAsInt(this XmlReader xw, string localName)
    {
      return int.Parse(xw.GetAttribute(localName));
    }

    public static double ReadAttributeAsDouble(this XmlReader xw, string localName)
    {
      return MyConvert.ToDouble(xw.GetAttribute(localName));
    }

    public static bool ReadAttributeAsBoolean(this XmlReader xw, string localName)
    {
      return bool.Parse(xw.GetAttribute(localName));
    }

    public static string ReadAttributeAsString(this XmlReader xw, string localName)
    {
      return xw.GetAttribute(localName);
    }
  }
}
