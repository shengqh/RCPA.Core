using System;
using System.Text.RegularExpressions;
namespace RCPA.Converter
{
  public class PropertyAliasConverter<T> : AbstractPropertyConverter<T>
  {
    private readonly string aliasName;
    private readonly IPropertyConverter<T> source;

    private readonly string version;

    public PropertyAliasConverter(IPropertyConverter<T> source, string aliasName)
    {
      this.source = source;
      this.aliasName = aliasName;

      InitializeAliasName(aliasName);
    }

    private Func<string, bool> hasNameFunc;
    private Regex reg;

    public PropertyAliasConverter(IPropertyConverter<T> source, string aliasName, string version)
    {
      this.source = source;
      this.aliasName = aliasName;
      this.version = version;

      InitializeAliasName(aliasName);
    }

    private void InitializeAliasName(string aliasName)
    {
      if (aliasName.StartsWith("REGEX::"))
      {
        reg = new Regex(aliasName.Substring(7));
        hasNameFunc = m => reg.Match(m).Success;
      }
      else
      {
        hasNameFunc = m => m.Equals(this.aliasName);
      }
    }

    public override string Name
    {
      get { return this.aliasName; }
    }

    public override bool HasName(string name)
    {
      return hasNameFunc(name);
    }

    public override string Version
    {
      get
      {
        if (null != this.version)
        {
          return this.version;
        }
        else
        {
          return base.Version;
        }
      }
    }

    public override string GetProperty(T t)
    {
      return this.source.GetProperty(t);
    }

    public override void SetProperty(T t, string value)
    {
      this.source.SetProperty(t, value);
    }

    public override string ToString()
    {
      return source.ToString() + " alias as " + this.aliasName;
    }
  }
}