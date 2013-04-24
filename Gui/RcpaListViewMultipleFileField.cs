using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RCPA.Gui.FileArgument;

namespace RCPA.Gui
{
  public class RcpaListViewMultipleFileField : RcpaListViewField
  {
    private readonly OpenFileArgument fileArgument;

    private IValidator validator;

    public RcpaListViewMultipleFileField(Button btnAddFiles, Button btnRemoveFiles, ListView lvItems, String key,
                                         OpenFileArgument fileArgument, bool required, bool validateSelectedItemOnly)
      : this(btnAddFiles, btnRemoveFiles, null, null, lvItems, key, fileArgument, required, validateSelectedItemOnly)
    { }

    public RcpaListViewMultipleFileField(Button btnAddFiles, Button btnRemoveFiles, Button btnLoad, Button btnSave,
                                         ListView lvItems, String key, OpenFileArgument fileArgument, bool required,
                                         bool validateSelectedItemOnly)
      : base(btnRemoveFiles, btnLoad, btnSave, lvItems, key, fileArgument.GetFileDescription())
    {
      this.fileArgument = fileArgument;

      this.validator = new ItemInfosValidator(new ItemInfosListViewAdaptor(lvItems), validateSelectedItemOnly, required, (m => File.Exists(m)), fileArgument.GetFileDescription(), "File not exists : {0}");

      btnAddFiles.Text = "Add";
      btnAddFiles.Click += AddFileClick;

      lvItems.AllowDrop = true;
      lvItems.DragEnter += this.lvDatFiles_DragEnter;
      lvItems.DragDrop += this.lvDatFiles_DragDrop;
    }

    private void AddFileClick(object sender, EventArgs e)
    {
      var dialog = this.fileArgument.GetFileDialog() as OpenFileDialog;
      dialog.Multiselect = true;

      if (lvItems.SelectedItems.Count > 0)
      {
        dialog.FileName = lvItems.SelectedItems[0].Text;
      }
      else if (lvItems.Items.Count > 0)
      {
        dialog.FileName = lvItems.Items[0].Text;
      }

      if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
      {
        AddItems(dialog.FileNames);
      }
    }

    private void lvDatFiles_DragDrop(object sender, DragEventArgs e)
    {
      try
      {
        string[] files = e.Data.GetData(DataFormats.FileDrop, false) as String[];
        AddItems(files);
      }
      catch (Exception)
      { }
    }

    private void lvDatFiles_DragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.FileDrop))
      {
        e.Effect = DragDropEffects.Copy;
      }
      else
      {
        e.Effect = DragDropEffects.None;
      }
    }


    public override void ValidateComponent()
    {
      validator.Validate();
    }

    public string[] FileNames
    {
      get
      {
        return this.GetAllItems();
      }
    }

    public string[] SelectFileNames
    {
      get
      {
        return this.GetSelectedItems();
      }
    }
  }
}