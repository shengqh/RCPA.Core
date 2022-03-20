using RCPA.Gui.Event;
using RCPA.Gui.FileArgument;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public partial class MultipleFileField : UserControl, IRcpaComponent
  {
    public MultipleFileField()
    {
      InitializeComponent();

      adaptor = new ItemInfosListBoxAdaptor(lbFiles);

      handlers = new ListBoxFileEventHandlers(lbFiles, null);

      ValidateSelectedItemOnly = false;

      Required = true;

      Key = "File";

      btnAdd.Click += handlers.AddEvent;

      btnRemove.Click += handlers.RemoveEvent;

      btnClear.Click += handlers.ClearEvent;

      btnLoad.Click += handlers.LoadEvent;

      btnSave.Click += handlers.SaveEvent;
    }

    private ItemInfosListBoxAdaptor adaptor;

    private ListBoxFileEventHandlers handlers;

    public OpenFileArgument FileArgument
    {
      get
      {
        return handlers.FileArgument;
      }
      set
      {
        if (value == null)
        {
          btnAdd.Visible = false;
        }

        btnAdd.Visible = true;
        handlers.FileArgument = value;
      }
    }

    [Localizable(true)]
    [Category("RcpaComponent"), DescriptionAttribute("Gets or sets if validate selected item only"), DefaultValue(false)]
    public bool ValidateSelectedItemOnly { get; set; }

    [Localizable(true)]
    [Category("RcpaComponent"), DescriptionAttribute("Gets or sets if the files are required"), DefaultValue(true)]
    public bool Required { get; set; }

    [Localizable(true)]
    [Category("RcpaComponent"), DescriptionAttribute("Gets or sets the key used to save in configuration file"), DefaultValue("")]
    public string Key { get; set; }

    [Localizable(true)]
    [Category("RcpaComponent"), DescriptionAttribute("Gets or sets the button panel visible"), DefaultValue(true)]
    public bool ButtonPanelVisible
    {
      get { return panel2.Visible; }
      set { panel2.Visible = value; }
    }

    [Localizable(true)]
    [Category("RcpaComponent"), DescriptionAttribute("Gets or sets the Load Button visible"), DefaultValue(true)]
    public bool LoadButtonVisible
    {
      get { return btnLoad.Visible; }
      set { btnLoad.Visible = value; }
    }

    [Localizable(true)]
    [Category("RcpaComponent"), DescriptionAttribute("Gets or sets the Save Button visible"), DefaultValue(true)]
    public bool SaveButtonVisible
    {
      get { return btnSave.Visible; }
      set { btnSave.Visible = value; }
    }

    [Localizable(true)]
    [Category("RcpaComponent"), DescriptionAttribute("Gets or sets the SelectionMode of ListBox"), DefaultValue(true)]
    public SelectionMode SelectionMode
    {
      get { return lbFiles.SelectionMode; }
      set { lbFiles.SelectionMode = value; }
    }

    [Localizable(true)]
    [Category("RcpaComponent"), DescriptionAttribute("Gets or sets the FileDescription value"), DefaultValue("")]
    public string FileDescription
    {
      get { return lblTitle.Text; }
      set { lblTitle.Text = value; }
    }

    [Localizable(true)]
    [Category("RcpaComponent"), DescriptionAttribute("Gets the ListBox")]
    public System.Windows.Forms.ListBox ListBox
    {
      get { return this.lbFiles; }
    }

    public ListBox.ObjectCollection Items
    {
      get { return ListBox.Items; }
    }

    public object SelectedItem
    {
      get { return ListBox.SelectedItem; }
      set { ListBox.SelectedItem = value; }
    }

    public int SelectedIndex
    {
      get { return ListBox.SelectedIndex; }
      set { ListBox.SelectedIndex = value; }
    }

    public string[] FileNames
    {
      get { return adaptor.Items.GetAllItems(); }
      set { adaptor.Items = new ItemInfoList(value); }
    }

    public string[] SelectedFileNames
    {
      get { return adaptor.Items.GetSelectedItems(); }
    }

    public IItemInfos GetItemInfos()
    {
      return adaptor;
    }

    public void SelectAll()
    {
      for (int i = 0; i < ListBox.Items.Count; i++)
      {
        ListBox.SetSelected(i, true);
      }
    }

    #region IRcpaComponent Members

    public void ValidateComponent()
    {
      new ItemInfosValidator(adaptor, ValidateSelectedItemOnly, Required, (m => File.Exists(m)), FileDescription, "File not exists : {0}").Validate();
    }

    #endregion

    #region IOptionFile Members

    public void RemoveFromXml(System.Xml.Linq.XElement option)
    {
      new OptionFileItemInfosAdaptor(adaptor, Key).RemoveFromXml(option);
    }

    public void LoadFromXml(System.Xml.Linq.XElement option)
    {
      new OptionFileItemInfosAdaptor(adaptor, Key).LoadFromXml(option);
    }

    public void SaveToXml(System.Xml.Linq.XElement option)
    {
      new OptionFileItemInfosAdaptor(adaptor, Key).SaveToXml(option);
    }

    #endregion

    private void lbFiles_DragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.FileDrop))
      {
        e.Effect = DragDropEffects.Copy;
      }
    }

    private void lbFiles_DragDrop(object sender, DragEventArgs e)
    {
      string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
      foreach (string file in files)
      {
        if (!this.lbFiles.Items.Contains(file))
        {
          this.lbFiles.Items.Add(file);
        }
      }
    }
  }
}
