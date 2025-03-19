using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplexNumbersFunctions
{
    public class Complex
    {
        double eps = 0.000001;
        public double Re { get; set; }
        public double Im { get; set; }
        public Complex() { Re = 0;Im = 0; }
        public Complex(Complex other) { this.Re = other.Re;this.Im = other.Im; }
        public Complex(double Real) { Re = Real;Im = 0; }
        public Complex(double Real,double Imag) { Re = Real;Im = Imag; }
        public override string ToString()
        {
            if (Im == 0)
                return Re.ToString();
            else if (Im > 0)
            {
                if (Re != 0)
                    return Re.ToString() + " + i" + Im.ToString();
                else
                    return "i * " + Im.ToString();
            }
            else
            {
                if (Re != 0)
                    return Re.ToString() + " - i" + Math.Abs(Im).ToString();
                else
                    return "-i * " + Math.Abs(Im).ToString();
            }
        }
        public double Find_arg()//phi
        {
            if (Re > 0)
                return Math.Atan(Im / Re);
            else if (Re < 0 && Im >= 0)
                return Math.PI + Math.Atan(Im / Re);
            else if (Re < 0 && Im < 0)
                return -Math.PI + Math.Atan(Im / Re);
            else if (Math.Abs(Re - eps) < 0 && Im > 0)
                return Math.PI / 2;
            else if (Math.Abs(Re - eps) < 0 && Im < 0)
                return -Math.PI / 2;
            else //Re=Im=0
                return 0;
        }
        public double Find_mod()//r
        {
            return Math.Sqrt(Re * Re + Im * Im);
        }
        public static Complex operator +(Complex a,Complex b)
        {
            return new Complex(a.Re + b.Re, a.Im + b.Im);
        }
        public static Complex operator +(double a, Complex b)
        {
            return new Complex(a + b.Re, b.Im);
        }
        public static Complex operator +(Complex a, double b)
        {
            return new Complex(a.Re + b, a.Im);
        }
        public static Complex operator -(Complex a, Complex b)
        {
            return new Complex(a.Re - b.Re, a.Im - b.Im);
        }
        public static Complex operator *(Complex a, Complex b)
        {
            return new Complex(a.Re * b.Re - a.Im * b.Im, a.Im * b.Re + a.Re * b.Im);
        }
        public static Complex operator *(Complex a, double b)
        {
            return new Complex(a.Re * b, a.Im * b);
        }
        public static Complex operator *(double a, Complex b)
        {
            return new Complex(b.Re * a, b.Im * a);
        }
        public static Complex operator !(Complex a)
        {
            return new Complex(a.Re , 0 - a.Im);
        }
        public static Complex operator /(Complex a, double b)
        {
            return new Complex(a.Re / b, a.Im / b);
        }
        public static Complex operator /(double a, Complex b)
        {
            return new Complex(a) / b;
        }
        public static Complex operator /(Complex a, Complex b)
        {
            Complex t = a * (!b);
            return new Complex(t.Re / (b.Re * b.Re + b.Im * b.Im), t.Im / (b.Re * b.Re + b.Im * b.Im));
        }
        public static Complex cos(Complex x)
        {
            return new Complex(Math.Cos(x.Re) * Math.Cosh(x.Im),
                                -Math.Sin(x.Re) * Math.Sinh(x.Im));
        }
        public static Complex sin(Complex x)
        {
            return new Complex(Math.Sin(x.Re) * Math.Cosh(x.Im),
                                Math.Cos(x.Re) * Math.Sinh(x.Im));
        }
        public static Complex Ln(Complex x)
        {

            return new Complex(Math.Log(x.Find_mod()), x.Find_arg());
        }
        public static Complex exp(Complex x)
        {
            return new Complex(Math.Cos(x.Im), Math.Sin(x.Im)) * Math.Exp(x.Re);
        }
        public static Complex pow(Complex x, Complex p)
        {
            return exp(p * Complex.Ln(x));
        }
        public static Complex pow(Complex x, int n)
        {//формула Муавра cyclowiki.org/wiki/Возведение_комплексного_числа_в_степень_натурального_числа
            double r = Math.Pow(x.Find_mod(), n);
            double phi = n * x.Find_arg();
            return new Complex(r * Math.Cos(phi), r * Math.Sin(phi));
        }
    }
}
