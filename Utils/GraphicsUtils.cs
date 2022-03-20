using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace RCPA.Utils
{
  public static class GraphicsUtils
  {
    public static Bitmap greyscale;

    static GraphicsUtils()
    {
      greyscale = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
      ColorPalette ImagePal = greyscale.Palette;
      for (int i = 0; i < ImagePal.Entries.Length; i++)
      {
        ImagePal.Entries[i] = Color.FromArgb(i, i, i);
      }
      greyscale.Palette = ImagePal;
    }

    public static double DegreeToRadian(double degree)
    {
      return Math.PI * degree / 180;
    }

    public static double RadianToDegree(double radian)
    {
      return 180 * radian / Math.PI;
    }

    /// <summary>
    /// Transfer byte array to bitmap in safe mode
    /// </summary>
    /// <param name="src">color array in [Height, Width]</param>
    /// <returns>bitmap</returns>
    public static Bitmap ByteToBitmapSafe(byte[,] src)
    {
      int FrameWidth = src.GetLength(1);
      int FrameHeight = src.GetLength(0);

      Bitmap result = new Bitmap(FrameWidth, FrameHeight);
      for (int y = 0; y < FrameHeight; y++)
      {
        for (int x = 0; x < FrameWidth; x++)
        {
          int color = src[y, x];
          Color grey = Color.FromArgb(color, color, color);
          result.SetPixel(x, y, grey);
        }
      }

      return result;
    }

    /// <summary>
    /// Transfer byte array to bitmap in unsafe mode
    /// </summary>
    /// <param name="src">color array in [Height, Width]</param>
    /// <returns>bitmap</returns>
    public unsafe static Bitmap ByteToBitmap(byte[,] src)
    {
      int FrameWidth = src.GetLength(1);
      int FrameHeight = src.GetLength(0);

      Bitmap result = new Bitmap(FrameWidth, FrameHeight, PixelFormat.Format24bppRgb);
      BitmapData bData = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.ReadWrite, result.PixelFormat);
      try
      {
        byte bitsPerPixel = GetBitsPerPixel(bData.PixelFormat);
        int stride = bData.Stride;
        int hsize = bitsPerPixel / 8;

        /*This time we convert the IntPtr to a ptr*/
        byte* scan0 = (byte*)bData.Scan0.ToPointer();
        for (int i = 0; i < bData.Height; ++i)
        {
          for (int j = 0; j < bData.Width; ++j)
          {
            //data is a pointer to the first byte of the 3-byte color data        
            byte* data = scan0 + i * stride + j * hsize;
            data[0] = src[i, j];
            data[1] = src[i, j];
            data[2] = src[i, j];
          }
        }
      }
      finally
      {
        result.UnlockBits(bData);
      }

      return result;
    }

    public static Bitmap ResizeAndRotateCenter(Bitmap bmpSrc, int newWidth, int newHeight, float theta)
    {
      if (theta == 0 && bmpSrc.Width == newWidth && bmpSrc.Height == newHeight)
      {
        return bmpSrc;
      }

      Matrix mRotate = new Matrix();
      if (bmpSrc.Width != newWidth || bmpSrc.Height != newHeight)
      {
        mRotate.Scale((float)newWidth / bmpSrc.Width, (float)newHeight / bmpSrc.Height, MatrixOrder.Append);
      }
      mRotate.Translate((float)(newWidth / -2.0), (float)(newHeight / -2.0), MatrixOrder.Append);
      if (theta % 360 != 0 && theta % -360 != 0)
      {
        mRotate.RotateAt(-theta, new Point(0, 0), MatrixOrder.Append);
      }
      using (GraphicsPath gp = new GraphicsPath())
      {  // transform image points by rotation matrix
        gp.AddPolygon(new Point[] { new Point(0, 0), new Point(bmpSrc.Width, 0), new Point(0, bmpSrc.Height), new Point(bmpSrc.Width, bmpSrc.Height) });
        gp.Transform(mRotate);
        PointF[] pts = gp.PathPoints;

        Rectangle rec = Rectangle.Round(gp.GetBounds());

        // create destination bitmap sized to contain rotated source image
        Bitmap bmpDest = new Bitmap(rec.Width, rec.Height, bmpSrc.PixelFormat);

        using (Graphics gDest = Graphics.FromImage(bmpDest))
        {  // draw source into dest
          Matrix mDest = new Matrix();
          mDest.Translate(bmpDest.Width / 2, bmpDest.Height / 2, MatrixOrder.Append);
          gDest.Transform = mDest;
          gDest.DrawImage(bmpSrc, new PointF[] { pts[0], pts[1], pts[2] });
          return bmpDest;
        }
      }
    }

    public static byte[,] ResizeAndRotateCenter(byte[,] src, int newWidth, int newHeight, float theta)
    {
      if (newWidth == src.GetLength(1) && newHeight == src.GetLength(0) && theta == 0)
      {
        return src;
      }
      //TimeStampCounter counter = new TimeStampCounter();
      //counter.Start();

      using (Bitmap b = ByteToBitmap(src))
      {
        //counter.Stop();
        //Console.WriteLine("ByteToBitmap cost {0:0.0}ms", counter.Duration());

        //counter.Start();
        using (Bitmap two = ResizeAndRotateCenter(b, newWidth, newHeight, theta))
        {
          //counter.Stop();
          //Console.WriteLine("ResizeAndRotateCenter cost {0:0.0}ms", counter.Duration());

          //counter.Start();
          var result = BitmapToBytes(two);
          //counter.Stop();
          //Console.WriteLine("BitmapToBytes cost {0:0.0}ms", counter.Duration());

          return result;
        }
      }
    }

    private static byte GetBitsPerPixel(PixelFormat pixelFormat)
    {
      switch (pixelFormat)
      {
        case PixelFormat.Format24bppRgb:
          return 24;
        case PixelFormat.Format32bppArgb:
        case PixelFormat.Format32bppPArgb:
        case PixelFormat.Format32bppRgb:
          return 32;
        case PixelFormat.Format8bppIndexed:
          return 8;
        default:
          throw new ArgumentException("Only 8, 24 and 32 bit images are supported");
      }
    }

    public unsafe static byte[,] BitmapToBytes(Bitmap r)
    {
      byte[,] result = new byte[r.Height, r.Width];

      BitmapData bData = r.LockBits(new Rectangle(0, 0, r.Width, r.Height), ImageLockMode.ReadOnly, r.PixelFormat);
      try
      {
        byte bitsPerPixel = GetBitsPerPixel(bData.PixelFormat);
        /*This time we convert the IntPtr to a ptr*/
        byte* scan0 = (byte*)bData.Scan0.ToPointer();
        for (int i = 0; i < bData.Height; ++i)
        {
          for (int j = 0; j < bData.Width; ++j)
          {
            //data is a pointer to the first byte of the 3-byte color data        
            byte* data = scan0 + i * bData.Stride + j * bitsPerPixel / 8;
            result[i, j] = data[0];
          }
        }
      }
      finally
      {
        r.UnlockBits(bData);
      }

      return result;
    }

    public static Size GetRotatedSize(Size oldSize, float theta)
    {
      int halfWidth = oldSize.Width / 2;
      int halfHeight = oldSize.Height / 2;

      Matrix mRotate = new Matrix();
      mRotate.Translate(-halfWidth, -halfHeight, MatrixOrder.Append);
      mRotate.RotateAt(theta, new Point(0, 0), MatrixOrder.Append);

      Point topLeft = new Point(-halfWidth, -halfHeight);
      Point topRight = new Point(halfWidth, -halfHeight);
      Point bottomRight = new Point(halfWidth, halfHeight);
      Point bottomLeft = new Point(-halfWidth, halfHeight);
      Point[] points = new Point[] { topLeft, topRight, bottomRight, bottomLeft };
      GraphicsPath gp = new GraphicsPath(points, new byte[] { (byte)PathPointType.Start, (byte)PathPointType.Line, (byte)PathPointType.Line, (byte)PathPointType.Line });
      gp.Transform(mRotate);

      return Size.Round(gp.GetBounds().Size);
    }

    private static Rectangle boundingBox(Image img, Matrix matrix)
    {
      GraphicsUnit gu = new GraphicsUnit();
      Rectangle rImg = Rectangle.Round(img.GetBounds(ref gu));

      // Transform the four points of the image, to get the resized bounding box.
      Point topLeft = new Point(rImg.Left, rImg.Top);
      Point topRight = new Point(rImg.Right, rImg.Top);
      Point bottomRight = new Point(rImg.Right, rImg.Bottom);
      Point bottomLeft = new Point(rImg.Left, rImg.Bottom);
      Point[] points = new Point[] { topLeft, topRight, bottomRight, bottomLeft };
      GraphicsPath gp = new GraphicsPath(points, new byte[] { (byte)PathPointType.Start, (byte)PathPointType.Line, (byte)PathPointType.Line, (byte)PathPointType.Line });
      gp.Transform(matrix);
      return Rectangle.Round(gp.GetBounds());
    }
  }
}
