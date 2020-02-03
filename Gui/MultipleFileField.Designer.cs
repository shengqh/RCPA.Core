namespace RCPA.Gui
{
  partial class MultipleFileField
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
      this.lbFiles = new System.Windows.Forms.ListBox();
      this.panel1 = new System.Windows.Forms.Panel();
      this.lblTitle = new System.Windows.Forms.Label();
      this.panel2 = new System.Windows.Forms.Panel();
      this.btnSave = new System.Windows.Forms.Button();
      this.btnLoad = new System.Windows.Forms.Button();
      this.btnClear = new System.Windows.Forms.Button();
      this.btnRemove = new System.Windows.Forms.Button();
      this.btnAdd = new System.Windows.Forms.Button();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      // 
      // lbFiles
      // 
      this.lbFiles.AllowDrop = true;
      this.lbFiles.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lbFiles.FormattingEnabled = true;
      this.lbFiles.ItemHeight = 16;
      this.lbFiles.Location = new System.Drawing.Point(0, 28);
      this.lbFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.lbFiles.Name = "lbFiles";
      this.lbFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.lbFiles.Size = new System.Drawing.Size(916, 516);
      this.lbFiles.Sorted = true;
      this.lbFiles.TabIndex = 5;
      this.lbFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbFiles_DragDrop);
      this.lbFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbFiles_DragEnter);
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.lblTitle);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(1083, 28);
      this.panel1.TabIndex = 8;
      // 
      // lblTitle
      // 
      this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblTitle.Location = new System.Drawing.Point(0, 0);
      this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new System.Drawing.Size(1083, 28);
      this.lblTitle.TabIndex = 2;
      this.lblTitle.Text = "Files";
      this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.btnSave);
      this.panel2.Controls.Add(this.btnLoad);
      this.panel2.Controls.Add(this.btnClear);
      this.panel2.Controls.Add(this.btnRemove);
      this.panel2.Controls.Add(this.btnAdd);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
      this.panel2.Location = new System.Drawing.Point(916, 28);
      this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(167, 516);
      this.panel2.TabIndex = 9;
      // 
      // btnSave
      // 
      this.btnSave.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnSave.Location = new System.Drawing.Point(0, 124);
      this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(167, 31);
      this.btnSave.TabIndex = 12;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      // 
      // btnLoad
      // 
      this.btnLoad.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnLoad.Location = new System.Drawing.Point(0, 93);
      this.btnLoad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.btnLoad.Name = "btnLoad";
      this.btnLoad.Size = new System.Drawing.Size(167, 31);
      this.btnLoad.TabIndex = 11;
      this.btnLoad.Text = "Load";
      this.btnLoad.UseVisualStyleBackColor = true;
      // 
      // btnClear
      // 
      this.btnClear.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnClear.Location = new System.Drawing.Point(0, 62);
      this.btnClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new System.Drawing.Size(167, 31);
      this.btnClear.TabIndex = 10;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      // 
      // btnRemove
      // 
      this.btnRemove.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnRemove.Location = new System.Drawing.Point(0, 31);
      this.btnRemove.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new System.Drawing.Size(167, 31);
      this.btnRemove.TabIndex = 9;
      this.btnRemove.Text = "Remove";
      this.btnRemove.UseVisualStyleBackColor = true;
      // 
      // btnAdd
      // 
      this.btnAdd.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnAdd.Location = new System.Drawing.Point(0, 0);
      this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new System.Drawing.Size(167, 31);
      this.btnAdd.TabIndex = 8;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      // 
      // MultipleFileField
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.lbFiles);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.panel1);
      this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.Name = "MultipleFileField";
      this.Size = new System.Drawing.Size(1083, 544);
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListBox lbFiles;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnLoad;
    private System.Windows.Forms.Button btnClear;
    private System.Windows.Forms.Button btnRemove;
    private System.Windows.Forms.Button btnAdd;
  }
}
