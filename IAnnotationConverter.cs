using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCPA
{
  public interface IAnnotationPropertyConverter<T>
  {
    void AnnotationToProperty(IAnnotation ann, T t);
    void PropertyToAnnotation(T t, IAnnotation ann);
  }
}
