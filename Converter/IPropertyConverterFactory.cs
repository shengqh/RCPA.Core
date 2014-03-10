using System.Collections.Generic;
using System.Text.RegularExpressions;
using RCPA.Proteomics.PropertyConverter;

namespace RCPA.Converter
{
  public interface IPropertyConverterFactory<T> where T : IAnnotation
  {
    IPropertyConverter<T> FindConverter(string name);

    IPropertyConverter<T> FindConverter(string name, string version);

    IPropertyConverter<T> GetConverters(string header, char delimiter, string version = "");

    IPropertyConverter<T> GetConverters(string header, char delimiter, string version, List<T> items);

    T Allocate();
  }

  public abstract class AbstractPropertyConverterFactory<T> : IPropertyConverterFactory<T> where T : IAnnotation
  {
    #region IPropertyConverterFactory<T> Members

    public virtual IPropertyConverter<T> FindConverter(string name)
    {
      return FindConverter(name, "");
    }

    public abstract IPropertyConverter<T> FindConverter(string name, string version);

    public abstract IPropertyConverter<T> GetConverters(string header, char delimiter, string version);

    public abstract IPropertyConverter<T> GetConverters(string header, char delimiter, string version, List<T> items);

    public abstract T Allocate();

    #endregion
  }
}