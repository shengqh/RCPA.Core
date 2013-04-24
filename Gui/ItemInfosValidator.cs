using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCPA.Gui
{
  public class ItemInfosValidator : IValidator
  {
    private IItemInfos items;
    private bool required;
    private bool validateSelectedItemOnly;

    public Func<string, bool> ValidateFunc { get; set; }

    private string description;
    private string errorFormat;

    public ItemInfosValidator(IItemInfos items, bool validateSelectedItemOnly, bool required, Func<string, bool> validateFunc, string description, string errorFormat)
    {
      this.items = items;
      this.validateSelectedItemOnly = validateSelectedItemOnly;
      this.required = required;
      this.ValidateFunc = validateFunc;
      this.description = description;
      this.errorFormat = errorFormat;
    }

    public void Validate()
    {
      if (!required)
      {
        return;
      }

      string[] items = validateSelectedItemOnly ? GetSelectedItems() : GetAllItems();

      if (items.Length == 0)
      {
        throw new InvalidOperationException("Input/select " + description + " first.");
      }

      foreach (string value in items)
      {
        if (!ValidateFunc(value))
        {
          throw new InvalidOperationException(MyConvert.Format(errorFormat, value));
        }
      }
    }

    private string[] GetAllItems()
    {
      return items.Items.ConvertAll(m => m.SubItems[0]).ToArray();
    }

    private string[] GetSelectedItems()
    {
      return (from item in items.Items
              where item.Selected
              select item.SubItems[0]).ToArray();
    }
  }
}
