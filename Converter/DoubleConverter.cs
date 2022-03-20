namespace RCPA.Converter
{
  public class DoubleConverter<T> : AbstractPropertyNameConverter<T>
  {
    private string format;

    public DoubleConverter(string propertyName, string format = "")
      : base(propertyName)
    {
      this.format = format;
      if (!format.Contains("{0"))
      {
        this.format = "{0}";
      }
    }

    public override string GetProperty(T t)
    {
      var d = pi.GetValue(t, null);
      return MyConvert.Format(format, d);
    }

    public override void SetProperty(T t, string value)
    {
      double outValue;
      if (MyConvert.TryParse(value, out outValue))
      {
        pi.SetValue(t, outValue, null);
      }
    }
  }
}
