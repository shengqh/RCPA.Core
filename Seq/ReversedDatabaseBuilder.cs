using System;
using System.Collections.Generic;
using System.Text;
using RCPA.Utils;
using System.IO;
using RCPA.Seq;

namespace RCPA.Seq
{
  public class ReversedDatabaseBuilder : AbstractThreadFileProcessor
  {
    private bool combined;

    private PseudoSequenceBuilder builder;

    private bool pseudoAminoacid;

    private string contaminantFile;

    public ReversedDatabaseBuilder(bool combined)
      : this(combined, null)
    {
    }

    public ReversedDatabaseBuilder(bool combined, string contaminantFile)
      : this(combined, false, "", false, contaminantFile)
    {
    }

    public ReversedDatabaseBuilder(bool combined, bool pseudoAminoacid, string aminoacids, bool forward)
      : this(combined, pseudoAminoacid, aminoacids, forward, null)
    {
    }

    public ReversedDatabaseBuilder(bool combined, bool pseudoAminoacid, string aminoacids, bool forward, string contaminantFile)
    {
      this.combined = combined;
      this.contaminantFile = contaminantFile;

      this.pseudoAminoacid = pseudoAminoacid;
      if (pseudoAminoacid)
      {
        builder = new PseudoSequenceBuilder(aminoacids, forward);
      }
    }

    public override IEnumerable<string> Process(string sourceDatabase)
    {
      string targetFilename;

      if (combined)
      {
        targetFilename = FileUtils.ChangeExtension(sourceDatabase, "REVERSED.fasta");
      }
      else
      {
        targetFilename = FileUtils.ChangeExtension(sourceDatabase, "REVERSED_ONLY.fasta");
      }

      using (StreamWriter sw = new StreamWriter(targetFilename))
      {
        int index = 0;

        if (contaminantFile != null)
        {
          Progress.SetMessage("Processing contaminant proteins : " + contaminantFile + " ...");
          ProcessFile(ref index, sw, contaminantFile, true);
        }

        Progress.SetMessage("Generating and writing reversed version of " + sourceDatabase + " ...");
        ProcessFile(ref index, sw, sourceDatabase, false);
      }

      Progress.SetMessage("Finished.");

      return new[] { targetFilename };
    }

    private void ProcessFile(ref int index, StreamWriter sw, string fastaFile, bool isContaminant)
    {
      FastaFormat ff = new FastaFormat();

      using (StreamReader sr = new StreamReader(fastaFile))
      {
        Progress.SetRange(0, sr.BaseStream.Length);

        Sequence seq;
        while ((seq = ff.ReadSequence(sr)) != null)
        {
          Progress.SetPosition(sr.BaseStream.Position);

          if (isContaminant)
          {
            if (!seq.Reference.StartsWith("CON_"))
            {
              seq.Reference = "CON_" + seq.Reference;
            }
          }

          if (combined)
          {
            ff.WriteSequence(sw, seq);
          }

          if (pseudoAminoacid)
          {
            builder.Build(seq);
          }

          index++;
          Sequence reversedSeq = SequenceUtils.GetReversedSequence(seq.SeqString, index);

          ff.WriteSequence(sw, reversedSeq);
        }
      }
    }
  }
}
