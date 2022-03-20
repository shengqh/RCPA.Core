using System;
using System.Linq;
using System.Xml.Linq;

namespace RCPA.Gui
{
  public class OptionFileTextAdaptor<T> : AbstractOptionFileAdaptor
  {
    private T txtValue;

    private string key;

    private string defaultValue;

    protected Func<T, string> getFunc;

    protected Action<T, string> setFunc;

    public OptionFileTextAdaptor(T txtValue, string key, string defaultValue, Func<T, string> getFunc, Action<T, string> setFunc)
      : base(key)
    {
      this.txtValue = txtValue;
      this.key = key;
      this.defaultValue = defaultValue;
      this.getFunc = getFunc;
      this.setFunc = setFunc;
    }

    public override void LoadFromXml(XElement option)
    {
      var result =
        (from item in option.Descendants(key)
         select item).FirstOrDefault();

      if (null == result)
      {
        setFunc(txtValue, defaultValue);
      }
      else
      {
        setFunc(txtValue, result.Value);
      }
    }

    public override void SaveToXml(XElement option)
    {
      option.Add(new XElement(key, getFunc(txtValue)));
    }

  }
}
