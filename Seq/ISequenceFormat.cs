using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace RCPA.Seq
{
  public interface ISequenceFormat
  {
    Sequence ReadSequence(StreamReader reader);

    void WriteSequence(StreamWriter writer, Sequence seq);

    void WriteSequence(StreamWriter writer, string reference, string sequence);
  }
}
