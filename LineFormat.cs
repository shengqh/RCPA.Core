using RCPA.Converter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RCPA
{
  public class LineFormat<T> where T : IAnnotation
  {
    public delegate T AllocateFunction();

    public IPropertyConverter<T> Converter { get; private set; }

    public IPropertyConverterFactory<T> _factory;

    public IPropertyConverterFactory<T> Factory
    {
      get
      {
        return _factory;
      }
      set
      {
        _factory = value;
        this.Converter = _factory.GetConverters(this.Converter.Name, '\t');
      }
    }

    public LineFormat(IPropertyConverterFactory<T> factory, string headers, string engine = "")
    {
      _factory = factory;
      this.Converter = _factory.GetConverters(headers, '\t', engine);
    }

    public LineFormat(IPropertyConverterFactory<T> factory, string headers, string engine, List<T> items)
    {
      _factory = factory;
      this.Converter = _factory.GetConverters(headers, '\t', engine, items);
    }

    public T ParseString(string line)
    {
      T result = this.Factory.Allocate();

      this.Converter.SetProperty(result, line);

      return result;
    }

    public void Reset(IPropertyConverterFactory<T> factory, string headers)
    {
      _factory = factory;
      InitializeConverters(headers);
    }

    public void ParseString(T result, string line)
    {
      this.Converter.SetProperty(result, line);
    }

    public string GetHeader()
    {
      return Headers;
    }

    public string Headers
    {
      get
      {
        return this.Converter.Name;
      }
      set
      {
        InitializeConverters(value);
      }
    }

    private void InitializeConverters(string value)
    {
      this.Converter = this.Factory.GetConverters(value, '\t');
    }

    public string GetString(T t)
    {
      return this.Converter.GetProperty(t);
    }

    public LineFormat<T> GetLineFormat(string headers)
    {
      return new LineFormat<T>(this.Factory, headers);
    }

    public LineFormat<T> GetLineFormatWithIgnoreColumns(string[] ignoreKeys)
    {
      var headers = (from header in Headers.Split('\t')
                     where Array.IndexOf(ignoreKeys, header) == -1
                     select header).Merge("\t");
      return new LineFormat<T>(this.Factory, headers);
    }
  }
}