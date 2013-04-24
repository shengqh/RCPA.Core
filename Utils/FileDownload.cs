using System;
using System.Collections.Generic;
using System.Text;
using Batte.CodeProject.Download;
using System.Threading;
using System.IO;

namespace RCPA.Utils
{
  public class FileDownload : AbstractThreadFileProcessor
  {
    private string url;

    private bool removeOldFile;

    public FileDownload(string url, bool removeOldFile)
    {
      this.url = url;
      this.removeOldFile = removeOldFile;
    }

    public string Url
    {
      get { return url; }
      set { url = value; }
    }

    // to adjust how many bytes are read from the url at a time,
    // simply change this constant:
    private const int downloadBlockSize = 4096;

    public override IEnumerable<string> Process(string fileName)
    {
      if (removeOldFile && new FileInfo(fileName).Exists)
      {
        new FileInfo(fileName).Delete();
      }

      using (DownloadData data = DownloadData.Create(url, fileName))
      {
        bool bShowProgress = data.IsProgressKnown;

        if (bShowProgress)
        {
          Progress.SetRange(0, data.FileSize);
        }

        // send the new download state
        Progress.SetMessage("Downloading ...");

        // create the download buffer
        byte[] buffer = new byte[downloadBlockSize];

        int readCount;

        // update how many bytes have already been read
        long totalDownloaded = data.StartPoint;

        using (FileStream f = File.Open(fileName, FileMode.Create, FileAccess.Write))
        {
          // read a block of bytes and get the number of bytes read
          while ((int)(readCount = data.DownloadStream.Read(buffer, 0, downloadBlockSize)) > 0)
          {
            // break on cancel
            if (Progress.IsCancellationPending())
            {
              throw new UserTerminatedException();
            }

            // update total bytes read
            totalDownloaded += readCount;

            // send progress info
            if (bShowProgress)
            {
              Progress.SetPosition(totalDownloaded);
            }
            else
            {
              Progress.SetMessage("Downloaded " + totalDownloaded + " bytes ...");
            }

            // save block to end of file
            f.Write(buffer, 0, readCount);

            // break on cancel
            if (Progress.IsCancellationPending())
            {
              throw new UserTerminatedException();
            }
          }

          // send 100% completion if url size is known and user hasn't cancelled
          if (bShowProgress)
          {
            Progress.SetPosition(data.FileSize);
          }
          else
          {
            Progress.SetMessage("Finished, downloaded " + totalDownloaded + " bytes");
          }
        }
      }

      return new string[] { fileName };
    }
  }
}
