using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RCPA.Gui.FileArgument;
using System.Diagnostics;

namespace RCPA.Gui.Event
{
  public class ListBoxFileEventHandlers : ListBoxEventHandlers
  {
    public OpenFileArgument FileArgument { get; set; }

    public ListBoxFileEventHandlers(ListBox lstFiles, OpenFileArgument fileArgument)
      : base(lstFiles)
    {
      this.FileArgument = fileArgument;
    }

    public void AddEvent(object sender, EventArgs e)
    {
      Debug.Assert(null != FileArgument);

      var dialog = this.FileArgument.GetFileDialog() as OpenFileDialog;
      dialog.Multiselect = true;

      if (this.lbFiles.SelectedItem != null)
      {
        dialog.FileName = (string)this.lbFiles.SelectedItem;
      }
      else if (this.lbFiles.Items.Count > 0)
      {
        dialog.FileName = (string)this.lbFiles.Items[0];
      }

      if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
      {
        foreach (string fileName in dialog.FileNames)
        {
          if (!this.lbFiles.Items.Contains(fileName))
          {
            this.lbFiles.Items.Add(fileName);
          }

          if (dialog.FileNames.Length > 1)
          {
            this.lbFiles.SetSelected(lbFiles.Items.IndexOf(fileName), true);
          }
          else
          {
            this.lbFiles.SelectedItem = fileName;
          }
        }
      }
    }

  }
}
