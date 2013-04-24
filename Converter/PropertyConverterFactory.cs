using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RCPA.Proteomics.PropertyConverter;

namespace RCPA.Converter
{
  public abstract class PropertyConverterFactory<T> : IPropertyConverterFactory<T> where T : IAnnotation
  {
    protected HashSet<string> _ignoreKey = new HashSet<string>();

    protected List<IPropertyConverter<T>> itemMap = new List<IPropertyConverter<T>>();

    public void RegisterConverter(IPropertyConverter<T> item)
    {
      itemMap.Add(item);
    }

    public void RegisterConverter(IPropertyConverter<T> item, params string[] aliasNames)
    {
      itemMap.Add(item);
      foreach (var name in aliasNames)
      {
        itemMap.Add(new PropertyAliasConverter<T>(item, name));
      }
    }

    public IPropertyConverter<T> FindConverter(string name, string version)
    {
      var result = itemMap.Find(m => m.HasName(name) && m.Version == version);
      if (result != null)
      {
        return result.GetConverter(name);
      }

      result = itemMap.Find(m => m.HasName(name));
      if (result != null)
      {
        return result.GetConverter(name);
      }

      if (name.Trim().Length == 0)
      {
        return new EmptyConverter<T>();
      }

      return new AnnotationConverter<T>(name);
    }

    public virtual IPropertyConverter<T> FindConverter(string name)
    {
      return FindConverter(name, "");
    }

    /// <summary>
    /// 从标题出发创建converter，用于读取文件。
    /// 首先根据header产生所有原始的converter以及其衍生的converter。
    /// 对于任意一个衍生converter，如果找到与其名字相同的原始converter，
    /// 同时原始的converter是AnnotationConverter，则用衍生converter替换掉原始converter。 
    /// </summary>
    /// <param name="header"></param>
    /// <param name="delimiter"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    public IPropertyConverter<T> GetConverters(string header, char delimiter, string version)
    {
      string[] parts = header.Split(new char[] { delimiter });
      var result = new List<IPropertyConverter<T>>();
      foreach (string part in parts)
      {
        if (_ignoreKey.Contains(part))
        {
          continue;
        }

        var conv = FindConverter(part, version);
        result.Add(conv);
      }

      var relativeConvs = new List<IPropertyConverter<T>>();
      foreach (var conv in result)
      {
        var ret = conv.GetRelativeConverter(header, delimiter);
        if (ret != null)
        {
          relativeConvs.AddRange(ret);
        }
      }

      foreach (var conv in relativeConvs)
      {
        for (int i = 0; i < result.Count; i++)
        {
          if ((result[i] is AnnotationConverter<T>) && result[i].Name.Equals(conv.Name))
          {
            result[i] = conv;
            break;
          }
        }
      }

      return new CompositePropertyConverter<T>(result, delimiter);
    }

    /// <summary>
    /// 用于从真实数据出发，创建converter。
    /// 首先根据header产生所有可能的converter以及其衍生的converter。
    /// 然后如果找到名字相同的两个converter（c1和c2）：
    /// 1，c1是AnnotationConverter
    /// 1.1，c2是AnnotationConverter，删除c2
    /// 1.2，否则，删除c1
    /// 2，否则，删除c2
    /// </summary>
    /// <param name="header"></param>
    /// <param name="delimiter"></param>
    /// <param name="version"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    public IPropertyConverter<T> GetConverters(string header, char delimiter, string version, List<T> items)
    {
      string[] parts = header.Split(new char[] { delimiter });
      var result = new List<IPropertyConverter<T>>();
      foreach (string part in parts)
      {
        var conv = FindConverter(part, version);
        result.Add(conv);

        var ret = conv.GetRelativeConverter(items);
        if (ret != null)
        {
          result.AddRange(ret);
        }
      }

      var removed = new List<int>();
      for (int i = 0; i < result.Count; i++)
      {
        var conv1 = result[i];
        for (int j = i + 1; j < result.Count; j++)
        {
          var conv2 = result[j];
          if (conv1.Name.Equals(conv2.Name))
          {
            if (conv1 is AnnotationConverter<T>)
            {
              if (conv2 is AnnotationConverter<T>)
              {
                removed.Add(j);
              }
              else
              {
                removed.Add(i);
                break;
              }
            }
            else
            {
              removed.Add(j);
            }
          }
        }
      }

      removed = (from m in removed
                 orderby m descending
                 select m).Distinct().ToList();

      removed.ForEach(m => result.RemoveAt(m));

      result.RemoveAll(m => _ignoreKey.Contains(m.Name));

      return new CompositePropertyConverter<T>(result, delimiter);
    }

    public IPropertyConverter<T> GetConverters(string header, char delimiter)
    {
      return GetConverters(header, delimiter, "");
    }

    public abstract T Allocate();
  }
}