namespace RCPA.Gui
{
  partial class ProgressField
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.lblProgress1 = new System.Windows.Forms.Label();
      this.progressBar = new System.Windows.Forms.ProgressBar();
      this.SuspendLayout();
      // 
      // lblProgress1
      // 
      this.lblProgress1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblProgress1.Location = new System.Drawing.Point(0, 0);
      this.lblProgress1.Name = "lblProgress1";
      this.lblProgress1.Size = new System.Drawing.Size(100, 26);
      this.lblProgress1.TabIndex = 0;
      // 
      // progressBar
      // 
      this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.progressBar.Location = new System.Drawing.Point(0, 26);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new System.Drawing.Size(100, 23);
      this.progressBar.TabIndex = 2;
      // 
      // ProgressField
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.Controls.Add(this.lblProgress1);
      this.Controls.Add(this.progressBar);
      this.MaximumSize = new System.Drawing.Size(0, 49);
      this.MinimumSize = new System.Drawing.Size(100, 49);
      this.Name = "ProgressField";
      this.Size = new System.Drawing.Size(100, 49);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lblProgress1;
    private System.Windows.Forms.ProgressBar progressBar;
  }
}
