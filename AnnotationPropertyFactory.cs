using RCPA.Converter;
using System.Collections.Generic;

namespace RCPA
{
  public class AnnotationPropertyFactory : IPropertyConverterFactory<Annotation>
  {
    private AnnotationPropertyFactory() { }

    public static AnnotationPropertyFactory GetInstance()
    {
      return new AnnotationPropertyFactory();
    }

    public IPropertyConverter<Annotation> FindConverter(string name)
    {
      return new AnnotationConverter<Annotation>(name);
    }

    public IPropertyConverter<Annotation> FindConverter(string name, string version)
    {
      return new AnnotationConverter<Annotation>(name);
    }

    public IPropertyConverter<Annotation> GetConverters(string header, char delimiter)
    {
      string[] parts = header.Split(new char[] { delimiter });
      var result = new List<IPropertyConverter<Annotation>>();
      foreach (string part in parts)
      {
        result.Add(FindConverter(part));
      }

      return new CompositePropertyConverter<Annotation>(result, delimiter);
    }

    public IPropertyConverter<Annotation> GetConverters(string header, char delimiter, string version)
    {
      return GetConverters(header, delimiter);
    }

    public IPropertyConverter<Annotation> GetConverters(string header, char delimiter, string version, List<Annotation> items)
    {
      return GetConverters(header, delimiter);
    }

    Annotation IPropertyConverterFactory<Annotation>.Allocate()
    {
      return new Annotation();
    }
  }
}
