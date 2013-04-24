using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Batte.CodeProject.Download
{
  public class FileDownloader : IDisposable
  {
    // to adjust how many bytes are read from the url at a time,
    // simply change this constant:
    private const int downloadBlockSize = 1024;
    private WaitHandle cancelEvent;

    #region IDisposable Members

    public void Dispose()
    {
      this.cancelEvent = null;
    }

    #endregion

    public event DownloadProgressHandler ProgressChanged;
    public event DownloadProgressHandler StateChanged;

    public void Download(string url, string file, WaitHandle cancelEvent)
    {
      DownloadData data = null;

      try
      {
        this.cancelEvent = cancelEvent;

        // exit on cancel
        if (HasUserCancelled())
          return;

        // get download details
        data = DownloadData.Create(url, file);

        // send the new download state
        RaiseStateChanged(data.DownloadState);

        // create the download buffer
        var buffer = new byte[downloadBlockSize];

        int readCount;

        // update how many bytes have already been read
        long totalDownloaded = data.StartPoint;

        // read a block of bytes and get the number of bytes read
        while ((readCount = data.DownloadStream.Read(buffer, 0, downloadBlockSize)) > 0)
        {
          // break on cancel
          if (HasUserCancelled())
            break;

          // update total bytes read
          totalDownloaded += readCount;

          // send progress info
          if (data.IsProgressKnown)
            RaiseProgressChanged(totalDownloaded, data.FileSize);

          // save block to end of file
          SaveToFile(buffer, readCount, file);

          // break on cancel
          if (HasUserCancelled())
            break;
        }

        // send 100% completion if url size is known and user hasn't cancelled
        if (!HasUserCancelled() && data.IsProgressKnown)
          RaiseProgressChanged(data.FileSize, data.FileSize);
      }
      finally
      {
        if (data != null)
          data.Close();
      }
    }

    private void SaveToFile(byte[] buffer, int count, string fileName)
    {
      FileStream f = null;

      try
      {
        f = File.Open(fileName, FileMode.Append, FileAccess.Write);
        f.Write(buffer, 0, count);
      }
      finally
      {
        if (f != null)
          f.Close();
      }
    }

    private void RaiseStateChanged(string state)
    {
      if (StateChanged != null)
        StateChanged(this, new DownloadEventArgs(state));
    }

    private void RaiseProgressChanged(long current, long target)
    {
      var percent = (int)((((double)current) / target) * 100);
      if (ProgressChanged != null)
        ProgressChanged(this, new DownloadEventArgs(percent));
    }

    private bool HasUserCancelled()
    {
      return (this.cancelEvent != null && this.cancelEvent.WaitOne(0, false));
    }
  }

  public class DownloadData : IDisposable
  {
    private readonly bool progressKnown;
    private readonly HttpWebResponse response;
    private readonly long size;
    private readonly long start;
    private bool connected;
    private Stream stream;

    private DownloadData(HttpWebResponse response, long size, long start, bool progressKnown)
    {
      this.response = response;
      this.size = size;
      this.start = start;
      this.stream = null;
      this.progressKnown = progressKnown;
      this.connected = true;
    }

    public Stream DownloadStream
    {
      get
      {
        if (this.start == this.size)
          return Stream.Null;
        if (this.stream == null)
          this.stream = this.response.GetResponseStream();
        return this.stream;
      }
    }

    public long FileSize
    {
      get { return this.size; }
    }

    public long StartPoint
    {
      get { return this.start; }
    }

    public string DownloadState
    {
      get { return this.response.StatusCode.ToString(); }
    }

    public bool IsProgressKnown
    {
      get { return this.progressKnown; }
    }

    #region IDisposable Members

    public void Dispose()
    {
      Close();
    }

    #endregion

    public static DownloadData Create(string url, string file)
    {
      bool progressKnown;
      bool resume = false;
      long urlSize = GetFileSize(url, out progressKnown);
      long startPoint = 0;
      HttpWebRequest req = GetRequest(url);

      if (progressKnown && File.Exists(file))
      {
        startPoint = new FileInfo(file).Length;
        if (startPoint != urlSize)
        {
          resume = true;
          req.AddRange((int)startPoint);
        }
      }
      else if (File.Exists(file))
      {
        File.Delete(file);
      }

      var response = (HttpWebResponse)req.GetResponse();

      if (resume && response.StatusCode == HttpStatusCode.OK)
      {
        File.Delete(file);
        startPoint = 0;
      }

      return new DownloadData(response, urlSize, startPoint, progressKnown);
    }

    private static long GetFileSize(string url, out bool progressKnown)
    {
      long size = -1;

      using (var response = (HttpWebResponse)GetRequest(url).GetResponse())
      {
        size = response.ContentLength;

        if (size == -1)
        {
          progressKnown = false;
        }
        else
        {
          progressKnown = true;
        }
      }

      return size;
    }

    private static HttpWebRequest GetRequest(string url)
    {
      var request = (HttpWebRequest)WebRequest.Create(url);
      request.Credentials = CredentialCache.DefaultCredentials;
      return request;
    }

    public void Close()
    {
      if (this.connected)
      {
        this.response.Close();
        this.connected = false;
      }
    }
  }
}