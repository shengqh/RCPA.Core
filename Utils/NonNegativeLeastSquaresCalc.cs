using System;

/*****************************************************************************

  nnls.c  (c) 2002-2004 Turku PET Centre

  This file contains the routine NNLS (nonnegative least squares)
  and the subroutines required by it.
  
  This routine is based on the text and fortran code in
  C.L. Lawson and R.J. Hanson, Solving Least Squares Problems,
  Prentice-Hall, Englewood Cliffs, New Jersey, 1974.

  Version:
  2002-08-19 Vesa Oikonen
  2003-05-08 Kaisa Sederholm & VO
    Included function nnlsWght().
  2003-05-12 KS
    Variable a_dim1 excluded
    Usage of the coefficient matrix altered so that it is
    given in a[][] instead of a[].
  2003-11-06 VO
    If n<2, then itmax is set to n*n instead of previous n*3.
  2004-09-17 VO
    Doxygen style comments.
  2007-01-23 Quanhu Sheng
    Change to C# code
*****************************************************************************/

namespace RCPA.Utils
{
  public class NonNegativeLeastSquaresCalc
  {
    /****************************************************************************/
    /** Algorithm NNLS (Non-negative least-squares)
     *
     *  Given an m by n matrix A, and an m-vector B, computes an n-vector X,
     *  that solves the least squares problem
     *      A * X = B   , subject to X>=0
     *
     *  Instead of pointers for working space, null can be given to let this
     *  function to allocate and free the required memory.
     *  return Function returns 0 if succesful, 1, if iteration count exceeded 3*N,
     *  or 2 in case of invalid problem dimensions or memory allocation error.
     */
    public static int NNLS(
      /** On entry, a[n][m] contains the m by n matrix A. On exit, a[][] contains 
          the product matrix Q*A, where Q is an m by n orthogonal matrix generated
          implicitly by this function.*/
      double[][] a,
      /** Matrix dimension m */
      int m,
      /** Matrix dimension n */
      int n,
      /** On entry, b[] must contain the m-vector B.
          On exit, b[] contains Q*B */
      double[] b,
      /** On exit, x[] will contain the solution vector */
      double[] x,
      /** On exit, rnorm contains the Euclidean norm of the residual vector */
      out double rnorm,
      /** An n-array of working space, w[].
          On exit, w[] will contain the dual solution vector.
          w[i]=0.0 for all i in set p and w[i]<=0.0 for all i in set z. */
      double[] wp,
      /** An m-array of working space, zz[]. */
      double[] zzp,
      /** An n-array of working space, index[]. */
      int[] indexp
    )
    {
      int pfeas, ret = 0, iz, jz, iz1, iz2, npp1;
      int[] index;
      double d1, d2, sm, up = 0, ss;
      double[] w, zz;
      int iter, k, j = 0, l, itmax, izmax = 0, nsetp, ii, jj = 0, ip;
      double temp, wmax, t, alpha, asave, unorm, ztest, cc;
      double[] dummy = new double[1];


      /* Check the parameters and data */
      if (m <= 0 || n <= 0 || a == null || b == null || x == null)
      {
        throw new ArgumentException("m <= 0 || n <= 0 || a == null || b == null || x == null");
      }

      /* Allocate memory for working space, if required */
      if (wp != null)
      {
        w = wp;
      }
      else
      {
        w = new double[n];
      }

      if (zzp != null)
      {
        zz = zzp;
      }
      else
      {
        zz = new double[m];
      }

      if (indexp != null)
      {
        index = indexp;
      }
      else
      {
        index = new int[n];
      }

      if (w == null || zz == null || index == null)
      {
        throw new Exception("Allocate memory error!");
      }

      /* Initialize the arrays INDEX[] and X[] */
      for (k = 0; k < n; k++)
      {
        x[k] = 0.0;
        index[k] = k;
      }

      iz2 = n - 1;
      iz1 = 0;
      nsetp = 0;
      npp1 = 0;

      /* Main loop; quit if all coeffs are already in the solution or */
      /* if M cols of A have been triangularized */
      iter = 0;

      if (n < 3)
      {
        itmax = n * 3;
      }
      else
      {
        itmax = n * n;
      }

      while (iz1 <= iz2 && nsetp < m)
      {
        /* Compute components of the dual (negative gradient) vector W[] */
        for (iz = iz1; iz <= iz2; iz++)
        {
          j = index[iz];
          sm = 0.0;
          for (l = npp1; l < m; l++)
          {
            sm += a[j][l] * b[l];
          }
          w[j] = sm;
        }

        while (true)
        {
          /* Find largest positive W[j] */
          for (wmax = 0.0, iz = iz1; iz <= iz2; iz++)
          {
            j = index[iz];
            if (w[j] > wmax)
            {
              wmax = w[j]; izmax = iz;
            }
          }

          /* Terminate if wmax<=0.; */
          /* it indicates satisfaction of the Kuhn-Tucker conditions */
          if (wmax <= 0.0)
          {
            break;
          }

          iz = izmax;
          j = index[iz];

          /* The sign of W[j] is ok for j to be moved to set P. */
          /* Begin the transformation and check new diagonal element to avoid */
          /* near linear dependence. */
          asave = a[j][npp1];

          _nnls_h12(1, npp1, npp1 + 1, m, a[j], 1, ref up, dummy, 1, 1, 0);

          unorm = 0.0;

          if (nsetp != 0)
          {
            for (l = 0; l < nsetp; l++)
            {
              d1 = a[j][l];
              unorm += d1 * d1;
            }
          }

          unorm = Math.Sqrt(unorm);

          d1 = a[j][npp1];
          d2 = unorm + Math.Abs(d1) * 0.01;

          if (d2 - unorm > 0.0)
          {
            /* Col j is sufficiently independent. Copy B into ZZ, update ZZ */
            /* and solve for ztest ( = proposed new value for X[j] ) */
            for (l = 0; l < m; l++)
            {
              zz[l] = b[l];
            }

            _nnls_h12(2, npp1, npp1 + 1, m, a[j], 1, ref up, zz, 1, 1, 1);

            ztest = zz[npp1] / a[j][npp1];

            /* See if ztest is positive */
            if (ztest > 0.0)
            {
              break;
            }
          }

          /* Reject j as a candidate to be moved from set Z to set P. Restore */
          /* A[npp1,j], set W[j]=0., and loop back to test dual coeffs again */
          a[j][npp1] = asave;
          w[j] = 0.0;
        } /* while(true) */

        if (wmax <= 0.0)
        {
          break;
        }

        /* Index j=INDEX[iz] has been selected to be moved from set Z to set P. */
        /* Update B and indices, apply householder transformations to cols in */
        /* new set Z, zero subdiagonal elts in col j, set W[j]=0. */
        for (l = 0; l < m; ++l)
        {
          b[l] = zz[l];
        }

        index[iz] = index[iz1];
        index[iz1] = j;
        iz1++;
        nsetp = npp1 + 1;
        npp1++;

        if (iz1 <= iz2)
        {
          for (jz = iz1; jz <= iz2; jz++)
          {
            jj = index[jz];
            _nnls_h12(2, nsetp - 1, npp1, m, a[j], 1, ref up, a[jj], 1, m, 1);
          }
        }

        if (nsetp != m)
        {
          for (l = npp1; l < m; l++)
          {
            a[j][l] = 0.0;
          }
        }

        w[j] = 0.0;

        /* Solve the triangular system; store the solution temporarily in Z[] */
        for (l = 0; l < nsetp; l++)
        {
          ip = nsetp - (l + 1);
          if (l != 0)
          {
            for (ii = 0; ii <= ip; ii++)
            {
              zz[ii] -= a[jj][ii] * zz[ip + 1];
            }
          }

          jj = index[ip];
          zz[ip] /= a[jj][ip];
        }

        /* Secondary loop begins here */
        while (++iter < itmax)
        {
          /* See if all new constrained coeffs are feasible; if not, compute alpha */
          for (alpha = 2.0, ip = 0; ip < nsetp; ip++)
          {
            l = index[ip];
            if (zz[ip] <= 0.0)
            {
              t = -x[l] / (zz[ip] - x[l]);
              if (alpha > t)
              {
                alpha = t;
                jj = ip - 1;
              }
            }
          }

          /* If all new constrained coeffs are feasible then still alpha==2. */
          /* If so, then exit from the secondary loop to main loop */
          if (alpha == 2.0)
          {
            break;
          }

          /* Use alpha (0.<alpha<1.) to interpolate between old X and new ZZ */
          for (ip = 0; ip < nsetp; ip++)
          {
            l = index[ip];
            x[l] += alpha * (zz[ip] - x[l]);
          }

          /* Modify A and B and the INDEX arrays to move coefficient i */
          /* from set P to set Z. */
          k = index[jj + 1];
          pfeas = 1;
          do
          {
            x[k] = 0.0;
            if (jj != (nsetp - 1))
            {
              jj++;
              for (j = jj + 1; j < nsetp; j++)
              {
                ii = index[j];
                index[j - 1] = ii;

                _nnls_g1(a[ii][j - 1], a[ii][j], out cc, out ss, out a[ii][j - 1]);

                for (a[ii][j] = 0.0, l = 0; l < n; l++)
                {
                  if (l != ii)
                  {
                    /* Apply procedure G2 (CC,SS,A(J-1,L),A(J,L)) */
                    temp = a[l][j - 1];
                    a[l][j - 1] = cc * temp + ss * a[l][j];
                    a[l][j] = -ss * temp + cc * a[l][j];
                  }
                }

                /* Apply procedure G2 (CC,SS,B(J-1),B(J)) */
                temp = b[j - 1];
                b[j - 1] = cc * temp + ss * b[j];
                b[j] = -ss * temp + cc * b[j];
              }
            }

            npp1 = nsetp - 1;
            nsetp--;
            iz1--;
            index[iz1] = k;

            /* See if the remaining coeffs in set P are feasible; they should */
            /* be because of the way alpha was determined. If any are */
            /* infeasible it is due to round-off error. Any that are */
            /* nonpositive will be set to zero and moved from set P to set Z */
            for (jj = 0; jj < nsetp; jj++)
            {
              k = index[jj];
              if (x[k] <= 0.0)
              {
                pfeas = 0;
                break;
              }
            }
          } while (pfeas == 0);

          /* Copy B[] into zz[], then solve again and loop back */
          for (k = 0; k < m; k++)
          {
            zz[k] = b[k];
          }

          for (l = 0; l < nsetp; l++)
          {
            ip = nsetp - (l + 1);

            if (l != 0)
            {
              for (ii = 0; ii <= ip; ii++)
              {
                zz[ii] -= a[jj][ii] * zz[ip + 1];
              }
            }

            jj = index[ip];
            zz[ip] /= a[jj][ip];
          }
        } /* end of secondary loop */

        if (iter > itmax)
        {
          ret = 1;
          break;
        }

        for (ip = 0; ip < nsetp; ip++)
        {
          k = index[ip];
          x[k] = zz[ip];
        }
      } /* end of main loop */

      /* Compute the norm of the final residual vector */
      sm = 0.0;
      if (npp1 < m)
      {
        for (k = npp1; k < m; k++)
        {
          sm += (b[k] * b[k]);
        }
      }
      else
      {
        for (j = 0; j < n; j++)
        {
          w[j] = 0.0;
        }
      }

      rnorm = Math.Sqrt(sm);

      return (ret);
    } /* nnls_ */

