using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace LAB4
{
    public class d4
    {
        public static Vector E1, E2, E3, E4;//базисы
        public double X, Y, Z, T;//координаты (для юзера)
        protected double x, y, z, t;//координаты (для гиперпространства)
        protected static double a1, a2, a3, a4, a5, a6;//углы поворота гиперпространства
        public int Size = 5;
        public d4()
        {

        }
        public d4(double X_coord, double Y_coord, double Z_coord, double T_coord)
        {
            //reset();
            X = X_coord;
            Y = Y_coord;
            Z = Z_coord;
            T = T_coord;
            x = X * E1.C[0] + Y * E1.C[1] + Z * E1.C[2] + T * E1.C[3];
            y = X * E2.C[0] + Y * E2.C[1] + Z * E2.C[2] + T * E2.C[3];
            z = X * E3.C[0] + Y * E3.C[1] + Z * E3.C[2] + T * E3.C[3];
            t = X * E4.C[0] + Y * E4.C[1] + Z * E4.C[2] + T * E4.C[3];
        }
        public static void rotate(double angle1, double angle2, double angle3, double angle4, double angle5, double angle6)
        {
            angle1 *= Math.PI / 180;
            angle2 *= Math.PI / 180;
            angle3 *= Math.PI / 180;
            angle4 *= Math.PI / 180;
            angle5 *= Math.PI / 180;
            angle6 *= Math.PI / 180;
            //a1 += angle1 * Math.PI / 180;
            //a2 += angle2 * Math.PI / 180;
            //a3 += angle3 * Math.PI / 180;
            //a4 += angle4 * Math.PI / 180;
            //a5 += angle5 * Math.PI / 180;
            //a6 += angle6 * Math.PI / 180;
            Vector e1, e2, e3, e4;
            //e1 = new Vector(1, 0, 0, 0);//E1;// 
            //e2 = new Vector(0, 1, 0, 0);//E2;// 
            //e3 = new Vector(0, 0, 1, 0);//E3;// 
            //e4 = new Vector(0, 0, 0, 1);//E4;// 
            //reset();
            e1 = E1;
            e2 = E2;
            e3 = E3;
            e4 = E4;

            E1 = new Vector((Math.Cos(angle3) * Math.Cos(angle5) * Math.Cos(angle6)) * (e1[0]) + (Math.Cos(angle3) * Math.Cos(angle5) * Math.Sin(angle6)) * (e2[0]) + (Math.Cos(angle3) * Math.Sin(angle5)) * (e3[0]) + (Math.Sin(angle3)) * (e4[0]),
                (Math.Cos(angle3) * Math.Cos(angle5) * Math.Cos(angle6)) * (e1[1]) + (Math.Cos(angle3) * Math.Cos(angle5) * Math.Sin(angle6)) * (e2[1]) + (Math.Cos(angle3) * Math.Sin(angle5)) * (e3[1]) + (Math.Sin(angle3)) * (e4[1]),
                 (Math.Cos(angle3) * Math.Cos(angle5) * Math.Cos(angle6)) * (e1[2]) + (Math.Cos(angle3) * Math.Cos(angle5) * Math.Sin(angle6)) * (e2[2]) + (Math.Cos(angle3) * Math.Sin(angle5)) * (e3[2]) + (Math.Sin(angle3)) * (e4[2]),
                 (Math.Cos(angle3) * Math.Cos(angle5) * Math.Cos(angle6)) * (e1[3]) + (Math.Cos(angle3) * Math.Cos(angle5) * Math.Sin(angle6)) * (e2[3]) + (Math.Cos(angle3) * Math.Sin(angle5)) * (e3[3]) + (Math.Sin(angle3)) * (e4[3]));

            E2 = new Vector((-Math.Cos(angle5) * Math.Cos(angle6) * Math.Sin(angle2) * Math.Sin(angle3) - Math.Cos(angle2) * Math.Cos(angle6) * Math.Sin(angle4) * Math.Sin(angle5) - Math.Cos(angle2) * Math.Cos(angle4) * Math.Sin(angle6)) * (e1[0]) + (Math.Cos(angle2) * Math.Cos(angle4) * Math.Cos(angle6) - Math.Cos(angle5) * Math.Sin(angle2) * Math.Sin(angle3) * Math.Sin(angle6) - Math.Cos(angle2) * Math.Sin(angle4) * Math.Sin(angle5) * Math.Sin(angle6)) * (e2[0]) + (Math.Cos(angle2) * Math.Cos(angle5) * Math.Sin(angle4) - Math.Sin(angle2) * Math.Sin(angle3) * Math.Sin(angle5)) * (e3[0]) + (Math.Cos(angle3) * Math.Sin(angle2)) * (e4[0]),
                 (-Math.Cos(angle5) * Math.Cos(angle6) * Math.Sin(angle2) * Math.Sin(angle3) - Math.Cos(angle2) * Math.Cos(angle6) * Math.Sin(angle4) * Math.Sin(angle5) - Math.Cos(angle2) * Math.Cos(angle4) * Math.Sin(angle6)) * (e1[1]) + (Math.Cos(angle2) * Math.Cos(angle4) * Math.Cos(angle6) - Math.Cos(angle5) * Math.Sin(angle2) * Math.Sin(angle3) * Math.Sin(angle6) - Math.Cos(angle2) * Math.Sin(angle4) * Math.Sin(angle5) * Math.Sin(angle6)) * (e2[1]) + (Math.Cos(angle2) * Math.Cos(angle5) * Math.Sin(angle4) - Math.Sin(angle2) * Math.Sin(angle3) * Math.Sin(angle5)) * (e3[1]) + (Math.Cos(angle3) * Math.Sin(angle2)) * (e4[1]),
                 (-Math.Cos(angle5) * Math.Cos(angle6) * Math.Sin(angle2) * Math.Sin(angle3) - Math.Cos(angle2) * Math.Cos(angle6) * Math.Sin(angle4) * Math.Sin(angle5) - Math.Cos(angle2) * Math.Cos(angle4) * Math.Sin(angle6)) * (e1[2]) + (Math.Cos(angle2) * Math.Cos(angle4) * Math.Cos(angle6) - Math.Cos(angle5) * Math.Sin(angle2) * Math.Sin(angle3) * Math.Sin(angle6) - Math.Cos(angle2) * Math.Sin(angle4) * Math.Sin(angle5) * Math.Sin(angle6)) * (e2[2]) + (Math.Cos(angle2) * Math.Cos(angle5) * Math.Sin(angle4) - Math.Sin(angle2) * Math.Sin(angle3) * Math.Sin(angle5)) * (e3[2]) + (Math.Cos(angle3) * Math.Sin(angle2)) * (e4[2]),
                 (-Math.Cos(angle5) * Math.Cos(angle6) * Math.Sin(angle2) * Math.Sin(angle3) - Math.Cos(angle2) * Math.Cos(angle6) * Math.Sin(angle4) * Math.Sin(angle5) - Math.Cos(angle2) * Math.Cos(angle4) * Math.Sin(angle6)) * (e1[3]) + (Math.Cos(angle2) * Math.Cos(angle4) * Math.Cos(angle6) - Math.Cos(angle5) * Math.Sin(angle2) * Math.Sin(angle3) * Math.Sin(angle6) - Math.Cos(angle2) * Math.Sin(angle4) * Math.Sin(angle5) * Math.Sin(angle6)) * (e2[3]) + (Math.Cos(angle2) * Math.Cos(angle5) * Math.Sin(angle4) - Math.Sin(angle2) * Math.Sin(angle3) * Math.Sin(angle5)) * (e3[3]) + (Math.Cos(angle3) * Math.Sin(angle2)) * (e4[3]));

            E3 = new Vector((-Math.Cos(angle2) * Math.Cos(angle5) * Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle3) - Math.Cos(angle1) * Math.Cos(angle4) * Math.Cos(angle6) * Math.Sin(angle5) + Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle4) * Math.Sin(angle5) + Math.Cos(angle4) * Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle6) + Math.Cos(angle1) * Math.Sin(angle4) * Math.Sin(angle6)) * (e1[0]) + (-Math.Cos(angle4) * Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle2) - Math.Cos(angle1) * Math.Cos(angle6) * Math.Sin(angle4) - Math.Cos(angle2) * Math.Cos(angle5) * Math.Sin(angle1) * Math.Sin(angle3) * Math.Sin(angle6) - Math.Cos(angle1) * Math.Cos(angle4) * Math.Sin(angle5) * Math.Sin(angle6) + Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle4) * Math.Sin(angle5) * Math.Sin(angle6)) * (e2[0]) + (Math.Cos(angle1) * Math.Cos(angle4) * Math.Cos(angle5) - Math.Cos(angle5) * Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle4) - Math.Cos(angle2) * Math.Sin(angle1) * Math.Sin(angle3) * Math.Sin(angle5)) * (e3[0]) + (Math.Cos(angle2) * Math.Cos(angle3) * Math.Sin(angle1)) * (e4[0]),
                (-Math.Cos(angle2) * Math.Cos(angle5) * Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle3) - Math.Cos(angle1) * Math.Cos(angle4) * Math.Cos(angle6) * Math.Sin(angle5) + Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle4) * Math.Sin(angle5) + Math.Cos(angle4) * Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle6) + Math.Cos(angle1) * Math.Sin(angle4) * Math.Sin(angle6)) * (e1[1]) + (-Math.Cos(angle4) * Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle2) - Math.Cos(angle1) * Math.Cos(angle6) * Math.Sin(angle4) - Math.Cos(angle2) * Math.Cos(angle5) * Math.Sin(angle1) * Math.Sin(angle3) * Math.Sin(angle6) - Math.Cos(angle1) * Math.Cos(angle4) * Math.Sin(angle5) * Math.Sin(angle6) + Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle4) * Math.Sin(angle5) * Math.Sin(angle6)) * (e2[1]) + (Math.Cos(angle1) * Math.Cos(angle4) * Math.Cos(angle5) - Math.Cos(angle5) * Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle4) - Math.Cos(angle2) * Math.Sin(angle1) * Math.Sin(angle3) * Math.Sin(angle5)) * (e3[1]) + (Math.Cos(angle2) * Math.Cos(angle3) * Math.Sin(angle1)) * (e4[1]),
                 (-Math.Cos(angle2) * Math.Cos(angle5) * Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle3) - Math.Cos(angle1) * Math.Cos(angle4) * Math.Cos(angle6) * Math.Sin(angle5) + Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle4) * Math.Sin(angle5) + Math.Cos(angle4) * Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle6) + Math.Cos(angle1) * Math.Sin(angle4) * Math.Sin(angle6)) * (e1[2]) + (-Math.Cos(angle4) * Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle2) - Math.Cos(angle1) * Math.Cos(angle6) * Math.Sin(angle4) - Math.Cos(angle2) * Math.Cos(angle5) * Math.Sin(angle1) * Math.Sin(angle3) * Math.Sin(angle6) - Math.Cos(angle1) * Math.Cos(angle4) * Math.Sin(angle5) * Math.Sin(angle6) + Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle4) * Math.Sin(angle5) * Math.Sin(angle6)) * (e2[2]) + (Math.Cos(angle1) * Math.Cos(angle4) * Math.Cos(angle5) - Math.Cos(angle5) * Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle4) - Math.Cos(angle2) * Math.Sin(angle1) * Math.Sin(angle3) * Math.Sin(angle5)) * (e3[2]) + (Math.Cos(angle2) * Math.Cos(angle3) * Math.Sin(angle1)) * (e4[2]),
                 (-Math.Cos(angle2) * Math.Cos(angle5) * Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle3) - Math.Cos(angle1) * Math.Cos(angle4) * Math.Cos(angle6) * Math.Sin(angle5) + Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle4) * Math.Sin(angle5) + Math.Cos(angle4) * Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle6) + Math.Cos(angle1) * Math.Sin(angle4) * Math.Sin(angle6)) * (e1[3]) + (-Math.Cos(angle4) * Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle2) - Math.Cos(angle1) * Math.Cos(angle6) * Math.Sin(angle4) - Math.Cos(angle2) * Math.Cos(angle5) * Math.Sin(angle1) * Math.Sin(angle3) * Math.Sin(angle6) - Math.Cos(angle1) * Math.Cos(angle4) * Math.Sin(angle5) * Math.Sin(angle6) + Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle4) * Math.Sin(angle5) * Math.Sin(angle6)) * (e2[3]) + (Math.Cos(angle1) * Math.Cos(angle4) * Math.Cos(angle5) - Math.Cos(angle5) * Math.Sin(angle1) * Math.Sin(angle2) * Math.Sin(angle4) - Math.Cos(angle2) * Math.Sin(angle1) * Math.Sin(angle3) * Math.Sin(angle5)) * (e3[3]) + (Math.Cos(angle2) * Math.Cos(angle3) * Math.Sin(angle1)) * (e4[3]));

            E4 = new Vector((-Math.Cos(angle1) * Math.Cos(angle2) * Math.Cos(angle5) * Math.Cos(angle6) * Math.Sin(angle3) + Math.Cos(angle4) * Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle5) + Math.Cos(angle1) * Math.Cos(angle6) * Math.Sin(angle2) * Math.Sin(angle4) * Math.Sin(angle5) + Math.Cos(angle1) * Math.Cos(angle4) * Math.Sin(angle2) * Math.Sin(angle6) - Math.Sin(angle1) * Math.Sin(angle4) * Math.Sin(angle6)) *(e1[0]) + (-Math.Cos(angle1) * Math.Cos(angle4) * Math.Cos(angle6) * Math.Sin(angle2) + Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle4) - Math.Cos(angle1) * Math.Cos(angle2) * Math.Cos(angle5) * Math.Sin(angle3) * Math.Sin(angle6) + Math.Cos(angle4) * Math.Sin(angle1) * Math.Sin(angle5) * Math.Sin(angle6) + Math.Cos(angle1) * Math.Sin(angle2) * Math.Sin(angle4) * Math.Sin(angle5) * Math.Sin(angle6)) * (e2[0]) + (-Math.Cos(angle4) * Math.Cos(angle5) * Math.Sin(angle1) - Math.Cos(angle1) * Math.Cos(angle5) * Math.Sin(angle2) * Math.Sin(angle4) - Math.Cos(angle1) * Math.Cos(angle2) * Math.Sin(angle3) * Math.Sin(angle5)) * (e3[0]) + (Math.Cos(angle1) * Math.Cos(angle2) * Math.Cos(angle3)) * (e4[0]),
                 (-Math.Cos(angle1) * Math.Cos(angle2) * Math.Cos(angle5) * Math.Cos(angle6) * Math.Sin(angle3) + Math.Cos(angle4) * Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle5) + Math.Cos(angle1) * Math.Cos(angle6) * Math.Sin(angle2) * Math.Sin(angle4) * Math.Sin(angle5) + Math.Cos(angle1) * Math.Cos(angle4) * Math.Sin(angle2) * Math.Sin(angle6) - Math.Sin(angle1) * Math.Sin(angle4) * Math.Sin(angle6)) * (e1[1]) + (-Math.Cos(angle1) * Math.Cos(angle4) * Math.Cos(angle6) * Math.Sin(angle2) + Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle4) - Math.Cos(angle1) * Math.Cos(angle2) * Math.Cos(angle5) * Math.Sin(angle3) * Math.Sin(angle6) + Math.Cos(angle4) * Math.Sin(angle1) * Math.Sin(angle5) * Math.Sin(angle6) + Math.Cos(angle1) * Math.Sin(angle2) * Math.Sin(angle4) * Math.Sin(angle5) * Math.Sin(angle6))*(e2[1]) + (-Math.Cos(angle4) * Math.Cos(angle5) * Math.Sin(angle1) - Math.Cos(angle1) * Math.Cos(angle5) * Math.Sin(angle2) * Math.Sin(angle4) - Math.Cos(angle1) * Math.Cos(angle2) * Math.Sin(angle3) * Math.Sin(angle5)) * (e3[1]) + (Math.Cos(angle1) * Math.Cos(angle2) * Math.Cos(angle3)) * (e4[1]),
                 (-Math.Cos(angle1) * Math.Cos(angle2) * Math.Cos(angle5) * Math.Cos(angle6) * Math.Sin(angle3) + Math.Cos(angle4) * Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle5) + Math.Cos(angle1) * Math.Cos(angle6) * Math.Sin(angle2) * Math.Sin(angle4) * Math.Sin(angle5) + Math.Cos(angle1) * Math.Cos(angle4) * Math.Sin(angle2) * Math.Sin(angle6) - Math.Sin(angle1) * Math.Sin(angle4) * Math.Sin(angle6)) * (e1[2]) + (-Math.Cos(angle1) * Math.Cos(angle4) * Math.Cos(angle6) * Math.Sin(angle2) + Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle4) - Math.Cos(angle1) * Math.Cos(angle2) * Math.Cos(angle5) * Math.Sin(angle3) * Math.Sin(angle6) + Math.Cos(angle4) * Math.Sin(angle1) * Math.Sin(angle5) * Math.Sin(angle6) + Math.Cos(angle1) * Math.Sin(angle2) * Math.Sin(angle4) * Math.Sin(angle5) * Math.Sin(angle6)) * (e2[2]) +(-Math.Cos(angle4) * Math.Cos(angle5) * Math.Sin(angle1) - Math.Cos(angle1) * Math.Cos(angle5) * Math.Sin(angle2) * Math.Sin(angle4) - Math.Cos(angle1) * Math.Cos(angle2) * Math.Sin(angle3) * Math.Sin(angle5)) *(e3[2]) + (Math.Cos(angle1) * Math.Cos(angle2) * Math.Cos(angle3)) * (e4[2]),
                 (-Math.Cos(angle1) * Math.Cos(angle2) * Math.Cos(angle5) * Math.Cos(angle6) * Math.Sin(angle3) + Math.Cos(angle4) * Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle5) + Math.Cos(angle1) * Math.Cos(angle6) * Math.Sin(angle2) * Math.Sin(angle4) * Math.Sin(angle5) + Math.Cos(angle1) * Math.Cos(angle4) * Math.Sin(angle2) * Math.Sin(angle6) - Math.Sin(angle1) * Math.Sin(angle4) * Math.Sin(angle6)) * (e1[3]) + (-Math.Cos(angle1) * Math.Cos(angle4) * Math.Cos(angle6) * Math.Sin(angle2) + Math.Cos(angle6) * Math.Sin(angle1) * Math.Sin(angle4) - Math.Cos(angle1) * Math.Cos(angle2) * Math.Cos(angle5) * Math.Sin(angle3) * Math.Sin(angle6) + Math.Cos(angle4) * Math.Sin(angle1) * Math.Sin(angle5) * Math.Sin(angle6) + Math.Cos(angle1) * Math.Sin(angle2) * Math.Sin(angle4) * Math.Sin(angle5) * Math.Sin(angle6)) * (e2[3]) + (-Math.Cos(angle4) * Math.Cos(angle5) * Math.Sin(angle1) - Math.Cos(angle1) * Math.Cos(angle5) * Math.Sin(angle2) * Math.Sin(angle4) - Math.Cos(angle1) * Math.Cos(angle2) * Math.Sin(angle3) * Math.Sin(angle5)) * (e3[3]) + (Math.Cos(angle1) * Math.Cos(angle2) * Math.Cos(angle3))*(e4[3]));
            
            /*
            E1 = Math.Cos(aYZ)*Math.Cos(aYT)*Math.Cos(aZT)*(e1)+ Math.Cos(aYZ)*Math.Cos(aYT)*Math.Sin(aZT)*(e2)+ Math.Cos(aYZ)*Math.Sin(aYT)*(e3)+ Math.Sin(aYZ)*(e4);
            E2 = (-Math.Sin(aXZ)*Math.Sin(aYZ)*Math.Cos(aYT)*Math.Cos(aZT)-Math.Cos(aXZ)*Math.Cos(aXT)*Math.Sin(aZT)-Math.Cos(aXZ)*Math.Sin(aXT)*Math.Sin(aYT)*Math.Cos(aZT))*(e1)+ (-Math.Sin(aXZ)*Math.Sin(aYZ)*Math.Cos(aYT)*Math.Sin(aZT)+Math.Cos(aXZ)*Math.Cos(aXT)*Math.Cos(aZT)-Math.Cos(aXZ)*Math.Sin(aXT)*Math.Sin(aYT)*Math.Sin(aZT))*(e2)+ (-Math.Sin(aXZ)*Math.Sin(aYZ)*Math.Sin(aYT)+Math.Cos(aXZ)*Math.Sin(aXT)*Math.Cos(aYT))*(e3)+ Math.Cos(aYZ)*Math.Sin(aXZ)*(e4);
            E3 = (-Math.Sin(aXY)*Math.Cos(aXZ)*Math.Sin(aYZ)*Math.Cos(aYT)*Math.Cos(aZT)+Math.Sin(aXY)*Math.Sin(aXZ)*Math.Cos(aXT)*Math.Sin(aZT)+Math.Sin(aXT)*Math.Cos(aXY)*Math.Sin(aZT)+Math.Sin(aXY)*Math.Sin(aXZ)*Math.Sin(aXT)*Math.Sin(aYT)*Math.Cos(aZT)-Math.Cos(aXY)*Math.Cos(aXT)*Math.Sin(aYT)*Math.Cos(aZT))*(e1)+ (-Math.Sin(aXY)*Math.Cos(aXZ)*Math.Sin(aYZ)*Math.Cos(aYT)*Math.Sin(aZT)-Math.Sin(aXY)*Math.Sin(aXZ)*Math.Cos(aXT)*Math.Cos(aZT)-Math.Sin(aXT)*Math.Cos(aXY)*Math.Cos(aZT)+Math.Sin(aXY)*Math.Sin(aXZ)*Math.Sin(aXT)*Math.Sin(aYT)*Math.Sin(aZT)-Math.Cos(aXY)*Math.Cos(aXT)*Math.Sin(aYT)*Math.Sin(aZT))*(e2)+ (-Math.Sin(aXY)*Math.Cos(aXZ)*Math.Sin(aYZ)*Math.Sin(aYT)-Math.Sin(aXY)*Math.Sin(aXZ)*Math.Sin(aXT)*Math.Cos(aYT)+Math.Cos(aXY)*Math.Cos(aXT)*Math.Cos(aYT))*(e3)+ Math.Sin(aXY)*Math.Cos(aXZ)*Math.Cos(aYZ)*(e4);
            E4 = (-Math.Cos(aXY)*Math.Cos(aXZ)*Math.Sin(aYZ)*Math.Cos(aYT)*Math.Cos(aZT)+Math.Cos(aXY)*Math.Sin(aXZ)*Math.Cos(aXT)*Math.Sin(aZT)-Math.Sin(aXY)*Math.Sin(aXT)*Math.Sin(aZT)+Math.Cos(aXY)*Math.Cos(aXZ)*Math.Sin(aXT)*Math.Sin(aYT)*Math.Cos(aZT)+Math.Sin(aXY)*Math.Cos(aXT)*Math.Sin(aYT)*Math.Cos(aZT))*(e1)+ (-Math.Cos(aXY)*Math.Cos(aXZ)*Math.Sin(aYZ)*Math.Cos(aYT)*Math.Sin(aZT)-Math.Cos(aXY)*Math.Sin(aXZ)*Math.Cos(aXT)*Math.Cos(aZT)+Math.Sin(aXY)*Math.Sin(aXT)*Math.Cos(aZT)+Math.Cos(aXY)*Math.Sin(aXZ)*Math.Sin(aXT)*Math.Sin(aYT)*Math.Sin(aZT)+Math.Sin(aXY)*Math.Cos(aXT)*Math.Sin(aYT)*Math.Sin(aZT))*(e2)+ (-Math.Cos(aXY)*Math.Cos(aXZ)*Math.Sin(aYZ)*Math.Sin(aYT) - Math.Cos(aXY) * Math.Cos(aXZ) * Math.Sin(aXT) * Math.Cos(aYT)-Math.Sin(aXY)*Math.Cos(aXT)*Math.Cos(aYT)) *(e3)+ Math.Cos(aXY) *Math.Cos(aXZ) *Math.Cos(aYZ) *(e4);
            */ 

            /*
            if (angle1 != 0)
            {
                //E1 = new Vector(1, 0, 0, 0);
                //E2 = new Vector(0, 1, 0, 0);
                E3 = Math.Cos(angle1) * e3 - Math.Sin(angle1) * e4;
                E4 = Math.Sin(angle1) * e3 + Math.Cos(angle1) * e4;
                e1 = E1;// 
                e2 = E2;// 
                e3 = E3;// 
                e4 = E4;//
            }
            if (angle2 != 0)
            {
                //e1 = E1;// 
                //e2 = E2;// 
                //e3 = E3;// 
                //e4 = E4;//
                //E1 = new Vector(1, 0, 0, 0);
                E2 = Math.Cos(a2) * e2 - Math.Sin(aXZ) * e4;
                //E3 = new Vector(0, 0, 1, 0);
                E4 = Math.Sin(aXZ) * e2 + Math.Cos(aXZ) * e4;
            }
            if (angle3 != 0)
            {
                //e1 = E1;// 
                //e2 = E2;// 
                //e3 = E3;// 
                //e4 = E4;//
                //E1 = new Vector(1, 0, 0, 0);
                E2 = Math.Cos(aXT) * e2 - Math.Sin(aXT) * e3;
                E3 = Math.Sin(aXT) * e2 + Math.Cos(aXT) * e3;
                //E4 = new Vector(0, 0, 0, 1);
            }
            if (angle4 != 0)
            {
                //e1 = E1;// 
                //e2 = E2;// 
                //e3 = E3;// 
                //e4 = E4;//
                E1 = Math.Cos(aYZ) * e1 - Math.Sin(aYZ) * e4;
                //E2 = new Vector(0, 1, 0, 0);
                //E3 = new Vector(0, 0, 1, 0);
                E4 = Math.Sin(aYZ) * e1 + Math.Cos(aYZ) * e4;
            }
            if (angle5 != 0)
            {
                //e1 = E1;//
                //e2 = E2;//
                //e3 = E3;//
                //e4 = E4;//
                E1 = Math.Cos(aYT) * e1 - Math.Sin(aYT) * e3;
                //E2 = new Vector(0, 1, 0, 0);
                E3 = Math.Sin(aYT) * e1 + Math.Cos(aYT) * e3;
                //E4 = new Vector(0, 0, 0, 1);
            }
            if (angle6 != 0)
            {
                //e1 = E1;// 
                //e2 = E2;// 
                //e3 = E3;// 
                //e4 = E4;//
                E1 = Math.Cos(aZT) * e1 - Math.Sin(aZT) * e2;
                E2 = Math.Sin(aZT) * e1 + Math.Cos(aZT) * e2;
                //E3 = new Vector(0, 0, 1, 0);
                //E4 = new Vector(0, 0, 0, 1);
            }
            */
            //матрицы перехода для шести углов:
            /*
             XY:
            1    0    0    0
            0    1    0    0
            0    0  cosa  sina
            0    0 -sina  cosa
             XZ:
            1    0    0    0
            0   cosb  0   sinb
            0    0    1    0
            0  -sinb  0   cosb
             XT:
            1    0    0    0
            0  cosc  sinc  0
            0 -sinc  cosc  0
            0    0    0    1
             YZ:
           cosd  0    0   sind
            0    1    0    0
            0    0    1    0
          -sind  0    0   cosd
             YT:
           cose  0  sine   0
            0    1    0    0
          -sine  0  cose   0
            0    0    0    1
             ZT:
            cosf  sinf  0   0
           -sinf  cosf  0   0
             0     0    1   0
             0     0    0   1
             перемножив эти матрицы получим:
                *математический и тригонометрический ад*
             */
        }

        public static void draw_point(d4 point, PictureBox p, Graphics g)
        {
            point.x = point.X * E1[0] + point.Y * E2[0] + point.Z * E3[0] + point.T * E4[0];
            point.y = point.X * E1[1] + point.Y * E2[1] + point.Z * E3[1] + point.T * E4[1];
            g.FillEllipse(Brushes.Black, (float)point.x + p.Width / 2, (float)point.y + p.Height / 2, point.Size, point.Size);
        }
        public static void draw_point(d4 point, PictureBox p, Graphics g, Brush bruh,bool is_draw_border)
        {
            point.x = point.X * E1[0] + point.Y * E2[0] + point.Z * E3[0] + point.T * E4[0];
            point.y = point.X * E1[1] + point.Y * E2[1] + point.Z * E3[1] + point.T * E4[1];
            g.FillEllipse(bruh, (float)point.x + p.Width / 2, (float)point.y + p.Height / 2, point.Size, point.Size);
            //exlusively for current program:
            if (is_draw_border)
            {
                g.DrawEllipse(new Pen(Color.Black), (float)point.x + p.Width / 2, (float)point.y + p.Height / 2, point.Size, point.Size);
                //g.DrawEllipse(new Pen(Color.Black), (float)point.x + p.Width / 2 - 0.25f, (float)point.y + p.Height / 2 - 0.25f, point.Size + 0.5f, point.Size + 0.5f);
                g.DrawEllipse(new Pen(Color.White), (float)point.x + p.Width / 2 - 0.5f, (float)point.y + p.Height / 2 - 0.5f, point.Size + 1, point.Size + 1);
            }
        }
        public static void draw_line(d4 point1, d4 point2, PictureBox p, Graphics g)
        {
            point1.x = point1.X * E1[0] + point1.Y * E2[0] + point1.Z * E3[0] + point1.T * E4[0];
            point1.y = point1.X * E1[1] + point1.Y * E2[1] + point1.Z * E3[1] + point1.T * E4[1];
            point2.x = point2.X * E1[0] + point2.Y * E2[0] + point2.Z * E3[0] + point2.T * E4[0];
            point2.y = point2.X * E1[1] + point2.Y * E2[1] + point2.Z * E3[1] + point2.T * E4[1];
            g.DrawLine(Pens.Black, (float)point1.x + p.Width / 2, (float)point1.y + p.Height / 2, (float)point2.x + p.Width / 2, (float)point2.y + p.Height / 2);
        }
        static void draw_line_color(d4 point1, d4 point2, PictureBox p, Graphics g, Color col)
        {
            Pen pn = new Pen(col);
            g.DrawLine(pn, (float)point1.X + p.Width / 2, (float)point1.Y + p.Height / 2, (float)point2.X + p.Width / 2, (float)point2.Y + p.Height / 2);
        }
        public static void draw_basis(PictureBox p, Graphics g)
        {
            d4 x = new d4(100 * E1[0], 100 * E1[1], 100 * E1[2], 100 * E1[3]);
            d4 y = new d4(100 * E2[0], 100 * E2[1], 100 * E2[2], 100 * E2[3]);
            d4 z = new d4(100 * E3[0], 100 * E3[1], 100 * E3[2], 100 * E3[3]);
            d4 t = new d4(100 * E4[0], 100 * E4[1], 100 * E4[2], 100 * E4[3]);
            d4 zero = new d4(0, 0, 0, 0);
            draw_line_color(zero, x, p, g, Color.Red);
            draw_line_color(zero, y, p, g, Color.Green);
            draw_line_color(zero, z, p, g, Color.Blue);
            draw_line_color(zero, t, p, g, Color.Gold);

        }
        public static void reset()
        {
            E1 = new Vector(1, 0, 0, 0);
            E2 = new Vector(0, 1, 0, 0);
            E3 = new Vector(0, 0, 1, 0);
            E4 = new Vector(0, 0, 0, 1);
        }
    }
}
