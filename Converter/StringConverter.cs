using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace RCPA.Converter
{
  public class StringConverter<T> : AbstractPropertyNameConverter<T>
  {
    public StringConverter(string propertyName)
      : base(propertyName)
    { }

    public override void SetProperty(T t, string value)
    {
      pi.SetValue(t, value, null);
    }
  }
}
