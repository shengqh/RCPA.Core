using System;
using System.Collections.Generic;

namespace RCPA
{
  public interface IProcessor<T>
  {
    T Process(T t);
  }

  public class EmptyProcessor<T> : IProcessor<T>
  {
    #region IProcessor<T> Members

    public T Process(T t)
    {
      return t;
    }

    #endregion

    public override string ToString()
    {
      return "EmptyProcessor";
    }
  }

  public class CompositeProcessor<T> : List<IProcessor<T>>, IProcessor<T>
  {
    #region IProcessor<T> Members

    public T Process(T t)
    {
      T next = t;
      foreach (var processor in this)
      {
        next = processor.Process(next);
        if (null == next)
        {
          break;
        }
      }

      return next;
    }

    #endregion

    public override string ToString()
    {
      List<string> result = new List<string>();
      foreach (var processor in this)
      {
        var names = processor.ToString().Split('\n');
        foreach (var name in names)
        {
          if (!result.Contains(name))
          {
            result.Add(name);
          }
        }
      }

      return result.Merge('\n');
    }
  }
}