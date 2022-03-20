namespace RCPA.Gui
{
  public partial class DoubleField : TextField
  {
    public DoubleField()
    {
      InitializeComponent();

      this.ValidateFunc = (m =>
      {
        double value;
        return double.TryParse(m, out value);
      });
    }

    public double Value
    {
      get
      {
        return double.Parse(Text);
      }
      set
      {
        Text = value.ToString();
      }
    }
  }
}
