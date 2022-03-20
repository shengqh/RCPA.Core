using System;
using System.Drawing;

namespace RCPA.Utils
{
  public class ResizeAndRotationHelper
  {
    private byte[,] source;
    private int nWidth;
    private int nHeight;
    private byte backGroundColor;

    public ResizeAndRotationHelper(byte[,] source, byte backGroundColor)
    {
      this.source = source;
      this.nWidth = source.GetLength(1);
      this.nHeight = source.GetLength(0);
      this.backGroundColor = backGroundColor;
    }

    public byte[,] ResizeAndRotate(int tWidth, int tHeight, float angle)
    {
      if (tWidth == nWidth && tHeight == nHeight)
      {
        return new RotationHelper(source, backGroundColor).Rotate(angle);
      }

      // Negative the angle, because the y-axis is negative.
      double ang = -GraphicsUtils.DegreeToRadian(angle);
      double cos_angle = Math.Cos(ang);
      double sin_angle = Math.Sin(ang);

      // Calculate the size of the new bitmap
      Point p1 = new Point(0, 0);
      Point p2 = new Point(tWidth, 0);
      Point p3 = new Point(0, tHeight);
      Point p4 = new Point(tWidth, tHeight);
      PointF newP1, newP2, newP3, newP4, leftTop, rightTop, leftBottom, rightBottom;

      newP1 = new PointF((float)p1.X, (float)p1.Y);
      newP2 = new PointF((float)(p2.X * cos_angle - p2.Y * sin_angle), (float)(p2.X * sin_angle + p2.Y * cos_angle));
      newP3 = new PointF((float)(p3.X * cos_angle - p3.Y * sin_angle), (float)(p3.X * sin_angle + p3.Y * cos_angle));
      newP4 = new PointF((float)(p4.X * cos_angle - p4.Y * sin_angle), (float)(p4.X * sin_angle + p4.Y * cos_angle));

      leftTop = new PointF(Math.Min(Math.Min(newP1.X, newP2.X), Math.Min(newP3.X, newP4.X)), Math.Min(Math.Min(newP1.Y, newP2.Y), Math.Min(newP3.Y, newP4.Y)));
      rightBottom = new PointF(Math.Max(Math.Max(newP1.X, newP2.X), Math.Max(newP3.X, newP4.X)), Math.Max(Math.Max(newP1.Y, newP2.Y), Math.Max(newP3.Y, newP4.Y)));
      leftBottom = new PointF(leftTop.X, rightBottom.Y);
      rightTop = new PointF(rightBottom.X, leftTop.Y);

      int newWidth = (int)(0.5 + rightTop.X - leftTop.X);
      int newHeight = (int)(0.5 + leftBottom.Y - leftTop.Y);

      byte[,] result = new byte[newHeight, newWidth];
      int x, y, newX, newY, oldX, oldY;

      int startX = (int)leftTop.X;
      int endX = (int)rightTop.X;
      int startY = (int)leftTop.Y;
      int endY = (int)leftBottom.Y;

      float wRatio = (float)nWidth / tWidth;
      float hRatio = (float)nHeight / tHeight;

      for (x = startX, newX = 0; x < endX; x++, newX++)
      {
        for (y = startY, newY = 0; y < endY; y++, newY++)
        {
          oldX = (int)((x * cos_angle + y * sin_angle) * wRatio + 0.5);
          if (oldX < 0 || oldX >= nWidth)
          {
            result[newY, newX] = backGroundColor;
            continue;
          }

          oldY = (int)((y * cos_angle - x * sin_angle) * hRatio + 0.5);
          if (oldY < 0 || oldY >= nHeight)
          {
            result[newY, newX] = backGroundColor;
            continue;
          }

          result[newY, newX] = source[oldY, oldX];
        }
      }

      return result;
    }
  }
}
