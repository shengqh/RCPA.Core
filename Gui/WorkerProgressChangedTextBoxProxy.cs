using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace RCPA.Gui
{
  public class WorkerProgressChangedTextBoxProxy
  {
    private readonly TextBoxBase labels;
    private readonly ProgressBar[] progressBars;

    public WorkerProgressChangedTextBoxProxy(TextBoxBase labels, ProgressBar[] progressBars)
    {
      this.labels = labels;
      this.progressBars = progressBars;
    }

    public void ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      if (e.UserState is WorkerProgressUserState)
      {
        var eState = (WorkerProgressUserState)e.UserState;
        if (eState.IsProgress)
        {
          int progressBarIndex = Math.Min(eState.ProgressBarIndex, this.progressBars.Length - 1);
          var pb = this.progressBars[progressBarIndex];
          if (eState.ProgressValue >= 100)
          {
            pb.Value = 100;
          }
          else
          {
            pb.Value = (int)eState.ProgressValue;
          }
        }
        else
        {
          AddLine(eState.LabelText);
        }
      }

      if (e.UserState is string)
      {
        AddLine((string)e.UserState);
      }

      this.progressBars[0].Value = e.ProgressPercentage;
    }

    private void AddLine(string line)
    {
      if (this.labels.SelectionStart == this.labels.Text.Length)
      {
        this.labels.AppendLine(line);
        this.labels.SelectionStart = this.labels.Text.Length;
        this.labels.ScrollToCaret();
      }
      else
      {
        this.labels.AppendLine(line);
      }
    }
  }
}