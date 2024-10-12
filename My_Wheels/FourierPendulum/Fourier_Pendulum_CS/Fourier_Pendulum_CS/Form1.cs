using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fourier_Pendulum_CS
{
    public partial class Form1 : Form
    {
        int w, h;
        Graphics g;
        Bitmap bit;
        List<PointD> track;
        List<PointD> original;
        List<rotatingVector> vectors;
        bool is_ms_down = false;

        Color CircleCol = Color.FromArgb(30, 0, 50, 150), TrackCol = Color.Black, OrigCol = Color.LightCoral;
        public class PointD
        {
            public double X, Y;
            public PointD(double x, double y)
            {
                X = x;
                Y = y;
            }
        }
        public struct circle
        {
            public double x, y, r;
            public circle(double X, double Y, double R)
            {
                x = X;
                y = Y;
                r = R;
            }
            public void Draw(Graphics g, Color c)
            {
                g.FillEllipse(new SolidBrush(c), (float)(x - r), (float)(y - r), (float)r * 2, (float)r * 2);
            }
        }
        public struct rotatingVector
        {
            public double frequency;//speed
            public double phase;// { get; }
            public double amplitude;// { get; }

            public rotatingVector(double freq, double phi, double amp)
            {
                frequency = freq;
                phase = phi;
                amplitude = amp;
            }
            //circle c;
            //public float angle { get; }
            //public float dx { get { } }
            //public float dy { get { } }
            //public float X { get { return c.x; } }
            //public float Y { get { return c.y; } }
            //public float R { get { return c.r; } }
            //public rotatingVector(float X, float Y, float R, float S)
            //{
            //    c = new circle(X,Y,R);
            //    s = S;
            //}
            //public void Draw(Graphics g, Color col)
            //{
            //    g.FillEllipse(new SolidBrush(col), c.x - c.r, c.y - c.r, c.r * 2, c.r * 2);
            //}
        }
        public Form1()
        {
            InitializeComponent();
            track = new List<PointD>();
            
            original = new List<PointD>();
            vectors = new List<rotatingVector>();

            fourierX = new List<rotatingVector>();
            fourierY = new List<rotatingVector>();
            //var result = createSomethingCool(new PointD[] { 
            //    new PointD(100, 100),
            //    new PointD(120, 100),
            //    new PointD(140, 100),
            //    new PointD(160, 100),
            //    new PointD(180, 100),
            //    new PointD(200, 100),
            //    new PointD(200, 120),
            //    new PointD(200, 140),
            //    new PointD(200, 160),
            //    new PointD(200, 180),
            //    new PointD(200, 200),
            //    new PointD(180, 200),
            //    new PointD(160, 200),
            //    new PointD(140, 200),
            //    new PointD(120, 200),
            //    new PointD(100, 200),
            //    new PointD(100, 180),
            //    new PointD(100, 160),
            //    new PointD(100, 140),
            //    new PointD(100, 120) 
            //});
            //fourierX = result[0].ToList();
            //fourierY = result[1].ToList();
            //fourierX.Sort((a,b)=> b.amplitude.CompareTo(a.amplitude));
            //fourierY.Sort((a,b)=> b.amplitude.CompareTo(a.amplitude));
            init_picture();
            timer1.Interval = 1;
            timer1.Enabled = true;
        }
        void init_picture()
        {
            w = pictureBox1.Width;
            h = pictureBox1.Height;
            if (w > 0 && h > 0)
            {
                bit = new Bitmap(w, h);
                g = Graphics.FromImage(bit);
            }
            g.Clear(Color.White);
            pictureBox1.Image = bit;
        }
        double t = 0;
        List<rotatingVector> fourierX;
        List<rotatingVector> fourierY;
        double cos(double arg) { return Math.Cos(arg); }
        double sin(double arg) { return Math.Sin(arg); }
        double shiftX = 100, shiftY = 100;
        private void timer1_Tick(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            int n = fourierX.Count;//or fourierY.Count;
            if (n > 0)
            {
                circle[] cir1 = new circle[n];
                //float x = w/2, y=h/2;

                PointD[] lines = new PointD[(n + 1)];

                double x = 0, y = 0;
                double x1 = 0, y1 = 0, x2 = 0, y2 = 0;
                x = x1 = x2 = shiftX;
                y = y1 = y2 = shiftY;
                lines[0] = (new PointD(x1, y1));
                //lines[n+1] = (new PointD(x2, y2));
                for (int i = 0; i < n; i++)
                {
                    //float f = fourierX[i].frequency;// + fourierY[i].frequency;
                    //float R = fourierX[i].amplitude;// + fourierY[i].amplitude;


                    x1 += fourierX[i].amplitude * sin(fourierX[i].frequency * t + fourierX[i].phase + Math.PI / 2);
                    y1 += fourierX[i].amplitude * cos(fourierX[i].frequency * t + fourierX[i].phase + Math.PI / 2);
                    x2 += fourierY[i].amplitude * cos(fourierY[i].frequency * t + fourierY[i].phase + Math.PI / 2);
                    y2 += fourierY[i].amplitude * sin(fourierY[i].frequency * t + fourierY[i].phase + Math.PI / 2);
                    //x += fourierX[i].amplitude * cos(fourierX[i].frequency * t + fourierX[i].phase);//+(float)Math.PI/2);
                    //y += fourierY[i].amplitude * cos(fourierY[i].frequency * t + fourierY[i].phase);//+(float)Math.PI/2);



                    x += fourierX[i].amplitude * cos(fourierX[i].frequency * t + fourierX[i].phase);//+(float)Math.PI/2);
                    y += fourierY[i].amplitude * cos(fourierY[i].frequency * t + fourierY[i].phase);//+(float)Math.PI/2);
                    cir1[i] = new circle(x, y, (fourierX[i].amplitude + fourierY[i].amplitude) / 2);
                    //cir1[n + i] = new circle(x2, y2, fourierY[i].amplitude);
                    lines[i + 1] = (new PointD(x, y));
                    //lines[n+2 + i] = (new PointD(x2, y2));
                }
                PointD tmp = new PointD(x, y);
                track.Add(tmp);
                while (track.Count > 300) track.RemoveAt(0);
                t += Math.PI * 2 / n;//0.01f;//


                //for (int i = 0; i < n; i++)
                //{
                //    cir1[i].Draw(g, CircleCol);
                //}
                for (int i = 0; i < n; i++)
                {
                    if (i != n - 1 && i != n * 2 - 1)
                        cir1[i].Draw(g, Color.FromArgb(50, 50, 50, 50));
                }
                cir1[n - 1].Draw(g, Color.FromArgb(50, 0, 250, 0));
                //cir1[n*2-1].Draw(g, Color.FromArgb(50, 0, 250, 0));
                if (original.Count > 1)
                {
                    for (int i = 0; i < original.Count - 1; i++)
                    {
                        g.DrawLine(new Pen(Color.FromArgb(100, 255, 0, 0), 4), (float)original[i].X, (float)original[i].Y, (float)original[i + 1].X, (float)original[i + 1].Y);
                    }
                    //g.DrawLines(new Pen(Color.FromArgb(100,255,0,0),4), original.ToArray());
                    g.DrawLine(new Pen(Color.FromArgb(100, 255, 0, 0), 4), (float)original[0].X, (float)original[0].Y, (float)original[original.Count - 1].X, (float)original[original.Count - 1].Y);
                }

                if (track.Count > 1)
                {
                    for (int i = 0; i < track.Count - 1; i++)
                    {
                        g.DrawLine(new Pen(Color.Blue), (float)track[i].X, (float)track[i].Y, (float)track[i + 1].X, (float)track[i + 1].Y);
                    }
                    //g.DrawLine(new Pen(Color.Blue), (float)track[0].X, (float)track[0].Y, (float)track[track.Count-1].X, (float)track[track.Count-1].Y);
                    //g.DrawLines(new Pen(Color.Blue), track.ToArray());
                }

                for (int i = 1; i < lines.Length - 1; i++)
                {
                    g.DrawLine(new Pen(Color.Green), (float)lines[i].X, (float)lines[i].Y, (float)lines[i + 1].X, (float)lines[i + 1].Y);
                }
            }
            else 
            {
                if (original.Count > 1)
                {
                    for (int i = 0; i < original.Count - 1; i++)
                    {
                        g.DrawLine(new Pen(Color.FromArgb(100, 255, 0, 0), 4), (float)original[i].X, (float)original[i].Y, (float)original[i + 1].X, (float)original[i + 1].Y);
                    }
                    //g.DrawLines(new Pen(Color.FromArgb(100,255,0,0),4), original.ToArray());
                    g.DrawLine(new Pen(Color.FromArgb(100, 255, 0, 0), 4), (float)original[0].X, (float)original[0].Y, (float)original[original.Count - 1].X, (float)original[original.Count - 1].Y);
                }
            }
            //for (int i = lines.Length / 2; i < lines.Length-1; i++)
            //{
            //    g.DrawLine(new Pen(Color.Green), (float)lines[i].X, (float)lines[i].Y, (float)lines[i + 1].X, (float)lines[i + 1].Y);
            //}
            //g.DrawLines(new Pen(Color.Blue), lines.ToArray());

            //g.DrawLine(new Pen(Color.Blue), (float)x2, (float)y2, (float)x, (float)y);
            //g.DrawLine(new Pen(Color.Blue), (float)x1, (float)y1, (float)x, (float)y);
            //g.DrawLine(new Pen(Color.Blue),cir1[n-1].x+cir1[n-1].,,,);

            pictureBox1.Image = bit;

            //Step();
        }
        int msX, msY;

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            
            msX = e.X;
            msY = e.Y;
            if (is_ms_down)
            {
                PointD tmp = new PointD(msX, msY);
                original.Add(tmp);
                //circle tmp = new circle(msX, msY, 50);
                //circles.Add(tmp);
            }
            //Draw();
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            init_picture();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            original.Clear();
            is_ms_down = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            is_ms_down = false;
            //for(int i=0;i<original.Count;i++)
            //{
            //    original[i].X += shiftX;
            //    original[i].Y += shiftY;
            //}
            t = 0;
            var result = createSomethingCool(original.ToArray());
            fourierX = result[0].ToList();
            fourierY = result[1].ToList();
            fourierX.Sort((a, b) => b.amplitude.CompareTo(a.amplitude));
            fourierY.Sort((a, b) => b.amplitude.CompareTo(a.amplitude));
            //init_picture();
            timer1.Interval = 1;
            timer1.Enabled = true;
        }

        rotatingVector[][] createSomethingCool(PointD[]arr)
        {//generate arrows
            //fourierX = new List<float>();
            //fourierY = new List<float>();
            fourierX.Clear();
            fourierY.Clear();
            int N = arr.Length;
            rotatingVector[][] answer = new rotatingVector[2][];
            answer[0] = new rotatingVector[arr.Length];
            answer[1] = new rotatingVector[arr.Length];
            for (int k = 0; k < N; k++)
            {
                double sumX = 0, sumY = 0;
                for (int i = 0; i < N; i++)
                {
                    double tmp = 2 * Math.PI * k * i / N;
                    sumX += (arr[i].X-shiftX)*cos(tmp);
                    sumY += (arr[i].X-shiftX)*sin(tmp);
                }
                sumX /= N;
                sumY /= N;
                //answer[0][k] = new rotatingVector(k % 2 == 0 ? -k / 2 : 1 + k / 2, Math.Atan2(sumY, sumX), Math.Sqrt(sumX * sumX + sumY * sumY));
                answer[0][k] = new rotatingVector(k, Math.Atan2(sumY, sumX), Math.Sqrt(sumX * sumX + sumY * sumY));
                sumX = 0;
                sumY = 0;
                for (int i = 0; i < N; i++)
                {
                    double tmp = 2 * Math.PI * k * i / N;
                    sumX += (arr[i].Y-shiftY) * cos(tmp);
                    sumY += (arr[i].Y-shiftY) * sin(tmp);
                }
                sumX /= N;
                sumY /= N;
                //answer[1][k] = new rotatingVector(k % 2 == 0 ? -k / 2 : 1 + k / 2, Math.Atan2(sumY, sumX) , Math.Sqrt(sumX * sumX + sumY * sumY));
                answer[1][k] = new rotatingVector(k, Math.Atan2(sumY, sumX) , Math.Sqrt(sumX * sumX + sumY * sumY));
            }

            return answer;


            /*
            vectors = new List<rotatingVector>();

            vectors[0] = new rotatingVector(,,,);
            rotatingVector tmp = new rotatingVector();
            for (int i = 1; i < Number; i++)
            {
                tmp = vectors[i - 1];
                vectors[i] = new rotatingVector(tmp.X + tmp.R*, vectors[i - 1].Y, vectors[i - 1].R, );
            }
            */
        }

        void Step()
        {
            PointD tmp = new PointD(msX, msY);//
            //track.Enqueue(tmp);
            //while (track.Count > 50)
            //{
            //    track.Dequeue();
            //}
            
            
        }
        void Draw()
        {
            g.Clear(Color.White);

            //if (track.Count > 1)
            //    g.DrawLines(new Pen(TrackCol), track.ToArray());
            
            //if (original.Count > 1)
            //{
            //    g.DrawLines(new Pen(OrigCol), original.ToArray());
            //    g.DrawLine(new Pen(OrigCol), original[0], original[original.Count - 1]);
            //}

            //for (int i = 0; i < vectors.Count; i++)
            //{
            //    vectors[i].Draw(g, CircleCol);
            //}

            pictureBox1.Image = bit;
        }
    }
}
