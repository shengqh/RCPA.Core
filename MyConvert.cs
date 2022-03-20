using System;
using System.Globalization;

namespace RCPA
{
  public static class MyConvert
  {
    public static double ToDouble(string value)
    {
      return Convert.ToDouble(value, CultureInfo.InvariantCulture);
    }

    public static double ToDouble(object value)
    {
      return Convert.ToDouble(Format("{0}", value), CultureInfo.InvariantCulture);
    }

    public static double ToDouble(object value, double defaultValue)
    {
      double result;
      var f = Format("{0}", value);
      if (!double.TryParse(f, out result))
      {
        result = defaultValue;
      }

      return result;
    }

    public static int ToInt(object value, int defaultValue)
    {
      int result;
      var f = Format("{0}", value);
      if (!int.TryParse(f, out result))
      {
        result = defaultValue;
      }

      return result;
    }

    public static string Format(object arg)
    {
      return string.Format(CultureInfo.InvariantCulture, "{0}", arg);
    }

    public static string Format(string format, params object[] args)
    {
      return string.Format(CultureInfo.InvariantCulture, format, args);
    }

    public static bool TryParse(string value, out double result)
    {
      return double.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out result);
    }

    public static bool TryParse(object p, out double result)
    {
      return TryParse(Format("{0}", p), out result);
    }
  }
}
