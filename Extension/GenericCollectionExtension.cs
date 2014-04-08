using System;
using System.Collections.Generic;

namespace System
{
  public static class GenericCollectionExtension
  {
    public static void ForEach<T>(this T[] items, Action<T> action)
    {
      foreach (var item in items)
      {
        action(item);
      }
    }

    public static void ForEach<T, V>(this Dictionary<T, V> items, Action<KeyValuePair<T, V>> action)
    {
      foreach (var item in items)
      {
        action(item);
      }
    }
  }
}
