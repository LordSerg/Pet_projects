using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lightning
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOZORDER = 0x0004;

        private const int PREFERRED_MONITOR = 0;
        public Form1()
        {
            if (Screen.AllScreens.Length >= PREFERRED_MONITOR)
            {
                SetWindowPos(this.Handle,
                    IntPtr.Zero,
                    Screen.AllScreens[PREFERRED_MONITOR].WorkingArea.Left,
                    Screen.AllScreens[PREFERRED_MONITOR].WorkingArea.Top,
                    0, 0, SWP_NOSIZE | SWP_NOZORDER);
            }
            InitializeComponent();
            //lightToRender = new List<Lightning>();
            label1.Text = "% вторичного ветвления = " + (trackBar1.Value).ToString();
            label2.Text = "% третичного ветвления = " + (trackBar2.Value).ToString();
            label3.Text = "увеличить размер ветвления в " + ((double)trackBar3.Value / 100).ToString() + " раз";
            label4.Text = "отклонение от среднего = " + ((double)trackBar4.Value / 100).ToString();
            label5.Text = "количество делений на 2 = " + (trackBar5.Value).ToString();
            label6.Text = "максимальный угол ветвления = " + (trackBar6.Value).ToString();
        }
        Graphics g;
        Bitmap bit;
        class PointD
        {
            public double X, Y;
            public PointD(double x,double y)
            {
                X = x;
                Y = y;
            }
            public static PointD operator +(PointD p1,PointD p2)
            {
                return new PointD(p1.X + p2.X, p1.Y + p2.Y);
            }
            public static PointD operator -(PointD p1, PointD p2)
            {
                return new PointD(p1.X - p2.X, p1.Y - p2.Y);
            }
            public static PointD operator *(PointD p, double d)
            {
                return new PointD(p.X * d, p.Y * d);
            }
            public static explicit operator Point(PointD p)
            {
                return new Point((int) p.X, (int) p.Y);
            }
            public static implicit operator PointD(Point p)
            {
                return new PointD(p.X, p.Y);
            }
        }
        class Kusok
        {
            public PointD p1, p2;
            public Kusok(PointD pt1, PointD pt2)
            {
                p1 = pt1;
                p2 = pt2;
            }
        }
        class Lightning
        {
            List<Kusok> MainLightnin;//главная молния
            List<Kusok> SecondaryLightnin;//второстепенная молния
            List<Kusok> ThirdlyLightnin;//третестепенная молния
            public static double persentOtkloneniya=0.2;
            public static double scale = 0.7;//то, на сколько больше будет линия ветвления
            public static int veroyatnost1=50, veroyatnost2=50;
            public static int numOfDivisions = 6;
            public static int MaxAngle = 60;
            public int maxOtklonenie=300, iterationNum = 6;//максимальное отклонение и количество раз, которое мы делим линию на 2
            private static Random r=new Random();
            public Lightning(PointD start, PointD end)
            {
                PointD midPoint, direction, splitEnd;
                int offsetAmount;
                for (int i = 0; i < r.Next(50, 100); i++) ;
                double lightLngth = Math.Sqrt((start.X - end.X) * (start.X - end.X) + (start.Y - end.Y) * (start.Y - end.Y));
                //тут идет настройка параметров самой молнии, в зависимости от ее длинны:
                //maxOtklonenie = (int)lightLngth/5;
                maxOtklonenie = (int)(lightLngth* persentOtkloneniya);
                //if (lightLngth <= 50)
                //    iterationNum = 3;
                //else if (lightLngth <= 100)
                //    iterationNum = 4;
                //else if (lightLngth <= 500)
                //    iterationNum = 5;
                //else if (lightLngth <= 700)
                //    iterationNum = 6;
                //else if (lightLngth <= 1200)
                //    iterationNum = 7;
                //else if (lightLngth <= 1500)
                //    iterationNum = 8;
                //else //if (lightLngth <= 2000)
                //    iterationNum = 9;
                iterationNum = numOfDivisions;
                MainLightnin = new List<Kusok>();
                SecondaryLightnin = new List<Kusok>();
                ThirdlyLightnin = new List<Kusok>();
                //MainLightnin.Clear();
                //SecondaryLightnin.Clear();
                MainLightnin.Add(new Kusok(start, end));
                offsetAmount = maxOtklonenie;     // максимальное смещение вершины молнии
                for (int i = 0; i < iterationNum; i++) // (некоторое число итераций)
                {
                    int num = ThirdlyLightnin.Count;
                    for (int j = 0; j < num; j++)
                    {
                        Kusok k = ThirdlyLightnin[0];
                        ThirdlyLightnin.RemoveAt(0);//Вместо одного куска делаем два
                        midPoint = Average(k.p1, k.p2);
                        midPoint += Perpendicular(Normalize(k.p1 - k.p2)) * r.Next(-offsetAmount, offsetAmount);//Сдвигаем midPoint на случайное отклонение относительно центра куска "k"
                        //Добавляем два новых куска, вместо старого
                        ThirdlyLightnin.Add(new Kusok(k.p1, midPoint));
                        ThirdlyLightnin.Add(new Kusok(midPoint, k.p2));
                    }
                    num = SecondaryLightnin.Count;
                    for (int j = 0; j < num; j++)
                    {
                        Kusok k = SecondaryLightnin[0];
                        SecondaryLightnin.RemoveAt(0);
                        midPoint = Average(k.p1, k.p2);
                        midPoint += Perpendicular(Normalize(k.p1 - k.p2)) * r.Next(-offsetAmount, offsetAmount);
                        SecondaryLightnin.Add(new Kusok(k.p1, midPoint));
                        SecondaryLightnin.Add(new Kusok(midPoint, k.p2));
                        if (r.Next(100) < veroyatnost2)//... вероятности, что произойдет третичное ветвление
                        {//делаем ветвление
                            direction = midPoint - k.p1;
                            splitEnd = Rotate(direction, r.Next(-MaxAngle, MaxAngle) * Math.PI / 180) * scale + midPoint;
                            ThirdlyLightnin.Add(new Kusok(midPoint, splitEnd));
                        }
                    }
                    num = MainLightnin.Count;
                    for (int j = 0; j < num; j++)
                    {
                        Kusok k = MainLightnin[0];
                        MainLightnin.RemoveAt(0);
                        midPoint = Average(k.p1, k.p2);
                        midPoint += Perpendicular(Normalize(k.p1 - k.p2)) * r.Next(-offsetAmount, offsetAmount);
                        MainLightnin.Add(new Kusok(k.p1, midPoint));
                        MainLightnin.Add(new Kusok(midPoint, k.p2));
                        if (r.Next(100) < veroyatnost1)//... вероятности, что произойдет вторичное ветвление
                        {//делаем ветвление
                            direction = midPoint - k.p1;
                            splitEnd = Rotate(direction, r.Next(-MaxAngle, MaxAngle) * Math.PI / 180) * scale + midPoint;
                            SecondaryLightnin.Add(new Kusok(midPoint, splitEnd));
                        }
                    }
                    offsetAmount /= 2; // Каждый раз уменьшаем в два раза смещение центральной точки
                }
            }
            public void DrawLightning(Graphics gr,int brightness, float width=3)
            {
                r = new Random();
                //Color c1 = Color.FromArgb(brightness, brightness, brightness);
                
                int cr = r.Next(-50,50);
                cr = (brightness - cr)<0?0:( (brightness - cr)>255?255: (brightness - cr));
                int cg = r.Next(-50, 50);
                cg = (brightness - cg) < 0 ? 0 : ((brightness - cg) > 255 ? 255 : (brightness - cg));
                int cb = r.Next(-50, 50);
                cb = (brightness - cb) < 0 ? 0 : ((brightness - cb) > 255 ? 255 : (brightness - cb));
                Color c1 = Color.FromArgb(cr, cg, cb);
                cr = r.Next(50);
                cg = r.Next(50);
                cb = r.Next(50);
                Color c2 = Color.FromArgb(c1.R/2+cr, c1.G/2+cg, c1.B/2+cb);
                Color c3 = Color.FromArgb(c2.R/2, c2.G/2, c2.B/2);
                for (int i = 0; i < ThirdlyLightnin.Count; i++)
                {
                    gr.DrawLine(new Pen(c3), (Point)ThirdlyLightnin[i].p1, (Point)ThirdlyLightnin[i].p2);
                }
                for (int i = 0; i < SecondaryLightnin.Count; i++)
                {
                    gr.DrawLine(new Pen(c2, width/4), (Point)SecondaryLightnin[i].p1, (Point)SecondaryLightnin[i].p2);
                }
                for (int i = 0; i < MainLightnin.Count; i++)
                {
                    gr.DrawLine(new Pen(c1, width), (Point)MainLightnin[i].p1, (Point)MainLightnin[i].p2);
                }
            }
            PointD Average(PointD pt1, PointD pt2)
            {
                return new PointD((pt1.X + pt2.X) / 2, (pt1.Y + pt2.Y) / 2);
            }
            PointD Normalize(PointD pt)
            {
                double lngth = Math.Sqrt(pt.X * pt.X + pt.Y * pt.Y);
                return new PointD(pt.X / lngth, pt.Y / lngth);
            }
            PointD Perpendicular(PointD pt)
            {
                return new PointD(-pt.Y, pt.X);
            }
            PointD Rotate(PointD pt, double ang)
            {
                double cosA = Math.Cos(ang), sinA = Math.Sin(ang);
                return new PointD(pt.X * cosA - pt.Y * sinA, pt.X * sinA + pt.Y * cosA);
            }
        }
        int x1, y1, x2, y2;//начальная и конечная точка для молнии
        Lightning light1, light2, light3;
        //List<Lightning> lightToRender;
        //void DrawLightningStepByStep(List<Lightning> lights)
        //{
        //    lightToRender.Clear();
        //    foreach (Lightning li in lights)
        //    {
        //        lightToRender.Add(li);
        //    }
        //    timer1.Interval = 1;
        //    timer1.Enabled = true;
        //}
        void DrawStrike()
        {
            counter = 0;
            counter2 = 5;
            counter3 = 12;
            timer2.Interval = 1;
            timer2.Enabled = true;
        }
        int counter = 0, counter2 = 5, counter3 = 10;
        void action()
        {
            if (!checkBox1.Checked&& light1 != null && light2 != null)
            {
                g.Clear(Color.Black);
                light1 = new Lightning(new PointD(x1, y1), new PointD(x2, y2));
                light1.DrawLightning(g, 255, 5);
                pictureBox1.Image = bit;
            }
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {//%1
            Lightning.veroyatnost1 = trackBar1.Value;
            label1.Text = "% вторичного ветвления = " + (trackBar1.Value).ToString();
            action();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {//%2
            Lightning.veroyatnost2 = trackBar2.Value;
            label2.Text = "% третичного ветвления = " + (trackBar2.Value).ToString();
            action();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {//scale
            Lightning.scale = (double)trackBar3.Value / 100;
            label3.Text = "увеличить размер ветвления в " + ((double)trackBar3.Value / 100).ToString()+" раз";
            action();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {//% отклонения
            Lightning.persentOtkloneniya = (double)trackBar4.Value / 100;
            label4.Text = "отклонение от среднего = " + ((double)trackBar4.Value / 100).ToString();
            action();
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {//количество разделений
            Lightning.numOfDivisions = trackBar5.Value;
            label5.Text = "количество делений на 2 = " + (trackBar5.Value).ToString();
            action();
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            Lightning.MaxAngle = trackBar6.Value;
            label6.Text = "максимальный угол ветвления = " + (trackBar6.Value).ToString();
            action();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (light1 != null && light2 != null)
                    DrawStrike();
            }
            else
            {
                if (light1 != null && light2 != null)
                {
                    action();
                    timer2.Enabled = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(panel1.Width<40)
            {
                button1.Text = "←";
                panel1.Width = 250;
            }
            else
            {
                button1.Text = "→";
                panel1.Width = 33;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //x2 = e.X;
            //y2 = e.Y;
        }

        void mercsaniye(ref int i,ref Lightning l)
        {//мекцание молнии, изменяем яркость отрисовки молнии
            if (i <= 2)
            {
                l.DrawLightning(g, 10 + i * 20);
            }
            else if (i <= 4)
            {
                l.DrawLightning(g, 150 + i * 26);
            }
            else if (i <= 7)
            {
                l.DrawLightning(g, 255, i);
            }
            else if (i <= 9)
            {
                l.DrawLightning(g, 200 - i * 15);
            }
            else
            {
                l = new Lightning(new PointD(x1, y1), new PointD(x2, y2));
            }
            i++;
            if (i > 10) i = 0;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            g.Clear(Color.Black);
            mercsaniye(ref counter, ref light1);
            mercsaniye(ref counter2, ref light2);
            //mercsaniye(ref counter3, ref light3);
            pictureBox1.Image = bit;
        }

        private void timer1_Tick(object sender, EventArgs e){}

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Random random = new Random();
            x2 = e.X;
            y2 = e.Y;
            bit = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bit);
            g.Clear(Color.Black);
            timer2.Enabled = false;
            if (Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)) > 10)
            {
                light1 = new Lightning(new PointD(x1, y1), new PointD(x2, y2));
                light2 = new Lightning(new PointD(x1, y1), new PointD(x2, y2));
                //light3 = new Lightning(new PointD(x1, y1), new PointD(x2, y2));
                if (checkBox1.Checked)
                    DrawStrike();
                else
                    light1.DrawLightning(g, 255, 5);
            }
            else
            {
                timer2.Enabled = false;
                //Lightning li8;
                //for (int i = 0; i < 8; i++)
                //{
                //    li8 = new Lightning(new PointD(x2, y2), new PointD(x2 + 500*Math.Cos((i+random.NextDouble())*Math.PI/4), y2 + 500 * Math.Sin((i + random.NextDouble()) * Math.PI / 4)));
                //    li8.DrawLightning(g);
                //}
            }
            pictureBox1.Image = bit;
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            x1 = e.X;
            y1 = e.Y;
        }
    }
}
