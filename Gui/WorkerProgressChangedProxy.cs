using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public class WorkerProgressChangedProxy
  {
    private readonly Label[] labels;
    private readonly ProgressBar[] progressBars;

    public WorkerProgressChangedProxy(Label[] labels, ProgressBar[] progressBars)
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
          //pb.Refresh();
          //pb.CreateGraphics().DrawString(pb.Value.ToString() + "%", new Font("Arial", (float)8.25, FontStyle.Regular), Brushes.Black, new PointF(pb.Width / 2 - 10, pb.Height / 2 - 7));
        }
        else
        {
          int labelIndex = Math.Min(eState.LabelIndex, this.labels.Length - 1);
          this.labels[labelIndex].Text = eState.LabelText;
        }
        return;
      }

      if (e.UserState is string)
      {
        this.labels[0].Text = (string)e.UserState;
        return;
      }

      this.progressBars[0].Value = e.ProgressPercentage;
    }
  }
}