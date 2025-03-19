﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1
{
    class Net4
    {//сеть (2+1)-2-1
        class Net
        {
            public double OUT, IN, DELTA;
            public Net()
            {

            }
            public void culc()
            {//устаканиваем значения по сигмоиду
                OUT = 1 / (1 + Math.Pow(Math.E, -IN));
            }

        }
        class Synapse
        {
            public double Weight, GRAD, change = 0;
            public Synapse()
            {
                //Random r = new Random();
                //Weight = r.Next(-5, 6);
            }
            public void culc_gr(double delta_b, double out_a)
            {
                GRAD = delta_b * out_a;
            }
            public void culc_ch(double a, double b)
            {
                change = a * GRAD + change * b;
                Weight += change;
            }
        }
        static Net[] n;
        static Synapse[] s;
        public static double Net_answer, squed_sum_of_errors = 0, error;
        public static double study_speed = 0.5, moment = 0.8;
        static int sets = 1;
        public static void Activate()
        {
            s = new Synapse[8];
            n = new Net[6];
            Random r = new Random();
            for (int i = 0; i < 6; i++)
                n[i] = new Net();
            for (int i = 0; i < 8; i++)
            {
                s[i] = new Synapse();
                s[i].Weight = 1 + r.NextDouble();//10;//
            }
        }
        public static void Study(double in1, double in2, double out1)
        {
            n[0].OUT = in1;
            n[1].OUT = in2;
            n[2].OUT = 1;
            n[3].IN = s[0].Weight * n[0].OUT + s[2].Weight * n[1].OUT+ s[4].Weight * n[2].OUT;
            n[4].IN = s[1].Weight * n[0].OUT + s[3].Weight * n[1].OUT+ s[5].Weight * n[2].OUT;
            n[3].culc();
            n[4].culc();
            n[5].IN = s[6].Weight * n[3].OUT + s[7].Weight * n[4].OUT;
            n[5].culc();
            Net_answer = Convert.ToInt32(n[5].OUT);//если OUt>0.5, то 1 иначе - 0
            squed_sum_of_errors += (out1 - n[5].OUT) * (out1 - n[5].OUT);
            //squed_sum_of_errors += (real_answer - Net_answer) * (real_answer - Net_answer);
            error = Math.Sqrt(squed_sum_of_errors / sets);
            //подсчет дельты
            n[5].DELTA = (out1 - n[5].OUT) * (1 - n[5].OUT) * n[5].OUT;//дельта выходного нейрона

            n[4].DELTA = s[7].Weight * n[5].DELTA * (1 - n[4].OUT) * n[4].OUT;//дельты скрытых нейронов
            n[3].DELTA = s[6].Weight * n[5].DELTA * (1 - n[3].OUT) * n[3].OUT;
            //дельты для вводных нейронов считать не обязательно, так как к ним на вход не подключаются синапсы
            //нахождение градиента синапсов:
            s[7].culc_gr(n[5].DELTA, n[4].OUT);
            s[6].culc_gr(n[5].DELTA, n[3].OUT);
            s[5].culc_gr(n[4].DELTA, n[2].OUT);
            s[4].culc_gr(n[3].DELTA, n[2].OUT);
            s[3].culc_gr(n[2].DELTA, n[1].OUT);
            s[2].culc_gr(n[3].DELTA, n[1].OUT);
            s[1].culc_gr(n[4].DELTA, n[0].OUT);
            s[0].culc_gr(n[3].DELTA, n[0].OUT);
            //нахождение изменения веса синапса:
            for (int i = 0; i < 8; i++)
                s[i].culc_ch(study_speed, moment);
            sets++;
        }
        public static double Answer(double in1, double in2)
        {
            n[0].OUT = in1;
            n[1].OUT = in2;
            n[2].OUT = 1;
            n[3].IN = s[0].Weight * n[0].OUT + s[2].Weight * n[1].OUT + s[4].Weight * n[2].OUT;
            n[4].IN = s[1].Weight * n[0].OUT + s[3].Weight * n[1].OUT + s[5].Weight * n[2].OUT;
            n[3].culc();
            n[4].culc();
            n[5].IN = s[6].Weight * n[3].OUT + s[7].Weight * n[4].OUT;
            n[5].culc();
            return n[5].OUT;
        }
    }
}
