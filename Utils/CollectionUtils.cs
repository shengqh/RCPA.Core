using System;
using System.Collections.Generic;
using System.Linq;

namespace RCPA
{
  public static class CollectionUtils
  {
    public static bool ValueEquals<T>(List<T> obj1, List<T> obj2)
    {
      if ((Object)obj1 == null && (Object)obj2 == null)
      {
        return true;
      }

      if ((Object)obj1 == null || (Object)obj2 == null)
      {
        return false;
      }

      if (obj1.Count != obj2.Count)
      {
        return false;
      }

      for (int i = 0; i < obj1.Count; i++)
      {
        if (!Object.Equals(obj1[i], obj2[i]))
        {
          return false;
        }
      }

      return true;
    }

    public static bool ContainsAll<T>(List<T> obj1, List<T> obj2)
    {
      if ((Object)obj1 == null || (Object)obj2 == null)
      {
        return false;
      }

      if (obj1.Count < obj2.Count)
      {
        return false;
      }

      return obj2.All(m => obj1.Contains(m));
    }

    public static int FindMaxIndex(List<double> profile)
    {
      if (profile == null)
      {
        throw new ArgumentNullException("profile");
      }

      if (0 == profile.Count)
      {
        throw new ArgumentException("Argument is empty.", "profile");
      }

      double maxValue = profile.Max();

      return profile.FindIndex(m => m == maxValue);
    }

    public static Dictionary<TKey1, Dictionary<TKey2, TSource>> ToDoubleDictionary<TSource, TKey1, TKey2>(this IEnumerable<TSource> items, Func<TSource, TKey1> keySelector1, Func<TSource, TKey2> keySelector2)
    {
      var result = new Dictionary<TKey1, Dictionary<TKey2, TSource>>();

      foreach (var item in items)
      {
        TKey1 key1 = keySelector1(item);
        Dictionary<TKey2, TSource> map2;
        if (!result.TryGetValue(key1, out map2))
        {
          map2 = new Dictionary<TKey2, TSource>();
          result[key1] = map2;
        }

        map2[keySelector2(item)] = item;
      }

      return result;
    }

    //public static Dictionary<TKey1, Dictionary<TKey2, TSource>> ToDoubleDictionary<TSource, TKey1, TKey2>(this IEnumerable<TSource> items, Func<TSource, TKey1> keySelector1, Func<TSource, TKey2> keySelector2)
    //{
    //  var result = new Dictionary<TKey1, Dictionary<TKey2, TSource>>();

    //  foreach (var item in items)
    //  {
    //    TKey1 key1 = keySelector1(item);
    //    Dictionary<TKey2, TSource> map2;
    //    if (!result.TryGetValue(key1, out map2))
    //    {
    //      map2 = new Dictionary<TKey2, TSource>();
    //      result[key1] = map2;
    //    }

    //    map2[keySelector2(item)] = item;
    //  }

    //  return result;
    //}

    public static Dictionary<TKey1, Dictionary<TKey2, List<TSource>>> ToDoubleDictionaryGroup<TSource, TKey1, TKey2>(this IEnumerable<TSource> items, Func<TSource, TKey1> keySelector1, Func<TSource, TKey2> keySelector2)
    {
      var result = new Dictionary<TKey1, Dictionary<TKey2, List<TSource>>>();

      var groups = items.GroupBy(keySelector1);

      foreach (var group in groups)
      {
        result[group.Key] = group.GroupBy(keySelector2).ToDictionary(m => m.Key, m => m.ToList());
      }

      return result;
    }

    public static Dictionary<TKey, List<TSource>> ToGroupDictionary<TSource, TKey>(this IEnumerable<TSource> items, Func<TSource, TKey> keySelector)
    {
      var result = new Dictionary<TKey, List<TSource>>();
      foreach (var item in items)
      {
        var key = keySelector(item);
        List<TSource> lst;
        if (result.TryGetValue(key, out lst))
        {
          lst.Add(item);
        }
        else
        {
          lst = new List<TSource>();
          lst.Add(item);
          result[key] = lst;
        }
      }

      return result;
    }

    public static void Swap<T>(this List<T> lst, int from, int to)
    {
      if (from == to)
      {
        return;
      }

      T tmp = lst[from];
      lst[from] = lst[to];
      lst[to] = tmp;
    }
  }
}
