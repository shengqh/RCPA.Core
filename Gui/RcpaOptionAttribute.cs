using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace RCPA.Gui
{
  public enum RcpaOptionType { Int32, Double, String, Boolean, StringArray, StringList, IXml };

  [AttributeUsageAttribute(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
  public class RcpaOptionAttribute : Attribute
  {
    public RcpaOptionAttribute(string name, RcpaOptionType valueType)
    {
      this.Name = name;
      this.ValueType = valueType;
    }

    public RcpaOptionAttribute(string name, RcpaOptionType valueType, Func<Object> allocate)
    {
      this.Name = name;
      this.ValueType = valueType;
      this.Allocate = allocate;
    }

    public string Name { get; private set; }

    public RcpaOptionType ValueType { get; private set; }

    public Func<Object> Allocate { get; private set; }
  }
}