    /****************************************************************************/

    /****************************************************************************/
    /**
     *  Compute orthogonal rotation matrix:
     *    (C, S) so that (C, S)(A) = (sqrt(A**2+B**2))
     *    (-S,C)         (-S,C)(B)   (   0          )
     *  Compute sig = sqrt(A**2+B**2):
     *    sig is computed last to allow for the possibility that sig may be in
     *    the same location as A or B.
     */
    private static void _nnls_g1(double a, double b, out double cterm, out double sterm, out double sig)
    {
      double d1, xr, yr;

      if (Math.Abs(a) > Math.Abs(b))
      {
        xr = b / a;
        d1 = xr;
        yr = Math.Sqrt(d1 * d1 + 1.0);
        d1 = 1.0 / yr;
        cterm = (a >= 0.0 ? Math.Abs(d1) : -Math.Abs(d1));
        sterm = (cterm) * xr;
        sig = Math.Abs(a) * yr;
      }
      else if (b != 0.0)
      {
        xr = a / b;
        d1 = xr;
        yr = Math.Sqrt(d1 * d1 + 1.0);
        d1 = 1.0 / yr;
        sterm = (b >= 0.0 ? Math.Abs(d1) : -Math.Abs(d1));
        cterm = (sterm) * xr; sig = Math.Abs(b) * yr;
      }
      else
      {
        sig = 0.0;
        cterm = 0.0;
        sterm = 1.0;
      }
    } /* _nnls_g1 */
    /****************************************************************************/

