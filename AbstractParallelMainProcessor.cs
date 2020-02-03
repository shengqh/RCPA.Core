using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using RCPA.Gui;
using RCPA.Utils;

namespace RCPA
{
  public abstract class AbstractParallelMainProcessor : AbstractThreadProcessor, IProgressCallback
  {
    private CancellationTokenSource _tokenSource;
    public CancellationTokenSource TokenSource
    {
      get
      {
        return _tokenSource;
      }
    }

    private ParallelOptions _option;
    public ParallelOptions Option
    {
      get
      {
        return _option;
      }
    }

    private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
    protected ReaderWriterLockSlim Lock
    {
      get
      {
        return _lock;
      }
    }

    public bool ParallelMode { get; set; }

    public AbstractParallelMainProcessor()
    {
      _tokenSource = new CancellationTokenSource();
      _option = new ParallelOptions()
      {
        MaxDegreeOfParallelism = Environment.ProcessorCount - 1,
        CancellationToken = _tokenSource.Token
      };

      this.ParallelMode = true;
    }

    public override IEnumerable<string> Process()
    {
      PrepareBeforeProcessing();

      var result = new ConcurrentBag<string>();
      var taskProcessors = GetTaskProcessors();

      if (ParallelMode && taskProcessors.Count > 1)
      {
        var exceptions = new ConcurrentQueue<Exception>();

        int totalCount = taskProcessors.Count;

        Progress.SetRange(0, totalCount);

        var finishedProcessors = new ConcurrentList<IParallelTaskProcessor>();
        var curProcessors = new ConcurrentList<IParallelTaskProcessor>();

        Progress.SetMessage("Start processing ... ");
        DateTime start = DateTime.Now;
        Parallel.ForEach(taskProcessors, Option, (processor, loopState) =>
        {
          curProcessors.Add(processor);

          processor.LoopState = loopState;
          processor.Progress = Progress;
          try
          {
            var curResult = processor.Process();

            foreach (var f in curResult)
            {
              result.Add(f);
            }

            curProcessors.Remove(processor);
            finishedProcessors.Add(processor);

            if (!Progress.IsConsole())
            {
              Progress.SetPosition(finishedProcessors.Count);
            }

            DateTime end = DateTime.Now;
            var cost = end - start;
            var expectEnd = start.AddSeconds(cost.TotalSeconds / finishedProcessors.Count * totalCount) ;
            Progress.SetMessage("{0} threads running, {1} / {2} finished, expect to be end at {3}", curProcessors.Count, finishedProcessors.Count, totalCount, expectEnd);
          }
          catch (Exception e)
          {
            exceptions.Enqueue(e);
            loopState.Stop();
          }

          GC.Collect();
        });

        if (Progress.IsCancellationPending())
        {
          throw new UserTerminatedException();
        }

        if (exceptions.Count > 0)
        {
          if (exceptions.Count == 1)
          {
            throw exceptions.First();
          }
          else
          {
            StringBuilder sb = new StringBuilder();
            foreach (var ex in exceptions)
            {
              sb.AppendLine(ex.ToString());
            }
            throw new Exception(sb.ToString());
          }
        }
      }
      else
      {
        for (int i = 0; i < taskProcessors.Count; i++)
        {
          if (Progress.IsCancellationPending())
          {
            throw new UserTerminatedException();
          }

          string rootMsg = MyConvert.Format("{0} / {1}", i + 1, taskProcessors.Count);

          Progress.SetMessage(1, rootMsg);
          var processor = taskProcessors[i];
          processor.Progress = Progress;

          var curResult = processor.Process();

          foreach (var f in curResult)
          {
            result.Add(f);
          }
        }
      }

      DoAfterProcessing(result);

      return result;
    }

    protected virtual void DoAfterProcessing(ConcurrentBag<string> result) { }

    protected virtual void PrepareBeforeProcessing() { }

    protected abstract List<IParallelTaskProcessor> GetTaskProcessors();

    #region IProgressCallback Members

    public bool IsCancellationPending()
    {
      return Progress.IsCancellationPending();
    }

    public void Begin()
    {
      Progress.Begin();
    }

    public void SetRange(long minimum, long maximum)
    {
    }

    public void SetRange(int progressBarIndex, long minimum, long maximum)
    {
    }

    public void Increment(long value)
    {
    }

    public void Increment(int progressBarIndex, long value)
    {
    }

    public void SetPosition(long position)
    {
      throw new NotImplementedException();
    }

    public void SetPosition(int progressBarIndex, long position)
    {
      throw new NotImplementedException();
    }

    public void SetMessage(string message)
    {
      throw new NotImplementedException();
    }

    public void SetMessage(string format, params object[] args)
    {
      throw new NotImplementedException();
    }

    public void SetMessage(int labelIndex, string message)
    {
      throw new NotImplementedException();
    }

    public void SetMessage(int labelIndex, string format, params object[] args)
    {
      throw new NotImplementedException();
    }

    public void End()
    {
      throw new NotImplementedException();
    }

    public bool IsConsole()
    {
      return Progress.IsConsole();
    }

    #endregion
  }
}
