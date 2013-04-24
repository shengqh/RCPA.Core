using System.Collections.Generic;
using RCPA.Converter;
using System;
namespace RCPA
{
  public class LineFormat<T> where T : IAnnotation
  {
    public delegate T AllocateFunction();

    private IPropertyConverter<T> converter;
    private IPropertyConverterFactory<T> factory;

    public IPropertyConverter<T> Converter
    {
      get
      {
        return converter;
      }
    }

    public LineFormat(IPropertyConverterFactory<T> factory, string headers)
    {
      this.factory = factory;
      this.converter = factory.GetConverters(headers, '\t');
    }

    public LineFormat(IPropertyConverterFactory<T> factory, string headers, string engine)
    {
      this.factory = factory;
      this.converter = factory.GetConverters(headers, '\t', engine);
    }

    public LineFormat(IPropertyConverterFactory<T> factory, string headers, string engine, List<T> items)
    {
      this.factory = factory;
      this.converter = factory.GetConverters(headers, '\t', engine, items);
    }

    public T ParseString(string line)
    {
      T result = this.factory.Allocate();

      this.converter.SetProperty(result, line);

      return result;
    }

    public void ParseString(T result, string line)
    {
      this.converter.SetProperty(result, line);
    }

    public string GetHeader()
    {
      return this.converter.Name;
    }

    public string GetString(T t)
    {
      return this.converter.GetProperty(t);
    }
  }
}