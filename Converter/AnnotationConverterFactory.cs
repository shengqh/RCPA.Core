using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCPA.Converter
{
  public class AnnotationConverterFactory : PropertyConverterFactory<Annotation>
  {
    public override Annotation Allocate()
    {
      return new Annotation();
    }
  }
}
