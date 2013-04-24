using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCPA.Utils
{
  public class NumericalRecipes
  {
    public void nrerror(string error_text)
    {
      throw new Exception(error_text.ToString());
    }

    const double ITMAX = 100;
    const double EPS = 3.0e-7;
    const double FPMIN = 1.0e-30;
    private static double[] cof = null;

    public NumericalRecipes()
    {
      cof = new double[] { 76.18009172947146, -86.50532032941677, 24.01409824083091, -1.231739572450155, 0.1208650973866179e-2, -0.5395239384953e-5 };
    }

    public double gammln(double xx)
    {
      double x, y, tmp, ser;
      int j;

      y = x = xx;
      tmp = x + 5.5;
      tmp -= (x + 0.5) * Math.Log(tmp);
      ser = 1.000000000190015;
      for (j = 0; j <= 5; j++) ser += cof[j] / ++y;
      return -tmp + Math.Log(2.5066282746310005 * ser / x);
    }

    public void gser(ref double gamser, double a, double x, ref double gln)
    {
      int n;
      double sum, del, ap;

      gln = gammln(a);
      if (x <= 0.0)
      {
        if (x < 0.0) nrerror("x less than 0 in routine gser");
        gamser = 0.0;
        return;
      }
      else
      {
        ap = a;
        del = sum = 1.0 / a;
        for (n = 1; n <= ITMAX; n++)
        {
          ++ap;
          del *= x / ap;
          sum += del;
          if (Math.Abs(del) < Math.Abs(sum) * EPS)
          {
            gamser = sum * Math.Exp(-x + a * Math.Log(x) - gln);
            return;
          }
        }
        nrerror("a too large, ITMAX too small in routine gser");
        return;
      }
    }

    public void gcf(ref double gammcf, double a, double x, ref double gln)
    {
      int i;
      double an, b, c, d, del, h;

      gln = gammln(a);
      b = x + 1.0 - a;
      c = 1.0 / FPMIN;
      d = 1.0 / b;
      h = d;
      for (i = 1; i <= ITMAX; i++)
      {
        an = -i * (i - a);
        b += 2.0;
        d = an * d + b;
        if (Math.Abs(d) < FPMIN) d = FPMIN;
        c = b + an / c;
        if (Math.Abs(c) < FPMIN) c = FPMIN;
        d = 1.0 / d;
        del = d * c;
        h *= del;
        if (Math.Abs(del - 1.0) < EPS) break;
      }
      if (i > ITMAX) nrerror("a too large, ITMAX too small in gcf");
      gammcf = Math.Exp(-x + a * Math.Log(x) - gln) * h;
    }

    public double gammq(double a, double x)
    {
      double gamser = 0.0, gammcf = 0.0, gln = 0.0;

      if (x < 0.0 || a <= 0.0) nrerror("Invalid arguments in routine gammq");
      if (x < (a + 1.0))
      {
        gser(ref gamser, a, x, ref gln);
        return 1.0 - gamser;
      }
      else
      {
        gcf(ref gammcf, a, x, ref gln);
        return gammcf;
      }
    }

    private static double SQR(double p)
    {
      return p * p;
    }

    const double POTN = 1.571000;
    const double BIG = 1.0e30;
    const double PI = 3.14159265;
    const double ACC = 1.0e-3;

    private void avevar(double[] data, ref double ave, ref double var)
    {
      double ep = 0.0;

      ave = data.Average();

      var = 0.0;
      foreach (var d in data)
      {
        var s = d - ave;
        ep += s;
        var += s * s;
      }
      var = (var - ep * ep / data.Length) / (data.Length - 1);
    }

    const double CGOLD = 0.3819660;
    const double ZEPS = 1.0e-10;
    public void SHFT(ref double a, ref double b, ref double c, double d)
    {
      a = b;
      b = c;
      c = d;
    }

    public double SIGN(double a, double b)
    {
      if (b >= 0.0)
        return Math.Abs(a);
      else
        return -Math.Abs(a);
    }

    public double brent(double ax, double bx, double cx, Func<double, double> func, double tol, ref double xmin)
    {
      int iter;
      double a, b, d = 0.0, etemp, fu, fv, fw, fx, p, q, r, tol1, tol2, u, v, w, x, xm;
      double e = 0.0;

      a = (ax < cx ? ax : cx);
      b = (ax > cx ? ax : cx);
      x = w = v = bx;
      fw = fv = fx = func(x);
      for (iter = 1; iter <= ITMAX; iter++)
      {
        xm = 0.5 * (a + b);
        tol2 = 2.0 * (tol1 = tol * Math.Abs(x) + ZEPS);
        if (Math.Abs(x - xm) <= (tol2 - 0.5 * (b - a)))
        {
          xmin = x;
          return fx;
        }
        if (Math.Abs(e) > tol1)
        {
          r = (x - w) * (fx - fv);
          q = (x - v) * (fx - fw);
          p = (x - v) * q - (x - w) * r;
          q = 2.0 * (q - r);
          if (q > 0.0) p = -p;
          q = Math.Abs(q);
          etemp = e;
          e = d;
          if (Math.Abs(p) >= Math.Abs(0.5 * q * etemp) || p <= q * (a - x) || p >= q * (b - x))
            d = CGOLD * (e = (x >= xm ? a - x : b - x));
          else
          {
            d = p / q;
            u = x + d;
            if (u - a < tol2 || b - u < tol2)
              d = SIGN(tol1, xm - x);
          }
        }
        else
        {
          d = CGOLD * (e = (x >= xm ? a - x : b - x));
        }
        u = (Math.Abs(d) >= tol1 ? x + d : x + SIGN(tol1, d));
        fu = func(u);
        if (fu <= fx)
        {
          if (u >= x) a = x;
          else b = x;
          SHFT(ref v, ref w, ref x, u);
          SHFT(ref fv, ref fw, ref fx, fu);
        }
        else
        {
          if (u < x)
            a = u;
          else
            b = u;
          if (fu <= fw || w == x)
          {
            v = w;
            w = u;
            fv = fw;
            fw = fu;
          }
          else if (fu <= fv || v == x || v == w)
          {
            v = u;
            fv = fu;
          }
        }
      }
      nrerror("Too many iterations in brent");
      xmin = x;
      return fx;
    }

    //与chixy通讯的全局变量
    private int _nn;
    private double[] _xx;
    private double[] _yy;
    private double[] _sx;
    private double[] _sy;
    private double[] _ww;
    private double _aa;
    private double _offs;

    /// <summary>
    /// fitexy的辅助函数，对于b=tan(bang)，它返回(chi2 - offs）值。被标度的数据和offs通过全局变量进行传递。
    /// </summary>
    /// <param name="bang">角度</param>
    /// <returns>chi平方(chi2)</returns>
    private double chixy(double bang)
    {
      int j;
      double ans, avex = 0.0, avey = 0.0, sumw = 0.0, b;

      b = Math.Tan(bang);
      for (j = 0; j < _nn; j++)
      {
        _ww[j] = SQR(b * _sx[j]) + SQR(_sy[j]);
        sumw += (_ww[j] = (_ww[j] == 0.0 ? BIG : 1.0 / _ww[j]));
        avex += _ww[j] * _xx[j];
        avey += _ww[j] * _yy[j];
      }

      if (sumw == 0.0)
        sumw = BIG;

      avex /= sumw;
      avey /= sumw;
      _aa = avey - b * avex;

      for (ans = -_offs, j = 0; j < _nn; j++)
        ans += _ww[j] * SQR(_yy[j] - _aa - b * _xx[j]);

      return ans;
    }

    const double GOLD = 1.618034;
    const double GLIMIT = 100.0;
    const double TINY = 1.0e-20;

    public void mnbrak(ref double ax, ref double bx, ref double cx, ref double fa, ref double fb,
            ref double fc, Func<double, double> func)
    {

      double ulim, u, r, q, fu, dum = 0.0;

      fa = func(ax);
      fb = func(bx);
      if (fb > fa)
      {
        SHFT(ref dum, ref ax, ref bx, dum);
        SHFT(ref dum, ref fb, ref fa, dum);
      }
      cx = bx + GOLD * (bx - ax);
      fc = func(cx);
      while (fb > fc)
      {
        r = (bx - ax) * (fb - fc);
        q = (bx - cx) * (fb - fa);
        u = bx - ((bx - cx) * q - (bx - ax) * r) /
          (2.0 * SIGN(Math.Max(Math.Abs(q - r), TINY), q - r));
        ulim = bx + GLIMIT * (cx - bx);
        if ((bx - u) * (u - cx) > 0.0)
        {
          fu = func(u);
          if (fu < fc)
          {
            ax = bx;
            bx = u;
            fa = fb;
            fb = fu;
            return;
          }
          else if (fu > fb)
          {
            cx = u;
            fc = fu;
            return;
          }
          u = cx + GOLD * (cx - bx);
          fu = func(u);
        }
        else if ((cx - u) * (u - ulim) > 0.0)
        {
          fu = func(u);
          if (fu < fc)
          {
            SHFT(ref bx, ref cx, ref u, cx + GOLD * (cx - bx));
            SHFT(ref fb, ref fc, ref fu, func(u));
          }
        }
        else if ((u - ulim) * (ulim - cx) >= 0.0)
        {
          u = ulim;
          fu = func(u);
        }
        else
        {
          u = cx + GOLD * (cx - bx);
          fu = func(u);
        }
        SHFT(ref ax, ref bx, ref cx, u);
        SHFT(ref fa, ref fb, ref fc, fu);
      }
    }

    public double zbrent(Func<double, double> func, double x1, double x2, double tol)
    {
      int iter;
      double a = x1, b = x2, c = x2, d = 0.0, e = 0.0, min1, min2;
      double fa = func(a), fb = func(b), fc, p, q, r, s, tol1, xm;

      if ((fa > 0.0 && fb > 0.0) || (fa < 0.0 && fb < 0.0))
        nrerror("Root must be bracketed in zbrent");
      fc = fb;
      for (iter = 1; iter <= ITMAX; iter++)
      {
        if ((fb > 0.0 && fc > 0.0) || (fb < 0.0 && fc < 0.0))
        {
          c = a;
          fc = fa;
          e = d = b - a;
        }
        if (Math.Abs(fc) < Math.Abs(fb))
        {
          a = b;
          b = c;
          c = a;
          fa = fb;
          fb = fc;
          fc = fa;
        }
        tol1 = 2.0 * EPS * Math.Abs(b) + 0.5 * tol;
        xm = 0.5 * (c - b);
        if (Math.Abs(xm) <= tol1 || fb == 0.0) return b;
        if (Math.Abs(e) >= tol1 && Math.Abs(fa) > Math.Abs(fb))
        {
          s = fb / fa;
          if (a == c)
          {
            p = 2.0 * xm * s;
            q = 1.0 - s;
          }
          else
          {
            q = fa / fc;
            r = fb / fc;
            p = s * (2.0 * xm * q * (q - r) - (b - a) * (r - 1.0));
            q = (q - 1.0) * (r - 1.0) * (s - 1.0);
          }
          if (p > 0.0) q = -q;
          p = Math.Abs(p);
          min1 = 3.0 * xm * q - Math.Abs(tol1 * q);
          min2 = Math.Abs(e * q);
          if (2.0 * p < (min1 < min2 ? min1 : min2))
          {
            e = d;
            d = p / q;
          }
          else
          {
            d = xm;
            e = d;
          }
        }
        else
        {
          d = xm;
          e = d;
        }
        a = b;
        fa = fb;
        if (Math.Abs(d) > tol1)
          b += d;
        else
          b += SIGN(tol1, xm);
        fb = func(b);
      }
      nrerror("Maximum number of iterations exceeded in zbrent");
      return 0.0;
    }

    /// <summary>
    /// y测量不准确下的线性拟合 y = a + bx
    /// </summary>
    /// <param name="x">观测值x</param>
    /// <param name="y">观测值y</param>
    /// <param name="sig">x与y都具有的标准差</param>
    /// <param name="mwt">是否已知标准差。当设置为false时，表示不知道数据点标准差（相当于sig全部为1），q将以1.0返回，chi2归一化为所有各点的单位标准差</param>
    /// <param name="a">返回值a</param>
    /// <param name="b">返回值b</param>
    /// <param name="siga">a的不确定度</param>
    /// <param name="sigb">b的不确定度</param>
    /// <param name="chi2">最小化后的chi平方</param>
    /// <param name="q">拟合优度概率值（拟合将使得chi平方具有这么大或者更大）</param>
    public void fit(double[] x, double[] y, double[] sig, bool mwt, ref double a, ref double b, ref double siga, ref double sigb, ref double chi2, ref double q)
    {
      int i;
      double wt, t, sxoss, sx = 0.0, sy = 0.0, st2 = 0.0, ss, sigdat;

      int ndata = x.Length;

      b = 0.0;

      //累加求和
      if (mwt)
      {
        ss = 0.0;
        for (i = 0; i < ndata; i++)
        {
          wt = 1.0 / SQR(sig[i]);//考虑权重
          ss += wt;
          sx += x[i] * wt;
          sy += y[i] * wt;
        }
      }
      else
      {
        for (i = 0; i < ndata; i++)
        {
          sx += x[i];//没有权重
          sy += y[i];
        }
        ss = ndata;
      }
      sxoss = sx / ss;
      if (mwt)
      {
        for (i = 0; i < ndata; i++)
        {
          t = (x[i] - sxoss) / sig[i];
          st2 += t * t;
          b += t * y[i] / sig[i];
        }
      }
      else
      {
        for (i = 0; i < ndata; i++)
        {
          t = x[i] - sxoss;
          st2 += t * t;
          b += t * y[i];
        }
      }

      //求a,b,siga,sigb
      b /= st2;
      a = (sy - sx * (b)) / ss;
      siga = Math.Sqrt((1.0 + sx * sx / (ss * st2)) / ss);
      sigb = Math.Sqrt(1.0 / st2);
      //计算chi2
      chi2 = 0.0;
      if (!mwt)
      {
        //对无权重数据，用chi2求sig，并且调整标准差
        for (i = 0; i < ndata; i++)
          chi2 += SQR(y[i] - a - b * x[i]);
        q = 1.0;
        sigdat = Math.Sqrt(chi2 / (ndata - 2));
        siga *= sigdat;
        sigb *= sigdat;
      }
      else
      {
        for (i = 0; i < ndata; i++)
          chi2 += SQR((y[i] - a - b * x[i]) / sig[i]);
        q = gammq(0.5 * (ndata - 2), 0.5 * chi2);
      }
    }

    /// <summary>
    /// x,y均测量不准确下的线性拟合 y = a + bx
    /// </summary>
    /// <param name="x">观测值x</param>
    /// <param name="y">观测值y</param>
    /// <param name="sigx">x的标准差，如果为null，则sigx和sigy将被初始化为1.0</param>
    /// <param name="sigy">y的标准差</param>
    /// <param name="a">返回值a</param>
    /// <param name="b">返回值b</param>
    /// <param name="siga">a的不确定度</param>
    /// <param name="sigb">b的不确定度</param>
    /// <param name="chi2">最小化后的chi平方</param>
    /// <param name="q">拟合优度概率值（拟合将使得chi平方具有这么大或者更大）</param>
    public void fitexy(double[] x, double[] y, double[] sigx, double[] sigy,
      ref double a, ref double b, ref double siga, ref double sigb, ref double chi2, ref double q)
    {
      int j;
      double swap, amx, amn, varx = 0.0, vary = 0.0, scale, bmn, bmx, d1, d2, r2, dum1 = 0.0, dum2 = 0.0, dum3 = 0.0, dum4 = 0.0, dum5 = 0.0;
      double[] ang = new double[7];
      double[] ch = new double[7];

      int ndat = x.Length;

      _xx = new double[ndat];
      _yy = new double[ndat];
      _sx = new double[ndat];
      _sy = new double[ndat];
      _ww = new double[ndat];

      //寻找x和y变量，量化数据为全局变量，以便于chixy通讯
      avevar(x, ref dum1, ref varx);
      avevar(y, ref dum1, ref vary);

      if (sigx == null)
      {
        sigx = Enumerable.Repeat(1.0, ndat).ToArray();
        sigy = Enumerable.Repeat(1.0, ndat).ToArray();
      }

      scale = Math.Sqrt(varx / vary);
      _nn = ndat;
      for (j = 0; j < ndat; j++)
      {
        _xx[j] = x[j];
        _yy[j] = y[j] * scale;
        _sx[j] = sigx[j];
        _sy[j] = sigy[j] * scale;
        _ww[j] = Math.Sqrt(SQR(_sx[j]) + SQR(_sy[j]));//在第一次实验拟合中考虑全部x和y的权重
      }

      //对b实验拟合
      fit(_xx, _yy, _ww, true, ref dum1, ref b, ref dum2, ref dum3, ref dum4, ref dum5);

      //根据参考数据点构造几个角度，并使得b为其中一个角度
      _offs = ang[0] = 0.0;
      ang[1] = Math.Atan(b);
      //ang[2] will be calculated by mnbrak
      ang[3] = 0.0;
      ang[4] = ang[2];
      ang[5] = POTN;

      for (j = 3; j < 6; j++)
        ch[j] = chixy(ang[j]);

      mnbrak(ref ang[0], ref ang[1], ref ang[2], ref ch[0], ref ch[1], ref ch[2], chixy);

      chi2 = brent(ang[0], ang[1], ang[2], chixy, ACC, ref b);
      chi2 = chixy(b);
      a = _aa;

      //计算chi2概率
      q = gammq(0.5 * (_nn - 2), chi2 * 0.5); 

      //在最小值保留权重和的倒数
      r2 = 1 / _ww.Sum();

      bmx = BIG;
      bmn = BIG;
      _offs = chi2 + 1.0;
      for (j = 0; j < 6; j++)
      {
        //通过保存的值括出想要的根，注意倾斜角度的周期性
        if (ch[j] > _offs)
        {
          d1 = Math.Abs(ang[j] - b);
          while (d1 >= PI)
            d1 -= PI;
          d2 = PI - d1;
          if (ang[j] < b)
          {
            swap = d1;
            d1 = d2;
            d2 = swap;
          }
          if (d1 < bmx)
            bmx = d1;
          if (d2 < bmn)
            bmn = d2;
        }
      }

      if (bmx < BIG)
      {
        //调用zbrent寻根
        bmx = zbrent(chixy, b, b + bmx, ACC) - b;
        amx = _aa - a;
        bmn = zbrent(chixy, b, b - bmn, ACC) - b;
        amn = _aa - a;
        sigb = Math.Sqrt(0.5 * (bmx * bmx + bmn * bmn)) / (scale * SQR(Math.Cos(b)));
        siga = Math.Sqrt(0.5 * (amx * amx + amn * amn) + r2) / scale;
      }
      else
        sigb = siga = BIG;

      //将标度结果复原
      a /= scale;
      b = Math.Tan(b) / scale;
    }


    /// <summary>
    /// fitexy0的辅助函数，无截距，对于b=tan(bang)，它返回(chi2 - offs）值。被标度的数据和offs通过全局变量进行传递。
    /// </summary>
    /// <param name="bang">角度</param>
    /// <returns>chi平方(chi2)</returns>
    private double chixy0(double bang)
    {
      int j;
      double ans, avex = 0.0, avey = 0.0, sumw = 0.0, b;

      b = Math.Tan(bang);
      for (j = 0; j < _nn; j++)
      {
        _ww[j] = SQR(b * _sx[j]) + SQR(_sy[j]);
        sumw += (_ww[j] = (_ww[j] == 0.0 ? BIG : 1.0 / _ww[j]));
        avex += _ww[j] * _xx[j];
        avey += _ww[j] * _yy[j];
      }

      if (sumw == 0.0)
        sumw = BIG;

      avex /= sumw;
      avey /= sumw;

      for (ans = -_offs, j = 0; j < _nn; j++)
        ans += _ww[j] * SQR(_yy[j] - b * _xx[j]);

      return ans;
    }

    //http://blog.csdn.net/learnhard/archive/2007/07/06/1681398.aspx
    public void fit0(double[] x, double[] y, ref double b)
    {
      double sumXiYi = 0, sumXi_2 = 0;
      for (int i = 0; i < x.Length; i++)
      {
        sumXiYi += x[i] * y[i];
        sumXi_2 += x[i] * x[i];
      }
      b = sumXiYi / sumXi_2;
    }

    /// <summary>
    /// x,y均测量不准确下的线性拟合 y = bx
    /// </summary>
    /// <param name="x">观测值x</param>
    /// <param name="y">观测值y</param>
    /// <param name="b">返回值b</param>
    public void fitexy0(double[] x, double[] y, ref double b)
    {
      int j;
      double varx = 0.0, vary = 0.0, scale, dum1 = 0.0;
      double[] ang = new double[7];
      double[] ch = new double[7];

      int ndat = x.Length;

      _xx = new double[ndat];
      _yy = new double[ndat];
      _sx = new double[ndat];
      _sy = new double[ndat];
      _ww = new double[ndat];

      //寻找x和y变量，量化数据为全局变量，以便于chixy通讯
      avevar(x, ref dum1, ref varx);
      avevar(y, ref dum1, ref vary);

      var  sigx = Enumerable.Repeat(1.0, ndat).ToArray();
      var  sigy = Enumerable.Repeat(1.0, ndat).ToArray();

      scale = Math.Sqrt(varx / vary);
      _nn = ndat;
      for (j = 0; j < ndat; j++)
      {
        _xx[j] = x[j];
        _yy[j] = y[j] * scale;
        _sx[j] = sigx[j];
        _sy[j] = sigy[j] * scale;
        _ww[j] = Math.Sqrt(SQR(_sx[j]) + SQR(_sy[j]));//在第一次实验拟合中考虑全部x和y的权重
      }

      //对b实验拟合
      fit0(_xx, _yy, ref b);

      //根据参考数据点构造几个角度，并使得b为其中一个角度
      _offs = ang[0] = 0.0;
      ang[1] = Math.Atan(b);
      //ang[2] will be calculated by mnbrak
      ang[3] = 0.0;
      ang[4] = ang[2];
      ang[5] = POTN;

      for (j = 3; j < 6; j++)
        ch[j] = chixy0(ang[j]);

      mnbrak(ref ang[0], ref ang[1], ref ang[2], ref ch[0], ref ch[1], ref ch[2], chixy0);

      brent(ang[0], ang[1], ang[2], chixy0, ACC, ref b);

      b = Math.Tan(b) / scale;
    }
  }
}
