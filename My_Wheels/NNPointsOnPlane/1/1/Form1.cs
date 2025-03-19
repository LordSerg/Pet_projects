using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Brain x;
        Bitmap bit0, bit1, bit3, bit4, bit5, bit6, bit7,bitu,bitInput;
        Graphics g0, g1, g3, g4, g5, g6, g7,gu,gInput;

        private void label8_Click(object sender, EventArgs e)
        {

        }

        ball[] balls;
        int BallIndex = -1;
        //Net[] n;
        //Synapse[] s;
        //int sets = 1;
        //double Net_answer;
        //int real_answer;
        //double study_speed = 0.5, moment = 0.8, error;
        //double squed_sum_of_errors = 0;
        struct ball
        {
            public int x, y;
            public Color C;
            public ball(int _x,int _y,Color col)
            {
                x = _x;
                y = _y;
                C = col;
            }
        }
        class Net
        {
            public double OUT, IN, DELTA;
            public Net()
            {

            }
            public void culc()
            {//устаканиваем значения по сигмоиду
                OUT = 1 / (1 + Math.Pow(Math.E, -IN));
            }

        }
        class Synapse
        {
            public double Weight, GRAD, change = 0;
            public Synapse()
            {
                //Random r = new Random();
                //Weight = r.Next(-5, 6);
            }
            public void culc_gr(double delta_b, double out_a)
            {
                GRAD = delta_b * out_a;
            }
            public void culc_ch(double a, double b)
            {
                change = a * GRAD + change * b;
                Weight += change;
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            balls = new ball[10000];
            //x = new Brain();
            NetU.Activate(2,4);
            Net7.Activate();
            Net6.Activate();
            Net5.Activate();
            Net4.Activate();
            Net3.Activate();
            Net1.Activate();
            Net0.Activate();
            bit0 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bit1 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            bit3 = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            bit4 = new Bitmap(pictureBox4.Width, pictureBox4.Height);
            bit5 = new Bitmap(pictureBox5.Width, pictureBox5.Height);
            bit6 = new Bitmap(pictureBox6.Width, pictureBox6.Height);
            bit7 = new Bitmap(pictureBox7.Width, pictureBox7.Height);
            bitu = new Bitmap(pictureBox8.Width, pictureBox8.Height);
            bitInput = new Bitmap(pictureBox9.Width, pictureBox9.Height);
            g0 = Graphics.FromImage(bit0);
            g1 = Graphics.FromImage(bit1);
            g3 = Graphics.FromImage(bit3);
            g4 = Graphics.FromImage(bit4);
            g5 = Graphics.FromImage(bit5);
            g6 = Graphics.FromImage(bit6);
            g7 = Graphics.FromImage(bit7);
            gu = Graphics.FromImage(bitu);
            gInput = Graphics.FromImage(bitInput);
            //gInput;
            timer1.Interval = 1;
            timer1.Enabled = true;
            //g0 = Graphics.FromImage(bit0);
            //g0.Clear(Color.White);
        }
        int blacks=0, whites=0;
        /*
        BallIndex++;
            if (e.Button == MouseButtons.Right)
                balls[BallIndex] = new ball(e.X, e.Y, Color.Black);
            else
                balls[BallIndex] = new ball(e.X, e.Y, Color.White);
        g0.FillEllipse(new SolidBrush(balls[BallIndex].C), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
            g1.FillEllipse(new SolidBrush(balls[BallIndex].C), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
            g3.FillEllipse(new SolidBrush(balls[BallIndex].C), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
            g4.FillEllipse(new SolidBrush(balls[BallIndex].C), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
            g5.FillEllipse(new SolidBrush(balls[BallIndex].C), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
            g6.FillEllipse(new SolidBrush(balls[BallIndex].C), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
            g7.FillEllipse(new SolidBrush(balls[BallIndex].C), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
            gu.FillEllipse(new SolidBrush(balls[BallIndex].C), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
            //label3.Text = (((double)balls[BallIndex].x / (double)pictureBox1.Width) - 0.5) * 2 + "\r" + (((double)balls[BallIndex].y / (double)pictureBox1.Height) - 0.5) * 2;
            pictureBox1.Image = bit0;
            pictureBox2.Image = bit1;
            pictureBox3.Image = bit3;
            pictureBox4.Image = bit4;
            pictureBox5.Image = bit5;
            pictureBox6.Image = bit6;
            pictureBox7.Image = bit7;
            pictureBox8.Image = bitu;*/
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            /*if (BallIndex < 100)*/
            BallIndex++;
            if (e.Button == MouseButtons.Right)
            {
                balls[BallIndex] = new ball(e.X, e.Y, Color.FromArgb(50,0,0,0));
                gInput.FillEllipse(new SolidBrush(Color.Black), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
                blacks++;
            }
            else
            {
                balls[BallIndex] = new ball(e.X, e.Y, Color.FromArgb(50, 255, 255, 255));
                gInput.FillEllipse(new SolidBrush(Color.White), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
                whites++;
            }
            g0.FillEllipse(new SolidBrush(balls[BallIndex].C), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
            g1.FillEllipse(new SolidBrush(balls[BallIndex].C), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
            g3.FillEllipse(new SolidBrush(balls[BallIndex].C), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
            g4.FillEllipse(new SolidBrush(balls[BallIndex].C), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
            g5.FillEllipse(new SolidBrush(balls[BallIndex].C), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
            g6.FillEllipse(new SolidBrush(balls[BallIndex].C), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
            g7.FillEllipse(new SolidBrush(balls[BallIndex].C), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
            gu.FillEllipse(new SolidBrush(balls[BallIndex].C), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
            gInput.FillEllipse(new SolidBrush(balls[BallIndex].C), balls[BallIndex].x - 2, balls[BallIndex].y - 2, 4, 4);
            
            //label3.Text = (((double)balls[BallIndex].x / (double)pictureBox1.Width) - 0.5) * 2 + "\r" + (((double)balls[BallIndex].y / (double)pictureBox1.Height) - 0.5) * 2;
            pictureBox1.Image = bit0;
            pictureBox2.Image = bit1;
            pictureBox3.Image = bit3;
            pictureBox4.Image = bit4;
            pictureBox5.Image = bit5;
            pictureBox6.Image = bit6;
            pictureBox7.Image = bit7;
            pictureBox8.Image = bitu;
            pictureBox9.Image = bitInput;
            label4.Text = "Input here: left click = white("+whites+"), right click = black("+blacks+")";
        }
        int normalise(double s,int ss)
        {
            if (s > ss)
                return (int)s;
            if (s < 0)
                return 0;
            return (int)(s * 100 / ss);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "0")
                button1.Text = "1";
            else
                button1.Text = "0";
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //if(button1.Text == "1")
            /*{
                Random r = new Random();
                for(int i=0;i<pictureBox1.Height;i+=r.Next(1,3))
                {
                    for (int j = 0; j < pictureBox1.Width; j+= r.Next(1, 3))
                    {
                        n[0].OUT = ((double)j / (double)pictureBox1.Width) - 0.5;
                        n[1].OUT = ((double)i / (double)pictureBox1.Height) - 0.5;
                        n[2].IN = s[0].Weight * n[0].OUT + s[1].Weight * n[1].OUT;
                        n[3].IN = s[2].Weight * n[0].OUT + s[3].Weight * n[1].OUT;
                        n[2].culc();
                        n[3].culc();
                        n[4].IN = s[4].Weight * n[2].OUT + s[5].Weight * n[3].OUT;
                        n[4].culc();
                        if (n[4].OUT > 0.5)
                            bit0.SetPixel(j, i, Color.FromArgb(0, 0, (int)(255 * n[4].OUT)));
                        else
                            bit0.SetPixel(j, i, Color.FromArgb((int)(255 * (1-n[4].OUT)), 0, 0));
                    }
                }
            }
            for (int t = 0; t <= BallIndex; t++)
                bit0.SetPixel(balls[t].x, balls[t].y, balls[t].C);
            pictureBox1.Image = bit0;*/
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            int c;
            double answer = 0, x, y;
            for (int k = 0; k <= BallIndex; k++)
            {
                x = (((double)balls[k].x / (double)pictureBox1.Width) - 0.5) * 2;
                y = (((double)balls[k].y / (double)pictureBox1.Width) - 0.5) * 2;
                if (balls[k].C.R == Color.Black.R)
                    c = 1;
                else
                    c = 0;
                NetU.Study(x, y, c);
                Net7.Study(x, y, c);
                Net6.Study(x, y, c);
                Net5.Study(x, y, c);
                Net4.Study(x, y, c);
                Net3.Study(x, y, c);
                Net1.Study(x, y, c);
                Net0.Study(x, y, c);
            }
            for (int i = 0; i < pictureBox1.Height; i += r.Next(1, 3))
            {
                for (int j = 0; j < pictureBox1.Width; j += r.Next(1, 3))
                {
                    x = (((double)j / (double)pictureBox1.Width) - 0.5) * 2;
                    y = (((double)i / (double)pictureBox1.Width) - 0.5) * 2;
                    answer = NetU.Answer(x, y);
                    bitu.SetPixel(j, i, Color.FromArgb((int)(255 * (answer)), (int)(255 * (answer)), (int)(255 * (answer))));
                    answer = Net7.Answer(x, y);
                    bit7.SetPixel(j, i, Color.FromArgb((int)(255 * (answer)), (int)(255 * (answer)), (int)(255 * (answer))));
                    answer = Net6.Answer(x, y);
                    bit6.SetPixel(j, i, Color.FromArgb((int)(255 * (answer)), (int)(255 * (answer)), (int)(255 * (answer))));
                    answer = Net5.Answer(x, y);
                    bit5.SetPixel(j, i, Color.FromArgb((int)(255 * (answer)), (int)(255 * (answer)), (int)(255 * (answer))));
                    answer = Net4.Answer(x, y);
                    bit4.SetPixel(j, i, Color.FromArgb((int)(255 * (answer)), (int)(255 * (answer)), (int)(255 * (answer))));
                    answer = Net3.Answer(x, y);
                    bit3.SetPixel(j, i, Color.FromArgb((int)(255 * (answer)), (int)(255 * (answer)), (int)(255 * (answer))));
                    answer = Net1.Answer(x, y);
                    bit1.SetPixel(j, i, Color.FromArgb((int)(255 * (answer)), (int)(255 * (answer)), (int)(255 * (answer))));
                    answer = Net0.Answer(x, y);
                    bit0.SetPixel(j, i, Color.FromArgb((int)(255 * (answer)), (int)(255 * (answer)), (int)(255 * (answer))));
                }
            }

            for (int t = 0; t <= BallIndex; t++)
            {
                g0.FillEllipse(new SolidBrush(balls[t].C), balls[t].x - 2, balls[t].y - 2, 4, 4);
                g1.FillEllipse(new SolidBrush(balls[t].C), balls[t].x - 2, balls[t].y - 2, 4, 4);
                g3.FillEllipse(new SolidBrush(balls[t].C), balls[t].x - 2, balls[t].y - 2, 4, 4);
                g4.FillEllipse(new SolidBrush(balls[t].C), balls[t].x - 2, balls[t].y - 2, 4, 4);
                g5.FillEllipse(new SolidBrush(balls[t].C), balls[t].x - 2, balls[t].y - 2, 4, 4);
                g6.FillEllipse(new SolidBrush(balls[t].C), balls[t].x - 2, balls[t].y - 2, 4, 4);
                g7.FillEllipse(new SolidBrush(balls[t].C), balls[t].x - 2, balls[t].y - 2, 4, 4);
                gu.FillEllipse(new SolidBrush(balls[t].C), balls[t].x - 2, balls[t].y - 2, 4, 4);
            }
            pictureBox1.Image = bit0;
            pictureBox2.Image = bit1;
            pictureBox3.Image = bit3;
            pictureBox4.Image = bit4;
            pictureBox5.Image = bit5;
            pictureBox6.Image = bit6;
            pictureBox7.Image = bit7;
            pictureBox8.Image = bitu;

            //
            /*
            double[] input = new double[8];
            double[] answer = new double[4];
            answer[3] = 0;
            //input[2] = 0;
            //input[3] = 0;
            //input[4] = 0;
            //input[5] = 0;
            //input[6] = 0;
            //input[7] = 0;
            for (int t = 0; t <= BallIndex; t++)
            {
                input[0] = input[1] = input[2] = input[3] = (((double)balls[t].x/ (double)pictureBox1.Width)-0.5)*2;
                input[4] = input[5] = input[6] = input[7]  = (((double)balls[t].y/ (double)pictureBox1.Height)-0.5)*2;
                //x.GetAnswer(input);

                answer[0] = (double)balls[t].C.R/255;
                answer[1] = (double)balls[t].C.G/255;
                answer[2] = (double)balls[t].C.B/255;
                
                x.change_sin(input,answer);
            }
            label1.Text = x.error.ToString();
            if (button1.Text == "1")
                for (int i = 0; i < pictureBox1.Height; i += r.Next(1, 3))
                {
                    for (int j = 0; j < pictureBox1.Width; j += r.Next(1, 3))
                    {
                        input[0] = input[1] = input[2] = input[3] = (((double)j / (double)pictureBox1.Width) - 0.5)*2;
                        input[4] = input[5] = input[6] = input[7] = (((double)i / (double)pictureBox1.Height) - 0.5)*2;
                        answer = x.GetAnswer(input);
                        bit0.SetPixel(j, i, Color.FromArgb((int)(255 * answer[0]), (int)(255 * answer[1]), (int)(255 * answer[2])));
                    }
                }
            for (int t = 0; t <= BallIndex; t++)
                bit0.SetPixel(balls[t].x, balls[t].y, balls[t].C);
            pictureBox1.Image = bit0;
            /*
            label1.Text = x.error.ToString();

            label2.Text = "\n";
            for (int t = 0; t < x.n.Length; t++)
                label2.Text += x.n[t].value + "\n";
            label1.Text += "\nBC\n";
            for (int t = 0; t < 7; t++)
                for (int t1 = 0; t1 < 4; t1++)
                {
                    label1.Text += x.BC[t, t1].weight.ToString() + "\n";
                }
            */
        }
    }
}