    /****************************************************************************/
    /**
     *  Construction and/or application of a single Householder transformation:
     *           Q = I + U*(U**T)/B
     *
     *  Function returns 0 if succesful, or >0 in case of erroneous parameters.
     *
     */
    private static int _nnls_h12(
      /** mode=1 to construct and apply a Householder transformation, or
          mode=2 to apply a previously constructed transformation */
      int mode,
      /** Index of the pivot element */
      int lpivot,
      /** Transformation is constructed to zero elements indexed from l1 to M */
      int l1,
      /** Transformation is constructed to zero elements indexed from l1 to M */
      int m,
      /** With mode=1: On entry, u[] must contain the pivot vector.
         On exit, u[] and up contain quantities defining the vector u[] of
         the Householder transformation.
         With mode=2: On entry, u[] and up should contain quantities previously
         computed with mode=1. These will not be modified. */
      double[] u,
      /** u_dim1 is the storage increment between elements. */
      int u_dim1,
      /** With mode=1: On entry, u[] must contain the pivot vector.
         On exit, u[] and up contain quantities defining the vector u[] of
         the Householder transformation.
         With mode=2: On entry, u[] and up should contain quantities previously
         computed with mode=1. These will not be modified. */
      ref double up,
      /** On entry, cm[] must contain the matrix (set of vectors) to which the
         Householder transformation is to be applied. On exit, cm[] will contain
         the set of transformed vectors */
      double[] cm,
      /** Storage increment between elements of vectors in cm[] */
      int ice,
      /** Storage increment between vectors in cm[] */
      int icv,
      /** Nr of vectors in cm[] to be transformed;
          if ncv<=0, then no operations will be done on cm[] */
      int ncv)
    {
      double d1, d2, b, clinv, cl, sm;
      int incr, k, j, i2, i3, i4;

      /* Check parameters */
      if (mode != 1 && mode != 2)
      {
        throw new ArgumentException("Mode should be in {0,1}, now is " + mode);
      }

      if (m < 1 || u_dim1 < 1)
      {
        throw new ArgumentException(MyConvert.Format("m and u_dim1 should larger than 1, now is {0} and {1}", m, u_dim1));
      }

      if (lpivot < 0 || lpivot >= l1 || l1 >= m)
      {
        return (0);
      }

      /* Function Body */
      d1 = u[lpivot * u_dim1];
      cl = Math.Abs(d1);
      if (mode == 2)
      { /* Apply transformation I+U*(U**T)/B to cm[] */
        if (cl <= 0.0)
        {
          return (0);
        }
      }
      else
      { /* Construct the transformation */
        for (j = l1; j < m; j++)
        { /* Computing MAX */
          d1 = u[j * u_dim1];
          d2 = Math.Abs(d1);
          if (d2 > cl)
          {
            cl = d2;
          }
        }

        if (cl <= 0.0)
        {
          return (0);
        }

        clinv = 1.0 / cl;
        /* Computing 2nd power */
        d1 = u[lpivot * u_dim1] * clinv;
        sm = d1 * d1;
        for (j = l1; j < m; j++)
        {
          d1 = u[j * u_dim1] * clinv;
          sm += d1 * d1;
        }

        cl *= Math.Sqrt(sm);
        if (u[lpivot * u_dim1] > 0.0)
        {
          cl = -cl;
        }

        up = u[lpivot * u_dim1] - cl;
        u[lpivot * u_dim1] = cl;
      }
      if (ncv <= 0)
      {
        return (0);
      }

      b = up * u[lpivot * u_dim1];
      /* b must be nonpositive here; if b>=0., then return */
      if (b >= 0.0)
      {
        return (0);
      }

      b = 1.0 / b;
      i2 = 1 - icv + ice * lpivot;
      incr = ice * (l1 - lpivot);
      for (j = 0; j < ncv; j++)
      {
        i2 += icv;
        i3 = i2 + incr;
        i4 = i3;
        sm = cm[i2 - 1] * up;

        for (k = l1; k < m; k++)
        {
          sm += cm[i3 - 1] * u[k * u_dim1];
          i3 += ice;
        }

        if (sm != 0.0)
        {
          sm *= b;
          cm[i2 - 1] += sm * up;
          for (k = l1; k < m; k++)
          {
            cm[i4 - 1] += sm * u[k * u_dim1];
            i4 += ice;
          }
        }
      }
      return (0);
    } /* _nnls_h12 */
    /****************************************************************************/

