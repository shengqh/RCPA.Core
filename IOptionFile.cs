using System.Xml.Linq;

namespace RCPA
{
  public interface IOptionFile
  {
    void RemoveFromXml(XElement option);

    void LoadFromXml(XElement option);

    void SaveToXml(XElement option);
  }
}
