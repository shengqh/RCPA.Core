namespace RCPA.Gui
{
  public partial class IntegerField : TextField
  {
    public IntegerField()
    {
      InitializeComponent();

      this.ValidateFunc = (m =>
      {
        int value;
        return int.TryParse(m, out value);
      });
    }

    public int Value
    {
      get
      {
        return int.Parse(Text);
      }
      set
      {
        Text = value.ToString();
      }
    }
  }
}
