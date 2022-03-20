using System;
using System.Reflection;

namespace RCPA.Converter
{
  public abstract class AbstractPropertyNameConverter<T> : AbstractPropertyConverter<T>
  {
    protected string propertyName;
    protected PropertyInfo pi;

    public AbstractPropertyNameConverter(string propertyName)
    {
      this.propertyName = propertyName;
      this.pi = typeof(T).GetProperty(propertyName);
      if (this.pi == null)
      {
        throw new ArgumentException("There is no property named as " + propertyName + " in class " + typeof(T).FullName);
      }
    }

    public override string Name
    {
      get { return propertyName; }
    }

    public override string GetProperty(T t)
    {
      var d = pi.GetValue(t, null);
      return d == null ? string.Empty : d.ToString();
    }
  }
}
