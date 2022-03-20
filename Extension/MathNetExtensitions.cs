using MathNet.Numerics.Statistics;
using RCPA;
using System.Collections.Generic;
using System.Linq;

namespace MathNet.Numerics.Distributions
{
  public class MeanStandardDeviation
  {
    public MeanStandardDeviation(IEnumerable<double> values)
    {
      this.Mean = values.Mean();
      this.StdDev = values.StandardDeviation();
      this.Count = values.Count();
    }

    public double Mean { get; set; }

    public double StdDev { get; set; }

    public int Count { get; set; }
  }

  public static class DistributionExtensitions
  {
    public static double TwoTailProbability(this IUnivariateDistribution nd, double value)
    {
      var result = nd.CumulativeDistribution(value);
      if (result > 0.5)
      {
        result = 1 - result;
      }
      result *= 2;
      return result;
    }

    public static Pair<double, double> GetProb90Range(this Normal nr)
    {
      return GetProbRange(nr, 0.9);
    }

    public static Pair<double, double> GetProbRange(this Normal nr, double probability)
    {
      const double step = 0.01;
      double p = (1 - probability) / 2;

      double left90 = GetProbabilityValue(nr, -step, p);
      double right90 = GetProbabilityValue(nr, step, p);

      return new Pair<double, double>(left90, right90);
    }

    private static double GetProbabilityValue(Normal nr, double step, double p)
    {
      var result = nr.Mean;
      while (true)
      {
        double prob = GetProbability(nr, result);
        if (prob <= p)
        {
          break;
        }
        result += step;
      }
      return result;
    }

    public static double GetProbability(this Normal nr, double value)
    {
      double result = nr.CumulativeDistribution(value);
      if (value > nr.Mean)
      {
        result = 1 - result;
      }
      return result;
    }
  }
}
