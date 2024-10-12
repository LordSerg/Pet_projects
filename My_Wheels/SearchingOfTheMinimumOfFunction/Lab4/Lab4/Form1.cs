using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Lab4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            bit = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bit);
        }
        Bitmap bit;
        Graphics g;
        double xFrom, xTo, yFrom = -10, yTo = 10;
        private double f(double x)
        {
            //return Math.Abs(x) + Math.Pow(x - 2.0, 2);
            return Math.Cos(x*7)*5*Math.Sin(x*5)+Math.Cos(x*3)*5*Math.Sin(x*7);
        }
        private double find_min_dihotomia(double a, double b, double eps, double delt)
        {
            double a1 = a, b1 = b;
            //double delt = eps/3.0;
            double x1, x2, x_star = 0;
            int i = 1;
            listBox1.Items.Clear();
            while (b1 - a1 >= eps)
            {
                x1 = (a1 + b1 - delt) / 2;
                x2 = (a1 + b1 + delt) / 2;
                if (f(x1) <= f(x2))
                {
                    b1 = x2;
                    x_star = x1;
                }
                else
                {
                    a1 = x1;
                    x_star = x2;
                }
                //отрисовка найденого приближения
                listBox1.Items.Add(i + ". " + x_star);
                g.DrawEllipse(new Pen(Color.Red),
                    (float)((x_star - xFrom) * pictureBox1.Width / (xTo - xFrom)) - 3,
                    (float)(pictureBox1.Height - (f(x_star) - yFrom) * pictureBox1.Height / (yTo - yFrom)) - 3,
                    6, 6);
                g.DrawLine(new Pen(Color.LightCoral, 1), (float)((x_star - xFrom) * pictureBox1.Width / (xTo - xFrom)), 0,
                    (float)((x_star - xFrom) * pictureBox1.Width / (xTo - xFrom)), pictureBox1.Height);
                pictureBox1.Refresh();
                pictureBox1.Image = bit;
                if (tabControl1.SelectedIndex == 0) Thread.Sleep(100);
                i++;
            }
            g.FillEllipse(new SolidBrush(Color.Red),
                  (float)((x_star - xFrom) * pictureBox1.Width / (xTo - xFrom)) - 4,
                  (float)(pictureBox1.Height - (f(x_star) - yFrom) * pictureBox1.Height / (yTo - yFrom)) - 4,
                  8, 8);
            g.DrawLine(new Pen(Color.Red, 2), (float)((x_star - xFrom) * pictureBox1.Width / (xTo - xFrom)), 0,
                    (float)((x_star - xFrom) * pictureBox1.Width / (xTo - xFrom)), pictureBox1.Height);
            pictureBox1.Refresh();
            pictureBox1.Image = bit;
            listBox1.Items.Add(i + ". " + x_star + " - результат");
            return x_star;
        }
        private double find_min_gold(double a, double b, double eps, double delt)
        {
            double a1 = a, b1 = b;
            //double delt = eps / 3.0;
            double x1, x2, x_star = 0;
            int i = 1;
            listBox2.Items.Clear();
            while (b1 - a1 >= eps)
            {
                x1 = a1 + (3 - Math.Sqrt(5)) * (b1 - a1) / 2;
                x2 = a1 + (Math.Sqrt(5) - 1) * (b1 - a1) / 2;
                if (f(x1) <= f(x2))
                {
                    b1 = x2;
                    x_star = x1;
                }
                else
                {
                    a1 = x1;
                    x_star = x2;
                }
                listBox2.Items.Add(i + ". " + x_star);
                g.DrawEllipse(new Pen(Color.Green),
                   (float)((x_star - xFrom) * pictureBox1.Width / (xTo - xFrom)) - 3,
                   (float)(pictureBox1.Height - (f(x_star) - yFrom) * pictureBox1.Height / (yTo - yFrom)) - 3,
                   6, 6);
                g.DrawLine(new Pen(Color.LightGreen, 1), (float)((x_star - xFrom) * pictureBox1.Width / (xTo - xFrom)), 0,
                    (float)((x_star - xFrom) * pictureBox1.Width / (xTo - xFrom)), pictureBox1.Height);
                pictureBox1.Refresh();
                pictureBox1.Image = bit;
                if (tabControl1.SelectedIndex == 0) Thread.Sleep(50);
                i++;
            }
            g.FillEllipse(new SolidBrush(Color.Green),
                  (float)((x_star - xFrom) * pictureBox1.Width / (xTo - xFrom)) - 4,
                  (float)(pictureBox1.Height - (f(x_star) - yFrom) * pictureBox1.Height / (yTo - yFrom)) - 4,
                  8, 8);
            g.DrawLine(new Pen(Color.Green, 2), (float)((x_star - xFrom) * pictureBox1.Width / (xTo - xFrom)), 0,
                    (float)((x_star - xFrom) * pictureBox1.Width / (xTo - xFrom)), pictureBox1.Height);
            pictureBox1.Refresh();
            pictureBox1.Image = bit;
            listBox2.Items.Add(i + ". " + x_star + " - результат");
            return x_star;
        }
        private double F(int n)
        {
            return (Math.Pow((1 + Math.Sqrt(5)) / 2, n) - Math.Pow((1 - Math.Sqrt(5)) / 2, n)) / Math.Sqrt(5);
            //return (Math.Pow((1 + Math.Sqrt(5)) / 2, n)) / Math.Sqrt(5);
        }
        private double find_min_fibonachi(double a, double b, double eps, double delt, int n)
        {
            double a1 = a, b1 = b;
            //double delt = eps / 3.0;
            double x1, x2, x_star = 0;
            int k = 1;
            listBox3.Items.Clear();
            while (n != k && b1 - a1 >= eps)
            {
                x1 = a1 + (b - a) * F(n - k + 1) / F(n + 2);
                x2 = a1 + (b - a) * F(n - k + 2) / F(n + 2);
                if (f(x1) <= f(x2))
                {
                    b1 = x2;
                    x_star = x1;
                }
                else
                {
                    a1 = x1;
                    x_star = x2;
                }
                listBox3.Items.Add(k + ". " + x_star);
                g.DrawEllipse(new Pen(Color.Blue),
                   (float)((x_star - xFrom) * pictureBox1.Width / (xTo - xFrom)) - 3,
                   (float)(pictureBox1.Height - (f(x_star) - yFrom) * pictureBox1.Height / (yTo - yFrom)) - 3,
                   6, 6);
                g.DrawLine(new Pen(Color.LightBlue, 1), (float)((x_star - xFrom) * pictureBox1.Width / (xTo - xFrom)), 0,
                    (float)((x_star - xFrom) * pictureBox1.Width / (xTo - xFrom)), pictureBox1.Height);
                pictureBox1.Refresh();
                pictureBox1.Image = bit;
                if (tabControl1.SelectedIndex == 0) Thread.Sleep(50);
                k++;
            }
            g.FillEllipse(new SolidBrush(Color.Blue),
                  (float)((x_star - xFrom) * pictureBox1.Width / (xTo - xFrom)) - 4,
                  (float)(pictureBox1.Height - (f(x_star) - yFrom) * pictureBox1.Height / (yTo - yFrom)) - 4,
                  8, 8);
            g.DrawLine(new Pen(Color.Blue, 2), (float)((x_star - xFrom) * pictureBox1.Width / (xTo - xFrom)), 0,
                    (float)((x_star - xFrom) * pictureBox1.Width / (xTo - xFrom)), pictureBox1.Height);
            pictureBox1.Refresh();
            pictureBox1.Image = bit;
            listBox3.Items.Add(k + ". " + x_star + " - результат");
            return x_star;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Draw(true);
        }
        private void Draw(bool is_find)
        {
            g.Clear(Color.FromArgb(100, 100, 100));
            double x, y;
            xFrom = Convert.ToDouble(textBox1.Text);
            xTo = Convert.ToDouble(textBox2.Text);
            //yFrom = -1;// Convert.ToDouble(textBox5.Text);
            //yTo = 13; //Convert.ToDouble(textBox4.Text);
            //рисуем сетку:
            int k1 = pictureBox1.Width / 100, k2 = pictureBox1.Height / 100;
            for (double t = 0; t < xTo; t += (xTo - xFrom) / k1)
            {
                x = ((t - xFrom) * pictureBox1.Width / (xTo - xFrom));
                g.DrawLine(new Pen(Color.Gray), (int)x, 0, (int)x, pictureBox1.Height);
                g.DrawString(Math.Round(t, 2).ToString(), this.Font, new SolidBrush(Color.Gray), (int)x, pictureBox1.Height - 20);
            }
            for (double t = 0; t > xFrom; t -= (xTo - xFrom) / k1)
            {
                x = ((t - xFrom) * pictureBox1.Width / (xTo - xFrom));
                g.DrawLine(new Pen(Color.Gray), (int)x, 0, (int)x, pictureBox1.Height);
                g.DrawString(Math.Round(t, 2).ToString(), this.Font, new SolidBrush(Color.Gray), (int)x, pictureBox1.Height - 20);
            }
            for (double t = 0; t < yTo; t += (yTo - yFrom) / k2)
            {
                y = pictureBox1.Height - (t - yFrom) * pictureBox1.Height / (yTo - yFrom);
                g.DrawLine(new Pen(Color.Gray), 0, (int)y, pictureBox1.Width, (int)y);
                g.DrawString(Math.Round(t, 2).ToString(), this.Font, new SolidBrush(Color.Gray), 0, (int)y);
            }
            for (double t = 0; t > yFrom; t -= (yTo - yFrom) / k2)
            {
                y = pictureBox1.Height - (t - yFrom) * pictureBox1.Height / (yTo - yFrom);
                g.DrawLine(new Pen(Color.Gray), 0, (int)y, pictureBox1.Width, (int)y);
                g.DrawString(Math.Round(t, 2).ToString(), this.Font, new SolidBrush(Color.Gray), 0, (int)y);
            }
            //ось X:
            g.DrawLine(new Pen(Color.FromArgb(150, 150, 150), 4), 0, (int)(pictureBox1.Height - (-yFrom) * pictureBox1.Height / (yTo - yFrom)), pictureBox1.Width, (int)(pictureBox1.Height - (-yFrom) * pictureBox1.Height / (yTo - yFrom)));
            //ось Y:
            g.DrawLine(new Pen(Color.FromArgb(150, 150, 150), 4), (int)((-xFrom) * pictureBox1.Width / (xTo - xFrom)), 0, (int)((-xFrom) * pictureBox1.Width / (xTo - xFrom)), pictureBox1.Height);
            //график:
            for (double t = xFrom; t < xTo; t += 1 / (double)pictureBox1.Width)
            {
                x = ((t - xFrom) * pictureBox1.Width / (xTo - xFrom));
                y = pictureBox1.Height - (f(t) - yFrom) * pictureBox1.Height / (yTo - yFrom);
                if (y > 0 && y < pictureBox1.Height)
                    bit.SetPixel((int)(x), (int)(y), Color.Black);
                if (y + 1 > 0 && y + 1 < pictureBox1.Height)
                    bit.SetPixel((int)(x), (int)(y + 1), Color.Black);
                if (y - 1 > 0 && y - 1 < pictureBox1.Height)
                    bit.SetPixel((int)(x), (int)(y - 1), Color.Black);
            }
            if (is_find)
            {
                if (checkBox1.Checked)
                {//метод дехотомии
                    find_min_dihotomia(xFrom, xTo, Convert.ToDouble(textBox4.Text), Convert.ToDouble(textBox3.Text));
                }
                if (checkBox2.Checked)
                {//метод золотого сечения
                    find_min_gold(xFrom, xTo, Convert.ToDouble(textBox5.Text), Convert.ToDouble(textBox6.Text));
                }
                if (checkBox3.Checked)
                {//метод фибоначи
                    find_min_fibonachi(xFrom, xTo, Convert.ToDouble(textBox9.Text), Convert.ToDouble(textBox10.Text), Convert.ToInt32(textBox8.Text));
                }
            }
            //легенда:
            //if (checkBox1.Checked)
            //{
            //    g.DrawLine(new Pen(Color.Black), pictureBox1.Width - 60, pictureBox1.Height - 75, pictureBox1.Width - 40, pictureBox1.Height - 75);
            //    g.DrawString("L(x)", this.Font, new SolidBrush(Color.Gray), pictureBox1.Width - 40, pictureBox1.Height - 85);
            //}
            //if (checkBox2.Checked)
            //{
            //
            //    g.DrawLine(new Pen(Color.Green), pictureBox1.Width - 60, pictureBox1.Height - 50, pictureBox1.Width - 40, pictureBox1.Height - 50);
            //    g.DrawString("P(x)", this.Font, new SolidBrush(Color.Gray), pictureBox1.Width - 40, pictureBox1.Height - 60);
            //}
            //if (checkBox4.Checked)
            //{
            //
            //    g.DrawLine(new Pen(Color.Red), pictureBox1.Width - 60, pictureBox1.Height - 25, pictureBox1.Width - 40, pictureBox1.Height - 25);
            //    g.DrawString("Q(x)", this.Font, new SolidBrush(Color.Gray), pictureBox1.Width - 40, pictureBox1.Height - 35);
            //}
            //обновляем значения таблицы
            //if (X.Count >= 2)
            //{
            //    SetTable();
            //    double[] pol = MNK(X, Y, (int)numericUpDown1.Value + 1);
            //    double mnk = 0, midX = X[0] - (X[1] - X[0]) / 2;
            //    for (int i = 0; i < pol.Length; i++)
            //        mnk += pol[i] * Math.Pow(midX, pol.Length - 1 - i);
            //    dataGridView1.Rows.Add(-1, midX, IPL(X, Y, midX), IPN(X, Y, midX), mnk);
            //    for (int i = 0; i < X.Count - 1; i++)
            //    {
            //        mnk = 0;
            //        midX = X[i] + (X[i + 1] - X[i]) / 2;
            //        for (int j = 0; j < pol.Length; j++)
            //            mnk += pol[j] * Math.Pow(midX, pol.Length - 1 - j);
            //        dataGridView1.Rows.Add(i, midX, IPL(X, Y, midX), IPN(X, Y, midX), mnk);
            //    }
            //    mnk = 0;
            //    midX = X[X.Count - 1] + (X[X.Count - 1] - X[X.Count - 2]) / 2;
            //    for (int i = 0; i < pol.Length; i++)
            //        mnk += pol[i] * Math.Pow(midX, pol.Length - 1 - i);
            //    dataGridView1.Rows.Add(X.Count - 1, midX, IPL(X, Y, midX), IPN(X, Y, midX), mnk);
            //}
            pictureBox1.Image = bit;
        }
        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Width > 0 && pictureBox1.Height > 0)
            {
                bit = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                g = Graphics.FromImage(bit);
            }
            Draw(false);
        }

        private void tabPage2_SizeChanged(object sender, EventArgs e)
        {
            int sizi = (tabPage2.Width - 8) / 3;
            listBox1.Width = sizi;
            listBox1.Location = new Point(2, listBox1.Location.Y);
            label8.Location = new Point(2, label8.Location.Y);

            listBox2.Width = sizi;
            listBox2.Location = new Point(4 + sizi, listBox2.Location.Y);
            label12.Location = new Point(4 + sizi, label12.Location.Y);

            listBox3.Width = sizi;
            listBox3.Location = new Point(6 + 2 * sizi, listBox3.Location.Y);
            label13.Location = new Point(6 + 2 * sizi, label13.Location.Y);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            panel2.Enabled = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            panel3.Enabled = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            panel4.Enabled = checkBox3.Checked;
        }
    }
}
