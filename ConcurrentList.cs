using System.Collections.Generic;
using System.Threading;

namespace RCPA
{
  public class ConcurrentList<T>
  {
    private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

    private List<T> _objects = new List<T>();

    public void Add(T t)
    {
      _lock.EnterWriteLock();
      try
      {
        _objects.Add(t);
      }
      finally
      {
        _lock.ExitWriteLock();
      }
    }

    public void Remove(T t)
    {
      _lock.EnterWriteLock();
      try
      {
        _objects.Remove(t);
      }
      finally
      {
        _lock.ExitWriteLock();
      }
    }

    public int Count
    {
      get
      {
        _lock.EnterReadLock();
        try
        {
          return _objects.Count;
        }
        finally
        {
          _lock.ExitReadLock();
        }
      }
    }

    public IEnumerable<T> GetCopy()
    {
      return new List<T>(_objects);
    }
  }
}
