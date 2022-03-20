using RCPA.Gui.FileArgument;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public class RcpaListViewMultipleFileDirectoryField : RcpaListViewField
  {
    private OpenFileArgument fileArgument;
    private string directoryDescription;

    private IValidator validator;

    public RcpaListViewMultipleFileDirectoryField(Button btnAddFiles, Button btnAddDirectory, Button btnAddSubDirectory, Button btnRemove, ListView lvItems, String key,
                                         OpenFileArgument fileArgument, string directoryDescription, bool required, bool validateSelectedItemOnly)
      : this(btnAddFiles, btnAddDirectory, btnAddSubDirectory, btnRemove, null, null, lvItems, key, fileArgument, directoryDescription, required, validateSelectedItemOnly)
    { }

    public RcpaListViewMultipleFileDirectoryField(Button btnAddFiles, Button btnAddDirectory, Button btnAddSubDirectory, Button btnRemoveFiles, Button btnLoad, Button btnSave,
                                         ListView lvItems, String key, OpenFileArgument fileArgument, string directoryDescription, bool required,
                                         bool validateSelectedItemOnly)
      : base(btnRemoveFiles, btnLoad, btnSave, lvItems, key, fileArgument.GetFileDescription() + " : " + directoryDescription)
    {
      this.fileArgument = fileArgument;
      this.directoryDescription = directoryDescription;

      this.validator = new ItemInfosValidator(new ItemInfosListViewAdaptor(lvItems), validateSelectedItemOnly, required, (m => File.Exists(m) || Directory.Exists(m)), fileArgument.GetFileDescription(), "File/directory not exists : {0}");

      btnAddFiles.Text = "Add File";
      btnAddFiles.Click += AddFileClick;

      btnAddDirectory.Text = "Add Directory";
      btnAddDirectory.Click += AddDirectoryClick;

      btnAddSubDirectory.Text = "Add Sub Directories";
      btnAddSubDirectory.Click += AddSubDirectoryClick;

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

    private bool SelectDirectory(out string directory)
    {
      using (var form = new InputDirectoryForm("Input " + directoryDescription, "Folder", ""))
      {
        var result = form.ShowDialog(Form.ActiveForm) == DialogResult.OK;
        if (result)
        {
          directory = form.Value;
        }
        else
        {
          directory = null;
        }
        return result;
      }
    }

    private void AddDirectoryClick(object sender, EventArgs e)
    {
      string dir;
      if (SelectDirectory(out dir))
      {
        AddItems(new[] { dir });
      }
    }

    private void AddSubDirectoryClick(object sender, EventArgs e)
    {
      string dir;
      if (SelectDirectory(out dir))
      {
        string[] dirs = Directory.GetDirectories(dir);
        AddItems(dirs);
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

    public string[] Pathes
    {
      get
      {
        return this.GetAllItems();
      }
    }

    public string[] SelectPathes
    {
      get
      {
        return this.GetSelectedItems();
      }
    }

    public Dictionary<string, string> PathMap
    {
      get
      {
        return this.infoAdaptor.Items.GetAllItemMap();
      }
    }

    public Dictionary<string, string> SelectPathMap
    {
      get
      {
        return this.infoAdaptor.Items.GetSelectItemMap();
      }
    }
  }
}