using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1
{
    class Net6
    {//сеть 2-3-3-1
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
            s = new Synapse[18];
            n = new Net[9];
            Random r = new Random();
            for (int i = 0; i < 9; i++)
                n[i] = new Net();
            for (int i = 0; i < 18; i++)
            {
                s[i] = new Synapse();
                s[i].Weight = 1 + r.NextDouble();//10;//
            }
        }
        public static void Study(double in1, double in2, double out1)
        {
            double ans = Answer(in1, in2);
            Net_answer = Convert.ToInt32(ans);//если OUt>0.5, то 1 иначе - 0
            squed_sum_of_errors += (out1 - ans) * (out1 - ans);
            //squed_sum_of_errors += (real_answer - Net_answer) * (real_answer - Net_answer);
            error = Math.Sqrt(squed_sum_of_errors / sets);
            //подсчет дельты

            n[2 + 2 * 3].DELTA = (out1 - n[2 + 2 * 3].OUT) * (1 - n[2 + 2 * 3].OUT) * n[2 + 2 * 3].OUT;
            double sum = 0;
            //цикл для предпоследних HNum нейронов (для предпоследнего слоя)
            for(int i=2+2*3-1;i>2+2*3-1-3;i--)
            {
                n[i].DELTA = s[2*3+(2-1)*3*3+(i-(2+2*3-3))].Weight * n[2 + 2 * 3].DELTA * (1 - n[i].OUT) * n[i].OUT;
            }
            //
            for (int i = (2-1);/*всего доп слоев -1*/i > 0; i--)
                for (int j = (2+i*3)-1; j >= 2/*первые 2 нейрона*/; j--)
                {
                    sum = 0;
                    for (int k = 2+((i + 1) * 3) - 1, t = 0; t < 3; t++, k--)
                    {
                        sum += s[2*3+i*3*3-1-j*3-t].Weight * n[k].DELTA;
                    }
                    n[j].DELTA = sum * (1 - n[j].OUT) * n[j].OUT;
                }
            /*
            n[8].DELTA = (out1 - n[8].OUT) * (1 - n[8].OUT) * n[8].OUT;//дельта выходного нейрона

            n[7].DELTA = s[17].Weight * n[8].DELTA * (1 - n[7].OUT) * n[7].OUT;//дельты скрытых нейронов
            n[6].DELTA = s[16].Weight * n[8].DELTA * (1 - n[6].OUT) * n[6].OUT;//дельты скрытых нейронов
            n[5].DELTA = s[15].Weight * n[8].DELTA * (1 - n[5].OUT) * n[5].OUT;//дельты скрытых нейронов
            
            n[4].DELTA = (s[14].Weight * n[7].DELTA + s[13].Weight * n[6].DELTA + s[12].Weight * n[5].DELTA) * (1 - n[4].OUT) * n[4].OUT;//дельты скрытых нейроно
            n[3].DELTA = (s[11].Weight * n[7].DELTA + s[10].Weight * n[6].DELTA + s[9].Weight * n[5].DELTA) * (1 - n[3].OUT) * n[3].OUT;
            n[2].DELTA = (s[8].Weight * n[7].DELTA + s[7].Weight * n[6].DELTA + s[6].Weight * n[5].DELTA) * (1 - n[2].OUT) * n[2].OUT;//дельты скрытых нейронов
            */
            //n[3].DELTA = s[5].Weight * n[4].DELTA;
            //n[2].DELTA = s[4].Weight * n[4].DELTA;
            //дельты для вводных нейронов считать не обязательно, так как к ним на вход не подключаются синапсы
            //нахождение градиента синапсов:
            
            //от последнегоо слоя к предпоследнему
            for (int i = 1; i <= 3; i++)
                s[2*3+(2-1)*3*3+3-i].culc_gr(n[2+2*3].DELTA, n[2+2*3-i].OUT);
            //от предпоследнего ко второму
            for (int i = 2; i > 1; i--)
            {
                for (int j = 2 + 3 * i - 1,t=0; t<3; j--,t++)
                {
                    for (int u = 2 + (i - 1) * 3 - 1, k = 0; k < 3; u--, k++)
                    {
                        s[2 * 3 + (i - 1) * 3 * 3 - 1 - k * 3 - t].culc_gr(n[j].DELTA, n[u].OUT);
                    }
                }
            }
            //от второго к первому
            for(int i=2+3-1,k=0;i>1;i--,k++)
            {
                for (int j = 1; j >= 0; j--)
                    s[(j+1)*3-1-k].culc_gr(n[i].DELTA, n[j].OUT);
            }



            /*
            s[17].culc_gr(n[8].DELTA, n[7].OUT);
            s[16].culc_gr(n[8].DELTA, n[6].OUT);
            s[15].culc_gr(n[8].DELTA, n[5].OUT);
            s[14].culc_gr(n[7].DELTA, n[4].OUT);
            s[13].culc_gr(n[6].DELTA, n[4].OUT);
            s[12].culc_gr(n[5].DELTA, n[4].OUT);
            s[11].culc_gr(n[7].DELTA, n[3].OUT);
            s[10].culc_gr(n[6].DELTA, n[3].OUT);
            s[9].culc_gr(n[5].DELTA, n[3].OUT);
            s[8].culc_gr(n[7].DELTA, n[2].OUT);
            s[7].culc_gr(n[6].DELTA, n[2].OUT);
            s[6].culc_gr(n[5].DELTA, n[2].OUT);
            s[5].culc_gr(n[4].DELTA, n[1].OUT);
            s[4].culc_gr(n[3].DELTA, n[1].OUT);
            s[3].culc_gr(n[2].DELTA, n[1].OUT);
            s[2].culc_gr(n[4].DELTA, n[0].OUT);
            s[1].culc_gr(n[3].DELTA, n[0].OUT);
            s[0].culc_gr(n[2].DELTA, n[0].OUT);*/
            //нахождение изменения веса синапса:
            for (int i = 0; i < 2 * 3 + (2 - 1) * 3 * 3 + 3; i++)
                s[i].culc_ch(study_speed, moment);
            sets++;
        }
        public static double Answer(double in1, double in2)
        {
            n[0].OUT = in1;
            n[1].OUT = in2;
            for (int i = 2; i < 2 + 3; i++)//2 - входные Н, 3 - кол-во Н в доп.слое
            {
                n[i].IN = 0;
                for (int j = 0; j < 2; j++)//2 - входные Н
                    n[i].IN += s[j * 3 + i - 2].Weight * n[i - 2 + j].OUT;
                n[i].culc();
            }
            //2->3->4->...->Lnum-1
            for (int i = 1; i < (2 + 1) - 1; i++)//2 - кол-во доп. слоев
            {//(+1) - для сохранения нумирации слоев
             //(-1) - так как между n слоями находится n-1 промежуток
                for (int j = 0; j < 3; j++)//3 - кол-во нейронов в каждом доп. слое
                {
                    n[2 + i * 3 + j].IN = 0;
                    for (int k = 0; k < 3; k++)//3 - кол-во нейронов в каждом доп. слое
                    {
                        n[2 + j + i * 3].IN += s[2 * 3 + (i-1)*3*3 + j + k * 3].Weight * n[2 + (i - 1) * 3 + k].OUT;
                    }
                    n[2 + j + i * 3].culc();
                }
            }
            //от предпоследнего слоя к последнему:
            n[2 + 2 * 3].IN = 0;
            for (int j = 0; j < 3; j++)
            {
                n[2 + 2 * 3].IN += s[2 * 3 + (2 - 1) * 3 * 3+j].Weight * n[2 + 2 * 3 - 3 + j].OUT;
            }
            n[2 + 2 * 3].culc();
            /*
            n[2].IN = s[0].Weight * n[0].OUT + s[3].Weight * n[1].OUT;
            n[3].IN = s[1].Weight * n[0].OUT + s[4].Weight * n[1].OUT;
            n[4].IN = s[2].Weight * n[0].OUT + s[5].Weight * n[1].OUT;
            n[2].culc();
            n[3].culc();
            n[4].culc();
            n[5].IN = s[6].Weight * n[2].OUT + s[9].Weight * n[3].OUT + s[12].Weight * n[4].OUT;
            n[6].IN = s[7].Weight * n[2].OUT + s[10].Weight * n[3].OUT + s[13].Weight * n[4].OUT;
            n[7].IN = s[8].Weight * n[2].OUT + s[11].Weight * n[3].OUT + s[14].Weight * n[4].OUT;
            n[5].culc();
            n[6].culc();
            n[7].culc();
            n[8].IN = s[15].Weight * n[5].OUT + s[16].Weight * n[6].OUT + s[17].Weight * n[7].OUT;
            n[8].culc();
            */
            return n[2 + 2 * 3].OUT;
        }
    }
}
