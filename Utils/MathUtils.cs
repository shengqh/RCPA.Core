using System;
using System.Linq;

namespace RCPA.Utils
{
  public static class MathUtils
  {
    public static double Exp10(double x)
    {
      return Math.Exp(x * Math.Log(10));
    }

    public static double Int32BitsToSingle(int bits)
    {
      return new Int32SingleUnion(bits).AsSingle;
    }

    public static int SingleToInt32Bits(float value)
    {
      return new Int32SingleUnion(value).AsInt32;
    }

    public static double[] Byte64ToDoubleList(int expectedLength, bool isBigEndian, byte[] dataBytes)
    {
      IByteConvert convert = new ByteConvert(isBigEndian, 64);

      convert.ValidateLength(dataBytes, expectedLength);

      return convert.ByteToArray(dataBytes);
    }

    public static double[] Byte32ToDoubleList(int expectedLength, bool isBigEndian, byte[] dataBytes)
    {
      IByteConvert convert = new ByteConvert(isBigEndian, 32);

      convert.ValidateLength(dataBytes, expectedLength);

      return convert.ByteToArray(dataBytes);
    }

    /**
     *  	Filter width (2n+1)
     *  i	 11	  9	   7	  5
     * -5	-36	 	 	 
     * -4	  9	 -21	 	 
     * -3	 44	  14	-2	 
     * -2	 69	  39	 3	 -3
     * -1	 84	  54	 6	 12
     *  0	 89	  59	 7	 17
     *  1	 84	  54	 6	 12
     *  2	 69	  39	 3	 -3
     *  3	 44	  14	-2	 
     *  4	  9	 -21	 	 
     *  5	-36	 	 	 
     **/

    private static double[] weights5 = new double[] { -3, 12, 17, 12, -3 };
    private static double[] weights7 = new double[] { -2, 3, 6, 7, 6, 3, -2 };

    private static double[] SavitzkyGolay(double[] source, double[] weights)
    {
      if (source.Length < weights.Length)
      {
        return source;
      }

      double weightSum = weights.Sum();
      int n = weights.Length / 2;

      double[] result = new double[source.Length];
      for (int i = 0; i < n; i++)
      {
        result[i] = source[i];
        result[result.Length - 1 - i] = source[result.Length - 1 - i];
      }

      for (int i = n; i < source.Length - n; i++)
      {
        double sum = 0.0;
        for (int j = 0; j < weights.Length; j++)
        {
          sum += weights[j] * source[i - n + j];
        }
        result[i] = sum / weightSum;
      }

      return result;
    }

    public static double[] SavitzkyGolay5Point(double[] source)
    {
      return SavitzkyGolay(source, weights5);
    }

    public static double[] SavitzkyGolay7Point(double[] source)
    {
      return SavitzkyGolay(source, weights7);
    }

    /// <summary>
    /// ax2 + bx + c = 0
    /// 返回正值
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static double PositiveQuadraticFunction(double a, double b, double c)
    {
      double x1 = Math.Sqrt(b * b - 4 * a * c);
      return (-b + x1) / (2 * a);
    }

    /// <summary>
    /// 根据正反标下获取的结果进行矫正，获得真实的比例
    /// </summary>
    /// <param name="forwardRatio">正标比例</param>
    /// <param name="reverseRatio">反标比例</param>
    /// <returns></returns>
    public static double CarlibrateForwardReverseRatio(double forwardRatio, double reverseRatio)
    {
      double a = 1 + reverseRatio;
      double b = reverseRatio - forwardRatio;
      double c = -1 - forwardRatio;
      return PositiveQuadraticFunction(a, b, c);
    }
  }
}