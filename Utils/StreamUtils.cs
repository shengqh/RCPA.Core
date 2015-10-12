using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using System.Reflection;
using RCPA.Utils;
using ICSharpCode.SharpZipLib.GZip;

namespace System.IO
{
  public static class StreamUtils
  {
    delegate long GetCharposFunc(StreamReader s);

    private static GetCharposFunc funcByPlatform;

    private static long GetCharposWindowsMono4(StreamReader s)
    {
      Int32 charpos = (Int32)s.GetType().InvokeMember("charPos",
      BindingFlags.DeclaredOnly |
      BindingFlags.Public | BindingFlags.NonPublic |
      BindingFlags.Instance | BindingFlags.GetField
      , null, s, null);

      Int32 charlen = (Int32)s.GetType().InvokeMember("charLen",
      BindingFlags.DeclaredOnly |
      BindingFlags.Public | BindingFlags.NonPublic |
      BindingFlags.Instance | BindingFlags.GetField
      , null, s, null);

      return s.BaseStream.Position - charlen + charpos;
    }

    private static long GetCharposMono3(StreamReader s)
    {
      Int32 charpos = (Int32)s.GetType().InvokeMember("pos",
      BindingFlags.DeclaredOnly |
      BindingFlags.Public | BindingFlags.NonPublic |
      BindingFlags.Instance | BindingFlags.GetField
      , null, s, null);

      Int32 charlen = (Int32)s.GetType().InvokeMember("decoded_count",
      BindingFlags.DeclaredOnly |
      BindingFlags.Public | BindingFlags.NonPublic |
      BindingFlags.Instance | BindingFlags.GetField
      , null, s, null);

      return s.BaseStream.Position - charlen + charpos;
    }

    static StreamUtils()
    {
      if (SystemUtils.CurrentSystem == SystemType.Mono3Lower)
      {
        funcByPlatform = GetCharposMono3;
      }
      else
      {
        funcByPlatform = GetCharposWindowsMono4;
      }
    }

    public static long GetCharpos(this StreamReader s)
    {
      return funcByPlatform(s);
    }

    public static void SetCharpos(this StreamReader s, long positionFromBegin)
    {
      s.DiscardBufferedData();
      s.BaseStream.Seek(positionFromBegin, SeekOrigin.Begin);
    }

    public static StreamWriter GetWriter(string filename, bool gzipped)
    {
      //return gzipped ? new StreamWriter(new GZipStream(File.Create(filename), CompressionMode.Compress)) : new StreamWriter(filename);
      return gzipped ? new StreamWriter(new GZipOutputStream(File.Create(filename))) : new StreamWriter(filename);
    }

    public static StreamReader GetReader(string filename)
    {
      //return filename.ToLower().EndsWith(".gz") ? new StreamReader(new GZipStream(File.OpenRead(filename), CompressionMode.Decompress)) : new StreamReader(filename);
      return filename.ToLower().EndsWith(".gz") ? new StreamReader(new GZipInputStream(File.OpenRead(filename))) : new StreamReader(filename);
    }
  }
}
