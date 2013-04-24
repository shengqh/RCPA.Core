using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RCPA;

namespace System.Xml.Linq
{
  public static class XmlLinqExtension
  {
    public static string GetValue(this XElement ele)
    {
      if (ele != null && ele.FirstNode != null)
      {
        return ele.FirstNode.ToString();
      }

      return null;
    }

    public static void RemoveChild(this XElement parent, string childName)
    {
      var result = (from item in parent.Descendants(childName)
                    select item).ToList();

      result.ForEach(m => m.Remove());
    }

    public static string GetChildValue(this XElement parent, string childName)
    {
      var result = parent.Element(childName);

      if (null == result)
      {
        throw new Exception(MyConvert.Format("No child {0} exists", childName));
      }

      var node = result.FirstNode;

      if (null == node || node.NodeType == System.Xml.XmlNodeType.Element)
      {
        return string.Empty;
      }
      else if (node is XText)
      {
        return (node as XText).Value;
      }
      else
      {
        return node.ToString();
      }
    }

    public static string GetAttributeValue(this XElement parent, string attrName, string defaultValue)
    {
      var attr = parent.Attribute(attrName);
      if (attr == null)
      {
        return defaultValue;
      }
      else
      {
        return attr.Value;
      }
    }

    public static string GetChildValue(this XElement parent, string childName, string defaultValue)
    {
      var result = parent.Element(childName);

      if (null == result)
      {
        return defaultValue;
      }

      return result.Value;
    }

    public static int GetChildValue(this XElement parent, string childName, int defaultValue)
    {
      return int.Parse(GetChildValue(parent, childName, defaultValue.ToString()));
    }

    public static double GetChildValue(this XElement parent, string childName, double defaultValue)
    {
      return MyConvert.ToDouble(GetChildValue(parent, childName, defaultValue.ToString()));
    }

    public static bool GetChildValue(this XElement parent, string childName, bool defaultValue)
    {
      return bool.Parse(GetChildValue(parent, childName, defaultValue.ToString()));
    }

    public static XElement FindFirstChild(this XElement ele, string name)
    {
      if (ele.Name.LocalName == name)
      {
        return ele;
      }

      if (ele.HasElements)
      {
        foreach (var e in ele.Elements())
        {
          var ret = e.FindFirstChild(name);
          if (ret != null)
          {
            return ret;
          }
        }
      }

      return null;
    }

    public static XElement FindFirstChild(this XElement ele, string name, string attname, string attvalue)
    {
      if (ele.Name.LocalName == name)
      {
        var attr = ele.Attribute(attname);
        if (attr != null && attr.Value.Equals(attvalue))
        {
          return ele;
        }
      }

      if (ele.HasElements)
      {
        foreach (var e in ele.Elements())
        {
          var ret = e.FindFirstChild(name, attname, attvalue);
          if (ret != null)
          {
            return ret;
          }
        }
      }

      return null;
    }

    private static void DoFindChildren(this XElement ele, string name, List<XElement> result)
    {
      if (ele.Name.LocalName == name)
      {
        result.Add(ele);
        return;
      }

      if (ele.HasElements)
      {
        foreach (var e in ele.Elements())
        {
          e.DoFindChildren(name, result);
        }
      }
    }

    public static List<XElement> FindChildren(this XElement ele, string name)
    {
      List<XElement> result = new List<XElement>();

      ele.DoFindChildren(name, result);

      return result;
    }
  }
}
