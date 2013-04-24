using System;

namespace Batte.CodeProject.Download
{
  public class DownloadEventArgs : EventArgs
  {
    private readonly int p;
    private readonly string s;

    public DownloadEventArgs(int percentDone)
    {
      this.p = percentDone;
    }

    public DownloadEventArgs(string state)
    {
      this.s = state;
    }

    public DownloadEventArgs(int percentDone, string state)
    {
      this.p = percentDone;
      this.s = state;
    }

    public int PercentDone
    {
      get { return this.p; }
    }

    public string DownloadState
    {
      get { return this.s; }
    }
  }

  public delegate void DownloadProgressHandler(object sender, DownloadEventArgs e);
}