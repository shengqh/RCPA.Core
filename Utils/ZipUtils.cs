using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace RCPA.Utils
{
  public static class ZipUtils
  {
    public static StreamReader OpenFile(string filename)
    {
      ZipInputStream s = new ZipInputStream(new FileInfo(filename).OpenRead());
      ZipEntry theEntry;
      while ((theEntry = s.GetNextEntry()) != null)
      {
        if (theEntry.IsDirectory)
        {
          continue;
        }
        string fileName = Path.GetFileName(theEntry.Name);
        if (fileName != String.Empty)
        {
          return new StreamReader(s);
        }
      }

      return null;
    }

    public static bool HasFile(string filename, Func<string, bool> accept)
    {
      ZipInputStream s = new ZipInputStream(new FileInfo(filename).OpenRead());
      ZipEntry theEntry;
      while ((theEntry = s.GetNextEntry()) != null)
      {
        if (theEntry.IsDirectory)
        {
          continue;
        }
        string entryFileName = Path.GetFileName(theEntry.Name);
        if (entryFileName != String.Empty && accept(entryFileName))
        {
          return true;
        }
      }

      return false;
    }

    public static StreamReader OpenFile(string filename, Func<string,bool> accept)
    {
      ZipInputStream s = new ZipInputStream(new FileInfo(filename).OpenRead());
      ZipEntry theEntry;
      while ((theEntry = s.GetNextEntry()) != null)
      {
        if (theEntry.IsDirectory)
        {
          continue;
        }
        string entryFileName = Path.GetFileName(theEntry.Name);
        if (entryFileName != String.Empty && accept(entryFileName))
        {
          return new StreamReader(s);
        }
      }

      return null;
    }

  }
}
