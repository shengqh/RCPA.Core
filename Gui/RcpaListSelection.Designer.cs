namespace RCPA.Gui
{
  partial class RcpaListSelection
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
      this.lbLeft = new System.Windows.Forms.ListBox();
      this.panel1 = new System.Windows.Forms.Panel();
      this.btnDelete = new System.Windows.Forms.Button();
      this.btnAdd = new System.Windows.Forms.Button();
      this.lbRight = new System.Windows.Forms.ListBox();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // lbLeft
      // 
      this.lbLeft.Dock = System.Windows.Forms.DockStyle.Left;
      this.lbLeft.FormattingEnabled = true;
      this.lbLeft.Location = new System.Drawing.Point(0, 0);
      this.lbLeft.Name = "lbLeft";
      this.lbLeft.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.lbLeft.Size = new System.Drawing.Size(316, 439);
      this.lbLeft.TabIndex = 0;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.btnDelete);
      this.panel1.Controls.Add(this.btnAdd);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(316, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(99, 439);
      this.panel1.TabIndex = 1;
      // 
      // btnDelete
      // 
      this.btnDelete.Location = new System.Drawing.Point(12, 62);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size(75, 23);
      this.btnDelete.TabIndex = 1;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
      // 
      // btnAdd
      // 
      this.btnAdd.Location = new System.Drawing.Point(12, 33);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new System.Drawing.Size(75, 23);
      this.btnAdd.TabIndex = 0;
      this.btnAdd.Text = "Add >>";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
      // 
      // lbRight
      // 
      this.lbRight.Dock = System.Windows.Forms.DockStyle.Right;
      this.lbRight.FormattingEnabled = true;
      this.lbRight.Location = new System.Drawing.Point(415, 0);
      this.lbRight.Name = "lbRight";
      this.lbRight.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.lbRight.Size = new System.Drawing.Size(334, 439);
      this.lbRight.TabIndex = 2;
      // 
      // RcpaListSelection
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.lbRight);
      this.Controls.Add(this.lbLeft);
      this.Name = "RcpaListSelection";
      this.Size = new System.Drawing.Size(749, 439);
      this.SizeChanged += new System.EventHandler(this.RcpaListSelection_SizeChanged);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListBox lbLeft;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.ListBox lbRight;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnAdd;
  }
}
