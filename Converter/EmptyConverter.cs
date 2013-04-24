namespace RCPA.Proteomics.PropertyConverter
{
  public class EmptyConverter<T> : AbstractPropertyConverter<T>
  {
    public override string Name
    {
      get { return ""; }
    }

    public override string GetProperty(T t)
    {
      return "";
    }

    public override void SetProperty(T t, string value)
    {
    }
  }
}