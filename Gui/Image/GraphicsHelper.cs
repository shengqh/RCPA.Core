using System.Drawing;

namespace RCPA.Gui.Image
{
  public static class GraphicsHelper
  {
    public static void DrawVerticalString(this Graphics g2, Font font, Brush brush, float x, float y, string information)
    {
      g2.TranslateTransform(x, y);
      try
      {
        g2.RotateTransform(-90F);
        try
        {
          g2.DrawString(information, font, brush, new PointF(0, 0));
        }
        finally
        {
          g2.RotateTransform(90F);
        }
      }
      finally
      {
        g2.TranslateTransform(-x, -y);
      }
    }
  }
}
