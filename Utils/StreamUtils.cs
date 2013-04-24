using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace System.IO
{
  public static class StreamUtils
  {
    public static long GetCharpos(this StreamReader s)
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

    public static void SetCharpos(this StreamReader s, long positionFromBegin)
    {
      s.DiscardBufferedData();
      s.BaseStream.Seek(positionFromBegin, SeekOrigin.Begin);
    }
  }
}
