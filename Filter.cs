using System;
using System.Collections.Generic;

namespace RCPA
{
  public interface IFilter<T>
  {
    bool Accept(T t);
  }

  public class FilterTrue<T> : IFilter<T>
  {
    #region IFilter<T> Members

    public bool Accept(T t)
    {
      return true;
    }

    #endregion
  }

  public class FilterFalse<T> : IFilter<T>
  {
    #region IFilter<T> Members

    public bool Accept(T t)
    {
      return false;
    }

    #endregion
  }

  public class EqualsFilter<T> : IFilter<T>
  {
    private readonly T expect;

    public EqualsFilter(T expect)
    {
      this.expect = expect;
    }

    #region IFilter<T> Members

    public bool Accept(T t)
    {
      return this.expect.Equals(t);
    }

    #endregion
  }

  public class LargeThanFilter<T> : IFilter<T> where T : IComparable<T>
  {
    private readonly T expect;

    public LargeThanFilter(T expect)
    {
      this.expect = expect;
    }

    #region IFilter<T> Members

    public bool Accept(T t)
    {
      return t.CompareTo(this.expect) > 0;
    }

    #endregion
  }

  public class LargeThanOrEqualFilter<T> : IFilter<T> where T : IComparable<T>
  {
    private readonly T expect;

    public LargeThanOrEqualFilter(T expect)
    {
      this.expect = expect;
    }

    #region IFilter<T> Members

    public bool Accept(T t)
    {
      return t.CompareTo(this.expect) >= 0;
    }

    #endregion
  }

  public class LessThanFilter<T> : IFilter<T> where T : IComparable<T>
  {
    private readonly T expect;

    public LessThanFilter(T expect)
    {
      this.expect = expect;
    }

    #region IFilter<T> Members

    public bool Accept(T t)
    {
      return t.CompareTo(this.expect) < 0;
    }

    #endregion
  }

  public class LessThanOrEqualFilter<T> : IFilter<T> where T : IComparable<T>
  {
    private readonly T expect;

    public LessThanOrEqualFilter(T expect)
    {
      this.expect = expect;
    }

    #region IFilter<T> Members

    public bool Accept(T t)
    {
      return t.CompareTo(this.expect) <= 0;
    }

    #endregion
  }

  public class AndFilter<T> : IFilter<T>
  {
    private List<IFilter<T>> filters = new List<IFilter<T>>();

    public AndFilter(IEnumerable<IFilter<T>> filters)
    {
      this.filters.AddRange(filters);
    }

    public AndFilter()
    { }

    protected IEnumerable<IFilter<T>> Filters
    {
      get { return this.filters; }
    }

    public void AddFilter(IFilter<T> filter)
    {
      this.filters.Add(filter);
    }

    #region IFilter<T> Members

    public bool Accept(T t)
    {
      foreach (var filter in this.filters)
      {
        if (!filter.Accept(t))
        {
          return false;
        }
      }

      return true;
    }

    #endregion
  }

  public class OrFilter<T> : IFilter<T>
  {
    private readonly List<IFilter<T>> filters = new List<IFilter<T>>();

    public OrFilter(IEnumerable<IFilter<T>> filters)
    {
      this.filters.AddRange(filters);
    }

    protected IEnumerable<IFilter<T>> Filters
    {
      get { return this.filters; }
    }

    #region IFilter<T> Members

    public bool Accept(T t)
    {
      foreach (var filter in this.filters)
      {
        if (filter.Accept(t))
        {
          return true;
        }
      }

      return false;
    }

    #endregion
  }

  public class NotFilter<T> : IFilter<T>
  {
    private readonly List<IFilter<T>> filters = new List<IFilter<T>>();

    public NotFilter(IFilter<T> filter)
    {
      this.filters.Add(filter);
    }

    public NotFilter(List<IFilter<T>> filters)
    {
      this.filters.AddRange(filters);
    }

    #region IFilter<T> Members

    public bool Accept(T t)
    {
      foreach (var filter in this.filters)
      {
        if (filter.Accept(t))
        {
          return false;
        }
      }

      return true;
    }

    #endregion
  }
}