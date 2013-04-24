using System;
using System.Linq;
using System.Collections.Generic;
using MathNet.Numerics.Statistics;
using MathNet.Numerics.Distributions;
using MathNet.Numerics;
using Meta.Numerics.Statistics;

namespace RCPA.Utils
{
  public static class StatisticsUtils
  {
    public static double GetCombinationProbability(int isotopicNumber, int totalNumber, double isotopicProbability,
                                                   double monoProbability)
    {
      if (isotopicNumber > totalNumber)
      {
        throw new ArgumentException("isotopicNumber cannot larger than totalNumber");
      }

      return Combinatorics.Combinations(totalNumber, isotopicNumber)
             * Math.Pow(isotopicProbability, isotopicNumber)
             * Math.Pow(monoProbability, (totalNumber - isotopicNumber));
    }

    private static SS CalculateSS(double[] observed, double[] predicted)
    {
      double averageObserved = Statistics.Mean(observed);

      double sst = 0.0;
      double sse = 0.0;
      for (int i = 0; i < observed.Length; i++)
      {
        double sstI = observed[i] - averageObserved;
        sst += sstI * sstI;

        double sseI = observed[i] - predicted[i];
        sse += sseI * sseI;
      }
      return new SS(sst, sse);
    }

    public static double RSquare(double[] observed, double[] predicted)
    {
      SS ss = CalculateSS(observed, predicted);
      return ss.RSquare();
    }

    public static double FDistributionCriticalValue(int freedomA, int freedomB, double alpha)
    {
      if (freedomA == 0 || freedomB == 0)
      {
        return 0.0;
      }
      return new FisherSnedecor(freedomA, freedomB).CumulativeDistribution(alpha);
    }

    public static double FDistributionCriticalValueForLinearRegression(int pointCount, double alpha)
    {
      return FDistributionCriticalValue(1, pointCount - 2, alpha);
    }

    public static double FCalculatedValueForLinearRegression(double[] observed, double[] predicted)
    {
      SS ss = CalculateSS(observed, predicted);

      double msr = ss.SSR / 1;
      double mse = ss.SSE / (observed.Length - 2);

      return msr / mse;
    }

    public static double FProbabilityForLinearRegression(int pointCount, double fCalculatedValue)
    {
      return FDistributionCriticalValue(1, pointCount - 2, fCalculatedValue);
    }

    public static double CosineAngle(double[] x, double[] y)
    {
      if (null == x || null == y)
      {
        throw new ArgumentException("Argument x or y is null!");
      }

      if (0 == x.Length || 0 == y.Length)
      {
        throw new ArgumentException("Argument x or y is empty!");
      }

      int maxLength = Math.Min(x.Length, y.Length);

      double ab = 0.0;
      double aa = 0.0;
      double bb = 0.0;
      for (int i = 0; i < maxLength; i++)
      {
        ab += x[i] * y[i];
        aa += x[i] * x[i];
        bb += y[i] * y[i];
      }

      double aabb = Math.Sqrt(aa) * Math.Sqrt(bb);

      return ab / aabb;
    }

    public static double PearsonCorrelation(double[] x, double[] y)
    {
      if (null == x || null == y)
      {
        throw new ArgumentException("Argument x or y is null!");
      }

      if (0 == x.Length || 0 == y.Length)
      {
        throw new ArgumentException("Argument x or y is empty!");
      }

      if (x.Length != y.Length)
      {
        throw new ArgumentException("Length of argument x and y are not equalment!");
      }

      double avga = Statistics.Mean(x);
      double avgb = Statistics.Mean(y);
      double agr = 0;
      double vara = 0;
      double varb = 0;
      for (int i = 0; i < x.Length; i++)
      {
        agr += (x[i] - avga) * (y[i] - avgb);
        vara += Math.Pow((x[i] - avga), 2);
        varb += Math.Pow((y[i] - avgb), 2);
      }

      double result;
      if (vara == 0 || varb == 0)
      {
        result = 0;
      }
      else
      {
        result = (agr / Math.Sqrt(vara * varb));
      }

      if (Double.IsNaN(result))
      {
        throw new ArgumentException("NAN->" + agr + " " + vara + " " + varb);
      }
      return result;
    }

    #region Nested type: SS

    private class SS
    {
      public SS(double sst, double sse)
      {
        SST = sst;
        SSE = sse;
      }

      public double SSE { get; set; }

      public double SST { get; set; }

      public double SSR
      {
        get { return SST - SSE; }
      }

      public double RSquare()
      {
        return 1 - SSE / SST;
      }
    }

    #endregion

    public static double SpearmanCorrelation(List<double?> r1, List<double?> r2)
    {
      return alglib.spearmancorr2(
        (from r in r1 select r.Value).ToArray(),
        (from r in r2 select r.Value).ToArray());
    }

    /// <summary>
    /// Get spearman correlation coefficient from two vector.
    /// The function from Meta.Numberic shows different result with R, so I just want to use
    /// alglib's function here.
    /// </summary>
    /// <param name="r1"></param>
    /// <param name="r2"></param>
    /// <returns></returns>
    public static double SpearmanCorrelation(List<double> r1, List<double> r2)
    {
      return alglib.spearmancorr2(r1.ToArray(), r2.ToArray());
    }
  }
}