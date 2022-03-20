using System;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RCPA.Gui.Event
{
  public class ItemInfosEventHandlers
  {
    public static readonly string XName = "Items";

    private OpenFileDialog openDialog;

    private SaveFileDialog saveDialog;

    private OptionFileItemInfosAdaptor adaptor;

    public IOptionFile Adaptor { get { return adaptor; } }

    public ItemInfosEventHandlers(IItemInfos items)
    {
      this.adaptor = new OptionFileItemInfosAdaptor(items, XName);
    }

    public void LoadEvent(object sender, EventArgs e)
    {
      if (this.openDialog == null)
      {
        this.openDialog = new OpenFileDialog();
        this.openDialog.Filter = "File list(*.lst)|*.lst|All Files(*.*)|*.*";
      }

      if (this.openDialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
      {
        try
        {
          adaptor.LoadFromXml(XElement.Load(this.openDialog.FileName));
        }
        catch (Exception ex)
        {
          MessageBox.Show(Form.ActiveForm,
            MyConvert.Format("Exception thrown when loading configuration from file {0} : {1}", openDialog.FileName, ex.Message),
            "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    public void SaveEvent(object sender, EventArgs e)
    {
      if (this.saveDialog == null)
      {
        this.saveDialog = new SaveFileDialog();
        this.saveDialog.Filter = "File list(*.lst)|*.lst|All Files(*.*)|*.*";
      }

      if (this.saveDialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
      {
        XElement root = new XElement("configuration");
        adaptor.SaveToXml(root);
        root.Save(this.saveDialog.FileName);
      }
    }

  }
}
