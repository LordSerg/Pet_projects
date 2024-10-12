using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CelluarAutomats
{
    public partial class ConwaysLife : Form
    {
        Bitmap bit1,bit2;
        Graphics g1,g2;
        //int w=200, h=200;
        int w, h;
        Color dead = Color.FromArgb(128, 128, 128);
        Color alife = Color.FromArgb(200,200,200);
        bool[,] state1, state2;
        bool flag;
        int size = 4;
        int occupancy=40;
        int radius = 1;
        int bf=3, bt=3, lf=2, lt=3;
        public ConwaysLife()
        {
            InitializeComponent();
            init();
        }
        void init()
        {
            w = pictureBox1.Width / size;
            h = pictureBox1.Height / size;
            state1 = new bool[w, h];
            bit1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g1 = Graphics.FromImage(bit1);
            g1.Clear(dead);
            state2 = new bool[w, h];
            bit2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g2 = Graphics.FromImage(bit2);
            g2.Clear(dead);
            //pictureBox1.BackgroundImage = new Bitmap(1, 1);
            flag = true;
            pictureBox1.Image = bit1;
            timer1.Interval = 1;
        }
        private void button1_Click(object sender, EventArgs e)
        {//SetNewSet
            SetNewSet();
        }

        private void button2_Click(object sender, EventArgs e)
        {//go/stop
            if (timer1.Enabled)
                button2.Text = "Go";
            else
                button2.Text = "Stop";
            timer1.Enabled = !timer1.Enabled;
        }

        private void button3_Click(object sender, EventArgs e)
        {//oneStep
            timer1.Enabled = false;
            button2.Text = "Start";
            Step();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Task t = new Task(()=>Step());
            //t.Start();
            //t.Wait();
            Step();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            radius = trackBar3.Value;
            label9.Text = "Neigborhood radius = " + radius;
            numericUpDown1.Maximum = ((1 + 2 * radius) * (1 + 2 * radius)) - 1;
            numericUpDown2.Maximum = ((1 + 2 * radius) * (1 + 2 * radius)) - 1;
            numericUpDown3.Maximum = ((1 + 2 * radius) * (1 + 2 * radius)) - 1;
            numericUpDown4.Maximum = ((1 + 2 * radius) * (1 + 2 * radius)) - 1;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > numericUpDown2.Value)
            {
                numericUpDown2.Value = numericUpDown1.Value;
                bt = (int)numericUpDown2.Value;
            }
            bf = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > numericUpDown2.Value)
            {
                numericUpDown1.Value = numericUpDown2.Value;
                bf = (int)numericUpDown1.Value;
            }
            bt = (int)numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown3.Value > numericUpDown4.Value)
            {
                numericUpDown4.Value = numericUpDown3.Value;
                lt = (int)numericUpDown4.Value;
            }
            lf = (int)numericUpDown3.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown3.Value > numericUpDown4.Value)
            {
                numericUpDown3.Value = numericUpDown4.Value;
                lf = (int)numericUpDown3.Value;
            }
            lt = (int)numericUpDown4.Value;
        }

        private void SetNewSet()
        {
            Random r = new Random();
            g1.Clear(dead);
            for (int i = radius; i < w- radius; i++)
                for (int j = radius; j < h - radius; j++)
                {
                    if (r.Next(100) < occupancy)
                    {
                        //bit1.SetPixel(i, j, alife);
                        g1.FillRectangle(new SolidBrush(alife), i * size, j * size, size, size);
                        state1[i, j] = true;
                    }
                    else
                        state1[i, j] = false;

                    state2[i, j] = false;
                }
            //state1[1, 1] = true;
            //state1[1, 2] = true;
            //state1[2, 1] = true;
            //state1[2, 2] = true;
            flag = true;
            //pictureBox1.BackgroundImage = new Bitmap(1, 1);
            pictureBox1.Image = bit1;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            size = trackBar1.Value;
            label1.Text = "Size = " + size;
            init();
            SetNewSet();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            occupancy = trackBar2.Value*10;
            label2.Text = "Occupancy: " + occupancy + "%";
            SetNewSet();
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Height > 0 && pictureBox1.Width > 0)
            {
                init();
                SetNewSet();
            }
        }

        /*
        private int NumOfNeighbours_closedArea(int x,int y)
        {
            int result = 0;
            if(x==0)
            {
                if(y==0)
                {
                    if (state[x+1, y  ]) result++;
                    if (state[x, y+1  ]) result++;
                    if (state[x+1, y+1]) result++;
                }
                else if(y==h-1)
                {
                    if (state[x+1, y  ]) result++;
                    if (state[x+1, y-1]) result++;
                    if (state[x, y-1  ]) result++;
                }
                else
                {
                    if (state[x, y-1  ]) result++;
                    if (state[x+1, y-1]) result++;
                    if (state[x+1, y  ]) result++;
                    if (state[x+1, y+1]) result++;
                    if (state[x, y+1  ]) result++;
                }
            }
            else if(x==w-1)
            {
                if(y==0)
                {
                    if (state[x-1, y  ]) result++;
                    if (state[x, y+1  ]) result++;
                    if (state[x-1, y+1]) result++;
                }
                else if(y==h-1)
                {
                    if (state[x-1, y  ]) result++;
                    if (state[x-1, y-1]) result++;
                    if (state[x, y-1  ]) result++;
                }
                else
                {
                    if (state[x, y-1  ]) result++;
                    if (state[x-1, y-1]) result++;
                    if (state[x-1, y  ]) result++;
                    if (state[x-1, y+1]) result++;
                    if (state[x, y+1  ]) result++;
                }
            }
            else if(y==0)
            {
                if(x==0)
                {
                    if (state[x+1, y  ]) result++;
                    if (state[x, y+1  ]) result++;
                    if (state[x+1, y+1]) result++;
                }
                else if(x==w-1)
                {
                    if (state[x-1, y  ]) result++;
                    if (state[x, y+1  ]) result++;
                    if (state[x-1, y+1]) result++;
                }
                else
                {
                    if (state[x-1, y  ]) result++;
                    if (state[x-1, y+1]) result++;
                    if (state[x, y+1  ]) result++;
                    if (state[x+1, y+1]) result++;
                    if (state[x+1, y  ]) result++;
                }
            }
            else if (y == h-1)
            {
                if(x==0)
                {
                    if (state[x+1, y  ]) result++;
                    if (state[x, y-1  ]) result++;
                    if (state[x+1, y-1]) result++;
                }
                else if(x==w-1)
                {
                    if (state[x-1, y  ]) result++;
                    if (state[x, y-1  ]) result++;
                    if (state[x-1, y-1]) result++;
                }
                else
                {
                    if (state[x-1, y  ]) result++;
                    if (state[x-1, y-1]) result++;
                    if (state[x,   y-1]) result++;
                    if (state[x+1, y-1]) result++;
                    if (state[x+1, y  ]) result++;
                }
            }
            else
            {
                if (state[x+1, y  ]) result++;
                if (state[x+1, y+1]) result++;
                if (state[x, y+1  ]) result++;
                if (state[x-1, y+1]) result++;
                if (state[x-1, y  ]) result++;
                if (state[x-1, y-1]) result++;
                if (state[x, y-1  ]) result++;
                if (state[x+1, y-1]) result++;
            }
            return result;
        }
        */
        private int NumOfNeighbours_openedArea2(int x, int y)
        {
            int result = 0;
            for(int i=x-radius;i<=x+radius;i++)
                for(int j=y-radius;j<=y+radius;j++)
                        if (state2[i, j]) result++;
            if (state2[x, y]) result--;
            //if (state2[x + 1, y    ]) result++;
            //if (state2[x + 1, y + 1]) result++;
            //if (state2[x    , y + 1]) result++;
            //if (state2[x - 1, y + 1]) result++;
            //if (state2[x - 1, y    ]) result++;
            //if (state2[x - 1, y - 1]) result++;
            //if (state2[x    , y - 1]) result++;
            //if (state2[x + 1, y - 1]) result++;
            return result;
        }
        private int NumOfNeighbours_openedArea1(int x, int y)
        {
            int result = 0;
            for (int i = x - radius; i <= x + radius; i++)
                for (int j = y - radius; j <= y + radius; j++)
                        if (state1[i, j]) result++;
            if (state1[x, y]) result--;
            //if (state1[x + 1, y    ]) result++;
            //if (state1[x + 1, y + 1]) result++;
            //if (state1[x    , y + 1]) result++;
            //if (state1[x - 1, y + 1]) result++;
            //if (state1[x - 1, y    ]) result++;
            //if (state1[x - 1, y - 1]) result++;
            //if (state1[x    , y - 1]) result++;
            //if (state1[x + 1, y - 1]) result++;
            return result;
        }

        private void Step()
        {
            int n = 0;
            SolidBrush dd = new SolidBrush(dead);
            SolidBrush al = new SolidBrush(alife);
            //bool[,] t = new bool[w, h];
            if (flag)
            {
                g2.Clear(dead);
                for (int i = radius; i < w - radius; i++)
                    for (int j = radius; j < h - radius; j++)
                    {
                        //n = is_closed ? NumOfNeighbours_closedArea(i, j) : NumOfNeighbours_openedArea(i, j);
                        n = NumOfNeighbours_openedArea1(i, j);
                        //if (!state1[i, j] && n == 3)
                        if (!state1[i, j] && n>=bf&&n<=bt)
                        {
                            //bit.SetPixel(i, j, alife);
                            //g2.FillRectangle(al, i, j, 1, 1);
                            g2.FillRectangle(al, i*size, j*size, size, size);
                            state2[i, j] = true;
                        }
                        //else if (state1[i, j] && n > 1 && n < 4)
                        else if (state1[i, j] && n >= lf && n <= lt)
                        {
                            //bit.SetPixel(i, j, alife);
                            //g2.FillRectangle(al, i, j, 1, 1);
                            g2.FillRectangle(al, i * size, j * size, size, size);
                            state2[i, j] = true;
                        }
                        else //if (n < 2 || n >= 4)
                        {
                            //bit.SetPixel(i, j, dead);
                            //g2.FillRectangle(dd, i, j, 1, 1);
                            g2.FillRectangle(dd, i * size, j * size, size, size);
                            state2[i, j] = false;
                        }
                    }
                //pictureBox1.BackgroundImage = new Bitmap(1, 1);
                //for (int i = 1; i < w/size-1; i++)
                //    for (int j = 1; j < h/size-1; j++)
                //        g2.DrawString(NumOfNeighbours_openedArea2(i,j).ToString(),new Font("Areal",12),new SolidBrush(Color.White),i*size,j*size);
                pictureBox1.Image = bit2;
                
            }
            else
            {
                g1.Clear(dead);
                for (int i = radius; i < w - radius; i++)
                    for (int j = radius; j < h - radius; j++)
                    {
                        //n = is_closed ? NumOfNeighbours_closedArea(i, j) : NumOfNeighbours_openedArea(i, j);
                        n = NumOfNeighbours_openedArea2(i, j);
                        //if (!state2[i, j] && n == 3)
                        if (!state2[i, j] && n >= bf && n <= bt)
                        {
                            //bit.SetPixel(i, j, alife);
                            //g1.FillRectangle(al, i, j, 1, 1);
                            g1.FillRectangle(al, i * size, j * size, size, size);
                            state1[i, j] = true;
                        }
                        //else if (state2[i, j] && n > 1 && n < 4)
                        else if (state2[i, j] && n >= lf && n <= lt)
                        {
                            //bit.SetPixel(i, j, alife);
                            //g1.FillRectangle(al, i, j, 1, 1);
                            g1.FillRectangle(al, i * size, j * size, size, size);
                            state1[i, j] = true;
                        }
                        else //if (n < 2 || n >= 4)
                        {
                            //bit.SetPixel(i, j, dead);
                            //g1.FillRectangle(dd, i, j, 1, 1);
                            g1.FillRectangle(dd, i * size, j * size, size, size);
                            state1[i, j] = false;
                        }
                        //g1.DrawString(n.ToString(), new Font("Areal", 12), new SolidBrush(Color.White), i * size, j * size);
                    }
                //for (int i = 1; i < w / size - 1; i++)
                //    for (int j = 1; j < h / size - 1; j++)
                //        g1.DrawString(NumOfNeighbours_openedArea1(i, j).ToString(), new Font("Areal", 12), new SolidBrush(Color.White), i * size, j * size);
                pictureBox1.Image = bit1;
            }
            //pictureBox1.Image = bit2;
            flag = !flag;
        }
    }
}
