using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tangent_of_the_curve
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            allFunctions.Add(f0);
            allFunctions.Add(f1);
            allFunctions.Add(f2);
            allFunctions.Add(f3);
            allFunctions.Add(f4);
            allFunctions.Add(f5);
            allFunctions.Add(f6);
            allFunctions.Add(f7);
            //allFunctions.Add(f8);
            //allFunctions.Add(f9);
            //allFunctions.Add(f10);
            ph = pictureBox1.Height;
            pw = pictureBox1.Width;
        }
        public struct PointD
        {
            public double x, y;
            public PointD(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
            public static bool operator ==(PointD other, PointD thiss)
            {
                return other.x == thiss.x && other.y == thiss.y;
            }
            public static bool operator !=(PointD other, PointD thiss)
            {
                return !(other.x == thiss.x && other.y == thiss.y);
            }
        }
        public struct line
        {
            public double A, B, C;
            public line(double x1, double y1, double x2, double y2)
            {
                A = y2 - y1;
                B = x1 - x2;
                C = x1 * (y1 - y2) + y1 * (x2 - x1);
            }
            public line(double A, double B, double C)
            {
                this.A = A;
                this.B = B;
                this.C = C;
            }
            public double findX(double y)
            {
                return (-B * y - C) / A;
            }
            public double findY(double x)
            {
                return (-A * x - C) / B;
            }
            public override string ToString()
            {
                return Math.Round(A, 3).ToString() + "*x + " + Math.Round(B, 3).ToString() + "*y + " + Math.Round(C, 3).ToString() + " = 0";
            }
        }
        //some functions
        double f0(double x) { return -x * x; }
        double f1(double x) { return -x * x * x; }
        double f2(double x) { return -Math.Sin(x); }
        double f3(double x) { return -Math.Cos(x); }
        double f4(double x) { return x > 0 ? -Math.Pow(x, Math.Cos(x)) : 0; }
        double f5(double x) { return -(x * Math.Sin(x) + x * x * Math.Cos(x)); }
        double f6(double x) { return -(x * Math.Sin(x * 2) * Math.Cos(x * x)); }
        double f7(double x) { return -(Math.Sin(Math.Pow(Math.E, (Math.Cos(x))))); }
        double dist(double x1, double y1, double x2, double y2) { return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)); }
        double df(F fun, double x)
        {//первая производная
            double d = 0.001;
            return (fun(x + d) - fun(x)) / d;
        }
        delegate double F(double x);//delegat, that contains functions
        List<F> allFunctions = new List<F>();//list of all functions
        double delt = 0.01;
        double fromX, toX, fromY, toY;//view limits of function
        F curFunction;//function, that is shown right now
        int ph, pw;//height and width of pictureBox
        Bitmap bit_pts, bitmap;//our picture(s)
        Graphics gr;//graphics, that bound to picture
        PointD pt1, pt2;//moving points
        bool flag = true;//forfirst init
        int curX, curY;//mouse position
        bool is_capt1 = false, is_capt2 = false;//is captured first or second point
        line tangLine;
        string rtb = "";
        private void button1_Click(object sender, EventArgs e)
        {
            init();
        }
        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            ph = pictureBox1.Height;
            pw = pictureBox1.Width;
            bitmap = new Bitmap(pw, ph);
            gr = Graphics.FromImage(bitmap);
            gr.Clear(Color.Black);
            pictureBox1.Image = bitmap;
            //MegaDraw();
            //Draw();
            Draw2();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (trackBar1.Value != trackBar2.Value)
            {
                fromX = trackBar1.Value > trackBar2.Value ? trackBar2.Value : trackBar1.Value;
                toX = trackBar1.Value < trackBar2.Value ? trackBar2.Value : trackBar1.Value;
                fromX /= (double)10;
                toX /= (double)10;
                FindMaxMinHeightOfFunction();
            }
            //MegaDraw();
            //Draw();
            Draw2();
        }
        void init()
        {
            Random r = new Random();
            curFunction = new F(allFunctions[r.Next(allFunctions.Count())]);
            //curFunction = new F(f4);
            FindMaxMinHeightOfFunction();
            ph = pictureBox1.Height;
            pw = pictureBox1.Width;
            bitmap = new Bitmap(pw, ph);
            gr = Graphics.FromImage(bitmap);
            gr.Clear(Color.Black);
            pictureBox1.Image = bitmap;
            fromX = trackBar1.Value > trackBar2.Value ? trackBar2.Value : trackBar1.Value;
            toX = trackBar1.Value < trackBar2.Value ? trackBar2.Value : trackBar1.Value;
            fromX /= (double)10;
            toX /= (double)10;
            FindMaxMinHeightOfFunction();
            if (flag == true)
            {
                pt1 = new PointD(convX(pw / 3), curFunction(convX(pw / 3)));
                pt2 = new PointD(convX(pw * 2 / 3), curFunction(convX(pw * 2 / 3)));
                flag = false;
            }
            else
            {
                pt1 = new PointD(pt1.x, curFunction(pt1.x));
                pt2 = new PointD(pt2.x, curFunction(pt2.x));
            }
            //MegaDraw();
            //Draw();
            Draw2();
        }
        void FindMaxMinHeightOfFunction()
        {
            fromY = curFunction(fromX);
            toY = curFunction(fromX);
            for (double x = fromX; x <= toX; x += delt)
            {
                double val = curFunction(x);
                if (val > toY) toY = val;
                if (val < fromY) fromY = val;
            }
            toY += 1;
            fromY -= 1;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            init();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (dist(reconvX(pt1.x), reconvY(pt1.y), curX, curY) < 5)
                is_capt1 = true;
            else if (dist(reconvX(pt2.x), reconvY(pt2.y), curX, curY) < 5)
                is_capt2 = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            is_capt1 = false;
            is_capt2 = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            curX = e.X;
            curY = e.Y;
            if (is_capt1)
            {
                pt1.x = convX(curX);
                pt1.y = curFunction(pt1.x);
                Draw2();
            }
            else if (is_capt2)
            {
                pt2.x = convX(curX);
                pt2.y = curFunction(pt2.x);
                Draw2();
            }
            //Draw();
        }

        double convX(int x)
        {
            double xs = x;// = x - pw / 2;
            xs *= (double)(toX - fromX) / (double)pw;
            xs += fromX;
            //xs = toX * xs/pw + fromX*(1-xs/pw);//=fromX+(toX-fromX)*xs/pw
            return xs;
        }
        int reconvX(double x)
        {
            x -= fromX;
            x /= (double)(toX - fromX) / (double)pw;
            return (int)x;
        }
        double convY(int y)
        {
            double ys = y;// - pw / 2;
            //ys *= -1;
            ys *= (double)(toY - fromY) / (double)ph;
            ys += fromY;
            return ys;
        }
        int reconvY(double y)
        {
            y -= fromY;
            y /= (double)(toY - fromY) / (double)ph;
            //y *= -1;
            return (int)y;
        }
        void DrawTang(PointD pt, Pen pen)
        {
            line tangLine = new line(df(curFunction, pt.x), -1, curFunction(pt.x) - pt.x * df(curFunction, pt.x));
            rtb += tangLine.ToString() + "\n";
            PointD p1, p2;
            if (Math.Abs(tangLine.B) > 0.1)
            {
                p1 = new PointD(convX(0), tangLine.findY(convX(0)));
                p2 = new PointD(convX(pw), tangLine.findY(convX(pw)));
                gr.DrawLine(pen, reconvX(p1.x), reconvY(p1.y), reconvX(p2.x), reconvY(p2.y));
            }
            else// if (Math.Abs(tangLine.A) > 0.5)
            {
                p1 = new PointD(tangLine.findX(convY(0)), convY(0));
                p2 = new PointD(tangLine.findX(convY(ph)), convY(ph));
                gr.DrawLine(pen, reconvX(p1.x), reconvY(p1.y), reconvX(p2.x), reconvY(p2.y));
            }
        }
        Bitmap MergedBitmaps(Bitmap bmp1, Bitmap bmp2)
        {
            Bitmap result = new Bitmap(Math.Max(bmp1.Width, bmp2.Width),
                                       Math.Max(bmp1.Height, bmp2.Height));
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp2, Point.Empty);
                g.DrawImage(bmp1, Point.Empty);
            }
            return result;
        }
        /*
        void MegaDraw()
        {
            bitmap = new Bitmap(pw, ph);
            g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);
            //axis
            if (fromY <= 0) g.DrawLine(new Pen(Color.Black, 2), 0, reconvY(0), pw, reconvY(0));
            if (fromX <= 0) g.DrawLine(new Pen(Color.Black, 2), reconvX(0), 0, reconvX(0), ph);
            //func
            for (int i = 0; i < pw - 1; i++)
            {
                g.DrawLine(new Pen(Color.Black), i, reconvY(curFunction(convX(i))), i + 1, reconvY(curFunction(convX(i + 1))));
            }
            pictureBox1.Image = bitmap;
            bit_pts = new Bitmap(pw, ph);
            gr = Graphics.FromImage(bit_pts);
        }
        void Draw()
        {
            rtb = "";
            gr = pictureBox1.CreateGraphics();
            pictureBox1.Refresh();
            //bit_pts = new Bitmap(pw, ph);
            //gr = Graphics.FromImage(bit_pts);
            //gr.Clear(Color.Transparent);
            //punctires
            //int rex1 = reconvX(pt1.x), rex2 = reconvX(pt2.x);
            //for (int i = 0; i < ph; i += 40)
            //{
            //    gr.DrawLine(new Pen(Color.LightCoral), rex1, i, rex1, i + 10);
            //    gr.DrawLine(new Pen(Color.LightCoral), rex2, i, rex2, i + 10);
            //}
            //triangle
            gr.DrawLine(new Pen(Color.Blue, 2), reconvX(pt1.x), reconvY(pt1.y), reconvX(pt1.x), reconvY(pt2.y));
            gr.DrawLine(new Pen(Color.Blue, 2), reconvX(pt2.x), reconvY(pt2.y), reconvX(pt1.x), reconvY(pt2.y));
            //tang in each point
            //y=f'(x0)*(x-x0)+f(x0)
            rtb += "p1_tangent:\n";
            DrawTang(pt1, new Pen(Color.Green,2));
            //y=f'(x1)*(x-x1)+f(x1)
            rtb += "p2_tangent:\n";
            DrawTang(pt2,new Pen(Color.Green,2));
            //tang
            tangLine = new line((pt1.x), (pt1.y), (pt2.x), (pt2.y));
            PointD p1, p2;
            if (tangLine.B!=0)//(Math.Abs(tangLine.B) > 0.0001)
            {
                p1 = new PointD(convX(0), tangLine.findY(convX(0)));
                p2 = new PointD(convX(pw), tangLine.findY(convX(pw)));
                //gr.DrawLine(new Pen(Color.Red, 2), (int)(p1.x), (int)(p1.y), (int)(p2.x), (int)(p2.y));
                gr.DrawLine(new Pen(Color.Red, 2), reconvX(p1.x), reconvY(p1.y), reconvX(p2.x), reconvY(p2.y));
                rtb += "result_tangent(a):\n";
                rtb += tangLine.ToString() + "\n";
            }
            else if (tangLine.A!=0)//(Math.Abs(tangLine.A) > 0.0001)
            {
                p1 = new PointD(tangLine.findX(convY(0)), convY(0));
                p2 = new PointD(tangLine.findX(convY(ph)), convY(ph));
                //gr.DrawLine(new Pen(Color.Red, 2), (int)(p1.x), (int)(p1.y), (int)(p2.x), (int)(p2.y));
                gr.DrawLine(new Pen(Color.Red, 2), reconvX(p1.x), reconvY(p1.y), reconvX(p2.x), reconvY(p2.y));
                rtb += "result_tangent(b):\n";
                rtb += tangLine.ToString() + "\n";
            }
            else
            {
                //y=f'(x0)*(x-x0)+f(x0)
                rtb += "result_tangent:\n";
                DrawTang(pt2, new Pen(Color.Red, 2));
            }
            //points
            gr.FillEllipse(new SolidBrush(Color.Red), reconvX(pt1.x) - 5, reconvY(pt1.y) - 5, 10, 10);
            gr.FillEllipse(new SolidBrush(Color.Red), reconvX(pt2.x) - 5, reconvY(pt2.y) - 5, 10, 10);
            //show something in rtb
            richTextBox1.Text = rtb;
            //show picture
            //bit_pts = new Bitmap(bitmap.Width, bitmap.Height);
            //Bitmap result = MergedBitmaps(bit_pts,bitmap);
            //pictureBox1.Image = ;
            //pictureBox1.Image = bitmap;
        }
        */
        void Draw2()
        {
            gr.Clear(Color.Black);
            //axis
            if (fromY <= 0) gr.DrawLine(new Pen(Color.White, 2), 0, reconvY(0), pw, reconvY(0));
            if (fromX <= 0) gr.DrawLine(new Pen(Color.White, 2), reconvX(0), 0, reconvX(0), ph);
            //func
            for (int i = 0; i < pw - 1; i++)
            {
                gr.DrawLine(new Pen(Color.White), i, reconvY(curFunction(convX(i))), i + 1, reconvY(curFunction(convX(i + 1))));
            }
            rtb = "";
            //punctires
            //int rex1 = reconvX(pt1.x), rex2 = reconvX(pt2.x);
            //for (int i = 0; i < ph; i += 40)
            //{
            //    gr.DrawLine(new Pen(Color.LightCoral), rex1, i, rex1, i + 10);
            //    gr.DrawLine(new Pen(Color.LightCoral), rex2, i, rex2, i + 10);
            //}
            //triangle
            gr.DrawLine(new Pen(Color.Blue, 2), reconvX(pt1.x), reconvY(pt1.y), reconvX(pt1.x), reconvY(pt2.y));
            gr.DrawLine(new Pen(Color.Blue, 2), reconvX(pt2.x), reconvY(pt2.y), reconvX(pt1.x), reconvY(pt2.y));
            //tang in each point
            //y=f'(x0)*(x-x0)+f(x0)
            rtb += "p1_tangent:\n";
            DrawTang(pt1, new Pen(Color.Green, 2));
            //y=f'(x1)*(x-x1)+f(x1)
            rtb += "p2_tangent:\n";
            DrawTang(pt2, new Pen(Color.Green, 2));
            //tang
            tangLine = new line((pt1.x), (pt1.y), (pt2.x), (pt2.y));
            PointD p1, p2;
            if (tangLine.B != 0)//(Math.Abs(tangLine.B) > 0.0001)
            {
                p1 = new PointD(convX(0), tangLine.findY(convX(0)));
                p2 = new PointD(convX(pw), tangLine.findY(convX(pw)));
                //gr.DrawLine(new Pen(Color.Red, 2), (int)(p1.x), (int)(p1.y), (int)(p2.x), (int)(p2.y));
                gr.DrawLine(new Pen(Color.Red, 2), reconvX(p1.x), reconvY(p1.y), reconvX(p2.x), reconvY(p2.y));
                rtb += "result_tangent(a):\n";
                rtb += tangLine.ToString() + "\n";
            }
            else if (tangLine.A != 0)//(Math.Abs(tangLine.A) > 0.0001)
            {
                p1 = new PointD(tangLine.findX(convY(0)), convY(0));
                p2 = new PointD(tangLine.findX(convY(ph)), convY(ph));
                //gr.DrawLine(new Pen(Color.Red, 2), (int)(p1.x), (int)(p1.y), (int)(p2.x), (int)(p2.y));
                gr.DrawLine(new Pen(Color.Red, 2), reconvX(p1.x), reconvY(p1.y), reconvX(p2.x), reconvY(p2.y));
                rtb += "result_tangent(b):\n";
                rtb += tangLine.ToString() + "\n";
            }
            else
            {
                //y=f'(x0)*(x-x0)+f(x0)
                rtb += "result_tangent:\n";
                DrawTang(pt2, new Pen(Color.Red, 2));
            }
            //points
            gr.FillEllipse(new SolidBrush(Color.Red), reconvX(pt1.x) - 5, reconvY(pt1.y) - 5, 10, 10);
            gr.FillEllipse(new SolidBrush(Color.Red), reconvX(pt2.x) - 5, reconvY(pt2.y) - 5, 10, 10);
            //show something in rtb
            richTextBox1.Text = rtb;
            //show picture
            pictureBox1.Image = bitmap;
        }
    }
}
