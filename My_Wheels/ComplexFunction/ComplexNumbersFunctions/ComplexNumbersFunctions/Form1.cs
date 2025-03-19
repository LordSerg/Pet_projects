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
    public partial class Form1 : Form
    {
        Complex[,] arr;
        int w, h;//picturebox size
        double fromX=-5.0, fromY=-5.0, toX=5.0, toY=5.0;
        Graphics g;
        Bitmap bit;
        int Nx=200, Ny=200;
        Brush col = Brushes.White;
        int counter=0,mult=100;
        Random rand = new Random();
        Complex f0(Complex x) { return x * x; }
        Complex f1(Complex x) { return x * x * x; }
        Complex f2(Complex x) { return 1 / x; }
        Complex f3(Complex x) { return Complex.sin(x); }
        Complex f4(Complex x) { return Complex.cos(x); }
        Complex f5(Complex x) { return Complex.Ln(x); }
        Complex f6(Complex x) { return x / (!x); }
        Complex f7(Complex x) { return Complex.Ln(new Complex(x.Im-x.Re, x.Re)); }
        Complex f8(Complex x) { return Complex.pow(!x, !x); }
        Complex f9(Complex x) { return Complex.pow(new Complex(x.Im, x.Re), x); }
        delegate Complex F(Complex x);//delegat, that contains functions
        List<F> allFunctions = new List<F>();//list of all functions
        F curFunction;//function, that is shown right now
        private void timer1_Tick(object sender, EventArgs e)
        {
            Change();
            if (radioButton1.Checked)
                DrawLines();
            else
                DrawPoints();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Width > 0 && pictureBox1.Height > 0)
            {
                w = pictureBox1.Width;
                h = pictureBox1.Height;
                bit = new Bitmap(w, h);
                g = Graphics.FromImage(bit);
                g.Clear(Color.Black);
                pictureBox1.Image = bit;
            }
        }

        public Form1()
        {
            InitializeComponent();
            arr = new Complex[Nx, Ny];
            for (int i = 0; i < Nx; i++)
                for (int j = 0; j < Ny; j++)
                {
                    arr[i, j] = new Complex((fromX * ((Nx - i) / (double)Nx) + toX * (i / (double)Nx)),
                                            (fromY * ((Ny - j) / (double)Ny) + toY * (j / (double)Ny)));
                }
            allFunctions.Add(f0); comboBox1.Items.Add("x * x");
            allFunctions.Add(f1); comboBox1.Items.Add("x * x * x");
            allFunctions.Add(f2); comboBox1.Items.Add("1 / x");
            allFunctions.Add(f3); comboBox1.Items.Add("sin(x)");
            allFunctions.Add(f4); comboBox1.Items.Add("cos(x)");
            allFunctions.Add(f5); comboBox1.Items.Add("Ln(x)");
            allFunctions.Add(f6); comboBox1.Items.Add("x / (!x)");
            allFunctions.Add(f7); comboBox1.Items.Add("x*exp(cos(x)/sin(x))");
            allFunctions.Add(f8); comboBox1.Items.Add("pow(!x, !x)");
            allFunctions.Add(f9); comboBox1.Items.Add("pow((x.Im + i*x.Re), x)");
            comboBox1.SelectedIndex = 6;
            w = pictureBox1.Width;
            h = pictureBox1.Height;
            bit = new Bitmap(w, h);
            g = Graphics.FromImage(bit);
            g.Clear(Color.Black);
            pictureBox1.Image = bit;
            val = 0;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < Nx; i++)
                for (int j = 0; j < Ny; j++)
                {
                    arr[i, j] = new Complex((fromX * ((Nx - i) / (double)Nx) + toX * (i / (double)Nx)),
                                            (fromY * ((Ny - j) / (double)Ny) + toY * (j / (double)Ny)));
                }
            if (radioButton1.Checked)
                DrawLines();
            else
                DrawPoints();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            curFunction = new F(allFunctions[comboBox1.SelectedIndex]);
            for (int i = 0; i < Nx; i++)
                for (int j = 0; j < Ny; j++)
                {
                    arr[i, j] = new Complex((fromX * ((Nx - i) / (double)Nx) + toX * (i / (double)Nx)),
                                            (fromY * ((Ny - j) / (double)Ny) + toY * (j / (double)Ny)));
                }
        }
        int val;
        int cond=1;//1 or -1
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (val > trackBar1.Value)
                cond = -1;
            else
                cond = 1;
            val = trackBar1.Value;
            Change();
            if (radioButton1.Checked)
                DrawLines();
            else
                DrawPoints();
        }

        

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                DrawLines();
            else
                DrawPoints();
        }

        

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 1;
            timer1.Enabled = checkBox1.Checked;
            trackBar1.Enabled = !checkBox1.Checked;
        }

        int ReconvX(double x)
        {
            x -= fromX;
            x /= (double)(toX - fromX) / w;
            return (int)(x);
        }
        int ReconvY(double y)
        {
            y -= fromY;
            y /= (double)(toY - fromY) / h;
            y = h - y;
            return (int)(y);
        }
        bool isGood(Complex x)
        {
            return (!(x.Re > mult || x.Re < -mult || x.Im > mult || x.Im < -mult))&& (!double.IsNaN(x.Re) && !double.IsNaN(x.Im));
        }
        void DrawLines()
        {
            g.Clear(Color.Black);
            for (int i = 1; i < Nx; i++)
                for (int j = 1; j < Ny; j++)
                {
                    if (isGood(arr[i, j]))
                    {
                        if (isGood(arr[i - 1, j]))
                        {
                            //g.FillEllipse(col, ReconvX(arr[i, j].Re), ReconvY(arr[i, j].Im), 2, 2);
                            g.DrawLine(Pens.White, ReconvX(arr[i, j].Re), ReconvY(arr[i, j].Im), ReconvX(arr[i - 1, j].Re), ReconvY(arr[i - 1, j].Im));
                        }
                        if (isGood(arr[i, j - 1]))
                        {
                            g.DrawLine(Pens.White, ReconvX(arr[i, j].Re), ReconvY(arr[i, j].Im), ReconvX(arr[i, j-1].Re), ReconvY(arr[i, j-1].Im));
                        }
                    }
                }
            pictureBox1.Image = bit;
        }
        void DrawPoints()
        {
            g.Clear(Color.Black);
            for (int i = 0; i < Nx; i++)
                for (int j = 0; j < Ny; j++)
                {
                    if (isGood(arr[i, j]))
                    {
                        g.FillEllipse(col, ReconvX(arr[i, j].Re), ReconvY(arr[i, j].Im), 2, 2);
                    }
                }
            pictureBox1.Image = bit;
        }
        void Change()
        {
            for (int i = 0; i < Nx; i++)
                for (int j = 0; j < Ny; j++)
                {
                    //if (arr[i, j].Re > mult || arr[i, j].Re < -mult || arr[i, j].Im > mult || arr[i, j].Im < -mult)
                    //    //|| (((rand.Next(0, 10) + counter) % 10) == 0))
                    //    arr[i, j] = new Complex((fromX * ((Nx - i) / (double)Nx) + toX * (i / (double)Nx)),
                    //                            (fromY * ((Ny - j) / (double)Ny) + toY * (j / (double)Ny)));
                    //else
                    //    arr[i, j] += cond * curFunction(arr[i, j]) / 100.0;
                    if (!(!isGood(arr[i,j]) || arr[i, j].Re > mult || arr[i, j].Re < -mult || arr[i, j].Im > mult || arr[i, j].Im < -mult))
                        arr[i, j] += cond * curFunction(arr[i, j]) / 100.0;
                }
        }
    }
}
