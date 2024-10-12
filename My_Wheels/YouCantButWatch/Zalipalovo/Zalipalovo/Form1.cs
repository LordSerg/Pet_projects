using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zalipalovo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            init();
            this.pictureBox1.MouseWheel += PictureBox1_MouseWheel;
        }

       
        double angle;
        int h, w;
        double X, Y;
        Bitmap bit;
        Graphics g;
        Random R;
        int DiViDeBy_NuMbEr = 7;
        void init()
        {
            R = new Random();
            h = pictureBox1.Height;
            w = pictureBox1.Width;
            bit = new Bitmap(w,h);
            g = Graphics.FromImage(bit);
            g.Clear(Color.Black);
            pictureBox1.Image = bit;
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            X = convX(Cursor.Position.X-this.Location.X-10);
            Y = convY(Cursor.Position.Y-this.Location.Y-33);
            draw();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (mid_dist < 30)
                    mid_dist += 0.5d;
            }
            else
            {
                if (mid_dist >= 4d)
                    mid_dist -= 0.5d;
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            init();
        }

        double convX(int x) 
        {
            x -= w/ 2;
            return (double)x;
        }
        int reconvX(double x)
        {
            x += w / 2;
            return (int)x;
        }
        double convY(int y)
        {
            y -= h / 2;
            y *= -1;
            return (double)y;
        }
        int reconvY(double y)
        {
            y *= -1;
            y += h / 2;
            return (int)y;
        }
        struct vect
        {
            public double x1, y1, x2, y2;
            public double dx, dy;
            public vect(double x1, double y1, double x2, double y2)
            {
                this.x1 = x1;
                this.y1 = y1;
                this.x2 = x2;
                this.y2 = y2;
                dx = x2 - x1;
                dy = y2 - y1;
            }
            public vect(double dx, double dy)
            {
                x1 = 0;
                y1 = 0;
                x2 = dx;
                y2 = dy;
                this.dx = dx;
                this.dy = dy;
            }
            public static vect operator +(vect a, vect b)
            {
                return new vect(a.dx + b.dx, a.dy + b.dy);
            }
            //public static vect operator -(vect a, vect b)
            //{
            //    return new vect(a.dx - b.dx, a.dy - b.dy);
            //}
        }
        struct line
        {
            public double A, B, C;
            public line(double x1, double y1, double x2, double y2)
            {
                A = (y2 - y1);
                B = (x1 - x2);
                C = ((x2 - x1) * y1) + ((y1 - y2) * x1);
            }
            public double findX(double y) { return (-B * y - C) / A; }
            public double findY(double x) { return (-A * x - C) / B; }
            public double findAngle()
            {
                if (A < 0 && B < 0 || A < 0 && B > 0)
                    return Math.Atan(-A / B) + 180;
                else if (A > 0 && B > 0|| A > 0 && B < 0)
                    return Math.Atan(-A / B);
                //else if (B == 0&&A<0) return 90;
                //else if (B == 0&&A>0) return 180;
                //else if (A == 0&&B>0) return 90;
                //else if (A == 0&&B<0) return 270;
                else return -1000;    

            }
        }
        double sin(double a) { return Math.Sin(a * Math.PI / 180);  }
        double cos(double a) { return Math.Cos(a * Math.PI / 180);  }
        bool is_fit(int somex, int somey) { return somex > 0 && somex < w && somey > 0 && somey < h; }
        Bitmap bit1;// = new Bitmap(bit);

        private void divideBy2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiViDeBy_NuMbEr = 2;
        }

        private void divideBy3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiViDeBy_NuMbEr = 3;
        }
        double mid_dist = 7;
        public void drawLine()
        {
            double tx, ty, p1x, p2x, p1y, p2y;
            double dist = mid_dist + (double)R.Next(-30, 30) / 10;
            tx = X + cos(angle);
            ty = Y + sin(angle);
            line defline = new line(X, Y, tx, ty);

            //g.Clear(Color.FromArgb(R.Next(50, 100), R.Next(100, 150), R.Next(50, 150)));
            double dx1 = dist * cos(angle - 90),
                   dy1 = dist * sin(angle - 90), 
                   dx2 = dist * cos(angle + 90),
                   dy2 = dist * sin(angle + 90);
            //Region region = new Region();
            //Region.
            //g.FillRegion
            
            g.Clear(Color.Black);
            //Bitmap bit2 = new Bitmap(bit);
            //
            //Graphics g1 = Graphics.FromImage(bit1);
            //Graphics g2 = Graphics.FromImage(bit2);

            //variant1:
            for (int i = -w/2; i < w/2; i++)
            {
                for (int j = h/2; j > -h/2; j--)
                {
                    if (Math.Abs(defline.A*i+defline.B*j+defline.C)>dist)
                    {
                        if (defline.A != 0)
                        {
                            if (defline.findX(j) >= i)
                            {
                                if (is_fit(reconvX(i + dx1), reconvY(j + dy1)))
                                    bit.SetPixel(reconvX(i), reconvY(j), bit1.GetPixel(reconvX(i + dx1), reconvY(j + dy1)));
                            }
                            else //if(defline.findX(j) < i)
                            {
                                if (is_fit(reconvX(i + dx2), reconvY(j + dy2)))
                                    bit.SetPixel(reconvX(i), reconvY(j), bit1.GetPixel(reconvX(i + dx2), reconvY(j + dy2)));
                            }
                        }
                        else
                        {
                            if (defline.findY(i) <= j)
                            {
                                if (is_fit(reconvX(i + dx1), reconvY(j + dy1)))
                                    bit.SetPixel(reconvX(i), reconvY(j), bit1.GetPixel(reconvX(i + dx1), reconvY(j + dy1)));
                            }
                            else //if (defline.findY(i) > j)
                            {
                                if (is_fit(reconvX(i + dx2), reconvY(j + dy2)))
                                    bit.SetPixel(reconvX(i), reconvY(j), bit1.GetPixel(reconvX(i + dx2), reconvY(j + dy2)));
                            }
                        }
                    }
                }
            }
            line k = new line(X +dx1, Y + dy1, tx + dx1, ty + dy1);
            if (k.A != 0)
            {
                p1x = k.findX(h  );
                p1y = (double)h  ;
                p2x = k.findX(-h );
                p2y = (double)-h ;
            }
            else //if (k.B != 0)
            {
                p1x = (double)w  ;
                p1y = k.findY(w  );
                p2x = (double)-w ;
                p2y = k.findY(-w );
            }
            
            g.DrawLine(new Pen(Color.White), reconvX(p1x), reconvY(p1y), reconvX(p2x), reconvY(p2y));
            k = new line(X + dx2, Y + dy2, tx + dx2, ty + dy2);
            if (k.A != 0)
            {
                p1x = k.findX(h  );
                p1y = (double)h  ;
                p2x = k.findX(-h );
                p2y = (double)-h ;
            }
            else //if (k.B != 0)
            {
                p1x = (double)w  ;
                p1y = k.findY(w  );
                p2x = (double)-w ;
                p2y = k.findY(-w );
            }
            g.DrawLine(new Pen(Color.White), reconvX(p1x), reconvY(p1y), reconvX(p2x), reconvY(p2y));
            //this.BackgroundImage = bit;
            pictureBox1.Image = bit;
        }

        public void drawLines()
        {
            double tx, ty, p1x, p2x, p1y, p2y;
            double dist = 5;// (double)R.Next(50, 100) / 10;
            tx = X + cos(angle);
            ty = Y + sin(angle);
            line defline = new line(X, Y, tx, ty);
            List<line> deflines = new List<line>();
            List<vect> defvectsOrto = new List<vect>();
            List<vect> defvects = new List<vect>();
            g.Clear(Color.Black);
            for(int i=0;i<DiViDeBy_NuMbEr;i++)
            {
                deflines.Add(new line(X, Y, X + cos(angle + i * (360 / DiViDeBy_NuMbEr)), Y + sin(angle + i * (360 / DiViDeBy_NuMbEr))));
                defvects.Add(new vect(X, Y, X + cos(angle + i * (360 / DiViDeBy_NuMbEr)), Y + sin(angle + i * (360 / DiViDeBy_NuMbEr))));
                defvectsOrto.Add(new vect(X, Y, X + cos(angle + 90 + i * (360 / DiViDeBy_NuMbEr)), Y + sin(angle + 90 + i * (360 / DiViDeBy_NuMbEr))));
                //g.DrawLine(new Pen(Color.White,3), reconvX(X), reconvY(Y), reconvX(X + 20*cos(angle + i * (360 / DiViDeBy_NuMbEr))), reconvY(Y + 20 * sin(angle + i * (360 / DiViDeBy_NuMbEr))));
            }
            double dx1 = dist * cos(angle - 90),
                   dy1 = dist * sin(angle - 90),
                   dx2 = dist * cos(angle + 90),
                   dy2 = dist * sin(angle + 90);
            //for (int i = -w / 2; i < w / 2; i++)
            //{
            //    for (int j = h / 2; j > -h / 2; j--)
            //    {
            //        if (Math.Abs(defline.A * i + defline.B * j + defline.C) > dist)
            //        {
            //            if (defline.A != 0)
            //            {
            //                if (defline.findX(j) >= i)
            //                {
            //                    if (is_fit(reconvX(i + dx1), reconvY(j + dy1)))
            //                        bit.SetPixel(reconvX(i), reconvY(j), bit1.GetPixel(reconvX(i + dx1), reconvY(j + dy1)));
            //                }
            //                else //if(defline.findX(j) < i)
            //                {
            //                    if (is_fit(reconvX(i + dx2), reconvY(j + dy2)))
            //                        bit.SetPixel(reconvX(i), reconvY(j), bit1.GetPixel(reconvX(i + dx2), reconvY(j + dy2)));
            //                }
            //            }
            //            else
            //            {
            //                if (defline.findY(i) <= j)
            //                {
            //                    if (is_fit(reconvX(i + dx1), reconvY(j + dy1)))
            //                        bit.SetPixel(reconvX(i), reconvY(j), bit1.GetPixel(reconvX(i + dx1), reconvY(j + dy1)));
            //                }
            //                else //if (defline.findY(i) > j)
            //                {
            //                    if (is_fit(reconvX(i + dx2), reconvY(j + dy2)))
            //                        bit.SetPixel(reconvX(i), reconvY(j), bit1.GetPixel(reconvX(i + dx2), reconvY(j + dy2)));
            //                }
            //            }
            //        }
            //    }
            //}

            //for(int i=0;i< DiViDeBy_NuMbEr;i++)
            //{ 
            //}

            double someK = Math.Sqrt(DiViDeBy_NuMbEr - 2);
            for (int i = 0; i < DiViDeBy_NuMbEr; i++)
            {
                line k = deflines[i];
                double ang = k.findAngle();
                if (defvects[i].dx == 0&& defvects[i].dy>0)
                {//90
                    p1x = k.findX(h);
                    p1y = (double)h;
                    //p1x = (double)w;
                    //p1y = k.findY(w);
                    //g.DrawLine(new Pen(Color.Green), reconvX(p1x), reconvY(p1y), reconvX(X), reconvY(Y));
                    g.DrawLine(new Pen(Color.White), reconvX(p1x + dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(p1y + dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy),
                        reconvX(X + dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(Y + dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy));
                    g.DrawLine(new Pen(Color.White), reconvX(p1x - dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(p1y - dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy),
                        reconvX(X - dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(Y - dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy));
                }
                else if (defvects[i].dx == 0&& defvects[i].dy<0)
                {//270
                    p1x = k.findX(-h);
                    p1y = (double)-h;
                    //p1x = (double)w;
                    //p1y = k.findY(w);
                    //g.DrawLine(new Pen(Color.Green), reconvX(p1x), reconvY(p1y), reconvX(X), reconvY(Y));
                    g.DrawLine(new Pen(Color.White), reconvX(p1x + dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(p1y + dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy),
                        reconvX(X + dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(Y + dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy));
                    g.DrawLine(new Pen(Color.White), reconvX(p1x - dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(p1y - dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy),
                        reconvX(X - dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(Y - dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy));
                }
                else if (defvects[i].dy == 0&& defvects[i].dx>0)
                {//0
                    p1x = (double)w;
                    p1y = k.findY(w);
                    //p1x = k.findX(-h);
                    //p1y = (double)-h;
                    //p2x =;
                    //p2y =;
                    //g.DrawLine(new Pen(Color.Red), reconvX(p1x), reconvY(p1y), reconvX(X), reconvY(Y));
                    g.DrawLine(new Pen(Color.White), reconvX(p1x + dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(p1y + dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy),
                        reconvX(X + dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(Y + dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy));
                    g.DrawLine(new Pen(Color.White), reconvX(p1x - dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(p1y - dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy),
                        reconvX(X - dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(Y - dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy));
                }
                else if (defvects[i].dy == 0&& defvects[i].dx<0)
                {//180
                    p1x = (double)-w;
                    p1y = k.findY(-w);
                    //p1x = k.findX(-h);
                    //p1y = (double)-h;
                    //p2x =;
                    //p2y =;
                    //g.DrawLine(new Pen(Color.Red), reconvX(p1x), reconvY(p1y), reconvX(X), reconvY(Y));
                    g.DrawLine(new Pen(Color.White), reconvX(p1x + dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(p1y + dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy),
                        reconvX(X + dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(Y + dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy));
                    g.DrawLine(new Pen(Color.White), reconvX(p1x - dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(p1y - dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy),
                        reconvX(X - dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(Y - dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy));
                }
                else if ((ang + 360) % 360 > 0 && (ang + 360) % 360 < 90)
                {
                    p1x = (double)w;
                    p1y = k.findY(w);
                    //p1x = k.findX(-h);
                    //p1y = (double)-h;
                    //p2x =;
                    //p2y =;
                    //g.DrawLine(new Pen(Color.Red), reconvX(p1x), reconvY(p1y), reconvX(X), reconvY(Y));
                    g.DrawLine(new Pen(Color.White), reconvX(p1x + dist * defvectsOrto[i].dx + dist* someK * defvects[i].dx), reconvY(p1y + dist * defvectsOrto[i].dy + dist* someK * defvects[i].dy),
                        reconvX(X + dist * defvectsOrto[i].dx + dist* someK * defvects[i].dx), reconvY(Y + dist * defvectsOrto[i].dy + dist* someK * defvects[i].dy));
                    g.DrawLine(new Pen(Color.White), reconvX(p1x - dist * defvectsOrto[i].dx + dist* someK * defvects[i].dx), reconvY(p1y - dist * defvectsOrto[i].dy + dist* someK * defvects[i].dy),
                        reconvX(X - dist * defvectsOrto[i].dx + dist* someK * defvects[i].dx), reconvY(Y - dist * defvectsOrto[i].dy + dist* someK * defvects[i].dy));
                }
                else if ((ang + 360) % 360 > 90 && (ang + 360) % 360 < 180)
                {
                    p1x = k.findX(-h);
                    p1y = (double)-h;
                    //p1x = (double)w;
                    //p1y = k.findY(w);
                    //g.DrawLine(new Pen(Color.Green), reconvX(p1x), reconvY(p1y), reconvX(X), reconvY(Y));
                    g.DrawLine(new Pen(Color.White), reconvX(p1x + dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(p1y + dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy),
                        reconvX(X + dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(Y + dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy));
                    g.DrawLine(new Pen(Color.White), reconvX(p1x - dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(p1y - dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy),
                        reconvX(X - dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(Y - dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy));
                }
                else if ((ang + 360) % 360 > 180 && (ang + 360) % 360 < 270)
                {
                    p1x = (double)-w;
                    p1y = k.findY(-w);
                    //p1x = k.findX(h);
                    //p1y = (double)h;
                    //g.DrawLine(new Pen(Color.Blue), reconvX(p1x), reconvY(p1y), reconvX(X), reconvY(Y));
                    g.DrawLine(new Pen(Color.White), reconvX(p1x + dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(p1y + dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy),
                        reconvX(X + dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(Y + dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy));
                    g.DrawLine(new Pen(Color.White), reconvX(p1x - dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(p1y - dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy),
                        reconvX(X - dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(Y - dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy));
                }
                else if ((ang + 360) % 360 > 270 && (ang + 360) % 360 < 360)
                {
                    p1x = k.findX(h);
                    p1y = (double)h;
                    //p1x = (double)-w;
                    //p1y = k.findY(-w);
                    //g.DrawLine(new Pen(Color.White), reconvX(p1x), reconvY(p1y), reconvX(X), reconvY(Y));
                    g.DrawLine(new Pen(Color.White), reconvX(p1x + dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(p1y + dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy),
                        reconvX(X + dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(Y + dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy));
                    g.DrawLine(new Pen(Color.White), reconvX(p1x - dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(p1y - dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy),
                        reconvX(X - dist * defvectsOrto[i].dx + dist * someK * defvects[i].dx), reconvY(Y - dist * defvectsOrto[i].dy + dist * someK * defvects[i].dy));
                }
            }
            //line k = new line(X + dx1, Y + dy1, tx + dx1, ty + dy1);
            //if (k.A != 0)
            //{
            //    p1x = k.findX(h);
            //    p1y = (double)h;
            //    p2x = k.findX(-h);
            //    p2y = (double)-h;
            //}
            //else //if (k.B != 0)
            //{
            //    p1x = (double)w;
            //    p1y = k.findY(w);
            //    p2x = (double)-w;
            //    p2y = k.findY(-w);
            //}

            //g.DrawLine(new Pen(Color.White), reconvX(p1x), reconvY(p1y), reconvX(p2x), reconvY(p2y));
            //k = new line(X + dx2, Y + dy2, tx + dx2, ty + dy2);
            //if (k.A != 0)
            //{
            //    p1x = k.findX(h);
            //    p1y = (double)h;
            //    p2x = k.findX(-h);
            //    p2y = (double)-h;
            //}
            //else //if (k.B != 0)
            //{
            //    p1x = (double)w;
            //    p1y = k.findY(w);
            //    p2x = (double)-w;
            //    p2y = k.findY(-w);
            //}
            //g.DrawLine(new Pen(Color.White), reconvX(p1x), reconvY(p1y), reconvX(p2x), reconvY(p2y));
            pictureBox1.Image = bit;
        }
        void draw()
        {
            angle = (double)R.Next(1800)/10;
            //angle = 0;
            //angle = R.Next(4)*45;
            bit1 = new Bitmap(bit);
            //Task task = Task.Run(()=>drawLine());
            drawLine();
            //pictureBox1.Image = bit;
        }
    }
}
