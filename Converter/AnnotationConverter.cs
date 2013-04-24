namespace RCPA.Converter
{
  public class AnnotationConverter<T> : AbstractPropertyConverter<T> where T : IAnnotation
  {
    private readonly string defaultValue;
    private readonly string key;

    public AnnotationConverter(string key, string defaultValue = "")
    {
      this.key = key;
      this.defaultValue = defaultValue;
    }

    public override string Name
    {
      get { return this.key; }
    }

    public override string GetProperty(T t)
    {
      if (t.Annotations.ContainsKey(this.key))
      {
        return MyConvert.Format(t.Annotations[this.key]);
      }
      return this.defaultValue;
    }

    public override void SetProperty(T t, string value)
    {
      t.Annotations[this.key] = value;
    }

    public override string ToString()
    {
      return "AnnotationConverter " + key;
    }
  }
}