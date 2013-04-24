using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace RCPA
{
  public class ObjectPool<T>
  {
    private ConcurrentBag<T> _objects;

    private Func<T> _objectGenerator;

    public ObjectPool(Func<T> objectGenerator)
    {
      if (objectGenerator == null)
        throw new ArgumentNullException("objectGenerator");
      _objects = new ConcurrentBag<T>();
      _objectGenerator = objectGenerator;
    }

    public T New()
    {
      T item;
      if (_objects.TryTake(out item))
        return item;
      return _objectGenerator();
    }

    public void Delete(T item)
    {
      _objects.Add(item);
    }
  }

}
