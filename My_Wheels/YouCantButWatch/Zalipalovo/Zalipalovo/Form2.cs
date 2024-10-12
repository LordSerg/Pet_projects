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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            
            init();
        }
        double speed = 0, angle = 0;
        int depth = 1;
        Bitmap bit;
        Graphics g;
        int size = 1;
        bool b1, b2;
        void init()
        {
            DoubleBuffered = true;
            bit = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bit);
            g.Clear(Color.FromArgb(128, 128, 128));
            pictureBox1.Image = bit;
            if (bit.Width > bit.Height)
                size = bit.Height;
            else
                size = bit.Width;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.GammaCorrected;
            //g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            speed = 1*trackBar1.Value;
            label1.Text = "Скорость: " + speed;
            trackBar2.Visible = true;
            label2.Visible = true;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            depth = trackBar2.Value;
            label2.Text = "Вложение: " + depth;
            trackBar3.Visible = true;
            label3.Visible = true;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            angleMultiplier = (double)trackBar3.Value/10.0;
            label3.Text = "Угловой множитель: " + angleMultiplier;
            checkBox1.Visible = true;
            checkBox2.Visible = true;
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Width > 0 && pictureBox1.Height > 0)
            {
                init();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (speed != 0)
            {
                b1 = checkBox1.Checked;
                b2 = checkBox2.Checked;
                g.Clear(Color.FromArgb(128, 128, 128));
                if (b1 && b2)
                {
                    g.FillPie(new SolidBrush(Color.Black), 0, 0, size, size, (float)angle, 180);
                    g.FillPie(new SolidBrush(Color.White), 0, 0, size, size, (float)angle + 180, 180);
                    f(depth, size / 2.0, size / 2.0, angle, size);
                    f(depth, size / 2.0, size / 2.0, angle + 180, size);
                }
                else if (b1)
                {
                    g.FillEllipse(new SolidBrush(Color.Black), 0, 0, size, size);
                    f(depth, size / 2.0, size / 2.0, angle, size);
                    f(depth, size / 2.0, size / 2.0, angle + 180, size);
                }
                else if (b2)
                {
                    g.FillEllipse(new SolidBrush(Color.White), 0, 0, size, size);
                    f(depth, size / 2.0, size / 2.0, angle, size);
                    f(depth, size / 2.0, size / 2.0, angle + 180, size);
                }
                else
                {
                    g.FillPie(new SolidBrush(Color.Black), 0, 0, size, size, (float)angle, 180);
                    g.FillPie(new SolidBrush(Color.White), 0, 0, size, size, (float)angle + 180, 180);
                    double ellipseSize = (double)size / 2.0;
                    PointF p1 = new PointF((float)(Math.Cos(angle * Math.PI / 180) * ellipseSize / 2 + ellipseSize / 2),
                                           (float)(Math.Sin(angle * Math.PI / 180) * ellipseSize / 2 + ellipseSize / 2));
                    g.FillEllipse(new SolidBrush(Color.White), p1.X, p1.Y, (float)ellipseSize, (float)ellipseSize);
                    PointF p2 = new PointF((float)(Math.Cos((angle + 180) * Math.PI / 180) * ellipseSize / 2 + ellipseSize / 2),
                                           (float)(Math.Sin((angle + 180) * Math.PI / 180) * ellipseSize / 2 + ellipseSize / 2));
                    g.FillEllipse(new SolidBrush(Color.Black), p2.X, p2.Y, (float)ellipseSize, (float)ellipseSize);
                    f(depth, size / 2.0, size / 2.0, (360-angle)/(-angleMultiplier), size);
                    f(depth, size / 2.0, size / 2.0, (360-angle + 180)/(-angleMultiplier), size);
                }
                
                /*
                g.FillPie(new SolidBrush(Color.Black), 0, 0, size, size, angle, 180);
                g.FillPie(new SolidBrush(Color.White), 0, 0, size, size, angle + 180, 180);
                double ellipseSize = (double)size / 2.0;
                PointF p1 = new PointF((double)(Math.Cos(angle * Math.PI / 180) * ellipseSize / 2 + ellipseSize / 2),
                                       (double)(Math.Sin(angle * Math.PI / 180) * ellipseSize / 2 + ellipseSize / 2));
                g.FillEllipse(new SolidBrush(Color.White),p1.X,p1.Y,ellipseSize, ellipseSize);
                PointF p2 = new PointF((double)(Math.Cos((angle + 180) * Math.PI / 180) * ellipseSize / 2 + ellipseSize / 2),
                                       (double)(Math.Sin((angle + 180) * Math.PI / 180) * ellipseSize / 2 + ellipseSize / 2));
                g.FillEllipse(new SolidBrush(Color.Black), p2.X, p2.Y, ellipseSize, ellipseSize);
                f(depth, (p1.X + ellipseSize / 2), (p1.Y + ellipseSize / 2), angle, ellipseSize);
                f(depth, (p2.X + ellipseSize / 2), (p2.Y + ellipseSize / 2), angle+180, ellipseSize);
                */
                angle += speed;
                if (angle > 360) angle -= 360;
                if (angle < -360) angle += 360;
                pictureBox1.Image = bit;
            }
        }

        double angleMultiplier = 1;
        void f(int d, double parentCenterX, double parentCenterY, double parentAngle, double parentSize)
        {
            double sz = parentSize / 2.0;
            double ang = (360 - parentAngle)* angleMultiplier;
            PointF p1 = new PointF((float)(Math.Cos(ang * Math.PI / 180) * sz / 2 + parentCenterX - sz / 2.0),
                                   (float)(Math.Sin(ang * Math.PI / 180) * sz / 2 + parentCenterY - sz / 2.0));
            PointF p2 = new PointF((float)(Math.Cos((ang + 180) * Math.PI / 180) * sz / 2 + parentCenterX - sz / 2.0),
                                   (float)(Math.Sin((ang + 180) * Math.PI / 180) * sz / 2 + parentCenterY - sz / 2.0));
            if (b1 && b2)
            {
                g.FillPie(new SolidBrush(Color.Black), (float)((float)(parentCenterX - parentSize / 2.0)), (float)(parentCenterY - parentSize / 2.0), (float)parentSize, (float)parentSize, (float)ang, 180.0f);
                g.FillPie(new SolidBrush(Color.White), (float)((float)(parentCenterX - parentSize / 2.0)), (float)(parentCenterY - parentSize / 2.0), (float)parentSize, (float)parentSize, (float)(ang + 180), 180.0f);
                g.FillEllipse(new SolidBrush(Color.White), p1.X, p1.Y, (float)sz, (float)sz);
                g.FillEllipse(new SolidBrush(Color.Black), p2.X, p2.Y, (float)sz, (float)sz);
            }
            else
            {
                if (b1)
                {
                    if (depth % 2 == 0)
                    {
                        if (d % 2 == 0) g.FillPie(new SolidBrush(Color.Black), (float)(parentCenterX - parentSize / 2.0), (float)(parentCenterY - parentSize / 2.0), (float)parentSize, (float)parentSize, (float)ang, 180);
                        else g.FillPie(new SolidBrush(Color.White), (float)(parentCenterX - parentSize / 2.0), (float)(parentCenterY - parentSize / 2.0), (float)parentSize, (float)parentSize, (float)ang, 180);
                    }
                    else
                    {
                        if (d % 2 == 0) g.FillPie(new SolidBrush(Color.White), (float)(parentCenterX - parentSize / 2.0), (float)(parentCenterY - parentSize / 2.0), (float)parentSize, (float)parentSize, (float)ang, 180);
                        else g.FillPie(new SolidBrush(Color.Black), (float)(parentCenterX - parentSize / 2.0), (float)(parentCenterY - parentSize / 2.0), (float)parentSize, (float)parentSize, (float)ang, 180);
                    }
                    g.FillEllipse(new SolidBrush(Color.Black), p1.X, p1.Y, (float)sz, (float)sz);
                    g.FillEllipse(new SolidBrush(Color.Black), p2.X, p2.Y, (float)sz, (float)sz);
                }
                if (b2)
                {
                    if (depth % 2 == 0)
                    {
                        if (d % 2 == 0) g.FillPie(new SolidBrush(Color.White), (float)(parentCenterX - parentSize / 2.0), (float)(parentCenterY - parentSize / 2.0), (float)parentSize, (float)parentSize, (float)(ang + 180), 180);
                        else g.FillPie(new SolidBrush(Color.Black), (float)(parentCenterX - parentSize / 2.0), (float)(parentCenterY - parentSize / 2.0), (float)parentSize, (float)parentSize, (float)(ang + 180), 180);
                    }
                    else
                    {
                        if (d % 2 == 0) g.FillPie(new SolidBrush(Color.Black), (float)(parentCenterX - parentSize / 2.0), (float)(parentCenterY - parentSize / 2.0), (float)parentSize, (float)parentSize, (float)(ang + 180), 180);
                        else g.FillPie(new SolidBrush(Color.White), (float)(parentCenterX - parentSize / 2.0), (float)(parentCenterY - parentSize / 2.0), (float)parentSize, (float)parentSize, (float)(ang + 180), 180);
                    }
                    g.FillEllipse(new SolidBrush(Color.White), p1.X, p1.Y, (float)sz, (float)sz);
                    g.FillEllipse(new SolidBrush(Color.White), p2.X, p2.Y, (float)sz, (float)sz);
                }
            }
            if (d-1 > 0)
            {
                f(d - 1, (p2.X + sz / 2), (p2.Y + sz / 2), ang+180, sz);
                f(d - 1, (p1.X + sz / 2), (p1.Y + sz / 2), ang, sz);
            }
            else
            {
                if ((b1 && b2) || (!b1 && !b2))
                {
                    g.FillEllipse(new SolidBrush(Color.Black), p1.X + (float)sz / 4, p1.Y + (float)sz / 4, (float)sz / 2, (float)sz / 2);
                    g.FillEllipse(new SolidBrush(Color.White), p2.X + (float)sz / 4, p2.Y + (float)sz / 4, (float)sz / 2, (float)sz / 2);
                }
                else if (b1)
                {
                    g.FillEllipse(new SolidBrush(Color.White), p1.X + (float)sz / 4, p1.Y + (float)sz / 4, (float)sz / 2, (float)sz / 2);
                    g.FillEllipse(new SolidBrush(Color.White), p2.X + (float)sz / 4, p2.Y + (float)sz / 4, (float)sz / 2, (float)sz / 2);
                }
                else if (b2)
                {
                    g.FillEllipse(new SolidBrush(Color.Black), p1.X + (float)sz / 4, p1.Y + (float)sz / 4, (float)sz / 2, (float)sz / 2);
                    g.FillEllipse(new SolidBrush(Color.Black), p2.X + (float)sz / 4, p2.Y + (float)sz / 4, (float)sz / 2, (float)sz / 2);
                }
            }
        }

    }
}
