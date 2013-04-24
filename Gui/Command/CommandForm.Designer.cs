namespace RCPA.Gui.Command
{
  partial class CommandForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.mainMenu = new System.Windows.Forms.MenuStrip();
      this.optionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.showConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.mainMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // mainMenu
      // 
      this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionToolStripMenuItem});
      this.mainMenu.Location = new System.Drawing.Point(0, 0);
      this.mainMenu.Name = "mainMenu";
      this.mainMenu.Size = new System.Drawing.Size(579, 25);
      this.mainMenu.TabIndex = 0;
      this.mainMenu.Text = "menuStrip1";
      // 
      // optionToolStripMenuItem
      // 
      this.optionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showConsoleToolStripMenuItem});
      this.optionToolStripMenuItem.Name = "optionToolStripMenuItem";
      this.optionToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
      this.optionToolStripMenuItem.Text = "Option";
      this.optionToolStripMenuItem.Click += new System.EventHandler(this.optionToolStripMenuItem_Click);
      // 
      // showConsoleToolStripMenuItem
      // 
      this.showConsoleToolStripMenuItem.Name = "showConsoleToolStripMenuItem";
      this.showConsoleToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
      this.showConsoleToolStripMenuItem.Text = "Show Console";
      this.showConsoleToolStripMenuItem.Click += new System.EventHandler(this.showConsoleToolStripMenuItem_Click);
      // 
      // CommandForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(579, 110);
      this.Controls.Add(this.mainMenu);
      this.Name = "CommandForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "MainForm";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CommandForm_FormClosing);
      this.mainMenu.ResumeLayout(false);
      this.mainMenu.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    protected System.Windows.Forms.MenuStrip mainMenu;
    private System.Windows.Forms.ToolStripMenuItem optionToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem showConsoleToolStripMenuItem;

  }
}