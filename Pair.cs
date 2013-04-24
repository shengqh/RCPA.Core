using System;

namespace RCPA
{
  public class Pair<K, V> : IComparable<Pair<K, V>>
    where K : IComparable<K>
    where V : IComparable<V>
  {
    private K first;

    private V second;

    public Pair(K fst, V snd)
    {
      this.first = fst;
      this.second = snd;
    }

    public Pair(Pair<K, V> source)
    {
      this.first = source.first;
      this.second = source.second;
    }

    public K First
    {
      get { return this.first; }
      set { this.first = value; }
    }

    public V Second
    {
      get { return this.second; }
      set { this.second = value; }
    }

    #region IComparable<Pair<K,V>> Members

    public int CompareTo(Pair<K, V> other)
    {
      int result = this.first.CompareTo(other.first);
      if (result == 0)
      {
        result = this.second.CompareTo(other.second);
      }
      return result;
    }

    #endregion

    private String GetIdentity()
    {
      return "[" + this.first + " , " + this.second + "]";
    }

    public override string ToString()
    {
      return GetIdentity();
    }

    public override int GetHashCode()
    {
      return this.first.GetHashCode() ^ this.second.GetHashCode();
    }

    public override bool Equals(Object obj)
    {
      // If parameter is null return false.
      if (obj == null)
      {
        return false;
      }

      // If parameter cannot be cast to Point return false.
      var p = obj as Pair<K, V>;
      if (p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return Equals(this.first, p.first) && Equals(this.second, p.second);
    }

    public bool Equals(Pair<K, V> p)
    {
      // If parameter is null return false:
      if (p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return Equals(this.first, p.first) && Equals(this.second, p.second);
    }
  }
}