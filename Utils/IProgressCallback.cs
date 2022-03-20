namespace RCPA.Utils
{
  public interface IProgressCallback
  {
    bool IsCancellationPending();

    void Begin();

    void SetRange(long minimum, long maximum);

    void SetRange(int progressBarIndex, long minimum, long maximum);

    void Increment(long value);

    void Increment(int progressBarIndex, long value);

    void SetPosition(long position);

    void SetPosition(int progressBarIndex, long position);

    void SetMessage(string message);

    void SetMessage(string format, params object[] args);

    void SetMessage(int labelIndex, string message);

    void SetMessage(int labelIndex, string format, params object[] args);

    void End();

    bool IsConsole();
  }

  public abstract class AbstractProgressCallback : IProgressCallback
  {
    #region IProgressCallback Members

    public abstract bool IsCancellationPending();

    public abstract void Begin();

    public virtual void SetRange(long minimum, long maximum)
    {
      SetRange(0, minimum, maximum);
    }

    public abstract void SetRange(int progressBarIndex, long minimum, long maximum);

    public virtual void Increment(long value)
    {
      Increment(0, value);
    }

    public abstract void Increment(int progressBarIndex, long value);

    public virtual void SetPosition(long position)
    {
      SetPosition(0, position);
    }

    public abstract void SetPosition(int progressBarIndex, long position);

    public virtual void SetMessage(string message)
    {
      SetMessage(0, message);
    }

    public virtual void SetMessage(string format, params object[] args)
    {
      SetMessage(0, format, args);
    }

    public abstract void SetMessage(int labelIndex, string message);

    public virtual void SetMessage(int labelIndex, string format, params object[] args)
    {
      SetMessage(labelIndex, MyConvert.Format(format, args));
    }

    public abstract void End();

    public virtual bool IsConsole()
    {
      return true;
    }

    #endregion
  }

  public class EmptyProgressCallback : AbstractProgressCallback
  {
    #region IProgressCallback Members

    public override void Begin() { }

    public override bool IsCancellationPending()
    {
      return false;
    }

    public override void SetRange(long minimum, long maximum) { }

    public override void SetRange(int progressBarIndex, long minimum, long maximum) { }

    public override void SetPosition(long position) { }

    public override void SetPosition(int progressBarIndex, long position) { }

    public override void Increment(int progressBarIndex, long value) { }

    public override void SetMessage(int labelIndex, string message) { }

    public override void End() { }

    #endregion
  }
}
