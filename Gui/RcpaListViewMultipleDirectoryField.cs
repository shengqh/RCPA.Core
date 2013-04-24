using System;
using System.IO;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public class RcpaListViewMultipleDirectoryField : RcpaListViewField
  {
    public ItemInfosValidator Validator { get; set; }

    public RcpaListViewMultipleDirectoryField(Button btnAdd, Button btnRemove, ListView lstDirectories, String key,
                                              string description, bool required, bool validateSelectedItemOnly)
      : this(btnAdd, btnRemove, null, null, lstDirectories, key, description, required, validateSelectedItemOnly)
    { }

    public RcpaListViewMultipleDirectoryField(Button btnAdd, Button btnRemove, Button btnLoad, Button btnSave,
                                              ListView lstDirectories, string key, string description, bool required,
                                              bool validateSelectedItemOnly)
      : base(btnRemove, btnLoad, btnSave, lstDirectories, key, description)
    {
      btnAdd.Text = "Add";
      btnAdd.Click += AddClick;

      this.Validator = new ItemInfosValidator(new ItemInfosListViewAdaptor(lvItems), validateSelectedItemOnly, required, (m => Directory.Exists(m)), description, "Directory not exists : {0}");
    }

    private void AddClick(object sender, EventArgs e)
    {
      var dialog = new FolderBrowserDialog();
      dialog.Description = description;

      if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
      {
        AddItems(new[] { dialog.SelectedPath });
      }
    }

    public void AddDirectories(string[] directories)
    {
      AddItems(directories);
    }

    public override void ValidateComponent()
    {
      Validator.Validate();
    }
  }
}