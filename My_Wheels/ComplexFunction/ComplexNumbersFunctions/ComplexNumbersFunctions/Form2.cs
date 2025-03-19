using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComplexNumbersFunctions
{
    public partial class Form2 : Form
    {
        int w, h, max_size = 200;
        Graphics g;
        Bitmap bit;
        //1080*1920
        //108 *192
        //54  *96
        //9   *16
        //-> x=[-8,8] y=[-4.5,4.5]

        double fromX = -8, fromY = -4.5, toX = 8, toY = 4.5;
        Complex q;
        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            w = pictureBox1.Width;
            h = pictureBox1.Height;
            if (w > 0 && h > 0)
            {
                //if (w > h) w = h;
                //if (h > w) h = w;
                //if (w > max_size) { w = h = max_size; }
                bit = new Bitmap(w, h);
                g = Graphics.FromImage(bit);
                g.Clear(Color.White);
                pictureBox1.Image = bit;
                Draw();
            }
        }

        public Form2()
        {
            InitializeComponent();
            q = new Complex();
            label1.Text = q.ToString();
            w = pictureBox1.Width;
            h = pictureBox1.Height;
            //if (w > h) w = h;
            //if (h > w) h = w;
            //if (w > max_size) { w = h = max_size; }
            bit = new Bitmap(w,h);
            g = Graphics.FromImage(bit);
            g.Clear(Color.White);
            pictureBox1.Image = bit;
            Draw();//ask task = Task.Run(() => Draw());
        }
        Complex f(Complex p, Complex a)
        {
            //f = x^2
            //return p*p;
            //return Complex.cos(p);
            //return p * p * p * p;
            //return Complex.exp(Complex.cos(p)+Complex.sin(a));
            //return Complex.sin(a) / Complex.cos(p);
            //return Complex.cos(a)*(p / (!p));
            //return Complex.pow(!p, !a);
            //return Complex.pow(new Complex(p.Im, p.Re), a);
            //return  a*Complex.exp(a.Im/Complex.cos(p)+a.Re/Complex.sin(p));
            return !p*a/!Complex.sin(p);
            //Complex t = Complex.pow(a, new Complex(p.Find_mod() / p.Find_arg(), p.Find_arg() / p.Find_mod()));
            //Complex t = Complex.Ln(a)*p;
            //return t+1/t;
        }
        double Mod2(double v) 
        {
            if (v < 0)
            {
                v = v + (Math.Floor(Math.Abs(v) / 2)+1) * 2;

                //while (v < 0) v += 2;
                return v;
            }
            else
            {
                v = v - (Math.Floor(Math.Abs(v) / 2)) * 2;

                //while (v > 2) v -= 2;
                return v;
            }
        }
        Color HSV_to_RGB(double H, double S, double V)
        {
            H = Math.Abs(H % 360);
            double C = V * S;
            double X = C * (1 - Math.Abs((Mod2(H/60)) - 1));
            double m = V - C;
            double r_=0, g_=0, b_=0;
            if (H >= 0 && H < 60)         {r_=C;g_=X;b_=0;}
            else if (H >= 60 && H < 120)  {r_=X;g_=C;b_=0;}
            else if (H >= 120 && H < 180) {r_=0;g_=C;b_=X;}
            else if (H >= 180 && H < 240) {r_=0;g_=X;b_=C;}
            else if (H >= 240 && H < 300) {r_=X;g_=0;b_=C;}
            else if (H >= 300 && H < 360) {r_=C;g_=0;b_=X;}
            return Color.FromArgb(Convert.ToInt32((r_ + m) * 255),
                                  Convert.ToInt32((g_ + m) * 255), 
                                  Convert.ToInt32((b_ + m) * 255));
        }
        Color convert(Complex x)
        {
            
            double l = x.Find_mod();
            double a = x.Find_arg();
            double thresh = 1;
            double H = x.Find_arg() * 360 / Math.PI;
            double S = 0.5 + 0.5 * (l - Math.Floor(l));
            double V = Math.Abs(Math.Pow(Math.Sin(x.Re * Math.PI), thresh)) * Math.Abs(Math.Pow(Math.Sin(x.Im * Math.PI), thresh));
            
            //HSV: H - angle, S, V=1;
            if (V > 1) V = 1;
            if (!double.IsNaN(H) && !double.IsNaN(S) && !double.IsNaN(V))
                return HSV_to_RGB(H, S, V);
            else
                return Color.Black;


            //double mod = x.Find_mod();
            //double arg = x.Find_arg();
            //int m=0;
            //if (!double.IsNaN(mod) && !double.IsInfinity(mod) && mod<int.MaxValue)
            //    m = Convert.ToInt32(mod);
            //else
            //    m = 255;
            //int a = Convert.ToInt32(arg);
            //if (m > 255) m %= 255;
            //if (m < 0) m = (-m)%255;
            //if (a > 255) a %= 255;
            //if (a < 0) a = (-a) % 255;
            //return Color.FromArgb(m, a, 128);
        }

        double convX(int x)
        {
            double xs = x;// = x - pw / 2;
            xs *= (double)(toX - fromX) / (double)w;
            xs += fromX;
            //xs = To * xs/pw + From*(1-xs/pw);//=From+(To-From)*xs/pw
            return xs;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            q.Im = trackBar1.Value / 10.0;
            q.Re = trackBar2.Value / 10.0;
            Draw();// Task task = Task.Run(() => Draw());
            label1.Text = q.ToString();
        }

        int reconvX(double x)
        {
            x -= fromX;
            x /= (double)(toX - fromX) / (double)w;
            return (int)x;
        }
        double convY(int y)
        {
            double ys = y;// - pw / 2;
            ys = h - ys;
            //ys *= -1;
            ys *= (double)(toY - fromY) / (double)h;
            ys += fromY;
            return ys;
        }
        int reconvY(double y)
        {
            y -= fromY;
            y /= (double)(toY - fromY) / (double)h;
            //y *= -1;
            y = h - y;
            return (int)y;
        }

        void Draw()
        {
            g.Clear(Color.White);
            double ii = fromX, jj = fromY, di = (double)(toX - fromX) / (double)w, dj = (double)(toY - fromY) / (double)h;
            Complex x = new Complex();
            for (int i = 0; i < w; i++,ii+=di)
                for (int j = 0; j < h; j++,jj+=dj)
                {
                    //g.FillRectangle(new SolidBrush(convert(f(new Complex(convX(i), convY(j))))), i, j, 1, 1);
                    //bit.SetPixel(i, j, convert(f(new Complex(convX(i), convY(j)))));
                    x.Re = convX(i);//ii;
                    x.Im = convY(j);//jj;
                    bit.SetPixel(i, j, convert(f(x,q)));
                }
            pictureBox1.Image = bit;
        }
    }
}
