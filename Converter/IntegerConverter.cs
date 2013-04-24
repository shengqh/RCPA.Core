﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace RCPA.Converter
{
  public class IntegerConverter<T> : AbstractPropertyNameConverter<T>
  {
    public IntegerConverter(string propertyName)
      : base(propertyName)
    { }

    public override void SetProperty(T t, string value)
    {
      int outValue;
      if (int.TryParse(value, out outValue))
      {
        pi.SetValue(t, outValue, null);
      }
    }
  }
}
