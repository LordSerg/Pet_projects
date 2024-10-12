using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zalipalovo
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            init();
        }
        Bitmap bit;
        Graphics g;
        GraphicsPath[,] gp_b,gp_w;
        Matrix mtr_moveF, mtr_rotR, mtr_rotL, mtr_moveB;
        Rectangle r;
        int squaresizi = 100;
        //int size;
        int speed = 1;
        //int num;
        int w, h;
        static Color c1 = Color.Red, c2 = Color.Black;
        SolidBrush C1 = new SolidBrush(c1);
        SolidBrush C2 = new SolidBrush(c2);
        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if(pictureBox1.Width>0&&pictureBox1.Height>0)
            {
                init();
            }
        }
        int []speeds = { 1, 2, 3, 5, 6, 9, 10, 15, 18, 30, 45 };
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            speed = speeds[trackBar2.Value];
            label3.Text = "Скорость: " + speed;
            init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            c1 = colorDialog1.Color;
            initColors();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            c2 = colorDialog1.Color;
            initColors();
        }

        void initColors()
        {

            pictureBox2.BackColor = c1;
            pictureBox3.BackColor = c2;
            C1 = new SolidBrush(c1);
            C2 = new SolidBrush(c2);
        }
        void init()
        {
            initColors();
            DoubleBuffered = true;
            w = pictureBox1.Width;
            h = pictureBox1.Height;
            //if (w > h)
            //    size = h;
            //else
            //    size = w;
            bit = new Bitmap(w, h);
            g = Graphics.FromImage(bit);
            g.Clear(c1);
            pictureBox1.Image = bit;
            counter = 1;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //num = 2;

            //g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.GammaCorrected;
            //g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            comboBox1.Items.Clear();
            linkLabel1.Text = "Шо эта такое?";
            //https://easings.net/
            comboBox1.Items.Add("easeInSine");
            comboBox1.Items.Add("easeInCubic");
            comboBox1.Items.Add("easeInQuint");
            comboBox1.Items.Add("easeInCirc");
            comboBox1.Items.Add("easeInElastic");
            comboBox1.Items.Add("easeInQuad");
            comboBox1.Items.Add("easeInQuart");
            comboBox1.Items.Add("easeInExpo");
            comboBox1.Items.Add("easeInBack");
            comboBox1.Items.Add("easeInBounce");

            int NumOfTimes_w = 1 + w / squaresizi;
            int NumOfTimes_h = 1 + h / squaresizi;
            gp_b = new GraphicsPath[NumOfTimes_w, NumOfTimes_h];
            gp_w = new GraphicsPath[NumOfTimes_w, NumOfTimes_h];
            
            for (int i = 0; i < NumOfTimes_w; i++)
            {
                for (int j = i%2; j < NumOfTimes_h; j+=2)
                {
                    r = new Rectangle(i * squaresizi, j * squaresizi, squaresizi, squaresizi);
                    gp_b[i, j] = new GraphicsPath();
                    gp_b[i, j].AddRectangle(r);
                }
            }
            for (int i = 0; i < NumOfTimes_w; i++)
            {
                for (int j = (i+1)%2; j < NumOfTimes_h; j+=2)
                {
                    r = new Rectangle(i * squaresizi, j * squaresizi, squaresizi, squaresizi);
                    gp_w[i, j] = new GraphicsPath();
                    gp_w[i, j].AddRectangle(r);
                }
            }

            float cos = (float)Math.Cos(speed*Math.PI / 180);
            float sin = (float)Math.Sin(speed*Math.PI / 180);

            mtr_moveF = new Matrix(1,0,0,1, -squaresizi/2, -squaresizi/2);
            mtr_rotR = new Matrix(cos, sin, -sin, cos, 0, 0);
            mtr_rotL = new Matrix(cos, -sin, sin, cos, 0, 0);
            mtr_moveB = new Matrix(1,0,0,1, squaresizi/2, squaresizi/2);

            timer1.Interval = 1;
            timer1.Enabled = true;
        }
        int counter = 1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            int NumOfTimes_w = 1 + w / squaresizi;
            int NumOfTimes_h = 1 + h / squaresizi;
            if (counter <= 90/speed)
            {
                g.Clear(c1);
                for (int i = 0; i < NumOfTimes_w; i++)
                {
                    for (int j = i % 2; j < NumOfTimes_h; j += 2)
                    {

                        mtr_moveF = new Matrix(1, 0, 0, 1, -i*squaresizi -squaresizi / 2, -j * squaresizi - squaresizi / 2);
                        mtr_moveB = new Matrix(1, 0, 0, 1, i * squaresizi+squaresizi / 2, j * squaresizi+squaresizi / 2);
                        gp_b[i,j].Transform(mtr_moveF);
                        gp_b[i,j].Transform(mtr_rotR);
                        gp_b[i,j].Transform(mtr_moveB);
                        g.FillPath(C2, gp_b[i,j]);
                    }
                }
            }
            else if (counter <= 180/speed)
            {
                g.Clear(c2);
                for (int i = 0; i < NumOfTimes_w; i++)
                {
                    for (int j = (i + 1) % 2; j < NumOfTimes_h; j += 2)
                    {
                        mtr_moveF = new Matrix(1, 0, 0, 1, -i * squaresizi - squaresizi / 2, -j * squaresizi - squaresizi / 2);
                        mtr_moveB = new Matrix(1, 0, 0, 1, i * squaresizi + squaresizi / 2, j * squaresizi + squaresizi / 2);
                        gp_w[i, j].Transform(mtr_moveF);
                        gp_w[i, j].Transform(mtr_rotR);
                        gp_w[i, j].Transform(mtr_moveB);
                        g.FillPath(C1, gp_w[i, j]);
                    }
                }
            }
            else
                counter = 0;
            pictureBox1.Image = bit;
            counter++;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            squaresizi = 10*trackBar1.Value;
            label1.Text = "Размер: " + squaresizi;
            init();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("https://easings.net/");
        }
    }
}
