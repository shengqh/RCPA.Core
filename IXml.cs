using System.Xml.Linq;

namespace RCPA
{
  public interface IXml
  {
    void Save(XElement parentNode);

    void Load(XElement parentNode);
  }

  public static class IXmlExtension
  {
    public static void SaveToFile(this IXml xml, string fileName)
    {
      XElement ele = new XElement("Root");
      xml.Save(ele);
      ele.Save(fileName);
    }

    public static void LoadFromFile(this IXml xml, string fileName)
    {
      XElement ele = XElement.Load(fileName);
      xml.Load(ele);
    }
  }
}
