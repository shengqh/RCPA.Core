using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Drawing.Drawing2D;
using MathNet.Numerics.Statistics;
using MathNet.Numerics.Distributions;
using RCPA.Utils;
using RCPA;

namespace ZedGraph
{
  public static class ZedGraphicExtension
  {
    public static void ClearData(this GraphPane myPane)
    {
      myPane.CurveList.Clear();
      myPane.GraphObjList.Clear();
    }

    public static MasterPane InitMasterPanel(this ZedGraphControl zgcGraph, Graphics g, int graphCount, string title, PaneLayout pl = PaneLayout.SingleColumn)
    {
      MasterPane myMaster = zgcGraph.MasterPane;
      myMaster.PaneList.Clear();

      myMaster.Margin.All = 10;
      myMaster.InnerPaneGap = 10;

      // Set the master pane title
      myMaster.Title.Text = title;
      myMaster.Title.IsVisible = true;
      myMaster.Legend.IsVisible = false;

      for (int i = 0; i < graphCount; i++)
      {
        myMaster.Add(new GraphPane());
      }
      myMaster.SetLayout(g, pl);
      zgcGraph.AxisChange();
      return myMaster;
    }

    public static GraphPane AddPanel(this ZedGraphControl zgcGraph)
    {
      var result = new GraphPane();
      zgcGraph.MasterPane.Add(result);
      zgcGraph.AxisChange();
      return result;
    }

    public static LineItem AddPoints(this GraphPane myPane, PointPairList ppl, Color color, string title = "")
    {
      if (ppl.Count > 0)
      {
        LineItem curve = myPane.AddCurve(title, ppl, color, SymbolType.Diamond);
        curve.Symbol.Size = 8;
        curve.Symbol.Fill = new Fill(color);
        curve.Symbol.Border.IsVisible = false;
        curve.Line.IsVisible = false;
        return curve;
      }

      return null;
    }

    public static LineItem AddPoints(this GraphPane myPane, PointPairList ppl, Color color, Color[] colors, string title = "")
    {
      if (ppl.Count > 0)
      {
        LineItem curve = myPane.AddCurve(title, ppl, color, SymbolType.Diamond);
        curve.Symbol.Size = 8;
        curve.Symbol.Fill = new Fill(colors)
        {
          Type = FillType.GradientByColorValue,
          RangeMin = 0,
          RangeMax = colors.Length - 1,
          SecondaryValueGradientColor = Color.Black
        };
        curve.Symbol.Border.IsVisible = false;
        curve.Line.IsVisible = false;
        return curve;
      }

      return null;
    }

    public static void ClearData(this ZedGraphControl zgc, bool update)
    {
      zgc.GraphPane.ClearData();

      if (update)
      {
        zgc.RestoreScale(zgc.GraphPane);
        UpdateGraph(zgc);
      }
    }

    public static void InitGraph(this ZedGraphControl zgc, string zgcTitle, string xTitle, string yTitle,
                                 bool bClusterScaleWidthAuto, double dClusterScaleWidth)
    {
      // Enable scrollbars if needed
      zgc.IsAutoScrollRange = true;
      zgc.GraphPane.InitGraphPane(zgcTitle, xTitle, yTitle, bClusterScaleWidthAuto, dClusterScaleWidth);

      UpdateGraph(zgc);
    }

    public static void InitGraphPane(this GraphPane myPane, string zgcTitle, string xTitle, string yTitle,
                                 bool bClusterScaleWidthAuto, double dClusterScaleWidth)
    {
      myPane.Border.IsVisible = false;

      // Set the titles and axis labels
      myPane.Title.Text = zgcTitle;
      myPane.Title.IsVisible = zgcTitle != null && zgcTitle.Length > 0;

      myPane.XAxis.Title.Text = xTitle;
      myPane.YAxis.Title.Text = yTitle;

      myPane.BarSettings.ClusterScaleWidthAuto = bClusterScaleWidthAuto;
      if (!bClusterScaleWidthAuto)
      {
        myPane.BarSettings.ClusterScaleWidth = dClusterScaleWidth;
      }
    }

