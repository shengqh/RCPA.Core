namespace RCPA.Converter
{
  public class BooleanConverter<T> : AbstractPropertyNameConverter<T>
  {
    public BooleanConverter(string propertyName)
      : base(propertyName)
    { }

    public override void SetProperty(T t, string value)
    {
      bool outValue;
      if (bool.TryParse(value, out outValue))
      {
        pi.SetValue(t, outValue, null);
      }
    }
  }
}
