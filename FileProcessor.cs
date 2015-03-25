using System.Collections.Generic;
using RCPA.Utils;
using RCPA.Gui;
using System.IO;
using System.Linq;
using System.Threading;
using System.Globalization;
using System.Threading.Tasks;
using System;

namespace RCPA
{
  public interface IProcessor
  {
    IEnumerable<string> Process();
  }

  public interface IThreadProcessor : IProcessor
  {
    IProgressCallback Progress { get; set; }
  }

  public interface IParallelTaskProcessor : IThreadProcessor
  {
    ParallelLoopState LoopState { get; set; }
  }

  public abstract class AbstractThreadProcessor : ProgressClass, IThreadProcessor
  {
    public AbstractThreadProcessor()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    }

    #region IThreadFileProcessor Members

    public abstract IEnumerable<string> Process();

    #endregion
  }


  public interface IFileProcessor
  {
    IEnumerable<string> Process(string fileName);
  }

  public interface IThreadFileProcessor : IFileProcessor
  {
    IProgressCallback Progress { get; set; }
  }

  public interface IParallelTaskFileProcessor : IThreadFileProcessor
  {
    ParallelLoopState LoopState { get; set; }
  }

  public abstract class AbstractThreadFileProcessor : ProgressClass, IThreadFileProcessor
  {
    public AbstractThreadFileProcessor()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
      this.Progress = new ConsoleProgressCallback();
    }

    #region IThreadFileProcessor Members

    public abstract IEnumerable<string> Process(string fileName);

    #endregion
  }

  public abstract class AbstractDirectoryProcessor : ProgressClass, IThreadFileProcessor
  {
    #region IThreadFileProcessor Members

    public ParallelLoopState LoopState { get; set; }

    public IEnumerable<string> Process(string directoryName)
    {
      List<string> result = new List<string>();

      string[] files = GetFiles(directoryName);
      for (int i = 0; i < files.Length; i++)
      {
        if (Progress.IsCancellationPending())
        {
          throw new UserTerminatedException();
        }

        Progress.SetMessage(1, MyConvert.Format("{0}/{1}, Processing {2}", i + 1, files.Length, files[i]));

        IFileProcessor processor = GetFileProcessor();
        if (processor is IThreadFileProcessor)
        {
          (processor as IThreadFileProcessor).Progress = this.Progress;
        }

        result.AddRange(processor.Process(files[i]));
      }

      return result;
    }

    #endregion

    protected abstract IFileProcessor GetFileProcessor();

    protected abstract string[] GetFiles(string directoryName);
  }

}