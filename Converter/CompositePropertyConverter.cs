using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RCPA.Utils;
using System.Text;

namespace RCPA.Converter
{
  public class CompositePropertyConverter<T> : AbstractPropertyConverter<T>
  {
    private readonly char[] delimiter;
    private List<IPropertyConverter<T>> itemList = new List<IPropertyConverter<T>>();

    public List<IPropertyConverter<T>> ItemList
    {
      get
      {
        return itemList;
      }
    }

    private readonly string name;

    public CompositePropertyConverter(IEnumerable<IPropertyConverter<T>> items, char delimiter)
    {
      var nameList = new List<string>();
      foreach (AbstractPropertyConverter<T> item in items)
      {
        this.itemList.Add(item);
        nameList.Add(item.Name);
      }

      this.delimiter = new char[] { delimiter };
      this.name = StringUtils.Merge(nameList, delimiter);
    }

    public override string Name
    {
      get { return this.name; }
    }

    public override string GetProperty(T t)
    {
      StringBuilder sb = new StringBuilder();
      bool bFirst = true;

      foreach (var item in this.itemList)
      {
        if (bFirst)
        {
          bFirst = false;
        }
        else
        {
          sb.Append(delimiter);
        }
        item.AddPropertyTo(sb, t);
      }
      return sb.ToString();
    }

    public override void SetProperty(T t, string value)
    {
      string[] parts = value.Split(this.delimiter);
      if (parts.Length < this.itemList.Count)
      {
        throw new ArgumentException(
          MyConvert.Format("The property list ({0} items) is not match to title list ({1} items).\nTitle={2}\nInput property={3}", parts.Length, this.itemList.Count, this.name, value));
      }

      for (int i = 0; i < this.itemList.Count; i++)
      {
        this.itemList[i].SetProperty(t, parts[i]);
      }
    }

    public override bool HasName(string name)
    {
      foreach (var item in this.itemList)
      {
        if (item.HasName(name))
        {
          return true;
        }
      }
      return false;
    }

    public override List<IPropertyConverter<T>> GetRelativeConverter(List<T> items)
    {
      List<IPropertyConverter<T>> result = new List<IPropertyConverter<T>>();

      foreach (var conv in this.itemList)
      {
        var ret = conv.GetRelativeConverter(items);
        if (ret != null)
        {
          result.AddRange(ret);
        }
      }

      if (result.Count > 0)
      {
        return result;
      }

      return null;
    }

    public override List<IPropertyConverter<T>> GetRelativeConverter(string header, char delimiter)
    {
      List<IPropertyConverter<T>> result = new List<IPropertyConverter<T>>();

      foreach (var conv in this.itemList)
      {
        var ret = conv.GetRelativeConverter(header, delimiter);
        if (ret != null)
        {
          result.AddRange(ret);
        }
      }

      if (result.Count > 0)
      {
        return result;
      }

      return null;
    }
  }
}