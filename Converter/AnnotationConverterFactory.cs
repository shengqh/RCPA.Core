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
