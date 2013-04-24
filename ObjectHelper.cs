using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace RCPA
{
  public static class ObjectHelper
  {
    private const int _seedPrimeNumber = 691;
    private const int _fieldPrimeNumber = 397;

    public static int GetHashCodeFromFields(this object obj, params object[] fields)
    {
      unchecked
      { //unchecked to prevent throwing overflow exception
        int hashCode = _seedPrimeNumber;
        for (int i = 0; i < fields.Length; i++)
          if (fields[i] != null)
            hashCode *= _fieldPrimeNumber + fields[i].GetHashCode();
        return hashCode;
      }
    }
  }
}
