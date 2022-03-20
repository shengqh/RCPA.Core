namespace RCPA
{
  public interface IAnnotationPropertyConverter<T>
  {
    void AnnotationToProperty(IAnnotation ann, T t);
    void PropertyToAnnotation(T t, IAnnotation ann);
  }
}
