using System.Configuration;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public interface IRcpaComponent : IOptionFile
  {
    /**
     * Validate current component
     */
    void ValidateComponent();
  }

  public interface IDependentRcpaComponent : IRcpaComponent
  {
    CheckBox PreCondition { get; set; }

    bool Enabled { get; set; }
  }
}