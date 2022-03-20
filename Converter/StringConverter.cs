namespace RCPA.Converter
{
  public class StringConverter<T> : AbstractPropertyNameConverter<T>
  {
    public StringConverter(string propertyName)
      : base(propertyName)
    { }

    public override void SetProperty(T t, string value)
    {
      pi.SetValue(t, value, null);
    }
  }
}
