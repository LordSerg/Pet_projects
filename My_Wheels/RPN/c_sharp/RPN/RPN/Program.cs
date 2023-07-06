using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPN
{
    class Program
    {
        static void Main(string[] args)
        {
            //real equasion example
            Console.WriteLine("Input a math equasion to calculate, for example: '5 * 7 + ( 4 - 1 ) * 9'");
            Console.Write("input:\n      ");
            string input = Console.ReadLine(); //"3 + 4 * 2 / ( 1 - 5 ) ^ 2"; //
            //Console.WriteLine(input);
            Console.Write("output:\n       ");
            RealEquasion r = new RealEquasion(input);
            Console.WriteLine(r.ToString());
            double[] arr = new double[r.NumOfVariables];
            string aRgUmEnTs = "";
            for (int i = 0; i < r.NumOfVariables; i++)
            {
                arr[i] = i;
                aRgUmEnTs += arr[i] + ", ";
            }
            if (aRgUmEnTs.Length > 1)
                aRgUmEnTs = aRgUmEnTs.Remove(aRgUmEnTs.Length - 2, 2);
            Console.Write("f(" + aRgUmEnTs + ") = " + r.Calc(arr));

            

            /*
            //Boolean equasion example
            Console.WriteLine("Input a math equasion to calculate, avaible operations: \n" +
                "prior0:\t'(' or ')' - breckets" +         //+
                "\nprior1:\t'!' - not" +                   //+
                "\nprior2:\t'&' - and" +                   //+
                "\nprior3:\t'+' - XOR (or bitwise sum)" +  //-
                "\nprior3:\t'=' - equivalence" +           //-
                "\nprior3:\t'>' - implication" +           //-
                "\nprior4:\t'V' - or");                    //+
            Console.Write("input:\n      ");
            string input = Console.ReadLine(); //"x&z&y";//"3 + 4 * 2 / ( 1 - 5 ) ^ 2"; //
            //Console.WriteLine(input);
            BoolEquasion a = new BoolEquasion(input);
            Console.Write("output:\n");
            a.CalcForAllValues();
            */

            /*
            Equasion x = new Equasion();
            int a = 0;
            List<string> s = new List<string>();
            string []qwer = x.SplitString("x_0 + x_1",ref a,ref s);
            Console.WriteLine("Splitted list:");
            for (int i = 0; i < qwer.Length; i++)
            {
                Console.WriteLine(qwer[i]);
            }
            Console.WriteLine("Variables list:");
            for (int i = 0; i < s.Count; i++)
            {
                Console.WriteLine(s[i]);
            }
            */

            Console.ReadKey();
        }
        static void ShowStack(Stack<string> stack)
        {
            string []str= stack.ToArray();
            for(int i= str.Length-1; i>=0;i--)
                Console.Write(str[i] + " ");
            Console.WriteLine();
        }
    }
}
