using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CircleLineIntersection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        struct point
        {
            public double x, y;
            public point(double X,double Y) { x = X; y = Y; }
            public override string ToString()
            {
                return "("+Math.Round(x,4)+"; "+Math.Round(y,4)+")";
            }
            public void draw(Graphics g)
            {
                g.DrawEllipse(new Pen(Color.FromArgb(255,0,0)), (int)(x-3),(int)(y-3),6,6);
                //double r = 5;
                //double PI = Math.PI;
                //for (double i = 0; i < 2 * PI; i += 0.01)
                //    if((int)(x+r*Math.Cos(i))>0&&(int)(x+r*Math.Cos(i))<b.Width
                //        &&(int)(y+r*Math.Sin(i))>0&&(int)(y+r*Math.Sin(i))<b.Height)
                //    b.SetPixel((int)(x + r * Math.Cos(i)), (int)(y + r * Math.Sin(i)), Color.FromArgb(255, 0, 0));
            }
            public bool check_is_in(int X,int Y)
            {
                return (Math.Abs(X - x) <= 5) && (Math.Abs(Y - y) <= 5);
            }
        }
        struct circle
        {
            public double r;
            public point c;
            public circle(point C, double R) { c = C;r = R; }
            public void draw(Graphics g)
            {
                c.draw(g);
                g.DrawEllipse(new Pen(Color.FromArgb(0,0,0)), (int)(c.x-r),(int)(c.y-r),(int)r*2,(int)r*2);
                //double PI = Math.PI;
                //for(double i=0;i<2*PI;i+=0.01)
                //    if((int)(c.x+r*Math.Cos(i))>0&&(int)(c.x+r*Math.Cos(i))<b.Width
                //        &&(int)(c.y+r*Math.Sin(i))>0&&(int)(c.y+r*Math.Sin(i))<b.Height)
                //    b.SetPixel((int)(c.x+r*Math.Cos(i)),(int)(c.y+r*Math.Sin(i)),Color.FromArgb(0,0,0));
            }

        }
        struct line
        {
            public point a, b;
            public line(point A,point B) { a = A;b = B; }
            public void draw(Graphics g)
            {
                a.draw(g);
                b.draw(g);
                g.DrawLine(new Pen(Color.FromArgb(0,0,0)), (int)a.x, (int)a.y, (int)b.x,(int)b.y);
                //for (double t = 0; t <= 1; t += 0.002)
                //    if ((int)(a.x*(1-t)+ b.x*t) > 0 && (int)(a.x*(1-t)+ b.x*t) < bt.Width
                //        && (int)(a.y*(1-t)+b.y*t) > 0 && (int)(a.y*(1-t)+b.y*t) < bt.Height)
                //        bt.SetPixel((int)(a.x*(1-t)+ b.x*t),(int)(a.y*(1-t)+b.y*t),Color.FromArgb(0, 0, 0));
            }
        }
        circle krug;
        line pryamaya;
        point answer1, answer2;
        Bitmap bit;
        Graphics g;
        int zzz=0;
        private int whereCrossing()
        {
            double x0 = krug.c.x, x1 = pryamaya.a.x, x2 = pryamaya.b.x;
            double y0 = krug.c.y, y1 = pryamaya.a.y, y2 = pryamaya.b.y;
            double r = krug.r;
            double t1, t2;                                 //p1=center => 
            double A = ((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1));  //...
            double B = 2 * ((x2 - x1) * (x1 - x0) + (y2 - y1) * (y1 - y0));//0
            double C = (x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0) - r * r;  //0 => D=0!!!
                                                                               //if (B <= 0 && C <= 0) { B = C = 0; }
            double D = (B * B) - (4 * A * C);
            //std::cout << D << "               \r";
            t1 = (-B + Math.Sqrt(D)) / (2 * A);
            t2 = (-B - Math.Sqrt(D)) / (2 * A);
            if (t1 >= 0 && t1 <= 1 && t2 >= 0 && t2 <= 1)
            {
                answer1 = new point(x1 + (x2 - x1) * t1, y1 + (y2 - y1) * t1);
                answer2 = new point(x1 + (x2 - x1) * t2, y1 + (y2 - y1) * t2);
                return 2;
            }
            else if(t1 >= 0 && t1 <= 1)
            {
                answer1 = new point(x1 + (x2 - x1) * t1, y1 + (y2 - y1) * t1);
                answer2 = new point(0, 0);
                return 1;
            }
            else if (t2 >= 0 && t2 <= 1)
            {
                answer1 = new point(x1 + (x2 - x1) * t2, y1 + (y2 - y1) * t2);
                answer2 = new point(0, 0);
                return 1;
            }
            else
                return 0;
            //if (D == 0)
            //{
            //    t1 = (-B + Math.Sqrt(D)) / (2 * A);
            //    //t2 = (-B - sqrt(D)) / (2 * A);
            //    answer1 = new point(x1 + (x2 - x1) * t1, y1 + (y2 - y1) * t1);
            //    //answer2 = point(x1 + (x2 - x1) * t2, y1 + (y2 - y1) * t2);
            //    return 1;
            //}
            //else if (D > 0)
            //{
            //    t1 = (-B + Math.Sqrt(D)) / (2 * A);
            //    t2 = (-B - Math.Sqrt(D)) / (2 * A);
            //    answer1 = new point(x1 + (x2 - x1) * t1, y1 + (y2 - y1) * t1);
            //    answer2 = new point(x1 + (x2 - x1) * t2, y1 + (y2 - y1) * t2);
            //    return 2;
            //}
            //else
            //    return 0;
        }
        private void setValues()
        {
            krug = new circle(new point(Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox2.Text)),
                Convert.ToDouble(textBox3.Text));
            pryamaya = new line(new point(Convert.ToDouble(textBox4.Text), Convert.ToDouble(textBox5.Text)),
                new point(Convert.ToDouble(textBox6.Text), Convert.ToDouble(textBox7.Text)));
        }
        private void setAnswer()
        {
            g.Clear(Color.FromArgb(255,255,255));
            pryamaya.draw(g);
            krug.draw(g);
            int t=whereCrossing();
            label10.Text = "Intersection:";
            label10.Text += $"\nnum of intersections: {t}";
            if(t==2)
            {
                label10.Text += $"\nintersection 1: {answer1}";
                label10.Text += $"\nintersection 2: {answer2}";
                answer1.draw(g);
                answer2.draw(g);
            }
            else if(t==1)
            {
                label10.Text += $"\nintersection 1: {answer1}";
                answer1.draw(g);
            }
            pictureBox1.Image = bit;
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (pryamaya.a.check_is_in(e.X, e.Y))
            {//перетаскиваем точку А
                zzz = 1;
            }
            if (pryamaya.b.check_is_in(e.X, e.Y))
            {//перетаскиваем точку В
                zzz = 2;
            }
            if (krug.c.check_is_in(e.X, e.Y)) 
            {//перетаскиваем центр круга
                zzz = 3;
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //if(e.X>=5&&e.X<pictureBox1.Width&&e.Y>=5&&e.Y<pictureBox1.Height)
            if (zzz == 1)
            {//перетаскиваем точку А
                pryamaya = new line(new point(e.X, e.Y), pryamaya.b);
                textBox4.Text = e.X.ToString();
                textBox5.Text = e.Y.ToString();
            }
            else if (zzz == 2)
            {//перетаскиваем точку В
                pryamaya = new line(pryamaya.a, new point(e.X, e.Y));
                textBox6.Text = e.X.ToString();
                textBox7.Text = e.Y.ToString();
            }
            else if (zzz == 3)
            {//перетаскиваем центр круга
                krug = new circle(new point(e.X, e.Y), krug.r);
                textBox1.Text = e.X.ToString();
                textBox2.Text = e.Y.ToString();
            }
            label11.Text = (new point(e.X, e.Y)).ToString();
            setValues();
            setAnswer();
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            zzz = 0;
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            bit = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bit);
            setValues();
            setAnswer();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            bit = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bit);
            setValues();
            setAnswer();
        }
    }
}
