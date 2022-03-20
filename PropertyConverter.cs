using System;

namespace RCPA
{
  public class PropertyConverter<T> : AbstractPropertyConverter<T>
  {
    private string name;
    private Action<T, string> setFunc;
    private Func<T, string> getFunc;

    public PropertyConverter(string name, Action<T, string> setFunc, Func<T, string> getFunc)
    {
      this.name = name;
      this.setFunc = setFunc;
      this.getFunc = getFunc;
    }

    public override string Name
    {
      get { return this.name; }
    }

    public override string GetProperty(T t)
    {
      return getFunc(t);
    }

    public override void SetProperty(T t, string value)
    {
      setFunc(t, value);
    }
  }

  public class AliasPropertyConverter<T> : AbstractPropertyConverter<T>
  {
    private string name;
    private Func<string, bool> nameFunc;
    private Action<T, string> setFunc;
    private Func<T, string> getFunc;

    public AliasPropertyConverter(Func<string, bool> nameFunc, Action<T, string> setFunc, Func<T, string> getFunc)
    {
      this.name = string.Empty;
      this.nameFunc = nameFunc;
      this.setFunc = setFunc;
      this.getFunc = getFunc;
    }

    public override string Name
    {
      get { return this.name; }
    }

    public override string GetProperty(T t)
    {
      return getFunc(t);
    }

    public override void SetProperty(T t, string value)
    {
      setFunc(t, value);
    }

    public override bool HasName(string name)
    {
      if (nameFunc(name))
      {
        this.name = name;
        return true;
      }

      return false;
    }

    public override IPropertyConverter<T> GetConverter(string name)
    {
      return new PropertyConverter<T>(name, this.setFunc, this.getFunc);
    }
  }
}
