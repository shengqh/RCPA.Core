using System.Linq;
using System.Xml.Linq;

namespace RCPA
{
  public abstract class AbstractOptionFileAdaptor : IOptionFile
  {
    private string key;

    public AbstractOptionFileAdaptor(string key)
    {
      this.key = key;
    }

    #region IOptionFile Members

    public virtual void RemoveFromXml(XElement option)
    {
      var result = option.Descendants(key).ToArray();

      if (result.Count() > 0)
      {
        foreach (var item in result)
        {
          item.Remove();
        }
      }
    }

    public abstract void LoadFromXml(XElement option);

    public abstract void SaveToXml(XElement option);

    #endregion
  }
}
