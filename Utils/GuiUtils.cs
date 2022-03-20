using RCPA.Utils;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace RCPA
{
  public static class GuiUtils
  {
    public static ColumnHeader FindColumn(ListView lv, string text)
    {
      for (int i = 0; i < lv.Columns.Count; i++)
      {
        if (lv.Columns[i].Text.Equals(text))
        {
          return lv.Columns[i];
        }
      }

      throw new Exception("Cannot find " + text + " column.");
    }

    public static void SaveListViewColumnWidth(Configuration option, string formKey, ListView lvTarget)
    {
      option.AppSettings.Settings.Remove(formKey + "_ColumnCount");
      option.AppSettings.Settings.Add(formKey + "_ColumnCount", lvTarget.Columns.Count.ToString());
      for (int i = 0; i < lvTarget.Columns.Count; i++)
      {
        option.AppSettings.Settings.Remove(formKey + "_ColumnWidth_" + i);
        option.AppSettings.Settings.Add(formKey + "_ColumnWidth_" + i, lvTarget.Columns[i].Width.ToString());
      }
    }

    public static void LoadListViewColumnWidth(Configuration option, string formKey, ListView lvTarget)
    {
      int count = ConfigurationUtils.GetValue(option, formKey + "_ColumnCount", 0);
      while (lvTarget.Columns.Count < count)
      {
        lvTarget.Columns.Add("");
      }

      for (int i = 0; i < lvTarget.Columns.Count; i++)
      {
        lvTarget.Columns[i].Width = ConfigurationUtils.GetValue(option, formKey + "_ColumnWidth_" + i, lvTarget.Columns[i].Width);
      }
    }

    public static void HideTabPage(this TabControl tc, TabPage tp)
    {
      if (tc.TabPages.Contains(tp))
      {
        tc.TabPages.Remove(tp);
      }
    }

    public static void ShowTabPage(this TabControl tc, TabPage tp)
    {
      tc.ShowTabPage(tp, tc.TabPages.Count);
    }

    public static void ShowTabPageOnly(this TabControl tc, TabPage tp)
    {
      tc.TabPages.Clear();
      tc.TabPages.Add(tp);
    }

    public static void ShowTabPage(this TabControl tc, TabPage tp, int index)
    {
      if (!tc.TabPages.Contains(tp))
      {
        tc.InsertTabPage(tp, index);
      }
      tc.SelectedIndex = tc.TabPages.IndexOf(tp);
    }

    public static void AddTabPage(this TabControl tc, TabPage tabpage)
    {
      if (!tc.TabPages.Contains(tabpage))
      {
        tc.TabPages.Add(tabpage);
      }
    }


    public static void InsertTabPage(this TabControl tc, TabPage tabpage, int index)
    {
      if (index < 0 || index > tc.TabCount)
      {
        throw new ArgumentException("Index out of Range.");
      }

      tc.AddTabPage(tabpage);

      if (index < tc.TabCount - 1)
      {
        do
        {
          tc.SwapTabPages(tabpage, (tc.TabPages[tc.TabPages.IndexOf(tabpage) - 1]));
        }
        while (tc.TabPages.IndexOf(tabpage) != index);
      }

      tc.SelectedTab = tabpage;
    }

    public static void SwapTabPages(this TabControl tc, TabPage tp1, TabPage tp2)
    {
      if (!tc.TabPages.Contains(tp1) || !tc.TabPages.Contains(tp2))
      {
        throw new ArgumentException("TabPages must be in the TabControls TabPageCollection.");
      }

      int Index1 = tc.TabPages.IndexOf(tp1);
      int Index2 = tc.TabPages.IndexOf(tp2);
      tc.TabPages[Index1] = tp2;
      tc.TabPages[Index2] = tp1;
    }
  }
}