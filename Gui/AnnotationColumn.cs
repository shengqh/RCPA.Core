using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public class AnnotationColumn : DataGridViewColumn
  {
    public AnnotationColumn() : base(new AnnotationCell()) { }

    public override DataGridViewCell CellTemplate
    {
      get
      {
        return base.CellTemplate;
      }
      set
      {
        // Ensure that the cell used for the template is a AnnotationCell. 
        if (value != null &&
            !value.GetType().IsAssignableFrom(typeof(AnnotationCell)))
        {
          throw new InvalidCastException("Must be a AnnotationCell");
        }
        base.CellTemplate = value;
      }
    }

    private AnnotationCell AnnotationCellTemplate
    {
      get
      {
        return CellTemplate as AnnotationCell;
      }
    }

    [Category("Appearance"), Description("Indicates the key of annotation.")]
    public string Key
    {
      get
      {
        if (this.AnnotationCellTemplate == null)
        {
          throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
        }
        return this.AnnotationCellTemplate.Key;
      }
      set
      {
        if (this.AnnotationCellTemplate == null)
        {
          throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
        }
        // Update the template cell so that subsequent cloned cells use the new value.
        this.AnnotationCellTemplate.Key = value;
        if (this.DataGridView != null)
        {
          // Update all the existing DataGridViewNumericUpDownCell cells in the column accordingly.
          DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
          int rowCount = dataGridViewRows.Count;
          for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
          {
            // Be careful not to unshare rows unnecessarily. 
            // This could have severe performance repercussions.
            DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
            AnnotationCell dataGridViewCell =
                      dataGridViewRow.Cells[this.Index] as AnnotationCell;
            if (dataGridViewCell != null)
            {
              // Call the internal SetDecimalPlaces method instead of the                                         property to avoid invalidation                     // of each cell. The whole column is invalidated later in a single                                         operation for better performance.
              dataGridViewCell.Key = value;
            }
          }
          this.DataGridView.InvalidateColumn(this.Index);
          // TODO: Call the grid's autosizing methods to autosize the column,                              rows, column headers / row headers as needed.
        }
      }
    }
  }
}
