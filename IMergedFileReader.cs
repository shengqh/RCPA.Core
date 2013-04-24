using System;
using System.Collections.Generic;

namespace RCPA
{
  public interface IMergedFileReader : IDisposable
  {
    int FileCount { get; }

    bool HasNext { get; }

    string NextFilename { get; }

    bool Open(string filename);

    void Close();

    List<string> NextContent();

    void SkipNextContent();
  }
}