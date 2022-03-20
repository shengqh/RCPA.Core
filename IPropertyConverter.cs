using System.Collections.Generic;
using System.Text;
namespace RCPA
{
  public interface IPropertyConverter<T>
  {
    string Version { get; }

    string Name { get; }

    void AddPropertyTo(StringBuilder sb, T t);

    string GetProperty(T t);

    void SetProperty(T t, string value);

    /// <summary>
    /// 根据实际数据，产生相关联的Converter
    /// </summary>
    /// <param name="items">实际数据列表</param>
    /// <returns>关联Converter列表，默认返回null</returns>
    List<IPropertyConverter<T>> GetRelativeConverter(List<T> items);

    /// <summary>
    /// 根据文件读取或者设定的header，产生相关联的Converter
    /// </summary>
    /// <param name="header">文件读取（或者设定的）header</param>
    /// <param name="delimiter">文件读取（或者设定的）分隔字符</param>
    /// <returns>关联Converter列表，默认返回null</returns>
    List<IPropertyConverter<T>> GetRelativeConverter(string header, char delimiter);

    bool HasName(string name);

    IPropertyConverter<T> GetConverter(string name);
  }

  public abstract class AbstractPropertyConverter<T> : IPropertyConverter<T>
  {
    #region IPropertyConverter<T> Members

    public virtual string Version
    {
      get { return ""; }
    }

    public abstract string Name { get; }

    public void AddPropertyTo(StringBuilder sb, T t)
    {
      sb.Append(GetProperty(t));
    }

    public abstract string GetProperty(T t);

    public abstract void SetProperty(T t, string value);

    public virtual bool HasName(string name)
    {
      return this.Name.Equals(name);
    }

    public virtual List<IPropertyConverter<T>> GetRelativeConverter(List<T> items)
    {
      return null;
    }

    public virtual List<IPropertyConverter<T>> GetRelativeConverter(string header, char delimiter)
    {
      return null;
    }

    public virtual IPropertyConverter<T> GetConverter(string name)
    {
      return this;
    }

    #endregion

    public override string ToString()
    {
      return this.Name;
    }
  }
}