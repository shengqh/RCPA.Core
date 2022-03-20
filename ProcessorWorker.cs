using RCPA.Utils;
using System.ComponentModel;

namespace RCPA
{
  public class ProcessorWorker : BackgroundWorker
  {
    public volatile IProcessor FileProcessor;

    public ProcessorWorker()
    {
      WorkerReportsProgress = true;

      WorkerSupportsCancellation = true;
    }

    public ProcessorWorker(IProcessor fileProcessor)
      : this()
    {
      this.FileProcessor = fileProcessor;

      var processor = fileProcessor as IThreadProcessor;
      if (processor != null)
      {
        processor.Progress = new WorkerProgressCallback() { Worker = this };
      }
    }

    protected override void OnDoWork(DoWorkEventArgs e)
    {
      try
      {
        e.Result = this.FileProcessor.Process();
      }
      catch (UserTerminatedException)
      {
        e.Cancel = true;
      }
    }
  }
}