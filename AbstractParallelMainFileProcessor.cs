using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using RCPA.Gui;
using RCPA.Utils;
using System.IO;

namespace RCPA
{
  public abstract class AbstractParallelMainFileProcessor : AbstractThreadFileProcessor, IProgressCallback
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

    protected List<string> sourceFiles;

    public bool ParallelMode { get; set; }

    public virtual bool StopAtException { get { return true; } }

    public AbstractParallelMainFileProcessor(IEnumerable<string> ASourceFiles)
    {
      _tokenSource = new CancellationTokenSource();
      _option = new ParallelOptions()
      {
        MaxDegreeOfParallelism = Environment.ProcessorCount - 1,
        CancellationToken = _tokenSource.Token
      };

      this.sourceFiles = ASourceFiles.ToList();
      this.ParallelMode = true;
    }

    public override IEnumerable<string> Process(string aPath)
    {
      Progress.SetMessage("Preparing ... ");
      PrepareBeforeProcessing(aPath);

      var result = new ConcurrentBag<string>();

      Progress.SetMessage("Start processing ... ");
      DateTime start = DateTime.Now;
      int totalCount = sourceFiles.Count;
      Progress.SetRange(0, totalCount);

      var exceptions = new ConcurrentQueue<Exception>();
      if (ParallelMode && sourceFiles.Count > 1)
      {
        var curFiles = new ConcurrentList<string>();
        var finishedFiles = new ConcurrentList<string>();
        var curProcessors = new ConcurrentList<IParallelTaskFileProcessor>();

        Parallel.ForEach(sourceFiles, Option, (sourceFile, loopState) =>
        {
          curFiles.Add(sourceFile);

          IParallelTaskFileProcessor processor = GetTaskProcessor(aPath, sourceFile);

          if (processor == null)
          {
            curFiles.Remove(sourceFile);
            finishedFiles.Add(sourceFile);
            return;
          }

          processor.LoopState = loopState;
          processor.Progress = Progress;

          curProcessors.Add(processor);
          try
          {
            var curResult = processor.Process(sourceFile);

            foreach (var f in curResult)
            {
              result.Add(f);
            }

            curFiles.Remove(sourceFile);
            finishedFiles.Add(sourceFile);
            curProcessors.Remove(processor);

            if (!Progress.IsConsole())
            {
              Progress.SetPosition(finishedFiles.Count);
            }

            DateTime end = DateTime.Now;
            var cost = end - start;
            var expectEnd = end.AddSeconds(cost.TotalSeconds / finishedFiles.Count * (totalCount - finishedFiles.Count));
            Progress.SetMessage("Processed {0}, {1} processing, {2} / {3} finished, expect to finish at {4}", Path.GetFileName(sourceFile), curProcessors.Count, finishedFiles.Count, totalCount, expectEnd);
          }
          catch (Exception e)
          {
            curProcessors.Remove(processor);
            exceptions.Enqueue(new Exception("Failed to process " + sourceFile + " : " + e.Message, e));
            if (StopAtException)
            {
              loopState.Stop();
            }
            else
            {
              Progress.SetMessage("FAIL at {0} : {1}", Path.GetFileName(sourceFile), e.Message);
            }
          }
          GC.Collect();
        });

        if (Progress.IsCancellationPending())
        {
          throw new UserTerminatedException();
        }
      }
      else
      {
        for (int i = 0; i < sourceFiles.Count; i++)
        {
          if (Progress.IsCancellationPending())
          {
            throw new UserTerminatedException();
          }

          IParallelTaskFileProcessor processor = GetTaskProcessor(aPath, sourceFiles[i]);

          try
          {
            var curResult = processor.Process(sourceFiles[i]);

            foreach (var f in curResult)
            {
              result.Add(f);
            }

            DateTime end = DateTime.Now;
            var cost = end - start;
            var expectEnd = end.AddSeconds(cost.TotalSeconds / (i + 1) * (totalCount - i - 1));
            Progress.SetMessage("Processed {0}, {1} / {2} finished, expect to be end at {3}", Path.GetFileName(sourceFiles[i]), i + 1, totalCount, expectEnd);

            if (!Progress.IsConsole())
            {
              Progress.SetPosition(i + 1);
            }
          }
          catch (Exception e)
          {
            var errorMsg = "Failed to process " + sourceFiles[i] + " : " + e.Message;
            exceptions.Enqueue(new Exception(errorMsg, e));
            if (StopAtException)
            {
              break;
            }
            else
            {
              Progress.SetMessage("FAIL at {0} : {1}", Path.GetFileName(sourceFiles[i]), e.Message);
            }
          }
        }
      }

      if (exceptions.Count > 0)
      {
        if (!StopAtException)
        {
          using (var sw = new StreamWriter(Path.Combine(aPath, "failed.filelist")))
          {
            foreach (var ex in exceptions)
            {
              sw.WriteLine(ex.ToString());
            }
          }
        }

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

      DateTime totalEnd = DateTime.Now;
      var totalCost = totalEnd - start;
      Progress.SetMessage("Processed finished. Start at {0}, end at {1}, total time cost:{2}", start, totalEnd, MiscUtils.GetTimeCost(totalCost));

      DoAfterProcessing(aPath, result);

      if (result.Count > 100)
      {
        return new[] { "More than 100 files saved in " + aPath + "." };
      }
      else {
        return result;
      }
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

    public virtual bool IsConsole()
    {
      return Progress.IsConsole();
    }

    #endregion
  }
}