    public static void UpdateGraph(this ZedGraphControl zgc)
    {
      // Tell ZedGraph to calculate the axis ranges
      // Note that you MUST call this after enabling IsAutoScrollRange, since AxisChange() sets
      // up the proper scrolling parameters
      zgc.AxisChange();

      // Make sure the Graph gets redrawn
      zgc.Invalidate();
    }

    public static void SetDataToBar(this ZedGraphControl zgc, PointPairList list, String title, Color color)
    {
      ClearData(zgc, false);

      AddDataToBar(zgc, list, title, color, true);
    }

    public static void AddDataToBar(this ZedGraphControl zgc, PointPairList list, String title, Color color, bool update)
    {
      GraphPane myPane = zgc.GraphPane;

      BarItem barItem = myPane.AddBar(title, list, color);

      if (update)
      {
        UpdateGraph(zgc);
      }
    }

    public static void AddDataToLine(this ZedGraphControl zgc, String title, PointPairList list, Color color, bool update)
    {
      GraphPane myPane = zgc.GraphPane;

      myPane.AddCurve(title, list, color, SymbolType.None);

      if (update)
      {
        UpdateGraph(zgc);
      }
    }

    public static void AddPoly(this GraphPane myPane, String title, PointPairList list, Color color, Color[] fillColor)
    {
      var pds = new PointD[list.Count + 2];
      pds[0] = new PointD(list[0].X, 0);
      for (int i = 0; i < list.Count; i++)
      {
        pds[i + 1] = new PointD(list[i].X, list[i].Y);
      }
      pds[list.Count + 1] = new PointD(list[list.Count - 1].X, 0);
      var obj = new PolyObj(pds);
      obj.Border = new Border(false, Color.Empty, 0);

      if (fillColor.Length == 0)
      {
        throw new ArgumentException("fillColor should contains at least one color");
      }

      if (fillColor.Length == 1)
      {
        obj.Fill = new Fill(fillColor[0]);
      }
      else
      {
        obj.Fill = new Fill(fillColor, 90F);
      }
      obj.ZOrder = ZOrder.E_BehindCurves;


      myPane.GraphObjList.Add(obj);
    }

    public static void AddPoly(this ZedGraphControl zgc, String title, PointPairList list, Color color, bool update, Color[] fillColor)
    {
      zgc.GraphPane.AddPoly(title, list, color, fillColor);

      if (update)
      {
        UpdateGraph(zgc);
      }
    }

    private static Color defaultTagColor = Color.FromArgb(0, 255, 0);

    public static void AddIndividualLine(this GraphPane myPane, String title, PointPairList list, Color? defaultColor = null, Color? tagColor = null, int tagFontSize = 10, DashStyle dStyle = DashStyle.Solid)
    {
      int index = 0;

      Color defaultLineColor = defaultColor.HasValue ? defaultColor.Value : Color.Black;
      Color defaultTagColor = tagColor.HasValue ? tagColor.Value : Color.Green;

      list.ForEach(pt =>
      {
        var ppl = new PointPairList();
        ppl.Add(new PointPair(pt.X, 0));
        ppl.Add(pt);

        Color curColor = pt.Tag == null ? defaultLineColor : defaultTagColor;
        LineItem line;
        if (index == 0)
        {
          line = myPane.AddCurve(title, ppl, curColor, SymbolType.None);
          index++;
        }
        else
        {
          line = myPane.AddCurve("", ppl, curColor, SymbolType.None);
        }
        line.Line.Style = dStyle;

        if (pt.Tag != null)
        {
          TextObj text = new TextObj(pt.Tag.ToString(), pt.X, pt.Y, CoordType.AxisXYScale, AlignH.Left, AlignV.Center);

          text.ZOrder = ZOrder.A_InFront;
          // Hide the border and the fill
          text.FontSpec.Border.IsVisible = false;
          text.FontSpec.Fill.IsVisible = false;
          text.FontSpec.Angle = 90;  //字体倾斜度
          text.FontSpec.Size = tagFontSize;
          text.FontSpec.FontColor = curColor;
          myPane.GraphObjList.Add(text);
        }
      });
    }

