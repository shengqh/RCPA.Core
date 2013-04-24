using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public class FolderBrowser : IDisposable
  {
    private FolderBrowserDialog dialog = new FolderBrowserDialog();

    public FolderBrowser()
    {
    }

    public FolderBrowser(bool showNewFolderButton)
    {
      ShowNewFolderButton = showNewFolderButton;
    }

    public string SelectedPath
    {
      get { return this.dialog.SelectedPath; }
      set { this.dialog.SelectedPath = value; }
    }

    public bool ShowNewFolderButton
    {
      get { return this.dialog.ShowNewFolderButton; }
      set { this.dialog.ShowNewFolderButton = value; }
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
      return this.dialog.ShowDialog(owner);
    }
  }

}