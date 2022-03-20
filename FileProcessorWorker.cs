using RCPA.Utils;
using System.ComponentModel;

namespace RCPA
{
  public class FileProcessorWorker : BackgroundWorker
  {
    public volatile IFileProcessor fileProcessor;

    public volatile string originalFilename;

    public FileProcessorWorker()
    {
      WorkerReportsProgress = true;

      WorkerSupportsCancellation = true;
    }

    public FileProcessorWorker(IFileProcessor fileProcessor, string originalFilename)
      : this()
    {
      this.fileProcessor = fileProcessor;

      this.originalFilename = originalFilename;

      if (fileProcessor is IThreadFileProcessor)
      {
        ((IThreadFileProcessor)fileProcessor).Progress = new WorkerProgressCallback() { Worker = this };
      }
    }

    protected override void OnDoWork(DoWorkEventArgs e)
    {
      try
      {
        e.Result = this.fileProcessor.Process(this.originalFilename);
      }
      catch (UserTerminatedException)
      {
        e.Cancel = true;
      }
    }
  }
}