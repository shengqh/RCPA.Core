using System;

namespace RCPA.Gui
{
  public enum RcpaOptionType { Int32, Double, String, Boolean, StringArray, StringList, IXml };

  [AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
  public class RcpaOption : Attribute
  {
    public RcpaOption(string name, RcpaOptionType valueType)
    {
      this.Name = name;
      this.ValueType = valueType;
    }

    public RcpaOption(string name, RcpaOptionType valueType, Func<Object> allocate)
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
