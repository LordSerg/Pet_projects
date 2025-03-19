using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace LAB4
{

    public class d3
    {
        public static Vector E1, E2, E3;//базисы пространства
        public double X, Y, Z;//координаты для юзера
        protected double x, y, z;//координаты для пространства
        protected static double AngleOX = 0, AngleOY = 0, AngleOZ = 0;//углы поворота
        public int Size = 5;
        public d3()
        {

        }
        public d3(double coord_x,double coord_y,double coord_z)
        {
            X = coord_x;
            Y = coord_y;
            Z = coord_z;
            x = E1.C[0] * X + E1.C[1] * Y + E1.C[2] * Z;
            y = E2.C[0] * X + E2.C[1] * Y + E2.C[2] * Z;
            z = E3.C[0] * X + E3.C[1] * Y + E3.C[2] * Z;
        }
        public static void rotate(double angle1,double angle2,double angle3)
        {
            angle1 *= Math.PI / 180;
            angle2 *= Math.PI / 180;
            angle3 *= Math.PI / 180;
            //AngleOX += angle1 * Math.PI / 180;//вокруг OX
            //AngleOY += angle2 * Math.PI / 180;//вокруг OY
            //AngleOZ += angle3 * Math.PI / 180;//вокруг OZ
            Vector e1, e2, e3;
            e1 = E1;
            e2 = E2;
            e3 = E3;
            //вариант с перемножением матриц:
            //E1 = new Vector(Math.Cos(angle2) * Math.Cos(angle3) * (e1.C[0]) + Math.Cos(angle2) * Math.Sin(angle3) * (e2.C[0]) + Math.Sin(angle2) * (e3.C[0]), Math.Cos(angle2) * Math.Cos(angle3) * (e1.C[1]) + Math.Cos(angle2) * Math.Sin(angle3) * (e2.C[1]) + Math.Sin(angle2) * (e3.C[1]), Math.Cos(angle2) * Math.Cos(angle3) * (e1.C[2]) + Math.Cos(angle2) * Math.Sin(angle3) * (e2.C[2]) + Math.Sin(angle2) * (e3.C[2]));
            //E2 = new Vector((-(Math.Sin(angle1) * Math.Sin(angle2) * Math.Cos(angle3)) - (Math.Cos(angle1) * Math.Sin(angle3))) * (e1.C[0]) + (-(Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle3)) + (Math.Cos(angle1) * Math.Cos(angle3))) * (e2.C[0]) + Math.Sin(angle1) * Math.Cos(angle2) * (e3.C[0]), (-(Math.Sin(angle1) * Math.Sin(angle2) * Math.Cos(angle3)) - (Math.Cos(angle1) * Math.Sin(angle3))) * (e1.C[1]) +(-(Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle3)) + (Math.Cos(angle1) * Math.Cos(angle3)))  *(e2.C[1]) + Math.Sin(angle1) * Math.Cos(angle2) * (e3.C[1]), (-(Math.Sin(angle1) * Math.Sin(angle2) * Math.Cos(angle3)) - (Math.Cos(angle1) * Math.Sin(angle3))) * (e1.C[2]) + (-(Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle3)) + (Math.Cos(angle1) * Math.Cos(angle3))) * (e2.C[2]) + Math.Sin(angle1) * Math.Cos(angle2)*(e3.C[2]));
            //E3 = new Vector((-(Math.Cos(angle1) * Math.Sin(angle2) * Math.Cos(angle3)) + (Math.Sin(angle1) * Math.Sin(angle3))) * (e1.C[0]) + (-(Math.Cos(angle1) * Math.Sin(angle2) * Math.Sin(angle3)) - (Math.Sin(angle1) * Math.Cos(angle3))) * (e2.C[0]) + Math.Cos(angle1) * Math.Cos(angle2) * (e3.C[0]), (-(Math.Cos(angle1) * Math.Sin(angle2) * Math.Cos(angle3)) + (Math.Sin(angle1) * Math.Sin(angle3))) * (e1.C[1]) +(-(Math.Cos(angle1) * Math.Sin(angle2) * Math.Sin(angle3)) - (Math.Sin(angle1) * Math.Cos(angle3)))  *(e2.C[1]) + Math.Cos(angle1) * Math.Cos(angle2) * (e3.C[1]), (-(Math.Cos(angle1) * Math.Sin(angle2) * Math.Cos(angle3)) + (Math.Sin(angle1) * Math.Sin(angle3))) * (e1.C[2]) + (-(Math.Cos(angle1) * Math.Sin(angle2) * Math.Sin(angle3)) - (Math.Sin(angle1) * Math.Cos(angle3))) * (e2.C[2]) + Math.Cos(angle1) * Math.Cos(angle2)*(e3.C[2]));
            //E1 = new Vector(1, 0, 0);
            //E2 = new Vector(0, 1, 0);
            //E3 = new Vector(0, 0, 1);
            //e1 = E1;
            //e2 = E2;
            //e3 = E3;

            //вариант с поочередным поворотом с начала по OX, потом OY, потом OZ
            
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
            
            //матрицы перехода:
            /*
             вокруг оси Х:
             1    0      0
             0  cos(a)  sin(a)
             0 -sin(a)  cos(a)

             вокруг оси Y:
             cos(b)  0  sin(b)
               0     1     0
            -sin(b)  0  cos(b)

             вокруг оси Z:
             cos(c)  sin(c)  0
            -sin(c)  cos(c)  0
               0      0      1
             
             перемножив эти матрицы получим:
             cos(b)cos(c)           cos(b)sin(c)            sin(b)

        -sin(a)sin(b)cos(c)     -sin(a)sin(b)sin(c)     sin(a)cos(b)
            -cos(a)sin(c)           +cos(a)cos(c)

        -cos(a)sin(b)cos(c)     -cos(a)sin(b)sin(c)     cos(a)cos(b)
            +sin(a)sin(c)           -sin(a)cos(c)
             */
        }
        public static void reset()
        {
            E1 = new Vector(1, 0, 0);
            E2 = new Vector(0, 1, 0);
            E3 = new Vector(0, 0, 1);
        }
        public static void draw_point1(d3 point, PictureBox p, Graphics g)
        {//отображение точки на пикчерБоксе
            point.x = point.X * E1.C[0] + point.Y * E2.C[0] + point.Z * E3.C[0];
            point.y = point.X * E1.C[1] + point.Y * E2.C[1] + point.Z * E3.C[1];
            g.FillEllipse(Brushes.White, (float)point.x + p.Width / 2, (float)point.y + p.Height / 2, 2, 2);
            //отличие этого отображения в том, что при вращении это отображение будет вращатся вокруг статичного внешнего базиса
        }
        public static void draw_line1(d3 point1,d3 point2, PictureBox p, Graphics g)
        {//отображение прямой на пикчерБоксе
            point1.x = point1.X * E1.C[0] + point1.Y * E1.C[1] + point1.Z * E1.C[2];
            point1.y = point1.X * E2.C[0] + point1.Y * E2.C[1] + point1.Z * E2.C[2];
            point2.x = point2.X * E1.C[0] + point2.Y * E1.C[1] + point2.Z * E1.C[2];
            point2.y = point2.X * E2.C[0] + point2.Y * E2.C[1] + point2.Z * E2.C[2];
            g.DrawLine(Pens.White, (float)point1.x + p.Width / 2, (float)point1.y + p.Height / 2, (float)point2.x + p.Width / 2, (float)point2.y + p.Height / 2);
            //отличие этого отображения в том, что при вращении это отображение будет вращатся вокруг статичного внешнего базиса
        }
        public static void draw_line(d3 point1, d3 point2, PictureBox p, Graphics g)
        {
            point1.x = point1.X * E1.C[0] + point1.Y * E2.C[0] + point1.Z * E3.C[0];
            point1.y = point1.X * E1.C[1] + point1.Y * E2.C[1] + point1.Z * E3.C[1];
            point2.x = point2.X * E1.C[0] + point2.Y * E2.C[0] + point2.Z * E3.C[0];
            point2.y = point2.X * E1.C[1] + point2.Y * E2.C[1] + point2.Z * E3.C[1];
            g.DrawLine(Pens.Black, (float)point1.x + p.Width / 2, (float)point1.y + p.Height / 2, (float)point2.x + p.Width / 2, (float)point2.y + p.Height / 2);
        }
        public static void draw_point(d3 point, PictureBox p, Graphics g)
        {
            d3 x = point;
            x.x = x.X * E1.C[0] + x.Y * E2.C[0] + x.Z * E3.C[0];
            x.y = x.X * E1.C[1] + x.Y * E2.C[1] + x.Z * E3.C[1];
            g.FillEllipse(Brushes.Black, (float)x.x + p.Width / 2, (float)x.y + p.Height / 2, point.Size, point.Size);
            
        }
        public static void draw_point(d3 point, PictureBox p, Graphics g, Brush bruh,bool is_draw_border)
        {
            d3 x = point;
            x.x = x.X * E1.C[0] + x.Y * E2.C[0] + x.Z * E3.C[0];
            x.y = x.X * E1.C[1] + x.Y * E2.C[1] + x.Z * E3.C[1];
            g.FillEllipse(bruh, (float)x.x + p.Width / 2, (float)x.y + p.Height / 2, point.Size, point.Size);
            //exlusively for current program:
            if (is_draw_border)
            {
                g.DrawEllipse(new Pen(Color.Black), (float)point.x + p.Width / 2, (float)point.y + p.Height / 2, point.Size, point.Size);
                //g.DrawEllipse(new Pen(Color.Black), (float)point.x + p.Width / 2 - 0.25f, (float)point.y + p.Height / 2 - 0.25f, point.Size + 0.5f, point.Size + 0.5f);
                g.DrawEllipse(new Pen(Color.White), (float)point.x + p.Width / 2 - 0.5f, (float)point.y + p.Height / 2 - 0.5f, point.Size + 1, point.Size + 1);
            }
        }
        static void draw_line_color(d3 point1, d3 point2, PictureBox p, Graphics g, Color col)
        {
            Pen pn = new Pen(col);
            g.DrawLine(pn, (float)point1.X + p.Width / 2, (float)point1.Y + p.Height / 2, (float)point2.X + p.Width / 2, (float)point2.Y + p.Height / 2);
        }
        public static void draw_basis(PictureBox p, Graphics g)
        {
            d3 x = new d3(100 * E1[0], 100 * E1[1], 100 * E1[2]);
            d3 y = new d3(100 * E2[0], 100 * E2[1], 100 * E2[2]);
            d3 z = new d3(100 * E3[0], 100 * E3[1], 100 * E3[2]);
            d3 zero = new d3(0, 0, 0);
            draw_line_color(zero, x, p, g, Color.Red);
            draw_line_color(zero, y, p, g, Color.Green);
            draw_line_color(zero, z, p, g, Color.Blue);
        }
    }
}
