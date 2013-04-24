using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
using System.Xml.Linq;

namespace RCPA.Utils
{
  public class XmlHelper
  {
    public readonly string PREFIX = "nuri";

    private char[] splitParam = new char[] { '/' };

    private XmlNamespaceManager namespaceManager = null;

    public XmlNamespaceManager NamespaceManager { get { return namespaceManager; } }

    public XmlHelper(XmlDocument doc)
    {
      namespaceManager = new XmlNamespaceManager(doc.NameTable);
      namespaceManager.AddNamespace(PREFIX, doc.DocumentElement.NamespaceURI);
    }

    public XmlNode GetFirstChildByXPath(XmlNode parent, string xpath)
    {
      return parent.SelectSingleNode(xpath, namespaceManager);
    }

    public List<XmlNode> GetChildrenByXPath(XmlNode parent, string xpath)
    {
      List<XmlNode> result = new List<XmlNode>();

      XmlNodeList lst = parent.SelectNodes(xpath, namespaceManager);

      foreach (XmlNode node in lst)
      {
        result.Add(node);
      }

      return result;
    }

    private string GetNameXPath(string childName)
    {
      string result;
      if (childName.Contains("/"))
      {
        string[] childNames = childName.Split(splitParam);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < childNames.Length; i++)
        {
          if (i != 0)
          {
            sb.Append("/");
          }
          sb.Append(PREFIX).Append(":").Append(childNames[i]);
        }

        result = sb.ToString();
      }
      else
      {
        result = MyConvert.Format("{0}:{1}", PREFIX, childName);
      }

      return result;
    }

    private string GetNameAttributeXPath(string childName, string attributeName, string attributeValue)
    {
      return MyConvert.Format("{0}[@{1}=\"{2}\"]", GetNameXPath(childName), attributeName, attributeValue);
    }

    public XmlNode GetFirstChild(XmlNode parent, string childName)
    {
      return GetFirstChildByXPath(parent, GetNameXPath(childName));
    }

    public bool HasChild(XmlNode parent, string childName)
    {
      return null != GetFirstChild(parent, childName);
    }

    public List<XmlNode> GetChildren(XmlNode parent, string childName)
    {
      return GetChildrenByXPath(parent, GetNameXPath(childName));
    }

    public XmlNode GetFirstChildByNameAndAttribute(XmlNode parent, string childName, string attributeName, string attributeValue)
    {
      string xpath = GetNameAttributeXPath(childName, attributeName, attributeValue);
      return GetFirstChildByXPath(parent, xpath);
    }

    public List<XmlNode> GetChildrenByNameAndAttribute(XmlNode parent, string childName, string attributeName, string attributeValue)
    {
      string xpath = GetNameAttributeXPath(childName, attributeName, attributeValue);
      return GetChildrenByXPath(parent, xpath);
    }

    public string GetChildValue(XmlNode parent, string childName)
    {
      return GetValidChild(parent, childName).InnerText;
    }

    public string GetChildValue(XmlNode parent, string childName, string defaultValue)
    {
      XmlNode result = GetFirstChild(parent, childName);
      if (result != null)
      {
        return result.InnerText;
      }

      return defaultValue;
    }

    public XmlNode GetValidChild(XmlNode parent, string childName)
    {
      XmlNode result = GetFirstChild(parent, childName);

      if (result == null)
      {
        throw new ArgumentException("Cannot find child " + childName + " of " + parent.Name + " at Xml document");
      }

      return result;
    }
  }
}
