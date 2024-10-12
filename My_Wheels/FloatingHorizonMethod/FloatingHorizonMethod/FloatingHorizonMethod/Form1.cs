using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FloatingHorizonMethod
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            bit = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bit);
            E1 = new Vector(1, 0, 0);
            E2 = new Vector(0, 1, 0);
            E3 = new Vector(0, 0, 1);
            MaxCol = new List<float>();
            MinCol = new List<float>();
            MaxStr = new List<float>();
            MinStr = new List<float>();
        }
        List<float> MaxCol;
        List<float> MinCol;
        List<float> MaxStr;
        List<float> MinStr;
        Bitmap bit;
        Graphics g;
        float C1, C2, C3, C4, alpha, betta, gamma;
        Vector E1, E2, E3;
        class Vector
        {
            //C[0]~x;
            //C[1]~y;
            //C[2]~z;
            //...
            public double[] C;//некоторое количество "измерений" (или координат) вектора
            public double this[int i] => C[i];//для прямого обращения к элементам вектора
            public Vector() { }
            public Vector(Vector a)
            {
                C = new double[a.C.Length];
                for (int i = 0; i < a.C.Length; i++)
                {
                    C[i] = a[i];
                }
            }
            public Vector(params double[] coord1)
            {
                C = new double[coord1.Length];
                for (int i = 0; i < coord1.Length; i++)
                {
                    C[i] = coord1[i];
                }
            }
            public static Vector operator +(Vector a, Vector b)
            {//если вектора имеют разную размерность, то возвращаемый вектор
             //будет содержать в себе минимальное количество параметров 
                Vector x = new Vector();// = new Vector(a.C[0] + b.C[0], a.C[1] + b.C[1],...);
                int min;
                if (a.C.Length < b.C.Length)
                    min = a.C.Length;
                else
                    min = b.C.Length;
                x.C = new double[min];
                for (int i = 0; i < min; i++)
                    x.C[i] = a[i] + b[i];
                return x;
            }
            public static Vector operator -(Vector a, Vector b)
            {//если вектора имеют разную размерность, то возвращаемый вектор
             //будет содержать в себе минимальное количество параметров 
                Vector x = new Vector();// = new Vector(a.C[0] + b.C[0], a.C[1] + b.C[1],...);
                int min;
                if (a.C.Length < b.C.Length)
                    min = a.C.Length;
                else
                    min = b.C.Length;
                x.C = new double[min];
                for (int i = 0; i < min; i++)
                    x.C[i] = a[i] - b[i];
                return x;
            }
            public static Vector operator *(double s, Vector a)
            {//умножение скаляра на вектор
                Vector x = a;
                for (int i = 0; i < x.C.Length; i++)
                    x.C[i] *= s;
                return x;
            }
            public static Vector operator *(Vector a, Vector b)
            {//векторное умножение
                return new Vector(a[1] * b[2] - a[2] * b[1], a[2] * b[0] - a[0] * b[2], a[0] * b[1] - a[1] * b[0]);
            }
            public static double operator &(Vector a, Vector b)
            {//скалярное умножение
                double x = 0;
                int min;
                if (a.C.Length < b.C.Length)
                    min = a.C.Length;
                else
                    min = b.C.Length;
                for (int i = 0; i < min; i++)
                    x += a[i] * b[i];
                return x;
            }
            public static double operator !(Vector a)
            {//нахождение длинны вектора
                double s = 0;
                for (int i = 0; i < a.C.Length; i++)
                    s += a[i] * a[i];
                return Math.Sqrt(s);
                //return new Vector(-a[0], -a[1], -a[2], a[3]);//спряженый кватернион
            }
            public static Vector operator /(Vector a, Vector b)
            {//кватернионное умножение
                if (a.C.Length < 4)
                    a = new Vector(a[0], a[1], a[2], 0);
                if (b.C.Length < 4)
                    b = new Vector(b[0], b[1], b[2], 0);
                Vector x = new Vector(0, 0, 0, 0);
                x.C[3] = a[3] * b[3] - a[0] * b[0] - a[1] * b[1] - a[2] * b[2];
                Vector v = new Vector();//векторная часть
                v = (a[3] * b) + (b[3] * a) + (a * b);
                x.C[2] = v[2];
                x.C[1] = v[1];
                x.C[0] = v[0];
                return x;
            }
            public override string ToString()
            {//конвертация в стринг
                string x = "( ";
                for (int i = 0; i < C.Length; i++)
                    x += (Math.Round(C[i], 2)).ToString() + " ";
                x += ")";
                return x;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Draw();
            //Task task = Task.Run(() => Draw()); 

        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if(pictureBox1.Width>0&&pictureBox1.Height>0)
            {
                bit = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                g = Graphics.FromImage(bit);
            }
            //Task task = Task.Run(() => Draw());
            Draw();
        }
        //private int factorial(int t)
        //{
        //    int answer = 1;
        //    for(int i=1;i<=t;i++)
        //        answer *= i;
        //    return answer;
        //}
        //private float Cos(float x)
        //{
        //    float x2=x*x;
        //    float sum = 0;
        //    for(int i=2;i<=6;i+=2)
        //    {
        //        sum +=x2/ (float)factorial(i);
        //        x2 *= (-x) * x;
        //    }
        //    return 1+sum;
        //}
        //private float Sin(float x)
        //{
        //    float x2 = x;
        //    float sum = 0;
        //    for (int i = 1; i <= 5; i += 2)
        //    {
        //        sum += x2 / (float)factorial(i);
        //        x2 *= (-x) * x;
        //    }
        //    return sum;
        //}
        float diff = 10;
        private void button2_Click(object sender, EventArgs e)
        {
            alpha = -diff;
            betta = 0;
            gamma = 0;
            rotate();
            //Task task = Task.Run(() => Draw());
            Draw();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            alpha = diff;
            betta = 0;
            gamma = 0;
            rotate();
            //Task task = Task.Run(() => Draw());
            Draw();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            alpha = 0;
            betta = -diff;
            gamma = 0;
            rotate();
            //Task task = Task.Run(() => Draw());
            Draw();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            alpha = 0;
            betta = diff;
            gamma = 0;
            rotate();
            //Task task = Task.Run(() => Draw());
            Draw();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            alpha = 0;
            betta = 0;
            gamma = -diff;
            rotate();
            //Task task = Task.Run(() => Draw());
            Draw();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            alpha = 0;
            betta = 0;
            gamma = diff;
            rotate();
            //Task task = Task.Run(() => Draw());
            Draw();
        }
        float Koef1 = 1;
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            Koef1 = trackBar3.Value/10.0f;
            //Task task = Task.Run(() => Draw());
            Draw();
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            C4 = trackBar5.Value;
            //Task task = Task.Run(() => Draw());
            Draw();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            C1 = trackBar1.Value;
            //Task task = Task.Run(() => Draw());
            Draw();
            trackBar2.Visible = true;
            trackBar4.Visible = true;
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            C2 = trackBar2.Value;
            //Task task = Task.Run(() => Draw());
            Draw();
        }
        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            C3 = trackBar4.Value;
            //Task task = Task.Run(() => Draw());
            Draw();
        }
        private void rotate()
        {
            //PointF tp = new PointF(p_in.X, p_in.Y);
            float angle1 = alpha*(float)Math.PI / (float)180;
            float angle2 = betta*(float)Math.PI / (float)180;
            float angle3 = gamma*(float)Math.PI / (float)180;
            Vector e1, e2, e3;
            e1 = E1;
            e2 = E2;
            e3 = E3;
            if (angle1 != 0)
            {
                //E1 = e1;// new Vector(1 * e1.C[0] + 0 * e2.C[0] + 0 * e3.C[0], 1 * e1.C[1] + 0 * e2.C[1] + 0 * e3.C[1], 1 * e1.C[2] + 0 * e2.C[2] + 0 * e3.C[2]);
                E2 = new Vector(0 * e1.C[0] + Math.Cos(angle1) * e2.C[0] + Math.Sin(angle1) * e3.C[0], 0 * e1.C[1] + Math.Cos(angle1) * e2.C[1] + Math.Sin(angle1) * e3.C[1], 0 * e1.C[2] + Math.Cos(angle1) * e2.C[2] + Math.Sin(angle1) * e3.C[2]);
                E3 = new Vector(0 * e1.C[0] - Math.Sin(angle1) * e2.C[0] + Math.Cos(angle1) * e3.C[0], 0 * e1.C[1] - Math.Sin(angle1) * e2.C[1] + Math.Cos(angle1) * e3.C[1], 0 * e1.C[2] - Math.Sin(angle1) * e2.C[2] + Math.Cos(angle1) * e3.C[2]);
                //e1 = E1;
                e2 = E2;
                e3 = E3;

            }
            if (angle2 != 0)
            {
                E1 = new Vector(Math.Cos(angle2) * e1.C[0] + 0 * e2.C[0] + Math.Sin(angle2) * e3.C[0], Math.Cos(angle2) * e1.C[1] + 0 * e2.C[1] + Math.Sin(angle2) * e3.C[1], Math.Cos(angle2) * e1.C[2] + 0 * e2.C[2] + Math.Sin(angle2) * e3.C[2]);
                //E2 = e2;// new Vector(0 * e1.C[0] + 1 * e2.C[0] + 0 * e3.C[0], 0 * e1.C[1] + 1 * e2.C[1] + 0 * e3.C[1], 0 * e1.C[2] + 1 * e2.C[2] + 0 * e3.C[2]);
                E3 = new Vector(-Math.Sin(angle2) * e1.C[0] + 0 * e2.C[0] + Math.Cos(angle2) * e3.C[0], -Math.Sin(angle2) * e1.C[1] + 0 * e2.C[1] + Math.Cos(angle2) * e3.C[1], -Math.Sin(angle2) * e1.C[2] + 0 * e2.C[2] + Math.Cos(angle2) * e3.C[2]);
                e1 = E1;
                //e2 = E2;
                e3 = E3;
            }
            if (angle3 != 0)
            {
                E1 = new Vector(Math.Cos(angle3) * e1.C[0] + Math.Sin(angle3) * e2.C[0] + 0 * e3.C[0], Math.Cos(angle3) * e1.C[1] + Math.Sin(angle3) * e2.C[1] + 0 * e3.C[1], Math.Cos(angle3) * e1.C[2] + Math.Sin(angle3) * e2.C[2] + 0 * e3.C[2]);
                E2 = new Vector(-Math.Sin(angle3) * e1.C[0] + Math.Cos(angle3) * e2.C[0] + 0 * e3.C[0], -Math.Sin(angle3) * e1.C[1] + Math.Cos(angle3) * e2.C[1] + 0 * e3.C[1], -Math.Sin(angle3) * e1.C[2] + Math.Cos(angle3) * e2.C[2] + 0 * e3.C[2]);
                //E3 = e3;// new Vector(0 * e1.C[0] + 0 * e2.C[0] + 1 * e3.C[0], 0 * e1.C[1] + 0 * e2.C[1] + 1 * e3.C[1], 0 * e1.C[2] + 0 * e2.C[2] + 1 * e3.C[2]);
            }
        }
        private Vector rot_p(Vector p)
        {
            float X = (float)p.C[0];
            float Y = (float)p.C[1];
            float Z = (float)p.C[2];
            float x = (float)E1.C[0] * X + (float)E1.C[1] * Y + (float)E1.C[2] * Z;
            float y = (float)E2.C[0] * X + (float)E2.C[1] * Y + (float)E2.C[2] * Z;
            float z = (float)E3.C[0] * X + (float)E3.C[1] * Y + (float)E3.C[2] * Z;
            return new Vector(x, y, z);
        }
        public float f(float x,float y)
        {
            //return C1*Cos(x) + Sin(y) * C2;
            //return C1 *(float)Math.Cos(x/10.0f) + C2*(float)Math.Sin(y/10.0f) +200;
            //x = C1;
            //y = C2;
            //x = (x-C2*3-200)/10.0f;
            //y = (y-200)/10.0f;
            //return (float)(Math.Sin(x * x + y * y) / (x * x + y * y))*(20*C1)+100;
            //x *= 0.01f;
            //y *= 0.01f;
            //return (float)(Math.Sin((y * y - x * x)) + Math.Cos((x * x - y * y)))*C1 +x*C2+200;
            //x *= 0.01f;
            //y *= 0.01f;
            //return (float)(Math.Sin(x)*Math.Sin(x)*Math.Cos(y)*C2) + 200;
            x = (x-400)*0.005f;
            y = (y-200)*0.005f;
            return (float)((C1*Math.Cos(C2*Math.Sqrt(x * x + y * y)))/Math.Sqrt(x*x+y*y) - (C1 * Math.Sin(C3 * Math.Sqrt((x-0.1f) * (x-0.1f) + y * y)))) + 200;

        }
        private void Draw()
        {
            int w = pictureBox1.Width, h = pictureBox1.Height;
            //Bitmap bit = new Bitmap(w, h);
            //Graphics g = Graphics.FromImage(bit);
            g.Clear(Color.White);
            //float maxR=0, maxL=pictureBox1.Width, maxU=pictureBox1.Height, maxD=0;
            MaxCol = new List<float>();
            MinCol = new List<float>();
            MaxStr = new List<float>();
            MinStr = new List<float>();
            for (int i = 0; i < w; i++)
            {
                MaxCol.Add(0);
                MinCol.Add(pictureBox1.Height);
            }
            for (int i = 0; i < h; i++)
            {
                MaxStr.Add(0);
                MinStr.Add(pictureBox1.Width);
            }
            Koef1 = 2f;
            for (int y = 200; y >= 0; y--)
            {
                for (int x = 0; x < 300; x++)
                {
                    float y1 = y * Koef1;
                    float x1 = (x * Koef1) + y;
                    float fun = f(x1, y1) + y * Koef1*(float)Math.Cos(Math.PI/4);
                    Vector p = new Vector(x1, y1, fun);
                    //p = rot_p(p);
                    y1 = (float)p.C[2];
                    x1 = (float)p.C[0];
                    //while ((int)x1 >= MaxCol.Count())
                    //{
                    //    MaxCol.Add(0);
                    //    MinCol.Add(pictureBox1.Height);
                    //}
                    //if(y1 > 0 && y1 < h)
                    //while ((int)y1 >= MaxStr.Count())
                    //{
                    //    MaxStr.Add(0);
                    //    MinStr.Add(pictureBox1.Width);
                    //}
                    if (y1 > 0 && y1 < h && x1 > 0 && x1 < w)
                    {
                        if (MaxCol[(int)x1] < y1 || MinCol[(int)x1] > y1)//|| MaxStr[(int)y1] < x1 || MinStr[(int)y1] > x1)
                        {
                            //bit.SetPixel((int)(x1), (int)y1, Color.Black);//.FromArgb(y,y,y));
                            if (MaxCol[(int)x1] < y1)
                            {
                                MaxCol[(int)x1] = y1;
                                bit.SetPixel((int)(x1), (int)y1, Color.Red);
                            }
                            if (MinCol[(int)x1] > y1)
                            {
                                MinCol[(int)x1] = y1;
                                bit.SetPixel((int)(x1), (int)y1, Color.Green);
                            }
                            //if (MaxStr[(int)y1] < x1)
                            //{
                            //    MaxStr[(int)y1] = x1;
                            //    //bit.SetPixel((int)(x1), (int)y1, Color.Red);
                            //}
                            //if (MinStr[(int)y1] > x1)
                            //{
                            //    MinStr[(int)y1] = x1;
                            //    //bit.SetPixel((int)(x1), (int)y1, Color.Green);
                            //}
                        }
                    }
                }
            }
            pictureBox1.Image = bit;
        }
    }
}
