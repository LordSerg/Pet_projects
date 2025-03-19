using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


namespace LAB4
{
    public class d2
    {

        public static Vector E1, E2;//базисы пространства
        public double X, Y;//координаты точек для юзера
        protected double x, y;//координаты для плоскости
        protected static double Angle=0;//угол поворота
        public int Size = 5;//размер точки
        public d2()
        {
            //reset();
        }
        public d2(double X_coord,double Y_coord)
        {
            X = X_coord;
            Y = Y_coord;
            x = X * E1.C[0] + Y * E1.C[1];
            y = X * E2.C[0] + Y * E2.C[1];
        }
        public static void rotate(double angle)
        {
            angle *= Math.PI / 180;//градус
            Vector e1, e2;//врЕменные координаты
            e1 = E1;
            e2 = E2;
            E1 = new Vector(Math.Cos(angle) * (e1.C[0]) + Math.Sin(angle) * (e2.C[0]), Math.Cos(angle) * (e1.C[1]) + Math.Sin(angle) * (e2.C[1]));
            E2 = new Vector(-Math.Sin(angle) * (e1.C[0]) + Math.Cos(angle) * (e2.C[0]), -Math.Sin(angle) * (e1.C[1]) + Math.Cos(angle) * (e2.C[1]));
            //матрица перехода:
            /* x       y
             cos(a)  sin(a)
            -sin(a)  cos(a)
             */
        }
        public static void draw_point(d2 point, PictureBox p, Graphics g)
        {//отображение точки на пикчерБоксе
            point.x = point.X * E1[0] + point.Y * E2[0];
            point.y = point.X * E1[1] + point.Y * E2[1];
            g.FillEllipse(Brushes.Black, (float)point.x + p.Width / 2, (float)point.y + p.Height / 2, point.Size, point.Size);
        }
        public static void draw_point(d2 point, PictureBox p, Graphics g,Brush bruh,bool is_draw_border)
        {//отображение точки на пикчерБоксе
            point.x = point.X * E1[0] + point.Y * E2[0];
            point.y = point.X * E1[1] + point.Y * E2[1];
            g.FillEllipse(bruh, (float)point.x + p.Width / 2, (float)point.y + p.Height / 2, point.Size, point.Size);
            //exlusively for current program:
            if (is_draw_border)
            {
                g.DrawEllipse(new Pen(Color.Black), (float)point.x + p.Width / 2, (float)point.y + p.Height / 2, point.Size, point.Size);
                //g.DrawEllipse(new Pen(Color.Black), (float)point.x + p.Width / 2 - 0.25f, (float)point.y + p.Height / 2 - 0.25f, point.Size + 0.5f, point.Size + 0.5f);
                g.DrawEllipse(new Pen(Color.White), (float)point.x + p.Width / 2 - 0.5f, (float)point.y + p.Height / 2 - 0.5f, point.Size + 1, point.Size + 1);
            }
        }
        public static void draw_line(d2 point1, d2 point2, PictureBox p, Graphics g)
        {
            point1.x = point1.X * E1[0] + point1.Y * E2[0];
            point1.y = point1.X * E1[1] + point1.Y * E2[1];
            point2.x = point2.X * E1[0] + point2.Y * E2[0];
            point2.y = point2.X * E1[1] + point2.Y * E2[1];
            g.DrawLine(Pens.Black, (float)point1.x + p.Width / 2, (float)point1.y + p.Height / 2, (float)point2.x + p.Width / 2, (float)point2.y + p.Height / 2);
        }
        public static void reset()
        {//анулирование базисов (или инициализация)
            E1 = new Vector(1, 0);//базис Х
            E2 = new Vector(0, 1);//базис У
        }
        static void draw_line_color(d2 point1, d2 point2, PictureBox p, Graphics g, Color col)
        {
            Pen pn = new Pen(col);
            g.DrawLine(pn, (float)point1.X + p.Width / 2, (float)point1.Y + p.Height / 2, (float)point2.X + p.Width / 2, (float)point2.Y + p.Height / 2);
        }
        public static void draw_basis(PictureBox p, Graphics g)
        {
            d2 x = new d2(100 * E1[0], 100 * E1[1]);
            d2 y = new d2(100 * E2[0], 100 * E2[1]);
            d2 zero = new d2(0, 0);
            draw_line_color(zero, x, p, g, Color.Red);
            draw_line_color(zero, y, p, g, Color.Green);
        }
    }
}
