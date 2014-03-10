using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace RCPA.Gui
{
  public class WorkerProgressChangedTextBoxProxy
  {
    private readonly TextBoxBase labels;

    public WorkerProgressChangedTextBoxProxy(TextBoxBase labels)
    {
      this.labels = labels;
    }

    public void ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      if (e.UserState is WorkerProgressUserState)
      {
        return;
      }

      if (e.UserState is string)
      {
        this.labels.AppendLine((string)e.UserState);
      }
    }
  }
}