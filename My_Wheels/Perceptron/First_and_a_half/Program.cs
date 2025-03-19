using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First_and_a_half
{
    //НС на основе алгоритма обратного распознавания ошибки.
    //в данном случае я не стал прибегать к общему решению,
    //а просто обработал каждую операцию вручную
    //(сделана очень криво, но работает)

    class Program
    {
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
        static void Main(string[] args)
        {
            Synapse[] s = new Synapse[6];
            double Net_answer;
            int real_answer,  num = 0, sets = 1;
            double study_speed = 0.5, moment = 0.8, error;
            double squed_sum_of_errors = 0;
            Random r = new Random();
            Console.WriteLine("Starting sinaps weights");
            for (int i = 0; i < 6; i++)
            {
                s[i] = new Synapse();
                s[i].Weight = 1+r.NextDouble();//10;//
                Console.WriteLine("w[{0}]={1}", i, s[i].Weight);
            }
            /*s[0].Weight =1;
            s[1].Weight =0;
            s[2].Weight =0;
            s[3].Weight =1;
            s[4].Weight =-1;
            s[5].Weight =1;*/
            Net[] n = new Net[5];
            for (int i = 0; i < 5; i++)
                n[i] = new Net();
            do
            {
                //Console.Write("Введите x (1 или 0): ");
                n[0].OUT = r.Next(0, 2);//Convert.ToInt32(Console.ReadLine());//
                //Console.Write("Введите x (1 или 0): ");
                n[1].OUT = r.Next(0, 2);//Convert.ToInt32(Console.ReadLine());//
                if (n[0].OUT ==0|| n[1].OUT==0)//пересечение
                    real_answer = 0;
                else
                    real_answer = 1;

                //for (int i = 2; i < 4; i++)
                //{
                //    n[i].IN = 0;
                //    for(int j=0;j<2;j++)
                //    {
                //        n[i].IN += s[j+(i-2)*2].Weight * n[i].OUT;
                //    }
                //    n[i].culc();
                //}
                n[2].IN = s[0].Weight * n[0].OUT + s[1].Weight * n[1].OUT;
                n[3].IN = s[2].Weight * n[0].OUT + s[3].Weight * n[1].OUT;
                n[2].culc();
                n[3].culc();
                n[4].IN = s[4].Weight * n[2].OUT + s[5].Weight * n[3].OUT;
                n[4].culc();
                //Net_answer = (n[4].OUT > 0.8) ? 1 : (n[4].OUT < 0.2) ? 0 : n[4].OUT;
                Net_answer = Convert.ToInt32(n[4].OUT);//если OUt>0.5, то 1 иначе - 0
                squed_sum_of_errors += (real_answer - n[4].OUT) * (real_answer - n[4].OUT);
                //squed_sum_of_errors += (real_answer - Net_answer) * (real_answer - Net_answer);
                error = Math.Sqrt(squed_sum_of_errors / sets);
                //подсчет дельты
                n[4].DELTA = /*(error)*/(real_answer - n[4].OUT) * (1 - n[4].OUT) * n[4].OUT    ;//дельта выходного нейрона

                n[3].DELTA = s[5].Weight * n[4].DELTA * (1 - n[3].OUT) * n[3].OUT;//дельты скрытых нейронов
                n[2].DELTA = s[4].Weight * n[4].DELTA * (1 - n[2].OUT) * n[2].OUT;
                //n[3].DELTA = s[5].Weight * n[4].DELTA;
                //n[2].DELTA = s[4].Weight * n[4].DELTA;
                //дельты для вводных нейронов считать не обязательно, так как к ним на вход не подключаются синапсы
                //нахождение градиента синапсов:
                s[5].culc_gr(n[4].DELTA, n[3].OUT);
                s[4].culc_gr(n[4].DELTA, n[2].OUT);
                s[3].culc_gr(n[3].DELTA, n[1].OUT);
                s[2].culc_gr(n[3].DELTA, n[0].OUT);
                s[1].culc_gr(n[2].DELTA, n[1].OUT);
                s[0].culc_gr(n[2].DELTA, n[0].OUT);
                //нахождение изменения веса синапса:
                for (int i = 0; i < 6; i++)
                    s[i].culc_ch(study_speed, moment);
                if (real_answer == Net_answer)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    num++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    num = 0;
                }
                //Console.WriteLine("х = {0}; y = {1}; x+y(real)={2}; x+y(neiro)={4}={3}; error = {5}\n", n[0].OUT, n[1].OUT, real_answer, n[4].OUT, Net_answer, error);
                //Console.Write("\rerror = {0}    \tnum = {1}   ", error, num);
                Console.Write("\r");
                for (int i = 0; i < 6; i++)
                {
                    Console.Write("w[{0}]={1}\t", i, Math.Round(s[i].Weight, 2));
                }
                Console.Write("error = {0}", Math.Round(error, 2));
                sets++;
            } while (/*num < 10000*/error>0.03);
            Console.Write("\n\n");
            for (int i = 0; i < s.Length; i++)
                Console.WriteLine("w[{0}]={1}\t", i, s[i].Weight);
            Console.Write("\n\n");
            int x, y;
            do
            {
                Console.WriteLine("Enter x (1 or 0): ");
                n[0].OUT= Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter y (1 or 0): ");
                n[1].OUT= Convert.ToInt32(Console.ReadLine());

                n[2].IN = s[0].Weight * n[0].OUT + s[1].Weight * n[1].OUT;
                n[3].IN = s[2].Weight * n[0].OUT + s[3].Weight * n[1].OUT;
                n[2].culc();
                n[3].culc();
                n[4].IN = s[4].Weight * n[2].OUT + s[5].Weight * n[3].OUT;
                n[4].culc();
                Console.WriteLine("NN thinks that {0}&{1} = {2}", n[0].OUT, n[1].OUT, n[4].OUT);
            } while (n[0].OUT != 0 || n[0].OUT != 1);
            Console.ReadKey();
        }
    }
}
