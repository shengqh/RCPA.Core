using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace RCPA.Gui
{
  public static class WinFormConsole
  {
    [DllImport("kernel32.dll")]
    static extern bool AllocConsole();

    [DllImport("kernel32.dll")]
    static extern bool FreeConsole();

    [DllImport("user32.dll")]
    public static extern int ShowWindow(int Handle, int showState);

    [DllImport("kernel32.dll")]
    public static extern int GetConsoleWindow();

    private const int SW_SHOWNORMAL = 1;
    private const int SW_HIDE = 0;

    private static bool isVisible = false;

    public static bool Visible
    {
      get
      {
        return isVisible;
      }
      set
      {
        if (value)
        {
          Show();
        }
        else
        {
          Hide();
        }
      }
    }

    public static void Show()
    {
      int win = GetConsoleWindow();
      if (win == 0)
      {
        AllocConsole();
      }
      else
      {
        ShowWindow(win, SW_SHOWNORMAL);
      }
      isVisible = true;
    }

    public static void DisableCloseButton()
    {
      int win = GetConsoleWindow();
      if (win != 0)
      {
        CloseButtonHelper.Disable(win);
      }
    }

    public static void Hide()
    {
      int win = GetConsoleWindow();
      if (win != 0)
      {
        ShowWindow(win, SW_HIDE);
      }

      isVisible = false;
    }
  }
}
