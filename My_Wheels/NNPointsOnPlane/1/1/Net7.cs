using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1
{
    class Net7
    {//Deep Residual Network (DRN)
        //сеть 2-2-2-2-2-2-2-2-2
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
            s = new Synapse[33];
            n = new Net[17];
            Random r = new Random();
            for (int i = 0; i < 17; i++)
                n[i] = new Net();
            for (int i = 0; i < 33; i++)
            {
                s[i] = new Synapse();
                s[i].Weight = 1 + r.NextDouble();//10;//
            }
        }
        public static void Study(double in1, double in2, double out1)
        {
            n[0].OUT = in1;
            n[1].OUT = in2;
            n[2].IN = s[0].Weight * n[0].OUT + s[2].Weight * n[1].OUT;
            n[3].IN = s[1].Weight * n[0].OUT + s[3].Weight * n[1].OUT;
            n[2].culc();
            n[3].culc();
            n[4].IN = s[5].Weight * n[2].OUT + s[7].Weight * n[3].OUT;
            n[5].IN = s[6].Weight * n[2].OUT + s[8].Weight * n[3].OUT;
            n[4].culc();
            n[5].culc();
            n[6].IN = s[4].Weight * n[2].OUT + s[9].Weight * n[4].OUT + s[11].Weight * n[5].OUT;
            n[7].IN = s[10].Weight * n[4].OUT + s[12].Weight * n[5].OUT;
            n[6].culc();
            n[7].culc();
            n[8].IN = s[14].Weight * n[6].OUT + s[16].Weight * n[7].OUT;
            n[9].IN = s[15].Weight * n[6].OUT + s[17].Weight * n[7].OUT;
            n[8].culc();
            n[9].culc();
            n[10].IN = s[18].Weight * n[8].OUT + s[20].Weight * n[9].OUT + s[13].Weight * n[6].OUT;
            n[11].IN = s[19].Weight * n[8].OUT + s[21].Weight * n[9].OUT;
            n[10].culc();
            n[11].culc();
            n[12].IN = s[23].Weight * n[10].OUT + s[25].Weight * n[11].OUT;
            n[13].IN = s[24].Weight * n[10].OUT + s[26].Weight * n[11].OUT;
            n[12].culc();
            n[13].culc();
            n[14].IN = s[27].Weight * n[12].OUT + s[29].Weight * n[13].OUT + s[22].Weight * n[10].OUT;
            n[15].IN = s[28].Weight * n[12].OUT + s[30].Weight * n[13].OUT;
            n[14].culc();
            n[15].culc();
            n[16].IN = s[31].Weight * n[14].OUT + s[32].Weight * n[15].OUT;
            n[16].culc();
            Net_answer = Convert.ToInt32(n[16].OUT);//если OUt>0.5, то 1 иначе - 0
            squed_sum_of_errors += (out1 - n[16].OUT) * (out1 - n[16].OUT);
            //squed_sum_of_errors += (real_answer - Net_answer) * (real_answer - Net_answer);
            error = Math.Sqrt(squed_sum_of_errors / sets);
            //подсчет дельты
            n[16].DELTA = (out1 - n[16].OUT) * (1 - n[16].OUT) * n[16].OUT;//дельта выходного нейрона

            n[15].DELTA = s[32].Weight * n[16].DELTA * (1 - n[15].OUT) * n[15].OUT;//дельты скрытых нейронов
            n[14].DELTA = s[31].Weight * n[16].DELTA * (1 - n[14].OUT) * n[14].OUT;
            n[13].DELTA = (s[30].Weight * n[15].DELTA + s[29].Weight * n[14].DELTA) * (1 - n[13].OUT) * n[13].OUT;
            n[12].DELTA = (s[28].Weight * n[15].DELTA + s[27].Weight * n[14].DELTA) * (1 - n[12].OUT) * n[12].OUT;
            n[11].DELTA = (s[26].Weight * n[13].DELTA + s[25].Weight * n[12].DELTA) * (1 - n[11].OUT) * n[11].OUT;
            n[10].DELTA = (s[24].Weight * n[13].DELTA + s[23].Weight * n[12].DELTA + s[22].Weight * n[14].DELTA) * (1 - n[10].OUT) * n[10].OUT;
            n[9].DELTA = (s[21].Weight * n[11].DELTA + s[20].Weight * n[10].DELTA) * (1 - n[9].OUT) * n[9].OUT;
            n[8].DELTA = (s[19].Weight * n[11].DELTA + s[18].Weight * n[10].DELTA) * (1 - n[8].OUT) * n[8].OUT;
            n[7].DELTA = (s[17].Weight * n[9].DELTA + s[16].Weight * n[8].DELTA) * (1 - n[7].OUT) * n[7].OUT;
            n[6].DELTA = (s[15].Weight * n[9].DELTA + s[14].Weight * n[8].DELTA + s[13].Weight * n[10].DELTA) * (1 - n[6].OUT) * n[6].OUT;
            n[5].DELTA = (s[12].Weight * n[7].DELTA + s[11].Weight * n[6].DELTA) * (1 - n[5].OUT) * n[5].OUT;
            n[4].DELTA = (s[10].Weight * n[7].DELTA + s[9].Weight * n[6].DELTA) * (1 - n[4].OUT) * n[4].OUT;
            n[3].DELTA = (s[8].Weight * n[5].DELTA + s[7].Weight * n[4].DELTA) * (1 - n[3].OUT) * n[3].OUT;
            n[2].DELTA = (s[6].Weight * n[5].DELTA + s[5].Weight * n[4].DELTA + s[4].Weight * n[6].DELTA) * (1 - n[2].OUT) * n[2].OUT;
            //n[3].DELTA = s[5].Weight * n[4].DELTA;
            //n[2].DELTA = s[4].Weight * n[4].DELTA;
            //дельты для вводных нейронов считать не обязательно, так как к ним на вход не подключаются синапсы
            //нахождение градиента синапсов:
            s[32].culc_gr(n[16].DELTA, n[15].OUT);
            s[31].culc_gr(n[16].DELTA, n[14].OUT);
            s[30].culc_gr(n[15].DELTA, n[13].OUT);
            s[29].culc_gr(n[14].DELTA, n[13].OUT);
            s[28].culc_gr(n[15].DELTA, n[12].OUT);
            s[27].culc_gr(n[14].DELTA, n[12].OUT);
            s[26].culc_gr(n[13].DELTA, n[11].OUT);
            s[25].culc_gr(n[12].DELTA, n[11].OUT);
            s[24].culc_gr(n[13].DELTA, n[10].OUT);
            s[23].culc_gr(n[12].DELTA, n[10].OUT);
            s[22].culc_gr(n[14].DELTA, n[10].OUT);
            s[21].culc_gr(n[11].DELTA, n[9].OUT);
            s[20].culc_gr(n[10].DELTA, n[9].OUT);
            s[19].culc_gr(n[11].DELTA, n[8].OUT);
            s[18].culc_gr(n[10].DELTA, n[8].OUT);
            s[17].culc_gr(n[9].DELTA, n[7].OUT);
            s[16].culc_gr(n[8].DELTA, n[7].OUT);
            s[15].culc_gr(n[9].DELTA, n[6].OUT);
            s[14].culc_gr(n[8].DELTA, n[6].OUT);
            s[13].culc_gr(n[10].DELTA, n[6].OUT);
            s[12].culc_gr(n[7].DELTA, n[5].OUT);
            s[11].culc_gr(n[6].DELTA, n[5].OUT);
            s[10].culc_gr(n[7].DELTA, n[4].OUT);
            s[9].culc_gr(n[6].DELTA, n[4].OUT);
            s[8].culc_gr(n[5].DELTA, n[3].OUT);
            s[7].culc_gr(n[4].DELTA, n[3].OUT);
            s[6].culc_gr(n[5].DELTA, n[2].OUT);
            s[5].culc_gr(n[4].DELTA, n[2].OUT);
            s[4].culc_gr(n[6].DELTA, n[2].OUT);
            s[3].culc_gr(n[3].DELTA, n[1].OUT);
            s[2].culc_gr(n[2].DELTA, n[1].OUT);
            s[1].culc_gr(n[3].DELTA, n[0].OUT);
            s[0].culc_gr(n[2].DELTA, n[0].OUT);
            //нахождение изменения веса синапса:
            for (int i = 0; i < 33; i++)
                s[i].culc_ch(study_speed, moment);
            sets++;
        }
        public static double Answer(double in1, double in2)
        {
            n[0].OUT = in1;
            n[1].OUT = in2;
            n[2].IN = s[0].Weight * n[0].OUT + s[2].Weight * n[1].OUT;
            n[3].IN = s[1].Weight * n[0].OUT + s[3].Weight * n[1].OUT;
            n[2].culc();
            n[3].culc();
            n[4].IN = s[5].Weight * n[2].OUT + s[7].Weight * n[3].OUT;
            n[5].IN = s[6].Weight * n[2].OUT + s[8].Weight * n[3].OUT;
            n[4].culc();
            n[5].culc();
            n[6].IN = s[4].Weight * n[2].OUT + s[9].Weight * n[4].OUT + s[11].Weight * n[5].OUT;
            n[7].IN = s[10].Weight * n[4].OUT + s[12].Weight * n[5].OUT;
            n[6].culc();
            n[7].culc();
            n[8].IN = s[14].Weight * n[6].OUT + s[16].Weight * n[7].OUT;
            n[9].IN = s[15].Weight * n[6].OUT + s[17].Weight * n[7].OUT;
            n[8].culc();
            n[9].culc();
            n[10].IN = s[18].Weight * n[8].OUT + s[20].Weight * n[9].OUT + s[13].Weight * n[6].OUT;
            n[11].IN = s[19].Weight * n[8].OUT + s[21].Weight * n[9].OUT;
            n[10].culc();
            n[11].culc();
            n[12].IN = s[23].Weight * n[10].OUT + s[25].Weight * n[11].OUT;
            n[13].IN = s[24].Weight * n[10].OUT + s[26].Weight * n[11].OUT;
            n[12].culc();
            n[13].culc();
            n[14].IN = s[27].Weight * n[12].OUT + s[29].Weight * n[13].OUT + s[22].Weight * n[10].OUT;
            n[15].IN = s[28].Weight * n[12].OUT + s[30].Weight * n[13].OUT;
            n[14].culc();
            n[15].culc();
            n[16].IN = s[31].Weight * n[14].OUT + s[32].Weight * n[15].OUT;
            n[16].culc();
            return n[16].OUT;
        }
    }
}
