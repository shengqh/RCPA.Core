namespace RCPA
{
  public interface IParser<TSource, TTarget>
  {
    TTarget GetValue(TSource obj);
  }

  public interface IStringParser<TSource> : IParser<TSource, string>
  { }
}