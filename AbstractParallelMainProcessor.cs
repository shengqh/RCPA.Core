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
  public abstract class AbstractParallelMainProcessor : AbstractThreadFileProcessor, IProgressCallback
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

    private List<string> _sourceFiles;

    public bool ParallelMode { get; set; }

    public AbstractParallelMainProcessor(IEnumerable<string> ASourceFiles)
    {
      _tokenSource = new CancellationTokenSource();
      _option = new ParallelOptions()
      {
        MaxDegreeOfParallelism = Environment.ProcessorCount,
        CancellationToken = _tokenSource.Token
      };

      this._sourceFiles = ASourceFiles.ToList();
      this.ParallelMode = true;
    }

    public override IEnumerable<string> Process(string aPath)
    {
      PrepareBeforeProcessing(aPath);

      var result = new ConcurrentBag<string>();

      if (ParallelMode && _sourceFiles.Count > 1)
      {
        var exceptions = new ConcurrentQueue<Exception>();

        int totalCount = _sourceFiles.Count;

        Progress.SetRange(0, totalCount);

        var curFiles = new ConcurrentList<string>();
        var finishedFiles = new ConcurrentList<string>();
        var curProcessors = new ConcurrentList<IParallelTaskFileProcessor>();

        Parallel.ForEach(_sourceFiles, Option, (m, loopState) =>
        {
          curFiles.Add(m);

          Progress.SetMessage("Processing {0}, finished {1} / {2}", curFiles.Count, finishedFiles.Count, totalCount);

          IParallelTaskFileProcessor processor = GetTaskProcessor(aPath, m);

          if (processor == null)
          {
            curFiles.Remove(m);
            finishedFiles.Add(m);
            return;
          }

          processor.LoopState = loopState;
          processor.Progress = Progress;

          curProcessors.Add(processor);
          try
          {
            var curResult = processor.Process(m);

            foreach (var f in curResult)
            {
              result.Add(f);
            }

            curFiles.Remove(m);
            finishedFiles.Add(m);

            Progress.SetPosition(finishedFiles.Count);
            Progress.SetMessage("Processing {0}, finished {1} / {2}", curFiles.Count, finishedFiles.Count, totalCount);
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
        for (int i = 0; i < _sourceFiles.Count; i++)
        {
          if (Progress.IsCancellationPending())
          {
            throw new UserTerminatedException();
          }

          string rootMsg = MyConvert.Format("{0} / {1} : {2}", i + 1, _sourceFiles.Count, _sourceFiles[i]);

          Progress.SetMessage(1, rootMsg);

          IParallelTaskFileProcessor processor = GetTaskProcessor(aPath, _sourceFiles[i]);
          processor.Progress = Progress;

          var curResult = processor.Process(_sourceFiles[i]);

          foreach (var f in curResult)
          {
            result.Add(f);
          }
        }
      }

      DoAfterProcessing(aPath, result);

      return result;
    }

    protected virtual void DoAfterProcessing(string aPath, ConcurrentBag<string> result) { }

    protected virtual void PrepareBeforeProcessing(string aPath) { }

    protected abstract IParallelTaskFileProcessor GetTaskProcessor(string targetDir, string fileName);


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

    #endregion
  }
}