    public static void AddIndividualLine(this ZedGraphControl zgc, String title, PointPairList list, Color defaultColor, bool update)
    {
      zgc.GraphPane.AddIndividualLine(title, list, defaultColor);

      if (update)
      {
        UpdateGraph(zgc);
      }
    }

    public static void AddIndividualLine(this ZedGraphControl zgc, PointPairList list, Color[] colors, bool update)
    {
      GraphPane myPane = zgc.GraphPane;
      int index = 0;
      list.ForEach(pp =>
      {
        var ppl = new PointPairList();
        ppl.Add(new PointPair(pp.X, 0));
        ppl.Add(new PointPair(pp.X, pp.Y));
        myPane.AddCurve("", ppl, colors[index++], SymbolType.None);
      });

      if (update)
      {
        UpdateGraph(zgc);
      }
    }

    public static void AddPoints(this ZedGraphControl zgc, PointPairList ppl, Color color)
    {
      AddPoints(zgc, "", ppl, color);
    }

    public static void AddPoints(this ZedGraphControl zgc, string title, PointPairList ppl, Color color)
    {
      if (ppl.Count > 0)
      {
        LineItem curve = zgc.GraphPane.AddCurve(title, ppl, color, SymbolType.Diamond);
        curve.Symbol.Size = 8;
        curve.Symbol.Fill = new Fill(color);
        curve.Symbol.Border.IsVisible = false;
        curve.Line.IsVisible = false;
      }
    }

    public static PointPairList GetRegressionLine(this PointPairList pplTotal, double ratio)
    {
      var result = new PointPairList();
      result.Add(new PointPair(0.0, 0.0));
      if (ratio > 1)
      {
        double maxSample = pplTotal.Max(m => m.Y);

        result.Add(new PointPair(maxSample / ratio, maxSample));
      }
      else
      {
        double maxRef = pplTotal.Max(m => m.X);

        result.Add(new PointPair(maxRef, maxRef * ratio));
      }
      return result;
    }

    public static PointPairList GetRegressionLine(this PointPairList pplTotal, double ratio, double distance)
    {
      var result = new PointPairList();

      if (distance >= 0)
      {
        result.Add(new PointPair(0.0, distance));
      }
      else
      {
        result.Add(new PointPair(-distance / ratio, 0.0));
      }

      if (ratio > 1)
      {
        double maxSample =
          (from p in pplTotal
           orderby p.Y descending
           select p.Y).First();

        result.Add(new PointPair((maxSample - distance) / ratio, maxSample));
      }
      else
      {
        double maxRef =
          (from p in pplTotal
           orderby p.X descending
           select p.X).First();

        result.Add(new PointPair(maxRef, maxRef * ratio + distance));
      }
      return result;
    }

    public static object GetObject(this ZedGraphControl zgc, MouseEventArgs e)
    {
      for (int i = 0; i < zgc.MasterPane.PaneList.Count; i++)
      {
        GraphPane myPane = zgc.MasterPane.PaneList[i];
        CurveItem item;
        int iNearest;
        myPane.FindNearestPoint(new PointF(e.X, e.Y), out item, out iNearest);
        if (null != item)
        {
          return item.Points[iNearest].Tag;
        }
      }

      return null;
    }

    public static int GetPaneIndex(this ZedGraphControl zgc, MouseEventArgs e)
    {
      for (int i = 0; i < zgc.MasterPane.PaneList.Count; i++)
      {
        GraphPane myPane = zgc.MasterPane.PaneList[i];
        if (myPane.Rect.Left < e.X && myPane.Rect.Left + myPane.Rect.Width > e.X &&
          myPane.Rect.Top < e.Y && myPane.Rect.Top + myPane.Rect.Height > e.Y)
        {
          return i;
        }
      }
      return -1;
    }

