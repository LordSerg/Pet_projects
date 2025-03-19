using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1
{
    class Brain
    {//нейросеть
        //вход:  8      2
        //слой1: 6+1    4+1
        //выход: 4      2
        //8 → 6+1(энергия) → 4:
        public sinaps[,] AB, BC;
        public neuron[] n;
        double sq_sumError = 0;
        public double error;
        int sets = 1;
        double moment = 0.1, speed = 0.1;
        int n1, n2, n3;//соответственно входной, центральный и выходной слои
        public Brain(/*int _n1,int _n2, int _n3*/)
        {//инициализация
            Random r = new Random();
            /*n1 = _n1;//n1 = 8;
            n2 = _n2;//n2 = 7;
            n3 = _n3;//n3 = 4;
            n = new neuron[n1+n2+n3];
            AB = new sinaps[n1, n2-1];//синапсы от первого слоя ко второму
            for (int i = 0; i < n1; i++)
                for (int j = 0; j < n2-1; j++)
                    AB[i, j] = new sinaps(1 + r.NextDouble());//[1,2]
            BC = new sinaps[n2, n3];//синапсы от второго слоя к третьему
            for (int i = 0; i < n2; i++)
                for (int j = 0; j < n3; j++)
                    BC[i, j] = new sinaps(1 + r.NextDouble());//[1,2]
            */
            n = new neuron[19];
            AB = new sinaps[8, 6];//синапсы от первого слоя ко второму
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 6; j++)
                    AB[i, j] = new sinaps(1-r.NextDouble());//[1,2]
            BC = new sinaps[7, 4];//синапсы от второго слоя к третьему
            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 4; j++)
                    BC[i, j] = new sinaps(1-r.NextDouble());//[1,2]
            //...
        }
        public double[] GetAnswer(double[] a/*, int energy*/)
        {//функция, пропускающая вводные данные через персептрон и дающая ответ

            double sum;
            /*
            double[] answer = new double[n3];
            for (int i = 0; i < n1; i++)
            {
                n[i].value = a[i];
                n[i].value = neuron.ActivFunction(n[i].value);//?
            }
            //от первого слоя - ко второму:
            n[n1].value = 1;
            for (int i = 0; i < n2-1; i++)
            {
                sum = 0;
                for (int j = 0; j < n1; j++)
                {
                    sum += AB[j, i].weight * n[j].value;
                }
                n[i + n1+1].value = neuron.ActivFunction(sum);
            }
            //от второго к третьему:
            for (int i = 0; i < n3; i++)
            {
                sum = 0;
                for (int j = 0; j < n2; j++)
                {
                    sum += BC[j, i].weight * n[j + n1].value;
                }
                n[i + n1+n2].value = neuron.ActivFunction(n[i + n1+n2].value);
                answer[i] = n[i + n1+n2].value;
            }*/
            
            double[] answer = new double[4];
            for (int i = 0; i < 8; i++)
            {
                n[i].value = a[i];
                //n[i].value = neuron.ActivFunction(n[i].value);//?
            }
            //от первого слоя - ко второму:
            for (int i = 0; i < 6; i++)
            {
                sum = 0;
                for (int j = 0; j < 8; j++)
                {
                    sum += AB[j, i].weight * n[j].value;
                }
                n[i + 9].value = neuron.ActivFunction(sum);
            }
            //от второго к третьему:
            n[8].value = 1;
            for (int i = 0; i < 4; i++)
            {
                sum = 0;
                for (int j = 0; j < 7; j++)
                {
                    sum += BC[j, i].weight * n[j + 8].value;
                }
                n[i + 15].value = neuron.ActivFunction(n[i + 15].value);
                answer[i] = n[i + 15].value;
            }
            //answer[0] :   [0   ,0.25) ~ 1
            //              [0.25, 0.5) ~ 2
            //              [0.5 ,0.75) ~ 3
            //              [0.75,   1] ~ 4
            //answer[1,2,3]: max(answer[i]):i=[1,3]=>
            //            if  imax=1 => идти
            //            if  imax=2 => стрелять
            //            if  imax=3 => родить
            /*double[] answ = new double[2];
            answ[0] = answer[0];
            answ[1] = max(answer[1], max(answer[2], answer[3]));*/
            return answer;
        }
        double max(double v1, double v2)
        {
            if (v1 > v2)
                return v1;
            else
                return v2;
        }
        public void change_sin(double []a,double[] neededAnswer)
        {//функция, которая меняет весы синапсов

            //double []o = new double[4];
            //o = this.GetAnswer(a);
            int t = 0;
            //do
            {
                GetAnswer(a);
                for (int i = 0; i < 4; i++)
                {
                    sq_sumError += (neededAnswer[i] - n[i + 15].value) * (neededAnswer[i] - n[i + 15].value);
                    n[i + 15].delta = (neededAnswer[i] - n[i + 15].value) * (1 - n[i + 15].value) * n[i + 15].value;
                }
                error = Math.Sqrt(sq_sumError / sets);
                double sum;
                //расчет дельты от третьего слоя ко второму
                for (int i = 8/*9*/; i < 15; i++)
                {
                    sum = 0;
                    for (int j = 15; j < 19; j++)
                    {
                        sum += BC[i - 8, j - 15].weight * n[j].delta;
                    }
                    n[i].delta = sum * (1 - n[i].value) * n[i].value;
                }
                //расчет дельты от второго слоя к первому
                for (int i = 0; i < 8; i++)
                {
                    sum = 0;
                    for (int j = 9; j < 15; j++)
                    {
                        sum += AB[i, j - 9].weight * n[j].delta;
                    }
                    n[i].delta = sum * (1 - n[i].value) * n[i].value;
                }
                //подсчет градиента синапсов:
                for (int i = 15; i <= 18; i++)
                    for (int j = 8; j <= 14; j++)
                        BC[j - 8, i - 15].grad = n[i].delta * n[j].value;
                for (int i = 9; i <= 14; i++)
                    for (int j = 0; j <= 7; j++)
                        AB[j, i - 9].grad = n[i].delta * n[j].value;
                //меняем веса синапсов:
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 6; j++)
                        AB[i, j].ChangeWeight(speed, moment);
                for (int i = 0; i < 7; i++)
                    for (int j = 0; j < 4; j++)
                        BC[i, j].ChangeWeight(speed, moment);
                sets++;
                

            }
            //while (t < 0);//)error > 0.9);
            
        }//если я ничего не перепутал, то все должно работать♥
    }
    struct sinaps
    {
        public double weight, change, grad;
        public sinaps(double somth)
        {
            weight = somth;
            grad = 0;
            change = 0;
        }
        public void ChangeWeight(double studySpeed, double moment)
        {
            this.change = studySpeed * grad + change * moment;
            this.weight += change;
        }
        //public double ActivFunction(double sum)
        //{
        //    return 1 / (1 + Math.Pow(Math.E, -sum));
        //}
    }
    struct neuron
    {
        public double value;
        public double delta;
        public static double ActivFunction(double sum)
        {
            return 1 / (1 + Math.Pow(Math.E, -sum));
        }
    }
}
