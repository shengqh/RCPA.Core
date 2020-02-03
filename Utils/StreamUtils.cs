using RCPA.Utils;
using System.IO.Compression;
using System.Reflection;

namespace System.IO
{
  public static class StreamUtils
  {
    delegate long GetCharposFunc(StreamReader s);

    private static GetCharposFunc funcByPlatform;

    private static long GetCharposMono6(StreamReader s)
    {
      return s.BaseStream.Position;
    }

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
      else if (SystemUtils.CurrentSystem == SystemType.Mono6Upper)
      {
        funcByPlatform = GetCharposMono6;
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
      return gzipped ? new StreamWriter(new GZipStream(File.Create(filename), CompressionMode.Compress)) : new StreamWriter(filename);
    }

    public static StreamReader GetReader(string filename)
    {
      return filename.ToLower().EndsWith(".gz") ? new StreamReader(new GZipStream(File.OpenRead(filename), CompressionMode.Decompress)) : new StreamReader(filename);
    }
  }
}
