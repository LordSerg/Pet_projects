using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Lab5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            bit = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bit);
            SetValues();
            label10.ForeColor = Color.Black;
            label11.ForeColor = Color.LightGray;
            k1 = trackBar10.Value * 0.005f;
            k2 = trackBar11.Value * 0.005f;
            label18.Text = "X1 = [" + (-k1) + ", " + k1 + "]";
            label19.Text = "X2 = [" + (-k2) + ", " + k2 + "]";
            M1=new List<PointF>();
            M2=new List<PointF>();
            M3=new List<PointF>();
            NumOfSteps = (int)numericUpDown1.Value;
            //Draw();
        }
        float ka=4, kb=2, kc=4, kd=-2, ke=-3,kf,kg,kh,ki;
        float k1;
        float k2;
        string sign(float x) { return x > 0 ? "" : "-"; }
        void SetValues()
        {
            ka = trackBar1.Value*1f;
            kb = trackBar2.Value*1f;
            kc = trackBar3.Value*1f;
            kd = trackBar4.Value*1f;
            ke = trackBar5.Value*1f;
            kf = trackBar6.Value*1f;
            kg = trackBar7.Value*1f;
            kh = trackBar8.Value*1f;
            ki = trackBar9.Value*1f;
            label1.Text = ka.ToString();
            label2.Text = kb.ToString();
            label3.Text = kc.ToString();
            label4.Text = kd.ToString();
            label5.Text = ke.ToString();
            label6.Text = kf.ToString();
            label7.Text = kg.ToString();
            label8.Text = kh.ToString();
            label9.Text = ki.ToString();
            //функція:
            label10.Text = "f = " + (ka==0?"":(((ka==1||ka==-1)?(sign(ka)+"x1^2 "):(ka + "*x1^2 ")))) +
                (kb==0?"":ka==0?(kb+"*x1*x2 "):(kb<0?(("- "+(-kb)+"*x1*x2 ")):("+ "+kb+"*x1*x2 "))) +
                (kc==0?"":(ka==0&&kb==0)?(kc+"*x2^2 "):(kc<0?(("- "+(-kc)+"*x2^2 ")):("+ "+kc+"*x2^2 ")))+
                (kd==0?"":(ka==0&&kb==0&&kc==0)?(kd+"*x1 "):(kd<0?(("- "+(-kd)+"*x1 ")):("+ "+kd+"*x1 "))) +
                (ke==0?"":(ka==0&&kb==0&&kc==0&&kd==0)?(ke + "*x2 ") :(ke<0?(("- "+(-ke)+"*x2 ")):("+ "+ke+"*x2 ")));
            label11.Text = "g = 100*(x2 - x1^2)^2 + (1 - x1)^2";
        }
        float f(float x1,float x2)
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
        float f_pohidna_x1 (float x1,float x2)
        {
            return ka * 2 * x1 + kb * x2 + kd;
        }
        float f_pohidna_x2(float x1, float x2)
        {
            return kb * x1 + kc * 2 * x2 + ke;
        }
        float rez(float x1,float x2)
        {//ф-я Резенброка
            return 100 * (x2 - x1 * x1) * (x2 - x1 * x1) + (1 - x1) * (1 - x1);
        }
        float rez_pohidna_x1(float x1, float x2)
        {//
            return 100 * 2 * (x2 - x1 * x1) * (-2 * x1) + 2 * (1 - x1) * (-1);
        }
        float rez_pohidna_x1_x1(float x1, float x2)
        {//
            return -400*x2+1200*x1*x1+2;
        }
        float rez_pohidna_x1_x2(float x1, float x2)
        {//
            return -400*x1;
        }
        float rez_pohidna_x2(float x1, float x2)
        {//
            return 100 * 2 * (x2 - x1 * x1);
        }
        float rez_pohidna_x2_x1(float x1, float x2)
        {//
            return -400*x1;
        }
        float rez_pohidna_x2_x2(float x1, float x2)
        {//
            return 200;
        }
        Bitmap bit;
        Graphics g;
        int Wdth = 400, Hght = 400;float[] args = { 1,5,15,30,50, 100 };
        List<PointF> M1, M2, M3;
        float CurX;
        float CurY;
        int NumOfSteps;
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label10.ForeColor = Color.LightGray;
            label11.ForeColor = Color.Black;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label10.ForeColor = Color.Black;
            label11.ForeColor = Color.LightGray;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = Xconvert(CurX, trackBar10.Value * 0.005f).ToString();
            textBox2.Text = Yconvert(CurY, trackBar11.Value * 0.005f).ToString();
        }
        private void trackBar10_Scroll(object sender, EventArgs e)
        {
            float kk = trackBar10.Value * 0.005f;
            label18.Text = "X1 = ["+(-kk)+", "+kk+"]";
            k1 = kk;
        }
        private void trackBar11_Scroll(object sender, EventArgs e)
        {
            float kk = trackBar11.Value * 0.005f;
            label19.Text = "X2 = ["+(-kk)+", "+kk+"]";
            k2 = kk;
        }
        private void tabPage4_SizeChanged(object sender, EventArgs e)
        {
            int sizi = (tabPage4.Width - 8) / 3;
            listBox1.Width = sizi;
            listBox1.Location = new Point(2, listBox1.Location.Y);
            label21.Location = new Point(2, label21.Location.Y);

            listBox2.Width = sizi;
            listBox2.Location = new Point(4 + sizi, listBox2.Location.Y);
            label22.Location = new Point(4 + sizi, label22.Location.Y);

            listBox3.Width = sizi;
            listBox3.Location = new Point(6 + 2 * sizi, listBox3.Location.Y);
            label23.Location = new Point(6 + 2 * sizi, label23.Location.Y);
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            CurX = e.X;
            CurY = e.Y;
            //g.Clear(Color.White);
            //g.FillEllipse(new SolidBrush(Color.Red), CurX - 50, CurY - 50, 100, 100);
            //CurX = Xconvert(CurX, k1);
            //CurY = Yconvert(CurY, k2);
            //g.FillEllipse(new SolidBrush(Color.Green), ReXconvert(CurX, k1) - 3, ReYconvert(CurY, k2) - 3, 6, 6);
            //pictureBox1.Image = bit;
        }

        float dist(PointF a,PointF b)
        {
            return (float)Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
        void solveDivMethod_g() 
        {
            PointF x0 = new PointF((float)Convert.ToDouble(textBox1.Text), (float)Convert.ToDouble(textBox2.Text));
            label20.Visible = true;
            PointF x1;
            float beta = (float)Convert.ToDouble(textBox4.Text);
            float eps = (float)Convert.ToDouble(textBox3.Text);
            M2.Clear();
            M2.Add(x0);
            listBox2.Items.Clear();
            listBox2.Items.Add("1. g(" + x0.X + ", " + x0.Y + ") = " + rez(x0.X, x0.Y));
            int i = 2;
            do
            {
                x1 = new PointF(x0.X - beta * rez_pohidna_x1(x0.X, x0.Y),
                    x0.Y - beta * rez_pohidna_x2(x0.X, x0.Y));
                if (rez(x1.X, x1.Y) >= rez(x0.X, x0.Y)) { break; }
                x0 = x1;
                M2.Add(x0);
                listBox2.Items.Add(i + ". g(" + x0.X + ", " + x0.Y + ") = " + rez(x0.X, x0.Y));
                i++;
            }
            while (eps > dist(x0, x1)&&i<10000);
            label20.Text = "g(" + x0.X + ", " + x0.Y + ") = " + rez(x0.X, x0.Y);
        }
        void solveDivMethod_f()
        {//метод дроблення кроку
            PointF x0 = new PointF((float)Convert.ToDouble(textBox1.Text), (float)Convert.ToDouble(textBox2.Text));
            label20.Visible = true;
            PointF x1=new PointF();
            float beta = (float)Convert.ToDouble(textBox4.Text);
            float eps = (float)Convert.ToDouble(textBox3.Text);
            M2.Clear();
            M2.Add(x0);
            listBox2.Items.Clear();
            listBox2.Items.Add("1. f(" + x0.X + ", " + x0.Y + ") = " + f(x0.X, x0.Y));
            int i = 2;
            do
            {
                x1 = new PointF(x0.X-beta*f_pohidna_x1(x0.X,x0.Y),
                    x0.Y - beta * f_pohidna_x2(x0.X, x0.Y));
                if (f(x1.X, x1.Y) >= f(x0.X, x0.Y)) 
                    beta *= 0.5f;
                if (eps > dist(x0, x1) || i >= 10000) { break; }
                x0 = x1;
                M2.Add(x0);
                listBox2.Items.Add(i+". f(" + x0.X + ", " + x0.Y + ") = " + f(x0.X, x0.Y));
                i++;
            }
            while (true);
            label20.Text = "f(" + x0.X + ", " + x0.Y + ") = " + f(x0.X, x0.Y);

        }
        private void button3_Click(object sender, EventArgs e)
        {//метод дроблення кроку
            if (radioButton1.Checked == true) solveDivMethod_f();
            else solveDivMethod_g();
            Draw(2);
        }
        void solveNewtonMetod_g() 
        {//метод ньютона
            PointF x0 = new PointF((float)Convert.ToDouble(textBox1.Text), (float)Convert.ToDouble(textBox2.Text));
            PointF x1;
            label20.Visible = true;
            listBox3.Items.Clear();
            M3.Clear();
            M3.Add(x0);
            listBox3.Items.Add("1. g(" + x0.X + ", " + x0.Y + ") = " + rez(x0.X, x0.Y));
            int i = 2;
            float eps = (float)Convert.ToDouble(textBox3.Text);
            float[] matr = { rez_pohidna_x1_x1(x0.X, x0.Y), rez_pohidna_x1_x2(x0.X, x0.Y), rez_pohidna_x2_x1(x0.X, x0.Y), rez_pohidna_x2_x2(x0.X, x0.Y) };
            float det = matr[0] * matr[3] - matr[1] * matr[2];
            float[] NOTmatr = { matr[3] / det, -matr[1] / det, -matr[2] / det, matr[0] / det };
            do
            {
                x1 = new PointF(x0.X - NOTmatr[0] * rez_pohidna_x1(x0.X, x0.Y) - NOTmatr[1] * rez_pohidna_x2(x0.X, x0.Y),
                    x0.Y - NOTmatr[2] * rez_pohidna_x1(x0.X, x0.Y) - NOTmatr[3] * rez_pohidna_x2(x0.X, x0.Y));
                if (eps > dist(x0, x1) || i >= 1000) { break; }
                x0 = x1;
                M3.Add(x0);
                listBox3.Items.Add(i + ". g(" + x0.X + ", " + x0.Y + ") = " + rez(x0.X, x0.Y));
                i++;
            }
            while (true);
            label20.Text = "g(" + x0.X + ", " + x0.Y + ") = " + rez(x0.X, x0.Y);
        }
        void solveNewtonMetod_f()
        {//метод ньютона
            PointF x0 = new PointF((float)Convert.ToDouble(textBox1.Text), (float)Convert.ToDouble(textBox2.Text));
            PointF x1;
            label20.Visible = true;
            listBox3.Items.Clear();
            M3.Clear();
            M3.Add(x0);
            listBox3.Items.Add("1. f(" + x0.X + ", " + x0.Y + ") = " + f(x0.X, x0.Y));
            int i = 2;
            float eps = (float)Convert.ToDouble(textBox3.Text);
            float[] matr = { 2 * ka, kb, kb, 2 * kc };
            float det = matr[0] * matr[3] - matr[1] * matr[2];
            float[] NOTmatr = { matr[3] / det, -matr[1] / det, -matr[2] / det, matr[0] / det };
            do
            {
                x1 = new PointF(x0.X - NOTmatr[0] * f_pohidna_x1(x0.X, x0.Y) - NOTmatr[1] * f_pohidna_x2(x0.X, x0.Y),
                    x0.Y - NOTmatr[2] * f_pohidna_x1(x0.X, x0.Y) - NOTmatr[3] * f_pohidna_x2(x0.X, x0.Y));
                if(eps > dist(x0, x1) || i >= 1000) { break; }
                x0 = x1;
                M3.Add(x0);
                listBox3.Items.Add(i + ". f(" + x0.X + ", " + x0.Y + ") = " + f(x0.X, x0.Y));
                i++;
            }
            while(true);
            label20.Text = "f(" + x0.X + ", " + x0.Y + ") = " + f(x0.X, x0.Y);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            NumOfSteps = (int)numericUpDown1.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {//метод ньютона
            if (radioButton1.Checked == true) solveNewtonMetod_f();
            else solveNewtonMetod_g();
            Draw(3);
        }
        //void solveFastDescentMethod_g() 
        //{
        //    PointF x0 = new PointF((float)Convert.ToDouble(textBox1.Text), (float)Convert.ToDouble(textBox2.Text));
        //    M1 = new List<PointF>();
        //    listBox1.Items.Clear();
        //    listBox1.Items.Add("1. g(" + x0.X + ", " + x0.Y + ") = " + rez(x0.X, x0.Y));
        //    M1.Add(x0);
        //    rez(x0.X, x0.Y);
        //    label20.Visible = true;
        //    //label20.Text = "f(" + x0.X + ", " + x0.Y + ") = " + f(x0.X, x0.Y);
        //    int n = 4;
        //    for (int i = 0; i < n; i++)
        //    {
        //        float ch;
        //        float zn;
        //        float g1 = rez_pohidna_x1(x0.X, x0.Y);
        //        float g2 = rez_pohidna_x2(x0.X, x0.Y);
        //        float x01 = x0.X;
        //        float x02 = x0.Y;
        //        if (2*g1*g1 == 0)
        //        {
        //            return;
        //        }
        //        zn = 2*g1*g1;
        //        ch = 2 * x01 * g1 - g2;
        //        float alpha0 = ch / zn;
        //        x0 = new PointF(x01 - alpha0 * g1, x02 - alpha0 * g2);
        //        //f(x0.X, x0.Y);
        //        //listBox1.Items.Add("alpha = " + alpha0);
        //        listBox1.Items.Add((i + 2).ToString() + ". g(" + x0.X + ", " + x0.Y + ") = " + rez(x0.X, x0.Y));
        //        M1.Add(x0);
        //    }
        //    label20.Text = "g(" + x0.X + ", " + x0.Y + ") = " + rez(x0.X, x0.Y);
        //}
        void solveFastDescentMethod_f()
        {//метод найшвидшого спуску
            PointF x0 = new PointF((float)Convert.ToDouble(textBox1.Text), (float)Convert.ToDouble(textBox2.Text));
            M1 = new List<PointF>();
            listBox1.Items.Clear();
            listBox1.Items.Add("1. f("+x0.X+", "+x0.Y+") = "+ f(x0.X, x0.Y));
            M1.Add(x0);
            f(x0.X, x0.Y);
            label20.Visible = true;
            //label20.Text = "f(" + x0.X + ", " + x0.Y + ") = " + f(x0.X, x0.Y);
            for (int i = 0; i < NumOfSteps; i++)
            {
                float ch;
                float zn;
                float f1 = f_pohidna_x1(x0.X, x0.Y);
                float f2 = f_pohidna_x2(x0.X, x0.Y);
                float x01 = x0.X;
                float x02 = x0.Y;
                if (ka * f1 * f1 + kb * f1 * f2 + kc * f2 * f2 == 0)
                {
                    return;
                }
                zn = 2*(ka * f1 * f1 + kb * f1 * f2 + kc * f2 * f2);
                ch = ka * 2 * x01 * f1 + kb * f1 * x02 + kb * f2 * x01 + kc * 2 * x02 * f2 + kd * f1 + ke * f2;
                float alpha0 = ch / zn;
                x0 = new PointF(x01 - alpha0 * f1, x02 - alpha0 * f2);
                //f(x0.X, x0.Y);
                //listBox1.Items.Add("alpha = " + alpha0);
                listBox1.Items.Add((i + 2).ToString() + ". f(" + x0.X + ", " + x0.Y + ") = " + f(x0.X, x0.Y));
                M1.Add(x0);
            }
            label20.Text = "f(" + x0.X + ", " + x0.Y + ") = " + f(x0.X, x0.Y);
        }
        private void button1_Click(object sender, EventArgs e)
        {//метод найшвидшого спуску
            solveFastDescentMethod_f();
            Draw(1);
        }
        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if(pictureBox1.Width>0&&pictureBox1.Height>0)
            {
                bit = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                g = Graphics.FromImage(bit);
                //Draw();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            SetValues();
            //Draw();
        }

        void Draw(int alg=0)
        {
            Task th = Task.Run(() => megaDraw());
            th.Wait();
            Bitmap bitmap =new Bitmap(pictureBox1.Image);
            g = Graphics.FromImage(bitmap);
            try
            {
                if (alg == 0)
                {//отрисовать все
                    for (int i = 0; i < M1.Count - 1; i++)
                    {
                        g.FillEllipse(new SolidBrush(Color.Red), ReXconvert(M1[i].X, k1) - 3, ReYconvert(M1[i].Y, k2) - 3, 6, 6);
                        g.DrawLine(new Pen(Color.Red, 2), ReXconvert(M1[i].X, k1), ReYconvert(M1[i].Y, k2), ReXconvert(M1[i + 1].X, k1), ReYconvert(M1[i + 1].Y, k2));
                    }
                    g.FillEllipse(new SolidBrush(Color.Red), ReXconvert(M1[M1.Count - 1].X, k1) - 3, ReYconvert(M1[M1.Count - 1].Y, k1) - 3, 6, 6);
                    for (int i = 0; i < M2.Count - 1; i++)
                    {
                        g.FillEllipse(new SolidBrush(Color.Green), ReXconvert(M2[i].X, k1) - 3, ReYconvert(M2[i].Y, k2) - 3, 6, 6);
                        g.DrawLine(new Pen(Color.Green, 2), ReXconvert(M2[i].X, k1), ReYconvert(M2[i].Y, k2), ReXconvert(M2[i + 1].X, k1), ReYconvert(M2[i + 1].Y, k2));
                    }
                    g.FillEllipse(new SolidBrush(Color.Green), ReXconvert(M2[M2.Count - 1].X, k1) - 3, ReYconvert(M2[M2.Count - 1].Y, k1) - 3, 6, 6);
                    for (int i = 0; i < M3.Count - 1; i++)
                    {
                        g.FillEllipse(new SolidBrush(Color.Blue), ReXconvert(M3[i].X, k1) - 3, ReYconvert(M3[i].Y, k2) - 3, 6, 6);
                        g.DrawLine(new Pen(Color.Blue, 2), ReXconvert(M3[i].X, k1), ReYconvert(M3[i].Y, k2), ReXconvert(M3[i + 1].X, k1), ReYconvert(M3[i + 1].Y, k2));
                    }
                    g.FillEllipse(new SolidBrush(Color.Blue), ReXconvert(M3[M3.Count - 1].X, k1) - 3, ReYconvert(M3[M3.Count - 1].Y, k1) - 3, 6, 6);
                }
                if (alg == 1 && M1.Count > 0)
                {
                    for (int i = 0; i < M1.Count - 1; i++)
                    {
                        g.FillEllipse(new SolidBrush(Color.Red), ReXconvert(M1[i].X, k1) - 3, ReYconvert(M1[i].Y, k2) - 3, 6, 6);
                        g.DrawLine(new Pen(Color.Red, 2), ReXconvert(M1[i].X, k1), ReYconvert(M1[i].Y, k2), ReXconvert(M1[i + 1].X, k1), ReYconvert(M1[i + 1].Y, k2));
                    }
                    g.FillEllipse(new SolidBrush(Color.Red), ReXconvert(M1[M1.Count - 1].X, k1) - 3, ReYconvert(M1[M1.Count - 1].Y, k1) - 3, 6, 6);
                }
                if (alg == 2 && M2.Count > 0)
                {


                    for (int i = 0; i < M2.Count - 1; i++)
                    {
                        g.FillEllipse(new SolidBrush(Color.Green), ReXconvert(M2[i].X, k1) - 3, ReYconvert(M2[i].Y, k2) - 3, 6, 6);
                        g.DrawLine(new Pen(Color.Green, 2), ReXconvert(M2[i].X, k1), ReYconvert(M2[i].Y, k2), ReXconvert(M2[i + 1].X, k1), ReYconvert(M2[i + 1].Y, k2));
                    }
                    g.FillEllipse(new SolidBrush(Color.Green), ReXconvert(M2[M2.Count - 1].X, k1) - 3, ReYconvert(M2[M2.Count - 1].Y, k1) - 3, 6, 6);

                }
                if (alg == 3 && M3.Count > 0)
                {
                    for (int i = 0; i < M3.Count - 1; i++)
                    {
                        g.FillEllipse(new SolidBrush(Color.Blue), ReXconvert(M3[i].X, k1) - 3, ReYconvert(M3[i].Y, k2) - 3, 6, 6);
                        g.DrawLine(new Pen(Color.Blue, 2), ReXconvert(M3[i].X, k1), ReYconvert(M3[i].Y, k2), ReXconvert(M3[i + 1].X, k1), ReYconvert(M3[i + 1].Y, k2));
                    }
                    g.FillEllipse(new SolidBrush(Color.Blue), ReXconvert(M3[M3.Count - 1].X, k1) - 3, ReYconvert(M3[M3.Count - 1].Y, k1) - 3, 6, 6);
                }
            }
            catch (Exception) { }
            pictureBox1.Image = bitmap;
        }
        void megaDraw()
        {//будуємо лінії рівня ф-ї f(x1,x2)
            //Bitmap b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Wdth = pictureBox1.Width>0? pictureBox1.Width:1;
            Hght = pictureBox1.Height>0? pictureBox1.Height:1;
            Bitmap b = new Bitmap(Wdth, Hght);
            bool flag = radioButton1.Checked;
            for (int x = 0; x < b.Width; x++)
                for (int y = 0; y < b.Height; y++)
                {//x1=x, x2=y
                    float x1 = Xconvert(x,k1), y1 = Yconvert(y,k2);
                    float koef = 255;//Math.Abs(f(x1, y1) - args[0]) * 100;
                    foreach (float arg in args)
                    {
                        //float t = Math.Abs(f(x1, y1) - arg) * 100;
                        float t = (flag)?(Math.Abs(f(x1, y1) - arg) * 100) 
                            :(Math.Abs(rez(x1, y1) - arg) * 70);
                        if (koef > t&&t<10) koef = t;
                    }
                    //if (koef < 0) koef = 0;
                    //if (koef > 255) koef = 255;
                    //koef = 255 - koef;
                    b.SetPixel(x, y, Color.FromArgb(((int)koef), ((int)koef), ((int)koef)));
                    //b.SetPixel(x, y, Color.FromArgb(((int)koef%20)*12, ((int)koef%30)*8, ((int)koef%50)*5));
                }
            pictureBox1.Image = b;
        }
        float Xconvert(float x, float k=0.01f)
        {
            x -= Wdth / 2;
            x=x*k;
            return x;
        }
        float Yconvert(float y, float k=0.01f)
        {
            y -= Hght / 2;
            y=y*k;
            return y;
        }
        float ReXconvert(float x,float k=0.01f)
        {
            x = x / k;
            x += Wdth / 2;
            return x;
        }
        float ReYconvert(float y, float k = 0.01f)
        {
            y = y / k;
            y += Hght / 2;
            return y;
        }
    }
}
