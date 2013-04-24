namespace RCPA.Gui
{
  partial class AbstractMultipleProgressFileProcessorUI
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
      this.lblSecondProgress = new System.Windows.Forms.Label();
      this.pnlFile.SuspendLayout();
      this.SuspendLayout();
      // 
      // pnlFile
      // 
      this.pnlFile.Location = new System.Drawing.Point(0, 181);
      this.pnlFile.Size = new System.Drawing.Size(727, 22);
      // 
      // txtOriginalFile
      // 
      this.txtOriginalFile.Size = new System.Drawing.Size(481, 21);
      // 
      // lblProgress
      // 
      this.lblProgress.Location = new System.Drawing.Point(0, 224);
      this.lblProgress.Size = new System.Drawing.Size(727, 21);
      // 
      // progressBar
      // 
      this.progressBar.Location = new System.Drawing.Point(0, 245);
      this.progressBar.Size = new System.Drawing.Size(727, 21);
      // 
      // btnClose
      // 
      this.btnClose.Location = new System.Drawing.Point(411, 7);
      // 
      // btnCancel
      // 
      this.btnCancel.Location = new System.Drawing.Point(326, 7);
      // 
      // btnGo
      // 
      this.btnGo.Location = new System.Drawing.Point(241, 7);
      // 
      // lblSecondProgress
      // 
      this.lblSecondProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.lblSecondProgress.Location = new System.Drawing.Point(0, 203);
      this.lblSecondProgress.Name = "lblSecondProgress";
      this.lblSecondProgress.Size = new System.Drawing.Size(727, 21);
      this.lblSecondProgress.TabIndex = 25;
      // 
      // AbstractMultipleProgressFileProcessorUI
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.ClientSize = new System.Drawing.Size(727, 302);
      this.Controls.Add(this.lblSecondProgress);
      this.Name = "AbstractMultipleProgressFileProcessorUI";
      this.Controls.SetChildIndex(this.progressBar, 0);
      this.Controls.SetChildIndex(this.lblProgress, 0);
      this.Controls.SetChildIndex(this.lblSecondProgress, 0);
      this.Controls.SetChildIndex(this.pnlFile, 0);
      this.pnlFile.ResumeLayout(false);
      this.pnlFile.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    protected System.Windows.Forms.Label lblSecondProgress;
  }
}
