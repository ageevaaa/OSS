using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMkurs
{
    class vm
    {
        public static double getVal(int i, double[] args)
        {
            switch (i)
            {
                case 0:
                    return args[0] + AP.Math.Sqr(args[0]) - 2 * args[1] * args[2] - 0.1;
                    break;
                case 1:
                    return args[1] - AP.Math.Sqr(args[1]) + 3 * args[0] * args[2] + 0.2;
                    break;
                case 2:
                    return args[2] + AP.Math.Sqr(args[2]) + 2 * args[0] * args[1] - 0.3;
                    break;
            }
            return 0;
        }
        public static double derVal(int i, int j, double[] arg)
        {
            switch (i)
            {
                case 0:
                    switch (j)
                    {
                        case 0:
                            return 1 + 2 * arg[0];
                            break;
                        case 1:
                            return -2 * arg[2];
                            break;
                        case 2:
                            return -2 * arg[1];
                            break;
                    }
                    break;
                case 1:
                    switch (j)
                    {
                        case 0:
                            return 3 * arg[2];
                            break;
                        case 1:
                            return 1 - 2*arg[1];
                            break;
                        case 2:
                            return 3 * arg[0];
                            break;
                    }
                    break;
                case 2:
                    switch (j)
                    {
                        case 0:
                            return 2 * arg[1];
                            break;
                        case 1:
                            return 2 * arg[0];
                            break;
                        case 2:
                            return 1 + 2*arg[2];
                            break;
                    }
                    break;
            }
            return 0;
        }
        public static void clear (ref double[,] a, int size)
        {
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    a[i, j] = 0;            
        }

        public static double[,] transpl(double[,] a, int size)
        {
            double[,] temp = new double[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    temp[i, j] = a[j, i];
            return temp;
        }

        public static double derMin(double val, int num, int size, int min)
        {
            double res=new double();
            double[] sol=new double[size];
            sol[num-min]=val;
            for (int i = 0; i < size; i++)
            {
                if (res > derVal(i, num, sol))
                    res = derVal(i, num, sol);
            }
            return res;
        }
        public static double derMin(double val, int num, int size)
        {
            return derMin(val, num, size, 0);
        }

        public static double brentoptimize(double a, double b, double epsilon, ref double xmin, double[] tempsol, int size, int num)
        {
            epsilon = epsilon / 100000000000;
            double result = 0;
            double ia = 0;
            double ib = 0;
            double bx = 0;
            double d = 0;
            double e = 0;
            double etemp = 0;
            double fu = 0;
            double fv = 0;
            double fw = 0;
            double fx = 0;
            int iter = 0;
            double p = 0;
            double q = 0;
            double r = 0;
            double u = 0;
            double v = 0;
            double w = 0;
            double x = 0;
            double xm = 0;
            double cgold = 0;

            cgold = 0.3819660;
            bx = 0.5 * (a + b);
            if (a < b)
            {
                ia = a;
            }
            else
            {
                ia = b;
            }
            if (a > b)
            {
                ib = a;
            }
            else
            {
                ib = b;
            }
            v = bx;
            w = v;
            x = v;
            e = 0.0;
            fx = f(x, tempsol, size, num);
            fv = fx;
            fw = fx;
            for (iter = 1; iter <= 100; iter++)
            {
                xm = 0.5 * (ia + ib);
                if (Math.Abs(x - xm) <= epsilon * 2 - 0.5 * (ib - ia))
                {
                    break;
                }
                if (Math.Abs(e) > epsilon)
                {
                    r = (x - w) * (fx - fv);
                    q = (x - v) * (fx - fw);
                    p = (x - v) * q - (x - w) * r;
                    q = 2 * (q - r);
                    if (q > 0)
                    {
                        p = -p;
                    }
                    q = Math.Abs(q);
                    etemp = e;
                    e = d;
                    if (!(Math.Abs(p) >= Math.Abs(0.5 * q * etemp) | p <= q * (ia - x) | p >= q * (ib - x)))
                    {
                        d = p / q;
                        u = x + d;
                        if (u - ia < epsilon * 2 | ib - u < epsilon * 2)
                        {
                            d = mysign(epsilon, xm - x);
                        }
                    }
                    else
                    {
                        if (x >= xm)
                        {
                            e = ia - x;
                        }
                        else
                        {
                            e = ib - x;
                        }
                        d = cgold * e;
                    }
                }
                else
                {
                    if (x >= xm)
                    {
                        e = ia - x;
                    }
                    else
                    {
                        e = ib - x;
                    }
                    d = cgold * e;
                }
                if (Math.Abs(d) >= epsilon)
                {
                    u = x + d;
                }
                else
                {
                    u = x + mysign(epsilon, d);
                }
                fu = f(u, tempsol, size, num);
                if (fu <= fx)
                {
                    if (u >= x)
                    {
                        ia = x;
                    }
                    else
                    {
                        ib = x;
                    }
                    v = w;
                    fv = fw;
                    w = x;
                    fw = fx;
                    x = u;
                    fx = fu;
                }
                else
                {
                    if (u < x)
                    {
                        ia = u;
                    }
                    else
                    {
                        ib = u;
                    }
                    if (fu <= fw | w == x)
                    {
                        v = w;
                        fv = fw;
                        w = u;
                        fw = fu;
                    }
                    else
                    {
                        if (fu <= fv | v == x | v == 2)
                        {
                            v = u;
                            fv = fu;
                        }
                    }
                }
            }
            xmin = x;
            result = fx;
            return result;
        }


        private static double mysign(double a,
            double b)
        {
            double result = 0;

            if (b > 0)
            {
                result = Math.Abs(a);
            }
            else
            {
                result = -Math.Abs(a);
            }
            return result;
        }
        public static double f(double x, double[] tempsol, int size, int num)
        {
            tempsol[num] = x;
            double res=new double();
            for (int i = 0; i < size; i++)
                res = res + AP.Math.Sqr(getVal(i, tempsol));
            res = Math.Sqrt(res);
            return res;
        }

    }
}
