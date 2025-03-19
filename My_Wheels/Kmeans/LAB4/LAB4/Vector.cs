using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB4
{
    public class Vector
    {
        //C[0]~x;
        //C[1]~y;
        //C[2]~z;
        //...
        public double[] C;//некоторое количество "измерений" (или координат) вектора
        public double this[int i] => C[i];//для прямого обращения к элементам вектора
        public Vector() { }
        public Vector(Vector a)
        {
            C = new double[a.C.Length];
            for (int i = 0; i < a.C.Length; i++)
            {
                C[i] = a[i];
            }
        }
        public Vector(params double[] coord1)
        {
            C = new double[coord1.Length];
            for (int i = 0; i < coord1.Length; i++)
            {
                C[i] = coord1[i];
            }
        }
        public static Vector operator +(Vector a, Vector b)
        {//если вектора имеют разную размерность, то возвращаемый вектор
            //будет содержать в себе минимальное количество параметров 
            Vector x = new Vector();// = new Vector(a.C[0] + b.C[0], a.C[1] + b.C[1],...);
            int min;
            if (a.C.Length < b.C.Length)
                min = a.C.Length;
            else
                min = b.C.Length;
            x.C = new double[min];
            for (int i = 0; i < min; i++)
                x.C[i] = a[i] + b[i];
            return x;
        }
        public static Vector operator -(Vector a, Vector b)
        {//если вектора имеют разную размерность, то возвращаемый вектор
            //будет содержать в себе минимальное количество параметров 
            Vector x = new Vector();// = new Vector(a.C[0] + b.C[0], a.C[1] + b.C[1],...);
            int min;
            if (a.C.Length < b.C.Length)
                min = a.C.Length;
            else
                min = b.C.Length;
            x.C = new double[min];
            for (int i = 0; i < min; i++)
                x.C[i] = a[i] - b[i];
            return x;
        }
        public static Vector operator *(double s, Vector a)
        {//умножение скаляра на вектор
            Vector x = a;
            for (int i = 0; i < x.C.Length; i++)
                x.C[i] *= s;
            return x;
        }
        public static Vector operator *(Vector a, Vector b)
        {//векторное умножение
            return new Vector(a[1] * b[2] - a[2] * b[1], a[2] * b[0] - a[0] * b[2], a[0] * b[1] - a[1] * b[0]);
        }
        public static double operator &(Vector a, Vector b)
        {//скалярное умножение
            double x = 0;
            int min;
            if (a.C.Length < b.C.Length)
                min = a.C.Length;
            else
                min = b.C.Length;
            for (int i = 0; i < min; i++)
                x += a[i] * b[i];
            return x;
        }
        public static double operator !(Vector a)
        {//нахождение длинны вектора
            double s = 0;
            for (int i = 0; i < a.C.Length; i++)
                s += a[i] * a[i];
            return Math.Sqrt(s);
            //return new Vector(-a[0], -a[1], -a[2], a[3]);//спряженый кватернион
        }
        public static Vector operator /(Vector a, Vector b)
        {//кватернионное умножение
            if (a.C.Length < 4)
                a = new Vector(a[0], a[1], a[2], 0);
            if (b.C.Length < 4)
                b = new Vector(b[0], b[1], b[2], 0);
            Vector x = new Vector(0, 0, 0, 0);
            x.C[3] = a[3] * b[3] - a[0] * b[0] - a[1] * b[1] - a[2] * b[2];
            Vector v = new Vector();//векторная часть
            v = (a[3] * b) + (b[3] * a) + (a * b);
            x.C[2] = v[2];
            x.C[1] = v[1];
            x.C[0] = v[0];
            return x;
        }
        public override string ToString()
        {//конвертация в стринг
            string x = "( ";
            for (int i = 0; i < C.Length; i++)
                x += (Math.Round(C[i],2)).ToString() + " ";
            x += ")";
            return x;
        }
        /*public static Vector operator ~(Vector a)
        {//нахождение...
            return null;
        }*/

        //public static Vector M(double skal, Vector v)
        //{//умножение скаляра на вектор
        //    for (int i = 0; i < v.C.Length; i++)
        //        v.C[i] = v.C[i] * skal;
        //    return v;
        //}
        //public static Vector Add(Vector a, Vector b)
        //{//суммирование векторов
        //    if (a.C.Length == b.C.Length)
        //    {
        //        Vector x = new Vector();// = new Vector(a.C[0] + b.C[0], a.C[1] + b.C[1],...);
        //        x.C = new double[a.C.Length];
        //        for (int i = 0; i < x.C.Length; i++)
        //        {
        //            x.C[i] = a.C[i] + b.C[i];
        //        }
        //        return x;
        //    }
        //    else
        //        return null;
        //}
        //public static Vector Mult_v(Vector v,Vector u)
        //{//векторное умножение векторов
        //    return new Vector(v.C[1] * u.C[2] - v.C[2] * u.C[1], v.C[2] * u.C[0] - v.C[0] * u.C[2], v.C[0] * u.C[1] - v.C[1] * u.C[0]);
        //}
        //public static double m(Vector v, Vector u)
        //{//скалярное умножение векторов
        //    double a = 0;
        //    int min;
        //    if (v.C.Length < u.C.Length)
        //        min = v.C.Length;
        //    else
        //        min = u.C.Length;
        //    for (int i = 0; i < min; i++)
        //        a += u.C[i] * v.C[i];
        //    return a;
        //}
        //public static Vector Mult_q(Vector v, Vector u)
        //{//умножение кватернионов
        //    //Vector a = new Vector(0, 0, 0, 0);
        //    //return;//v + u;
        //    //a.C[3]=//скалярная часть
        //    //return a;
        //}
    }
}
