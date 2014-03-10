namespace RCPA.Gui
{
  partial class AbstractProcessorBaseUI
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
      this.lblProgress = new System.Windows.Forms.Label();
      this.progressBar = new System.Windows.Forms.ProgressBar();
      this.SuspendLayout();
      // 
      // btnClose
      // 
      this.btnClose.Location = new System.Drawing.Point(494, 9);
      // 
      // btnCancel
      // 
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // btnGo
      // 
      this.btnGo.Location = new System.Drawing.Point(324, 9);
      // 
      // lblProgress
      // 
      this.lblProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.lblProgress.Location = new System.Drawing.Point(0, 98);
      this.lblProgress.Name = "lblProgress";
      this.lblProgress.Size = new System.Drawing.Size(893, 23);
      this.lblProgress.TabIndex = 8;
      // 
      // progressBar
      // 
      this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.progressBar.Location = new System.Drawing.Point(0, 121);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new System.Drawing.Size(893, 23);
      this.progressBar.Step = 1;
      this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
      this.progressBar.TabIndex = 7;
      // 
      // AbstractProcessorUI
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.ClientSize = new System.Drawing.Size(893, 183);
      this.Controls.Add(this.lblProgress);
      this.Controls.Add(this.progressBar);
      this.Name = "AbstractProcessorUI";
      this.Controls.SetChildIndex(this.progressBar, 0);
      this.Controls.SetChildIndex(this.lblProgress, 0);
      this.ResumeLayout(false);

    }

    #endregion

    protected System.Windows.Forms.Label lblProgress;
    protected System.Windows.Forms.ProgressBar progressBar;

  }
}
