using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace RCPA.Gui
{
  public static class CloseButtonHelper
  {
    [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    private static extern int GetSystemMenu(int hwnd, int revert);

    [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    private static extern int EnableMenuItem(int menu, int ideEnableItem, int enable);


    private const int SC_CLOSE = 0xF060;
    private const int MF_BYCOMMAND = 0x00000000;
    private const int MF_GRAYED = 0x00000001;
    private const int MF_ENABLED = 0x00000002;

    public static void Disable(int handle)
    {
      EnableMenuItem(GetSystemMenu(handle, 0), SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
    }

    public static void Enabled(int handle)
    {
      EnableMenuItem(GetSystemMenu(handle, 0), SC_CLOSE, MF_ENABLED);
    }
  }
}
