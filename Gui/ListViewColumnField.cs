using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RCPA.Gui
{
  public class ListViewColumnField : IRcpaComponent
  {
    private ListView lvValue;

    private string key;

    public ListViewColumnField(ListView lvValue, string key)
    {
      this.lvValue = lvValue;
      this.key = key;
    }

    #region IRcpaComponent Members

    public void ValidateComponent()
    {
    }

    #endregion

    #region IOptionFile Members

    public void RemoveFromXml(System.Xml.Linq.XElement option)
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

    public void LoadFromXml(System.Xml.Linq.XElement option)
    {
      var keyele = option.Element(key);
      if (keyele == null)
      {
        return;
      }

      lvValue.Columns.Clear();

      var eles = keyele.Elements("column");
      if (eles != null)
      {
        foreach (var ele in eles)
        {
          var name = ele.GetChildValue("name");
          int width = Convert.ToInt32(ele.GetChildValue("width"));
          lvValue.Columns.Add(name).Width = width;
        }
      }
    }

    public void SaveToXml(System.Xml.Linq.XElement option)
    {
      var keyele = new XElement(key);
      option.Add(keyele);

      foreach (ColumnHeader col in lvValue.Columns)
      {
        keyele.Add(new XElement("column", new XElement("name", col.Text), new XElement("width", col.Width)));
      }
    }

    #endregion
  }
}
