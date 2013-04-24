using System;
using System.IO;
using System.Threading;

namespace Batte.CodeProject.Download
{
  public class BatchDownloader : IDisposable
  {
    private ManualResetEvent cancelEvent;
    private int downloadCount;
    private int totalDownloads;

    #region IDisposable Members

    public void Dispose()
    {
      this.cancelEvent = null;
    }

    #endregion

    public event DownloadProgressHandler StateChanged;
    public event DownloadProgressHandler CurrentProgressChanged;
    public event DownloadProgressHandler TotalProgressChanged;
    public event DownloadProgressHandler FileChanged;

    public void Download(string[] urls, string destinationFolder, ManualResetEvent cancelEvent)
    {
      this.cancelEvent = cancelEvent;
      this.downloadCount = 0;
      this.totalDownloads = urls.Length;

      foreach (string url in urls)
      {
        // break out if a cancellation has occurred
        if (HasUserCancelled())
          break;

        // create the destination path using the destination folder and the url
        string fileName = Path.GetFileName(url);
        string destPath = Path.Combine(destinationFolder, Path.GetFileName(url));

        // send the new filename back to the owner class
        if (FileChanged != null)
          FileChanged(this, new DownloadEventArgs(fileName));

        // download the file
        using (var dL = new FileDownloader())
        {
          dL.StateChanged += dL_StateChanged;
          dL.ProgressChanged += dL_ProgressChanged;
          dL.Download(url, destPath, this.cancelEvent);
        }

        this.downloadCount++;
      }

      // send a 100% complete message if the user hasn't cancelled
      if (!HasUserCancelled())
        dL_ProgressChanged(this, new DownloadEventArgs(100));
    }

    private bool HasUserCancelled()
    {
      return (this.cancelEvent != null && this.cancelEvent.WaitOne(0, false));
    }

    private void dL_StateChanged(object sender, DownloadEventArgs e)
    {
      if (StateChanged != null)
        StateChanged(this, e);
    }

    private void dL_ProgressChanged(object sender, DownloadEventArgs e)
    {
      // resend the progress info
      if (CurrentProgressChanged != null)
        CurrentProgressChanged(this, e);

      // calculate total progress
      double percentForEach = (double)100 / this.totalDownloads;
      var percent = (int)((((double)this.downloadCount) / this.totalDownloads) * 100);
      percent += (int)(percentForEach * ((double)e.PercentDone / 100));
      if (percent > 100)
        percent = 100;

      // send total progress info
      if (TotalProgressChanged != null)
        TotalProgressChanged(this, new DownloadEventArgs(percent));
    }
  }
}