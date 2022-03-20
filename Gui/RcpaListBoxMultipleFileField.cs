using RCPA.Gui.Event;
using RCPA.Gui.FileArgument;
using RCPA.Utils;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public class RcpaListBoxMultipleFileField : AbstractRcpaComponent
  {
    private readonly OpenFileArgument fileArgument;

    private readonly ListBox lstFiles;

    private ItemInfosListBoxAdaptor listBoxAdaptor;

    private ItemInfosValidator validator;

    public RcpaListBoxMultipleFileField(Button btnAddFiles, Button btnRemoveFiles, Button btnClear, Button btnLoad, Button btnSave,
                                 ListBox lstFiles, String key, OpenFileArgument fileArgument, bool required,
                                 bool validateSelectedItemOnly)
    {
      Childrens.Add(lstFiles);

      listBoxAdaptor = new ItemInfosListBoxAdaptor(lstFiles);

      validator = new ItemInfosValidator(listBoxAdaptor, validateSelectedItemOnly, required, (m => File.Exists(m)), fileArgument.GetFileDescription(), "File not exists : {0}");

      this.fileArgument = fileArgument;

      this.lstFiles = lstFiles;

      ListBoxFileEventHandlers handlers = new ListBoxFileEventHandlers(lstFiles, fileArgument);

      if (btnAddFiles != null)
      {
        btnAddFiles.Text = "Add";
        btnAddFiles.Click += handlers.AddEvent;
        Childrens.Add(btnAddFiles);
      }

      if (btnRemoveFiles != null)
      {
        btnRemoveFiles.Text = "Remove";
        btnRemoveFiles.Click += handlers.RemoveEvent;
        Childrens.Add(btnRemoveFiles);
      }

      if (btnLoad != null)
      {
        btnLoad.Text = "Load";
        btnLoad.Click += handlers.LoadEvent;
        Childrens.Add(btnLoad);
      }

      if (btnSave != null)
      {
        btnSave.Text = "Save";
        btnSave.Click += handlers.SaveEvent;
        Childrens.Add(btnSave);
      }

      Adaptor = new OptionFileItemInfosAdaptor(listBoxAdaptor, key);
    }

    public string[] FileNames
    {
      get
      {
        return lstFiles.Items.Cast<string>().ToArray();
      }
      set
      {
        this.lstFiles.Items.Clear();
        this.lstFiles.Items.AddRange(value);
      }
    }

    public string[] SelectedFileNames
    {
      get
      {
        return lstFiles.SelectedItems.Cast<string>().ToArray();
      }
      set
      {
        var allItems = FileNames.Union(value).OrderBy(m => m).ToArray();

        lstFiles.BeginUpdate();
        try
        {
          lstFiles.Items.Clear();
          lstFiles.Items.AddRange(allItems);
          value.ToList().ForEach(m => lstFiles.SetSelected(lstFiles.Items.IndexOf(m), true));
        }
        finally
        {
          lstFiles.EndUpdate();
        }
      }
    }

    #region IRcpaComponent Members

    public override void ValidateComponent()
    {
      validator.Validate();
    }

    #endregion
  }
}