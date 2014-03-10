namespace RCPA.Gui
{
  partial class DirectoryField
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
      this.btnOpen.Dock = System.Windows.Forms.DockStyle.Left;
      this.btnOpen.Location = new System.Drawing.Point(0, 0);
      this.btnOpen.Name = "btnOpen";
      this.btnOpen.Size = new System.Drawing.Size(226, 23);
      this.btnOpen.TabIndex = 0;
      this.btnOpen.Text = "button1";
      this.btnOpen.UseVisualStyleBackColor = true;
      // 
      // txtFile
      // 
      this.txtFile.Dock = System.Windows.Forms.DockStyle.Fill;
      this.txtFile.Location = new System.Drawing.Point(226, 0);
      this.txtFile.Name = "txtFile";
      this.txtFile.Size = new System.Drawing.Size(648, 20);
      this.txtFile.TabIndex = 1;
      // 
      // btnLoad
      // 
      this.btnLoad.Dock = System.Windows.Forms.DockStyle.Right;
      this.btnLoad.Location = new System.Drawing.Point(874, 0);
      this.btnLoad.Name = "btnLoad";
      this.btnLoad.Size = new System.Drawing.Size(73, 23);
      this.btnLoad.TabIndex = 2;
      this.btnLoad.Text = "Load";
      this.btnLoad.UseVisualStyleBackColor = true;
      this.btnLoad.Visible = false;
      // 
      // DirectoryField
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.txtFile);
      this.Controls.Add(this.btnLoad);
      this.Controls.Add(this.btnOpen);
      this.Name = "DirectoryField";
      this.Size = new System.Drawing.Size(947, 23);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnOpen;
    private System.Windows.Forms.TextBox txtFile;
    private System.Windows.Forms.Button btnLoad;
  }
}
