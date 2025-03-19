using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1
{
    class NetU
    {
        //автоматическая сеть:
        //изначально есть 2 входных и 1 выходной нейрон
        //пользователь контролирует количество промежуточных слоев
        //и количество скрытых нейронов в кажом слое 
        //(в каждом скрытом слое одинаковое количество нейронов)
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
        static int sets = 1, LNum, HNum;
        public static void Activate(int Layers,int Neurons)
        {//предполагается, что введен хотябы 1 доп. слой с неменее, чем одним нейроном
            LNum = Layers;
            HNum = Neurons;
            s = new Synapse[HNum * (3 + HNum * (LNum - 1))];
            n = new Net[3 + LNum * HNum];
            Random r = new Random();
            for (int i = 0; i < 3 + LNum * HNum; i++)
                n[i] = new Net();
            for (int i = 0; i < HNum * (3 + HNum * (LNum - 1)); i++)
            {
                s[i] = new Synapse();
                s[i].Weight = 1 + r.NextDouble();
            }
        }
        public static void Study(double in1,double in2,double out1)
        {
            double ans = Answer(in1, in2);
            Net_answer=Convert.ToInt32(ans);
            squed_sum_of_errors += (out1 - ans) * (out1 - ans);
            error = Math.Sqrt(squed_sum_of_errors/sets);
            //подсчет дельты
            n[2 + LNum * HNum].DELTA = (out1 - n[2 + LNum * HNum].OUT) * (1 - n[2 + LNum * HNum].OUT) * n[2 + LNum * HNum].OUT;

            double sum = 0;
            //цикл для предпоследних HNum нейронов (для предпоследнего слоя)
            for (int i = 2 + LNum * HNum - 1; i > 2 + LNum * HNum - 1 - HNum; i--)
            {
                n[i].DELTA = s[2 * HNum + (LNum - 1) * HNum * HNum + (i - (2 + LNum * HNum - HNum))].Weight * n[2 + LNum * HNum].DELTA * (1 - n[i].OUT) * n[i].OUT;
            }
            //
            for (int i = (LNum - 1);/*всего доп слоев -1*/i > 0; i--)
                for (int j = (2 + i * HNum) - 1; j >= 2/*первые 2 нейрона*/; j--)
                {
                    sum = 0;
                    for (int k = 2 + ((i + 1) * HNum) - 1, t = 0; t < HNum; t++, k--)
                    {
                        sum += s[2 * HNum + i * HNum * HNum - 1 - j * HNum - t].Weight * n[k].DELTA;
                    }
                    n[j].DELTA = sum * (1 - n[j].OUT) * n[j].OUT;
                }
            //нахождение градиента синапсов:

            //от последнегоо слоя к предпоследнему
            for (int i = 1; i <= HNum; i++)
                s[2 * HNum + (LNum - 1) * HNum * HNum + HNum - i].culc_gr(n[2 + LNum * HNum].DELTA, n[2 + LNum * HNum - i].OUT);
            //от предпоследнего ко второму
            for (int i = LNum; i > 1; i--)
            {
                for (int j = 2 + HNum * i - 1, t = 0; t < HNum; j--, t++)
                {
                    for (int u = 2 + (i - 1) * HNum - 1, k = 0; k < HNum; u--, k++)
                    {
                        s[2 * HNum + (i - 1) * HNum * HNum - 1 - k * HNum - t].culc_gr(n[j].DELTA, n[u].OUT);
                    }
                }
            }
            //от второго к первому
            for (int i = 2 + HNum - 1, k = 0; i > 1; i--, k++)
            {
                for (int j = 1; j >= 0; j--)
                    s[(j + 1) * HNum - 1 - k].culc_gr(n[i].DELTA, n[j].OUT);
            }

            for (int i = 0; i < 2 * HNum + (LNum - 1) * HNum * HNum + HNum; i++)
                s[i].culc_ch(study_speed, moment);
            sets++;
        }
        public static double Answer(double in1, double in2)
        {
            n[0].OUT = in1;
            n[1].OUT = in2;
            for (int i = 2; i < 2 + HNum; i++)//2 - входные Н, 3 - кол-во Н в доп.слое
            {
                n[i].IN = 0;
                for (int j = 0; j < 2; j++)//2 - входные Н
                    n[i].IN += s[j * HNum + i - 2].Weight * n[i - 2 + j].OUT;
                n[i].culc();
            }
            //2->3->4->...->Lnum-1
            for (int i = 1; i < (LNum + 1) - 1; i++)//2 - кол-во доп. слоев
            {//(+1) - для сохранения нумирации слоев
             //(-1) - так как между n слоями находится n-1 промежуток
                for (int j = 0; j < HNum; j++)
                {
                    n[2 + j + i * HNum].IN = 0;
                    for (int k = 0; k < HNum; k++)//3 - кол-во нейронов в каждом доп. слое
                    {
                        n[2 + j + i * HNum].IN += s[2 * HNum + (i-1)*HNum*HNum + j + k * HNum].Weight * n[2 + (i - 1) * HNum + k].OUT;
                    }
                    n[2 + j + i * HNum].culc();
                }
            }
            //от предпоследнего слоя к последнему:
            n[2 + LNum * HNum].IN = 0;
            for (int j = 0; j < HNum; j++)
            {
                n[2 + LNum * HNum].IN += s[2 * HNum + (LNum - 1) * HNum * HNum + j].Weight * n[2 + LNum * HNum - HNum + j].OUT;
            }
            n[2 + LNum * HNum].culc();
            return n[2 + LNum * HNum].OUT;
        }
    }
}
