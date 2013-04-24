using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace RCPA.Gui.Command
{
  public partial class CommandForm : ComponentUI
  {
    public CommandForm()
    {
      InitializeComponent();
    }

    public void AddCommand(IToolCommand command)
    {
      AddCommand(command, command.GetClassification());
    }

    public void AddCommand(IToolCommand command, string classification)
    {
      ToolStripMenuItem parent = null;
      foreach (ToolStripMenuItem pItem in this.mainMenu.Items)
      {
        if (pItem.Text.Equals(classification))
        {
          parent = pItem;
          break;
        }
      }

      if (parent == null)
      {
        parent = new ToolStripMenuItem(classification);
        this.mainMenu.Items.Add(parent);
      }

      if (command is IToolSecondLevelCommand)
      {
        ToolStripMenuItem sMenu = null;
        var sCommand = (IToolSecondLevelCommand)command;
        foreach (ToolStripItem subItem in parent.DropDownItems)
        {
          if (subItem.Text.Equals(sCommand.GetSecondLevelCommandItem()))
          {
            sMenu = subItem as ToolStripMenuItem;
            break;
          }
        }

        if (null == sMenu)
        {
          sMenu = new ToolStripMenuItem(sCommand.GetSecondLevelCommandItem());
          parent.DropDownItems.Add(sMenu);
        }

        parent = sMenu;
      }

      if (command is ToolCommandSeparator)
      {
        parent.DropDownItems.Add(new ToolStripSeparator());
      }
      else
      {
        var caption = command.GetCaption();
        if (command.GetVersion().Length > 0)
        {
          caption = caption + " - " + command.GetVersion();
        }
        var currCommand = new ToolStripMenuItem(caption);
        currCommand.Tag = command;
        currCommand.Click += CommandClick;
        parent.DropDownItems.Add(currCommand);
      }
    }

    protected void CommandClick(object who, EventArgs e)
    {
      var mi = (ToolStripMenuItem)who;
      var command = (IToolCommand)mi.Tag;
      command.Run();
    }

    [RcpaOptionAttribute("WinFormConsoleVisible", RcpaOptionType.Boolean)]
    public bool ViewConsole
    {
      get
      {
        return WinFormConsole.Visible;
      }
      set
      {
        WinFormConsole.Visible = value;
        if (WinFormConsole.Visible)
        {
          WinFormConsole.DisableCloseButton();
        }
      }
    }

    private void showConsoleToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ViewConsole = !ViewConsole;
    }

    private void optionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      showConsoleToolStripMenuItem.Checked = WinFormConsole.Visible;
    }

    private void CommandForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      SaveOption();
    }
  }
}