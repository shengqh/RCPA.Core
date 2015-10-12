using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCPA
{
  public abstract class AbstractParallelTaskFileProcessor : AbstractThreadFileProcessor, IParallelTaskFileProcessor
  {
    public AbstractParallelTaskFileProcessor()
    {
      PrefixMessage = string.Empty;
    }

    public ParallelLoopState LoopState { get; set; }

    public long From { get; set; }

    public long To { get; set; }

    public long Position { get; set; }

    public string PrefixMessage { get; set; }

    protected virtual bool IsLoopStopped
    {
      get
      {
        return null != LoopState && LoopState.IsStopped;
      }
    }

    protected virtual void SetMessage(string msg)
    {
      if (null == LoopState)
      {
        Progress.SetMessage(PrefixMessage + msg);
      }
    }

    protected virtual void SetRange(long from, long to)
    {
      this.From = from;
      this.To = to;
      if (null == LoopState)
      {
        Progress.SetRange(from, to);
      }
    }

    protected virtual void SetPosition(long position)
    {
      this.Position = position;
      if (null == LoopState)
      {
        Progress.SetPosition(position);
      }
    }
  }
}