    public static void SetListViewItemVisible(this ListView lv, object obj, bool unSelectOther)
    {
      if (null != obj)
      {
        foreach (ListViewItem lvi in lv.Items)
        {
          if (lvi.Tag == obj)
          {
            lvi.Selected = true;
            lvi.EnsureVisible();
          }
          else if (unSelectOther && lvi.Selected)
          {
            lvi.Selected = false;
          }
        }
      }
    }

    public static void SetListViewItemVisible(ListView lv, object sender, MouseEventArgs e, bool unSelectOther)
    {
      object obj = GetObject(sender as ZedGraphControl, e);
      SetListViewItemVisible(lv, obj, unSelectOther);
    }

    public static void AddText(this GraphPane panel, PointPairList ppl, int shift, int angle, Func<PointPair, string> ppFunc)
    {
      for (int i = 0; i < ppl.Count; i++)
      {
        // format the label string to have 1 decimal place
        string lab = ppFunc(ppl[i]);
        // create the text item (assumes the x axis is ordinal or text)
        // for negative bars, the label appears just above the zero value
        var text = new TextObj(lab, ppl[i].X, ppl[i].Y + shift);
        // tell Zedgraph to use user scale units for locating the TextItem
        text.Location.CoordinateFrame = CoordType.AxisXYScale;
        // AlignH the left-center of the text to the specified point
        text.Location.AlignH = AlignH.Left;
        text.Location.AlignV = AlignV.Center;
        text.FontSpec.Border.IsVisible = false;
        text.FontSpec.Fill.IsVisible = false;
        // rotate the text 90 degrees
        text.FontSpec.Angle = angle;
        // add the TextItem to the list
        panel.GraphObjList.Add(text);
      }
    }

    public static void AddTextUp(this GraphPane panel, PointPairList ppl, int shift, Func<PointPair, string> ppFunc)
    {
      AddText(panel, ppl, Math.Abs(shift), 90, ppFunc);
    }

    public static void AddTextDown(this GraphPane panel, PointPairList ppl, int shift, Func<PointPair, string> ppFunc)
    {
      AddText(panel, ppl, -Math.Abs(shift), 270, ppFunc);
    }

    public static void DrawProbabilityRange(this GraphPane panel, double maxX, List<double> ratios)
    {
      var skipCount = ratios.Count / 20;

      var keptRatios = ratios.Skip(skipCount).ToList().Take(ratios.Count - 2 * skipCount).ToList();

      if (keptRatios.Count > 1)
      {
        var mean = Statistics.Mean(keptRatios);
        var sd = Statistics.StandardDeviation(keptRatios);

        var nd = new Normal(mean, sd);

        PointPairList pplMean = new PointPairList();
        pplMean.Add(new PointPair(0, mean));
        pplMean.Add(new PointPair(maxX, mean));
        LineItem meanLine = panel.AddCurve(MyConvert.Format("Mean={0:0.0000}, Sigma={1:0.0000}", mean, sd), pplMean, Color.Red, SymbolType.None);
        meanLine.Line.IsVisible = true;

        var prob90range = nd.GetProb90Range();

        PointPairList pplMin90 = new PointPairList();
        pplMin90.Add(new PointPair(0, prob90range.First));
        pplMin90.Add(new PointPair(maxX, prob90range.First));
        LineItem min95Line = panel.AddCurve("90% Confidence", pplMin90, Color.Brown, SymbolType.None);
        min95Line.Line.IsVisible = true;

        PointPairList pplMax90 = new PointPairList();
        pplMax90.Add(new PointPair(0, prob90range.Second));
        pplMax90.Add(new PointPair(maxX, prob90range.Second));
        LineItem max95Line = panel.AddCurve("", pplMax90, Color.Brown, SymbolType.None);
        max95Line.Line.IsVisible = true;
      }
    }
  }
}