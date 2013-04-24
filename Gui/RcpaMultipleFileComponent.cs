using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using RCPA.Utils;
using System.Configuration;
using System.Xml.Linq;

namespace RCPA.Gui
{
  public class RcpaMultipleFileComponent : AbstractRcpaComponent
  {
    private IValidator validator;

    public RcpaMultipleFileComponent(IItemInfos items, string key, string description, bool validateSelectedItemOnly, bool required)
    {
      Adaptor = new OptionFileItemInfosAdaptor(items, key);

      validator = new ItemInfosValidator(items, validateSelectedItemOnly, required, (m => File.Exists(m)),description, "File not exists : {0}");
    }

    #region IRcpaComponent Members

    public override void ValidateComponent()
    {
      validator.Validate();
    }

    #endregion
  }
}
