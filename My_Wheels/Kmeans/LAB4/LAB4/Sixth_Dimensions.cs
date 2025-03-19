using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


namespace LAB4
{
    public class d6
    {
        public static Vector E1, E2,E3,E4,E5,E6;//базисы пространства
        public double X1, X2,X3,X4,X5,X6;//координаты точек для юзера
        protected double x1, x2,x3,x4,x5,x6;//координаты для плоскости
        public int Size = 5;//размер точки
        public d6()
        {
            //reset();
        }
        public d6(double X1_coord, double X2_coord, double X3_coord, double X4_coord, double X5_coord, double X6_coord)
        {
            X1 = X1_coord;
            X2 = X2_coord;
            X3 = X3_coord;
            X4 = X4_coord;
            X5 = X5_coord;
            X6 = X6_coord;
            x1 = X1 * E1[0] + X2 * E1[1] + X3 * E1[2] + X4 * E1[3] + X5 * E1[4] + X6 * E1[5];
            x2 = X1 * E2[0] + X2 * E2[1] + X3 * E2[2] + X4 * E2[3] + X5 * E2[4] + X6 * E2[5];
            x3 = X1 * E3[0] + X2 * E3[1] + X3 * E3[2] + X4 * E3[3] + X5 * E3[4] + X6 * E3[5];
            x4 = X1 * E4[0] + X2 * E4[1] + X3 * E4[2] + X4 * E4[3] + X5 * E4[4] + X6 * E4[5];
            x5 = X1 * E5[0] + X2 * E5[1] + X3 * E5[2] + X4 * E5[3] + X5 * E5[4] + X6 * E5[5];
            x6 = X1 * E6[0] + X2 * E6[1] + X3 * E6[2] + X4 * E6[3] + X5 * E6[4] + X6 * E6[5];
            //Size = 5;
        }
        public static void rotate(double a1, double a2, double a3, double a4, double a5, double a6, double a7, double a8, double a9, double a10, double a11, double a12, double a13, double a14, double a15)
        {
            a1 *= Math.PI / 180;//градусы
            a2 *= Math.PI / 180;
            a3 *= Math.PI / 180;
            a4 *= Math.PI / 180;
            a5 *= Math.PI / 180;
            a6 *= Math.PI / 180;
            a7 *= Math.PI / 180;
            a8 *= Math.PI / 180;
            a9 *= Math.PI / 180;
            a10 *= Math.PI / 180;
            a11 *= Math.PI / 180;
            a12 *= Math.PI / 180;
            a13 *= Math.PI / 180;
            a14 *= Math.PI / 180;
            a15 *= Math.PI / 180;
            Vector e1, e2,e3,e4,e5,e6;//врЕменные координаты
            e1 = E1;
            e2 = E2;
            e3 = E3;
            e4 = E4;
            e5 = E5;
            e6 = E6;
            if(a1!=0)
            {
                E5 = new Vector(Math.Cos(a1) * e5[0] + Math.Sin(a1) * e6[0], Math.Cos(a1) * e5[1] + Math.Sin(a1) * e6[1], Math.Cos(a1) * e5[2] + Math.Sin(a1) * e6[2], Math.Cos(a1) * e5[3] + Math.Sin(a1) * e6[3], Math.Cos(a1) * e5[4] + Math.Sin(a1) * e6[4], Math.Cos(a1) * e5[5] + Math.Sin(a1) * e6[5]);
                E6 = new Vector(-Math.Sin(a1) * e5[0] + Math.Cos(a1) * e6[0], -Math.Sin(a1) * e5[1] + Math.Cos(a1) * e6[1], -Math.Sin(a1) * e5[2] + Math.Cos(a1) * e6[2], -Math.Sin(a1) * e5[3] + Math.Cos(a1) * e6[3], -Math.Sin(a1) * e5[4] + Math.Cos(a1) * e6[4], -Math.Sin(a1) * e5[5] + Math.Cos(a1) * e6[5]);
                e5 = E5;
                e6 = E6;
            }
            if (a2 != 0)
            {
                E4 = new Vector(Math.Cos(a2) * e4[0] + Math.Sin(a2) * e6[0], Math.Cos(a2) * e4[1] + Math.Sin(a2) * e6[1], Math.Cos(a2) * e4[2] + Math.Sin(a2) * e6[2], Math.Cos(a2) * e4[3] + Math.Sin(a2) * e6[3], Math.Cos(a2) * e4[4] + Math.Sin(a2) * e6[4], Math.Cos(a2) * e4[5] + Math.Sin(a2) * e6[5]);
                E6 = new Vector(-Math.Sin(a2) * e4[0] + Math.Cos(a2) * e6[0], -Math.Sin(a2) * e4[1] + Math.Cos(a2) * e6[1], -Math.Sin(a2) * e4[2] + Math.Cos(a2) * e6[2], -Math.Sin(a2) * e4[3] + Math.Cos(a2) * e6[3], -Math.Sin(a2) * e4[4] + Math.Cos(a2) * e6[4], -Math.Sin(a2) * e4[5] + Math.Cos(a2) * e6[5]);
                e4 = E4;
                e6 = E6;
            }
            if (a3 != 0)
            {
                E3 = new Vector(Math.Cos(a3) * e3[0] + Math.Sin(a3) * e6[0], Math.Cos(a3) * e3[1] + Math.Sin(a3) * e6[1], Math.Cos(a3) * e3[2] + Math.Sin(a3) * e6[2], Math.Cos(a3) * e3[3] + Math.Sin(a3) * e6[3], Math.Cos(a3) * e3[4] + Math.Sin(a3) * e6[4], Math.Cos(a3) * e3[5] + Math.Sin(a3) * e6[5]);
                E6 = new Vector(-Math.Sin(a3) * e3[0] + Math.Cos(a3) * e6[0], -Math.Sin(a3) * e3[1] + Math.Cos(a3) * e6[1], -Math.Sin(a3) * e3[2] + Math.Cos(a3) * e6[2], -Math.Sin(a3) * e3[3] + Math.Cos(a3) * e6[3], -Math.Sin(a3) * e3[4] + Math.Cos(a3) * e6[4], -Math.Sin(a3) * e3[5] + Math.Cos(a3) * e6[5]);
                e3 = E3;
                e6 = E6;
            }
            if (a4 != 0)
            {
                E2 = new Vector(Math.Cos(a4) * e2[0] + Math.Sin(a4) * e6[0], Math.Cos(a4) * e2[1] + Math.Sin(a4) * e6[1], Math.Cos(a4) * e2[2] + Math.Sin(a4) * e6[2], Math.Cos(a4) * e2[3] + Math.Sin(a4) * e6[3], Math.Cos(a4) * e2[4] + Math.Sin(a4) * e6[4], Math.Cos(a4) * e2[5] + Math.Sin(a4) * e6[5]);
                E6 = new Vector(-Math.Sin(a4) * e2[0] + Math.Cos(a4) * e6[0], -Math.Sin(a4) * e2[1] + Math.Cos(a4) * e6[1], -Math.Sin(a4) * e2[2] + Math.Cos(a4) * e6[2], -Math.Sin(a4) * e2[3] + Math.Cos(a4) * e6[3], -Math.Sin(a4) * e2[4] + Math.Cos(a4) * e6[4], -Math.Sin(a4) * e2[5] + Math.Cos(a4) * e6[5]);
                e2 = E2;
                e6 = E6;
            }
            if (a5 != 0)
            {
                E1 = new Vector(Math.Cos(a5) * e1[0] + Math.Sin(a5) * e6[0], Math.Cos(a5) * e1[1] + Math.Sin(a5) * e6[1], Math.Cos(a5) * e1[2] + Math.Sin(a5) * e6[2], Math.Cos(a5) * e1[3] + Math.Sin(a5) * e6[3], Math.Cos(a5) * e1[4] + Math.Sin(a5) * e6[4], Math.Cos(a5) * e1[5] + Math.Sin(a5) * e6[5]);
                E6 = new Vector(-Math.Sin(a5) * e1[0] + Math.Cos(a5) * e6[0], -Math.Sin(a5) * e1[1] + Math.Cos(a5) * e6[1], -Math.Sin(a5) * e1[2] + Math.Cos(a5) * e6[2], -Math.Sin(a5) * e1[3] + Math.Cos(a5) * e6[3], -Math.Sin(a5) * e1[4] + Math.Cos(a5) * e6[4], -Math.Sin(a5) * e1[5] + Math.Cos(a5) * e6[5]);
                e1 = E1;
                e6 = E6;
            }
            if (a6 != 0)
            {
                E4 = new Vector(Math.Cos(a6) * e4[0] + Math.Sin(a6) * e5[0], Math.Cos(a6) * e4[1] + Math.Sin(a6) * e5[1], Math.Cos(a6) * e4[2] + Math.Sin(a6) * e5[2], Math.Cos(a6) * e4[3] + Math.Sin(a6) * e5[3], Math.Cos(a6) * e4[4] + Math.Sin(a6) * e5[4], Math.Cos(a6) * e4[5] + Math.Sin(a6) * e5[5]);
                E5 = new Vector(-Math.Sin(a6) * e4[0] + Math.Cos(a6) * e5[0], -Math.Sin(a6) * e4[1] + Math.Cos(a6) * e5[1], -Math.Sin(a6) * e4[2] + Math.Cos(a6) * e5[2], -Math.Sin(a6) * e4[3] + Math.Cos(a6) * e5[3], -Math.Sin(a6) * e4[4] + Math.Cos(a6) * e5[4], -Math.Sin(a6) * e4[5] + Math.Cos(a6) * e5[5]);
                e4 = E4;
                e5 = E5;
            }
            if (a7 != 0)
            {
                E3 = new Vector(Math.Cos(a7) * e3[0] + Math.Sin(a7) * e5[0], Math.Cos(a7) * e3[1] + Math.Sin(a7) * e5[1], Math.Cos(a7) * e3[2] + Math.Sin(a7) * e5[2], Math.Cos(a7) * e3[3] + Math.Sin(a7) * e5[3], Math.Cos(a7) * e3[4] + Math.Sin(a7) * e5[4], Math.Cos(a7) * e3[5] + Math.Sin(a7) * e5[5]);
                E5 = new Vector(-Math.Sin(a7) * e3[0] + Math.Cos(a7) * e5[0], -Math.Sin(a7) * e3[1] + Math.Cos(a7) * e5[1], -Math.Sin(a7) * e3[2] + Math.Cos(a7) * e5[2], -Math.Sin(a7) * e3[3] + Math.Cos(a7) * e5[3], -Math.Sin(a7) * e3[4] + Math.Cos(a7) * e5[4], -Math.Sin(a7) * e3[5] + Math.Cos(a7) * e5[5]);
                e3 = E3;
                e5 = E5;
            }
            if (a8 != 0)
            {
                E2 = new Vector(Math.Cos(a8) * e2[0] + Math.Sin(a8) * e5[0], Math.Cos(a8) * e2[1] + Math.Sin(a8) * e5[1], Math.Cos(a8) * e2[2] + Math.Sin(a8) * e5[2], Math.Cos(a8) * e2[3] + Math.Sin(a8) * e5[3], Math.Cos(a8) * e2[4] + Math.Sin(a8) * e5[4], Math.Cos(a8) * e2[5] + Math.Sin(a8) * e5[5]);
                E5 = new Vector(-Math.Sin(a8) * e2[0] + Math.Cos(a8) * e5[0], -Math.Sin(a8) * e2[1] + Math.Cos(a8) * e5[1], -Math.Sin(a8) * e2[2] + Math.Cos(a8) * e5[2], -Math.Sin(a8) * e2[3] + Math.Cos(a8) * e5[3], -Math.Sin(a8) * e2[4] + Math.Cos(a8) * e5[4], -Math.Sin(a8) * e2[5] + Math.Cos(a8) * e5[5]);
                e2 = E2;
                e5 = E5;
            }
            if (a9 != 0)
            {
                E1 = new Vector(Math.Cos(a9) * e1[0] + Math.Sin(a9) * e5[0], Math.Cos(a9) * e1[1] + Math.Sin(a9) * e5[1], Math.Cos(a9) * e1[2] + Math.Sin(a9) * e5[2], Math.Cos(a9) * e1[3] + Math.Sin(a9) * e5[3], Math.Cos(a9) * e1[4] + Math.Sin(a9) * e5[4], Math.Cos(a9) * e1[5] + Math.Sin(a9) * e5[5]);
                E5 = new Vector(-Math.Sin(a9) * e1[0] + Math.Cos(a9) * e5[0], -Math.Sin(a9) * e1[1] + Math.Cos(a9) * e5[1], -Math.Sin(a9) * e1[2] + Math.Cos(a9) * e5[2], -Math.Sin(a9) * e1[3] + Math.Cos(a9) * e5[3], -Math.Sin(a9) * e1[4] + Math.Cos(a9) * e5[4], -Math.Sin(a9) * e1[5] + Math.Cos(a9) * e5[5]);
                e1 = E1;
                e5 = E5;
            }
            if (a10 != 0)
            {
                E3 = new Vector(Math.Cos(a10) * e3[0] + Math.Sin(a10) * e4[0], Math.Cos(a10) * e3[1] + Math.Sin(a10) * e4[1], Math.Cos(a10) * e3[2] + Math.Sin(a10) * e4[2], Math.Cos(a10) * e3[3] + Math.Sin(a10) * e4[3], Math.Cos(a10) * e3[4] + Math.Sin(a10) * e4[4], Math.Cos(a10) * e3[5] + Math.Sin(a10) * e4[5]);
                E4 = new Vector(-Math.Sin(a10) * e3[0] + Math.Cos(a10) * e4[0], -Math.Sin(a10) * e3[1] + Math.Cos(a10) * e4[1], -Math.Sin(a10) * e3[2] + Math.Cos(a10) * e4[2], -Math.Sin(a10) * e3[3] + Math.Cos(a10) * e4[3], -Math.Sin(a10) * e3[4] + Math.Cos(a10) * e4[4], -Math.Sin(a10) * e3[5] + Math.Cos(a10) * e4[5]);
                e3 = E3;
                e4 = E4;
            }
            if (a11 != 0)
            {
                E2 = new Vector(Math.Cos(a11) * e2[0] + Math.Sin(a11) * e4[0], Math.Cos(a11) * e2[1] + Math.Sin(a11) * e4[1], Math.Cos(a11) * e2[2] + Math.Sin(a11) * e4[2], Math.Cos(a11) * e2[3] + Math.Sin(a11) * e4[3], Math.Cos(a11) * e2[4] + Math.Sin(a11) * e4[4], Math.Cos(a11) * e2[5] + Math.Sin(a11) * e4[5]);
                E4 = new Vector(-Math.Sin(a11) * e2[0] + Math.Cos(a11) * e4[0], -Math.Sin(a11) * e2[1] + Math.Cos(a11) * e4[1], -Math.Sin(a11) * e2[2] + Math.Cos(a11) * e4[2], -Math.Sin(a11) * e2[3] + Math.Cos(a11) * e4[3], -Math.Sin(a11) * e2[4] + Math.Cos(a11) * e4[4], -Math.Sin(a11) * e2[5] + Math.Cos(a11) * e4[5]);
                e2 = E2;
                e4 = E4;
            }
            if (a12 != 0)
            {
                E1 = new Vector(Math.Cos(a12) * e1[0] + Math.Sin(a12) * e4[0], Math.Cos(a12) * e1[1] + Math.Sin(a12) * e4[1], Math.Cos(a12) * e1[2] + Math.Sin(a12) * e4[2], Math.Cos(a12) * e1[3] + Math.Sin(a12) * e4[3], Math.Cos(a12) * e1[4] + Math.Sin(a12) * e4[4], Math.Cos(a12) * e1[5] + Math.Sin(a12) * e4[5]);
                E4 = new Vector(-Math.Sin(a12) * e1[0] + Math.Cos(a12) * e4[0], -Math.Sin(a12) * e1[1] + Math.Cos(a12) * e4[1], -Math.Sin(a12) * e1[2] + Math.Cos(a12) * e4[2], -Math.Sin(a12) * e1[3] + Math.Cos(a12) * e4[3], -Math.Sin(a12) * e1[4] + Math.Cos(a12) * e4[4], -Math.Sin(a12) * e1[5] + Math.Cos(a12) * e4[5]);
                e1 = E1;
                e4 = E4;
            }
            if (a13 != 0)
            {
                E2 = new Vector(Math.Cos(a13) * e2[0] + Math.Sin(a13) * e3[0], Math.Cos(a13) * e2[1] + Math.Sin(a13) * e3[1], Math.Cos(a13) * e2[2] + Math.Sin(a13) * e3[2], Math.Cos(a13) * e2[3] + Math.Sin(a13) * e3[3], Math.Cos(a13) * e2[4] + Math.Sin(a13) * e3[4], Math.Cos(a13) * e2[5] + Math.Sin(a13) * e3[5]);
                E3 = new Vector(-Math.Sin(a13) * e2[0] + Math.Cos(a13) * e3[0], -Math.Sin(a13) * e2[1] + Math.Cos(a13) * e3[1], -Math.Sin(a13) * e2[2] + Math.Cos(a13) * e3[2], -Math.Sin(a13) * e2[3] + Math.Cos(a13) * e3[3], -Math.Sin(a13) * e2[4] + Math.Cos(a13) * e3[4], -Math.Sin(a13) * e2[5] + Math.Cos(a13) * e3[5]);
                e2 = E2;
                e3 = E3;
            }
            if (a14 != 0)
            {
                E1 = new Vector(Math.Cos(a14) * e1[0] + Math.Sin(a14) * e3[0], Math.Cos(a14) * e1[1] + Math.Sin(a14) * e3[1], Math.Cos(a14) * e1[2] + Math.Sin(a14) * e3[2], Math.Cos(a14) * e1[3] + Math.Sin(a14) * e3[3], Math.Cos(a14) * e1[4] + Math.Sin(a14) * e3[4], Math.Cos(a14) * e1[5] + Math.Sin(a14) * e3[5]);
                E3 = new Vector(-Math.Sin(a14) * e1[0] + Math.Cos(a14) * e3[0], -Math.Sin(a14) * e1[1] + Math.Cos(a14) * e3[1], -Math.Sin(a14) * e1[2] + Math.Cos(a14) * e3[2], -Math.Sin(a14) * e1[3] + Math.Cos(a14) * e3[3], -Math.Sin(a14) * e1[4] + Math.Cos(a14) * e3[4], -Math.Sin(a14) * e1[5] + Math.Cos(a14) * e3[5]);
                e1 = E1;
                e3 = E3;
            }
            if (a15 != 0)
            {
                E1 = new Vector(Math.Cos(a15) * e1[0] + Math.Sin(a15) * e2[0], Math.Cos(a15) * e1[1] + Math.Sin(a15) * e2[1], Math.Cos(a15) * e1[2] + Math.Sin(a15) * e2[2], Math.Cos(a15) * e1[3] + Math.Sin(a15) * e2[3], Math.Cos(a15) * e1[4] + Math.Sin(a15) * e2[4], Math.Cos(a15) * e1[5] + Math.Sin(a15) * e2[5]);
                E2 = new Vector(-Math.Sin(a15) * e1[0] + Math.Cos(a15) * e2[0], -Math.Sin(a15) * e1[1] + Math.Cos(a15) * e2[1], -Math.Sin(a15) * e1[2] + Math.Cos(a15) * e2[2], -Math.Sin(a15) * e1[3] + Math.Cos(a15) * e2[3], -Math.Sin(a15) * e1[4] + Math.Cos(a15) * e2[4], -Math.Sin(a15) * e1[5] + Math.Cos(a15) * e2[5]);
            }
        }
        public static void draw_point(d6 point, PictureBox p, Graphics g)
        {//отображение точки на пикчерБоксе
            point.x1 = point.X1 * E1[0] + point.X2 * E2[0] + point.X3 * E3[0] + point.X4 * E4[0] + point.X5 * E5[0] + point.X6 * E6[0];
            point.x2 = point.X1 * E1[1] + point.X2 * E2[1] + point.X3 * E3[1] + point.X4 * E4[1] + point.X5 * E5[1] + point.X6 * E6[1];
            g.FillEllipse(Brushes.Red, (float)point.x1 + p.Width / 2, (float)point.x2 + p.Height / 2, point.Size, point.Size);
        }
        public static void draw_point(d6 point, PictureBox p, Graphics g, Brush bruh,bool is_draw_border)
        {//отображение точки на пикчерБоксе
            point.x1 = point.X1 * E1[0] + point.X2 * E2[0] + point.X3 * E3[0] + point.X4 * E4[0] + point.X5 * E5[0] + point.X6 * E6[0];
            point.x2 = point.X1 * E1[1] + point.X2 * E2[1] + point.X3 * E3[1] + point.X4 * E4[1] + point.X5 * E5[1] + point.X6 * E6[1];
            g.FillEllipse(bruh, (float)point.x1 + p.Width / 2, (float)point.x2 + p.Height / 2, point.Size, point.Size);
            //exlusively for current program:
            if (is_draw_border)
            {
                g.DrawEllipse(new Pen(Color.Black), (float)point.x1 + p.Width / 2, (float)point.x2 + p.Height / 2, point.Size, point.Size);
                //g.DrawEllipse(new Pen(Color.Black), (float)point.x1 + p.Width / 2 - 0.25f, (float)point.x2 + p.Height / 2 - 0.25f, point.Size + 0.5f, point.Size + 0.5f);
                g.DrawEllipse(new Pen(Color.White), (float)point.x1 + p.Width / 2 - 0.5f, (float)point.x2 + p.Height / 2 - 0.5f, point.Size + 1, point.Size + 1);
            }
        }
        public static void draw_point_col(d6 point, PictureBox p, Graphics g,Color col)
        {//отображение точки на пикчерБоксе
            Pen pn = new Pen(col);
            point.x1 = point.X1 * E1[0] + point.X2 * E2[0] + point.X3 * E3[0] + point.X4 * E4[0] + point.X5 * E5[0] + point.X6 * E6[0];
            point.x2 = point.X1 * E1[1] + point.X2 * E2[1] + point.X3 * E3[1] + point.X4 * E4[1] + point.X5 * E5[1] + point.X6 * E6[1];
            g.DrawRectangle(pn, (float)point.x1 + p.Width / 2, (float)point.x2 + p.Height / 2, 1, 1/*point.Size, point.Size*/);
        }
        public static void draw_line(d6 point1, d6 point2, PictureBox p, Graphics g)
        {
            point1.x1 = point1.X1 * E1[0] + point1.X2 * E2[0] + point1.X3 * E3[0] + point1.X4 * E4[0] + point1.X5 * E5[0] + point1.X6 * E6[0];
            point1.x2 = point1.X1 * E1[1] + point1.X2 * E2[1] + point1.X3 * E3[1] + point1.X4 * E4[1] + point1.X5 * E5[1] + point1.X6 * E6[1];
            point2.x1 = point2.X1 * E1[0] + point2.X2 * E2[0] + point2.X3 * E3[0] + point2.X4 * E4[0] + point2.X5 * E5[0] + point2.X6 * E6[0];
            point2.x2 = point2.X1 * E1[1] + point2.X2 * E2[1] + point2.X3 * E3[1] + point2.X4 * E4[1] + point2.X5 * E5[1] + point2.X6 * E6[1];
            g.DrawLine(Pens.Black, (float)point1.x1 + p.Width / 2, (float)point1.x2 + p.Height / 2, (float)point2.x1 + p.Width / 2, (float)point2.x2 + p.Height / 2);
        }
        public static void draw_line_col(d6 point1, d6 point2, PictureBox p, Graphics g,Color col)
        {
            Pen pn = new Pen(col);
            point1.x1 = point1.X1 * E1[0] + point1.X2 * E2[0] + point1.X3 * E3[0] + point1.X4 * E4[0] + point1.X5 * E5[0] + point1.X6 * E6[0];
            point1.x2 = point1.X1 * E1[1] + point1.X2 * E2[1] + point1.X3 * E3[1] + point1.X4 * E4[1] + point1.X5 * E5[1] + point1.X6 * E6[1];
            point2.x1 = point2.X1 * E1[0] + point2.X2 * E2[0] + point2.X3 * E3[0] + point2.X4 * E4[0] + point2.X5 * E5[0] + point2.X6 * E6[0];
            point2.x2 = point2.X1 * E1[1] + point2.X2 * E2[1] + point2.X3 * E3[1] + point2.X4 * E4[1] + point2.X5 * E5[1] + point2.X6 * E6[1];
            g.DrawLine(pn, (float)point1.x1 + p.Width / 2, (float)point1.x2 + p.Height / 2, (float)point2.x1 + p.Width / 2, (float)point2.x2 + p.Height / 2);
        }
        public static void reset()
        {//анулирование базисов (или инициализация)
            E1 = new Vector(1, 0, 0, 0, 0, 0);
            E2 = new Vector(0, 1, 0, 0, 0, 0);
            E3 = new Vector(0, 0, 1, 0, 0, 0);
            E4 = new Vector(0, 0, 0, 1, 0, 0);
            E5 = new Vector(0, 0, 0, 0, 1, 0);
            E6 = new Vector(0, 0, 0, 0, 0, 1);
        }
        static void draw_line_color(d6 point1, d6 point2, PictureBox p, Graphics g, Color col)
        {
            Pen pn = new Pen(col);
            g.DrawLine(pn, (float)point1.X1 + p.Width / 2, (float)point1.X2 + p.Height / 2, (float)point2.X1 + p.Width / 2, (float)point2.X2 + p.Height / 2);
        }
        public static void draw_basis(PictureBox p, Graphics g)
        {
            d6 y1 = new d6(100 * E1[0], 100 * E1[1], 100 * E1[2], 100 * E1[3], 100 * E1[4], 100 * E1[5]);
            d6 y2 = new d6(100 * E2[0], 100 * E2[1], 100 * E2[2], 100 * E2[3], 100 * E2[4], 100 * E2[5]);
            d6 y3 = new d6(100 * E3[0], 100 * E3[1], 100 * E3[2], 100 * E3[3], 100 * E3[4], 100 * E3[5]);
            d6 y4 = new d6(100 * E4[0], 100 * E4[1], 100 * E4[2], 100 * E4[3], 100 * E4[4], 100 * E4[5]);
            d6 y5 = new d6(100 * E5[0], 100 * E5[1], 100 * E5[2], 100 * E5[3], 100 * E5[4], 100 * E5[5]);
            d6 y6 = new d6(100 * E6[0], 100 * E6[1], 100 * E6[2], 100 * E6[3], 100 * E6[4], 100 * E6[5]);
            d6 zero = new d6(0, 0, 0, 0, 0, 0);
            draw_line_color(zero, y1, p, g, Color.FromArgb(255, 255, 0, 0));
            draw_line_color(zero, y2, p, g, Color.FromArgb(255, 0, 255, 0));
            draw_line_color(zero, y3, p, g, Color.FromArgb(255, 0, 0, 255));
            draw_line_color(zero, y4, p, g, Color.FromArgb(255, 255, 255, 0));
            draw_line_color(zero, y5, p, g, Color.FromArgb(255, 255, 0, 255));
            draw_line_color(zero, y6, p, g, Color.FromArgb(255, 0, 255, 255));
        }
    }
}