    ///****************************************************************************/
    ///** Algorithm for weighting the problem that is given to nnls-algorithm.
    //    Square roots of weights are used because in nnls the difference
    //    w*A-w*b is squared.
    //    return Algorithm returns zero if successful, 1 if arguments are inappropriate.
    //*/
    //public int nnlsWght(int N, int M, double **A, double *b, double *weight)
    //{
    //  int n, m;
    //  double *w;

    //  /* Check the arguments */
    //  if(N<1 || M<1 || A==null || b==null || weight==null) return(1);

    //  /* Allocate memory */
    //  w=(double*)malloc(M*sizeof(double)); if(w==null) return(2);

    //  /* Check that weights are not zero and get the square roots of them to w[] */
    //  for(m=0; m<M; m++) {
    //    if(weight[m]<=1.0e-20) w[m]=0.0;
    //    else w[m]=sqrt(weight[m]);
    //  }

    //  /* Multiply rows of matrix A and elements of vector b with weights*/
    //  for(m=0; m<M; m++) {
    //    for(n=0; n<N; n++) {
    //      A[n][m]*=w[m];
    //    }
    //    b[m]*=w[m];
    //  }

    //  free(w);
    //  return(0);
    //}
    /****************************************************************************/

    /****************************************************************************/

  }
}
