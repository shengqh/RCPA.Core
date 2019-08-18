namespace RCPA.Gui
{
  partial class FileField
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
      this.btnOpen = new System.Windows.Forms.Button();
      this.txtFile = new System.Windows.Forms.TextBox();
      this.btnLoad = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // btnOpen
      // 
      this.btnOpen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.btnOpen.Dock = System.Windows.Forms.DockStyle.Left;
      this.btnOpen.Location = new System.Drawing.Point(0, 0);
      this.btnOpen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.btnOpen.Name = "btnOpen";
      this.btnOpen.Size = new System.Drawing.Size(301, 28);
      this.btnOpen.TabIndex = 0;
      this.btnOpen.Text = "button1";
      this.btnOpen.UseVisualStyleBackColor = true;
      // 
      // txtFile
      // 
      this.txtFile.Dock = System.Windows.Forms.DockStyle.Fill;
      this.txtFile.Location = new System.Drawing.Point(301, 0);
      this.txtFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.txtFile.Name = "txtFile";
      this.txtFile.Size = new System.Drawing.Size(865, 22);
      this.txtFile.TabIndex = 1;
      // 
      // btnLoad
      // 
      this.btnLoad.Dock = System.Windows.Forms.DockStyle.Right;
      this.btnLoad.Location = new System.Drawing.Point(1166, 0);
      this.btnLoad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.btnLoad.Name = "btnLoad";
      this.btnLoad.Size = new System.Drawing.Size(97, 28);
      this.btnLoad.TabIndex = 2;
      this.btnLoad.Text = "Load";
      this.btnLoad.UseVisualStyleBackColor = true;
      this.btnLoad.Visible = false;
      // 
      // FileField
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.txtFile);
      this.Controls.Add(this.btnLoad);
      this.Controls.Add(this.btnOpen);
      this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.Name = "FileField";
      this.Size = new System.Drawing.Size(1263, 28);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnOpen;
    private System.Windows.Forms.TextBox txtFile;
    public System.Windows.Forms.Button btnLoad;
  }
}
