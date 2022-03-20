using Ookii.Dialogs.WinForms;
using System;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public class FolderBrowser : IDisposable
  {
    private FolderBrowserDialog dialog = new FolderBrowserDialog();
    private VistaFolderBrowserDialog vista = null;

    public FolderBrowser()
    {
      if (VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
      {
        vista = new VistaFolderBrowserDialog();
      }
    }

    public FolderBrowser(bool showNewFolderButton)
    {
      ShowNewFolderButton = showNewFolderButton;
    }

    public string SelectedPath
    {
      get
      {
        if (vista == null)
        {
          return this.dialog.SelectedPath;
        }

        return vista.SelectedPath;
      }
      set
      {
        if (vista == null)
        {
          this.dialog.SelectedPath = value;
        }
        vista.SelectedPath = value;
      }
    }

    public bool ShowNewFolderButton
    {
      get
      {
        if (vista == null)
        {
          return this.dialog.ShowNewFolderButton;
        }
        return vista.ShowNewFolderButton;
      }
      set
      {
        if (vista == null)
        {
          this.dialog.ShowNewFolderButton = value;
        }

        vista.ShowNewFolderButton = value;

      }
    }

    #region IDisposable Members

    public void Dispose()
    {
      this.dialog.Dispose();
      this.dialog = null;
    }

    #endregion

    public DialogResult ShowDialog()
    {
      return ShowDialog(null);
    }

    public DialogResult ShowDialog(IWin32Window owner)
    {
      if (vista == null)
      {
        return this.dialog.ShowDialog(owner);
      }

      return vista.ShowDialog(owner);
    }
  }
}