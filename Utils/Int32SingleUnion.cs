using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace RCPA.Utils
{
  [StructLayout(LayoutKind.Explicit)]
  public struct Int32SingleUnion
  {
    /// <summary>
    /// Int32 version of the value.
    /// </summary>
    [FieldOffset(0)]
    int i;
    /// <summary>
    /// Single version of the value.
    /// </summary>
    [FieldOffset(0)]
    float f;

    internal Int32SingleUnion(int i)
    {
      this.f = 0; // Just to keep the compiler happy
      this.i = i;
    }

    internal Int32SingleUnion(float f)
    {
      this.i = 0; // Just to keep the compiler happy
      this.f = f;
    }

    internal int AsInt32
    {
      get { return i; }
    }

    internal float AsSingle
    {
      get { return f; }
    }
  }
}
