using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ka = 7;//1!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            kb = 4;
            kc = 2;
            kd = -10;
            ke = -4;
            k1 = 0.01f;
            k2 = 0.01f;
            input_mode = 3;
            Wdth = pictureBox1.Width;
            Hght = pictureBox1.Height;
            bitmap = new Bitmap(Wdth, Hght);
        }
        float ka, kb, kc, kd, ke, kf, kg, kh, ki;
        float k1;
        float k2;
        //PointF[] ogranichenias = { new PointF(0, 4), new PointF(4, 0), new PointF(5, 2.75f) };
        Bitmap bitmap,bit_pts;
        Graphics g;
        int Wdth, Hght; float[] args = { 1, 5, 15, 30, 50, 100, 200 };
        List<PointF> M1, M2, M3;
        float CurX;
        float CurY;
        int input_mode = 3;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //g = Graphics.FromImage(bit_pts);
            if (input_mode == 2)
            {
                //bit_pts = new Bitmap(bitmap.Width, bitmap.Height);
                textBox2.Text = Xconvert(CurX, k1).ToString();
                textBox3.Text = Yconvert(CurY, k2).ToString();
                solve(Xconvert(CurX, k1), Yconvert(CurY, k2));
                Bitmap result = MergedBitmaps(bit_pts, bitmap);
                pictureBox1.Image = result;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            CurX = e.X;
            CurY = e.Y;
            //label2.Text = "f(" + Xconvert(CurX,k1)+ ", " + Yconvert(CurY,k2) + ") = " + f(Xconvert(CurX,k1), Yconvert(CurY,k2));
            g = pictureBox1.CreateGraphics();
            pictureBox1.Refresh();
            g.DrawLine(new Pen(Color.Black, 0.5f), CurX, ReYconvert(-100, k2), CurX, ReYconvert(100, k2));
            g.DrawLine(new Pen(Color.Black, 0.5f), ReXconvert(-100, k1), CurY, ReXconvert(100, k1), CurY);
            g.DrawString(Yconvert(CurY,k2).ToString(), new Font("Arial", 16), new SolidBrush(Color.Black), ReXconvert(0, k1), CurY);
            g.DrawString(Xconvert(CurX,k1).ToString(), new Font("Arial", 16), new SolidBrush(Color.Black), CurX, ReYconvert(0, k2));
            //bit_pts = new Bitmap(bitmap.Width, bitmap.Height);
            //g = Graphics.FromImage(bit_pts);
            if (input_mode == 1)
            {
                textBox2.Text = Xconvert(CurX, k1).ToString();
                textBox3.Text = Yconvert(CurY, k2).ToString();
                solve(Xconvert(CurX, k1), Yconvert(CurY, k2));
                Bitmap result = MergedBitmaps(bit_pts, bitmap);
                pictureBox1.Image = result;
            }
        }

        void solve(float x,float y)
        {
            float epsilon = float.Parse(textBox1.Text);
            bit_pts = new Bitmap(bitmap.Width, bitmap.Height);
            g = Graphics.FromImage(bit_pts);
            PointF h;
            PointF p = new PointF(x,y);
            PointF pi, temp;
            //label5.Text = "";
            int iiii = 0;
            richTextBox1.Text = "";
            string text = "";
            do
            {
                //PointF f_poshuk = new PointF(f_pohidna_x1(p.X,p.Y)*(x_1-p.X),
                //    f_pohidna_x2(p.X, p.Y) * (x_2 - p.Y));
                //3!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //float[,] table = {  {4, 1, 1},
                //                {-16, -1, -4},
                //                {-44, -11, 4},
                //                {-p.X*f_pohidna_x1(p.X,p.Y)-p.Y*f_pohidna_x2(p.X,p.Y), -Math.Abs(f_pohidna_x1(p.X,p.Y)), -Math.Abs(f_pohidna_x2(p.X,p.Y))} };
                float[,] table = {  {4, 1,  1},
                                {12,  1,  3},
                                {0, 1, 0},
                                {-p.X*f_pohidna_x1(p.X,p.Y)-p.Y*f_pohidna_x2(p.X,p.Y), -f_pohidna_x1(p.X,p.Y), -f_pohidna_x2(p.X,p.Y)} };

                //float[,] table = {  {7, 3,  1},
                //                {-5,  -4,  3},
                //                {-15, 1, -4},
                //                {-p.X*f_pohidna_x1(p.X,p.Y)-p.Y*f_pohidna_x2(p.X,p.Y), -f_pohidna_x1(p.X,p.Y), -f_pohidna_x2(p.X,p.Y)} };
                float[] result = new float[2];
                float[,] table_result;
                Simplex S = new Simplex(table);
                table_result = S.Calculate(result);
                //richTextBox1.Text += "\ntable "+iiii+" :\n";
                //for (int i = 0; i < table_result.GetLength(0); i++)
                //{
                //    for (int j = 0; j < table_result.GetLength(1); j++)
                //        richTextBox1.Text += table_result[i, j] + " ";
                //        richTextBox1.Text+="\n";
                //}
                text += (iiii + 1).ToString() + ") x(" + (iiii).ToString() + ") = (" + p.X + ", " + p.Y + ")\n";
                g.FillEllipse(new SolidBrush(Color.Green), ReXconvert(p.X, k1) - 5, ReYconvert(p.Y, k2) - 5, 10, 10);
                if (iiii % 2 == 1)
                {
                    float tp = result[1];
                    result[1] = result[0];
                    result[0] = tp;
                }
                text += "Simplex solution:\n";
                text += "    f(x1,x2) = (" + f_pohidna_x1(p.X, p.Y) + ")*x1 + (" + f_pohidna_x2(p.X, p.Y) + ")*x2 + (" + (-p.X * f_pohidna_x1(p.X, p.Y) - p.Y * f_pohidna_x2(p.X, p.Y)).ToString() + ")\n";
                text += "    f(" + result[1] + ", " + result[0] + ") = " + f_for_simplex(result[1], result[0], p) + "\n";
                text += "    x_(" + (iiii).ToString() + ") = (" + result[1] + ", " + result[0] + ")\n";
                //+ "f("+ "0"+", "+ "4"+") = "+ f_for_simplex(0,4,p)+"\n"
                //+ "f("+ "4"+", "+ "0"+") = "+ f_for_simplex(4,0,p)+"\n"
                h = new PointF(result[1] - p.X, result[0] - p.Y);
                text += "h_" + (iiii).ToString() + " = (" + h.X + "," + h.Y + ")\n";
                float alpha = 1;
                if ((2 * (ka * h.X * h.X + kb * h.X * h.Y + kc * h.Y * h.Y)) != 0)
                {
                    alpha = (-(2 * ka * h.X * p.X + kb * h.X * p.Y + kb * h.Y * p.X + 2 * kc * h.Y * p.Y + kd * h.X + ke * h.Y)) /
                                                (2 * (ka * h.X * h.X + kb * h.X * h.Y + kc * h.Y * h.Y));
                }
                else
                {
                    if ((-(2 * ka * h.X * p.X + kb * h.X * p.Y + kb * h.Y * p.X + 2 * kc * h.Y * p.Y + kd * h.X + ke * h.Y)) > 1)
                        alpha = 1;
                    else //if ((-(2 * ka * h.X * p.X + kb * h.X * p.Y + kb * h.Y * p.X + 2 * kc * h.Y * p.Y + kd * h.X + ke * h.Y)) < 0)
                        alpha = 0;
                }
                alpha = alpha < 0 ? 0 : (alpha > 1 ? 1 : alpha);
                text += "alpha" + " = " + alpha.ToString() + "\n";
                pi = new PointF(p.X + alpha * h.X, p.Y + alpha * h.Y);
                text += "x(" + (iiii + 1).ToString() + ") = (" + pi.X + ", " + pi.Y + ")\n";
                g.FillEllipse(new SolidBrush(Color.Green), ReXconvert(pi.X, k1) - 5, ReYconvert(pi.Y, k2) - 5, 10, 10);
                g.DrawLine(new Pen(Color.Green, 2), ReXconvert(p.X, k1), ReYconvert(p.Y, k2), ReXconvert(pi.X, k1), ReYconvert(pi.Y, k2));
                temp = new PointF(p.X, p.Y);
                p = new PointF(pi.X, pi.Y);
                text += "f = " + f(pi.X, pi.Y) + "\n";
                text += "||x(" + (iiii).ToString() + "), x(" + (iiii + 1).ToString() + ")|| = " + dist(temp, pi) + "\n\n";
                iiii++;
            }
            while (dist(temp, pi) > epsilon && iiii < 1000);
            richTextBox1.Text = text;
        }
        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            Wdth = pictureBox1.Width;
            Hght = pictureBox1.Height;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            k1 = trackBar1.Value * 0.005f;
            k2 = trackBar1.Value * 0.005f;
            label4.Text = k1.ToString();
            Draw();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            Draw();
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {//по кнопке
            input_mode = 3;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {//по мышке
            input_mode = 2;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {//динамически
            input_mode = 1;
        }
        float f(float x1, float x2)
        {
            //return x1 * x1*x2 - x1*x2 * x2;
            return ka * x1 * x1 + kb * x1 * x2 + kc * x2 * x2 + kd * x1 + ke * x2;
            //return ka * x1 * x1 * x1 + kb * x1 * x1 * x2 + kc * x1 * x2 * x2 + kd * x2 * x2 * x2 +
            //    ke * x1 * x1 + kf * x1 * x2 + kg * x2 * x2 +
            //    kh * x1 + ki * x2;
            //return (float)(ka * x1 * Math.Sin(kb * x1)+ kb * x1 * Math.Sin(kc * x2)+
            //    kd * x2 * Math.Sin(ke * x1)+ kf * x2 * Math.Sin(kg * x2))+kh;
            //4x^2 + 2xy + 4y^2 - 2x - 3y = c
        }
        float f_pohidna_x1(float x1, float x2)
        {
            return ka * 2 * x1 + kb * x2 + kd;
        }
        float f_pohidna_x2(float x1, float x2)
        {
            return kb * x1 + kc * 2 * x2 + ke;
        }
        float f_for_simplex(float x1, float x2, PointF p)
        {
            return f_pohidna_x1(p.X, p.Y) * (x1 - p.X) + f_pohidna_x2(p.X, p.Y) * (x2 - p.Y);
        }
        float dist(PointF p1, PointF p2)
        {
            return (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
        void Draw()
        {
            Task th = Task.Run(() => megaDraw());
            th.Wait();
            bitmap = new Bitmap(pictureBox1.Image);
            g = Graphics.FromImage(bitmap);
            //2!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            PointF[] ogranichenias = { new PointF(0, 4), new PointF(4, 0), new PointF(12, 0) };
            //PointF[] ogranichenias = { new PointF(1, 4), new PointF(5, 5), new PointF(2, 1) };
            //рисуем ограничения:
            for (int i = 0; i < ogranichenias.Length; i++)
            {
                ogranichenias[i] = new PointF(ReXconvert(ogranichenias[i].X, k1), ReYconvert(ogranichenias[i].Y, k2));
            }
            g.FillPolygon(new SolidBrush(Color.FromArgb(100,255,0,0)), ogranichenias);
            g.DrawPolygon(new Pen(Color.Black,2), ogranichenias);
            //рисуем сетку координат
            g.DrawLine(new Pen(Color.Blue, 2), ReXconvert(0, k1), ReYconvert(-10, k2), ReXconvert(0, k1), ReYconvert(10, k2));
            g.DrawLine(new Pen(Color.Red, 2), ReXconvert(-10, k1), ReYconvert(0, k2), ReXconvert(10, k1), ReYconvert(0, k2));
            for (int i = -20; i <= 20; i += 2)
            {
                if (i != 0)
                {
                    g.DrawLine(new Pen(Color.FromArgb(100,100,100,100), 0.5f), ReXconvert(i, k1), ReYconvert(-100, k2), ReXconvert(i, k1), ReYconvert(100, k2));
                    g.DrawLine(new Pen(Color.FromArgb(100,100,100,100), 0.5f), ReXconvert(-100, k1), ReYconvert(i, k2), ReXconvert(100, k1), ReYconvert(i, k2));
                }
                g.DrawString(i.ToString(), new Font("Arial", 16), new SolidBrush(Color.Black), ReXconvert(0,k1), ReYconvert(i,k2));
                g.DrawString(i.ToString(), new Font("Arial", 16), new SolidBrush(Color.Black), ReXconvert(i,k1), ReYconvert(0,k2));
            }
            //g = pictureBox1.CreateGraphics();
            //pictureBox1.Refresh();
            bit_pts = new Bitmap(bitmap.Width, bitmap.Height);
            //g = Graphics.FromImage(bit_pts);
            if (input_mode == 3)
            {
                solve(float.Parse(textBox2.Text), float.Parse(textBox3.Text));
            }
            Bitmap result = MergedBitmaps(bit_pts,bitmap);
            pictureBox1.Image = result;
        }
        void megaDraw()
        {//будуємо лінії рівня ф-ї f(x1,x2)
            //Bitmap b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Wdth = pictureBox1.Width > 0 ? pictureBox1.Width : 1;
            Hght = pictureBox1.Height > 0 ? pictureBox1.Height : 1;
            Bitmap b = new Bitmap(Wdth, Hght);
            //bool flag = radioButton1.Checked;
            for (int x = 0; x < Wdth; x++)
                for (int y = 0; y < Hght; y++)
                {//x1=x, x2=y
                    float x1 = Xconvert(x, k1), y1 = Yconvert(y, k2);
                    float koef = 255;//Math.Abs(f(x1, y1) - args[0]) * 100;
                    foreach (float arg in args)
                    {
                        float t = Math.Abs(f(x1, y1) - arg) * 100;
                        //float t = (flag) ? (Math.Abs(f(x1, y1) - arg) * 100)
                        //    : (Math.Abs(rez(x1, y1) - arg) * 70);
                        if (koef > t&&t<20) koef = t;
                    }
                    //if (koef < 0) koef = 0;
                    //if (koef > 255) koef = 255;
                    //koef = 255 - koef;
                    b.SetPixel(x, y, Color.FromArgb(((int)koef), ((int)koef), ((int)koef)));
                    //b.SetPixel(x, y, Color.FromArgb(((int)koef%20)*12, ((int)koef%30)*8, ((int)koef%50)*5));
                }
            pictureBox1.Image = b;
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
        float Xconvert(float x, float k)
        {
            x -= Wdth / 2;
            x = x * k;
            return x;
        }
        float Yconvert(float y, float k)
        {
            y -= Hght / 2;
            y = -y * k;
            return y;
        }
        float ReXconvert(float x, float k)
        {
            x = x / k;
            x += Wdth / 2;
            return x;
        }
        float ReYconvert(float y, float k)
        {
            y = -y / k;
            y += Hght / 2;
            return y;
        }
    }
}
