using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RCPA.Format;
using RCPA.Converter;

namespace RCPA.Format
{
  public class AnnotationToPropertyConverter<T> : AbstractPropertyConverter<T>
  {
    public FileDefinitionItem Item { get; set; }

    private IPropertyConverter<T> baseConverter;

    public AnnotationToPropertyConverter(FileDefinitionItem item)
    {
      this.Item = item;

      var valueType = item.ValueType.ToLower();
      if (valueType.Equals("double"))
      {
        this.baseConverter = new DoubleConverter<T>(item.PropertyName, item.Format);
      }
      else if (valueType.Equals("int") || valueType.Equals("integer"))
      {
        this.baseConverter = new IntegerConverter<T>(item.PropertyName);
      }
      else if (valueType.Equals("bool") || valueType.Equals("boolean"))
      {
        this.baseConverter = new BooleanConverter<T>(item.PropertyName);
      }
      else
      {
        this.baseConverter = new StringConverter<T>(item.PropertyName);
      }
    }

    public override string Name
    {
      get { return Item.AnnotationName; }
    }

    public override string GetProperty(T t)
    {
      return baseConverter.GetProperty(t);
    }

    public override void SetProperty(T t, string value)
    {
      baseConverter.SetProperty(t, value);
    }

    public bool AnnotationToProperty(IAnnotation ann, T t)
    {
      if (ann.Annotations.ContainsKey(Item.AnnotationName))
      {
        var value = MyConvert.Format(ann.Annotations[Item.AnnotationName]);
        SetProperty(t, value);
        return true;
      }
      else if (Item.Required)
      {
        return false;
      }
      else
      {
        return true;
      }
    }

    public void PropertyToAnnotation(T t, IAnnotation ann)
    {
      var value = GetProperty(t);
      ann.Annotations[Item.AnnotationName] = value;
    }
  }
}
