using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace RCPA.Utils
{
  public static class BeanUtils
  {
    public static void CopyPropeties(object source, object target)
    {
      PropertyInfo[] sProperties = source.GetType().GetProperties();

      var tMap = target.GetType().GetProperties().ToDictionary(m => m.Name);

      foreach (PropertyInfo pi in sProperties)
      {
        if (pi.CanRead && tMap.ContainsKey(pi.Name) && tMap[pi.Name].CanWrite)
        {
          tMap[pi.Name].SetValue(target, pi.GetValue(source, null), null);
        }
      }

      return;
    }

  }
}
