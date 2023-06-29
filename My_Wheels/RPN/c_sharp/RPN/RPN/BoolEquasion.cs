using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPN
{
    public class BoolEquasion: Equasion
    {
        //protected string input_string;//the input string
        //protected Stack<string> func;//reverse polish notation of input string
        //protected List<char> variables;//names of variables
        //protected int number_of_variables;//number of variables in function
        public BoolEquasion(string input)
        {
            input_string = input;
            variables = new List<string>();
            number_of_variables = 0;
            func = ToPostfixNotation(input, ref number_of_variables, ref variables);
        }
        /// <summary>
        /// returns postfix (or reverse polish) notation of the function
        /// </summary>
        /// <param name="input"> pure input data </param>
        /// <param name="num_of_vars"> return the number of variables in function </param>
        /// <param name="variables"> return names of variables, sorted from 'a' to 'z' </param>
        /// <returns></returns>
        protected Stack<string> ToPostfixNotation(string input, ref int num_of_vars, ref List<string> variables)
        {
            //not <- and <- or
            int index = 0;
            string[] sybls = SplitString(input, ref num_of_vars, ref variables);
            int max_length = sybls.Length;
            Stack<string> answer = new Stack<string>();
            Stack<char> st = new Stack<char>();
            while (index < max_length)
            {
                if (sybls[index] == "0" || sybls[index] == "1" || (sybls[index][0] >= 'a' && sybls[index][0] <= 'z'))
                {
                    answer.Push(sybls[index]);
                    //ShowStack(answer);
                    //calcBoolean(answer);
                    //ShowStack(answer);
                }
                else if (sybls[index] == "V")//preoryty = 4
                {//or
                    while (st.Count > 0 && (st.Peek() == '+' || st.Peek() == '=' || st.Peek() == '>' || st.Peek() == 'V' || st.Peek() == '&' || st.Peek() == '!'))
                    {
                        answer.Push(st.Pop().ToString());
                        //ShowStack(answer);
                        //calcBoolean(answer);
                        //ShowStack(answer);
                    }
                    st.Push(sybls[index][0]);
                }
                else if (sybls[index] == "+" || sybls[index] == "=" || sybls[index] == ">")//preoryty = 3
                {//XOR, equivalence, implication
                    while (st.Count > 0 && (st.Peek() == '+' || st.Peek() == '=' || st.Peek() == '>' || st.Peek() == '&' || st.Peek() == '!'))
                    {
                        answer.Push(st.Pop().ToString());
                        //ShowStack(answer);
                        //calcBoolean(answer);
                        //ShowStack(answer);
                    }
                    st.Push(sybls[index][0]);
                }
                else if (sybls[index] == "&")//preoryty = 2
                {//and
                    while (st.Count > 0 && (st.Peek() == '&' || st.Peek() == '!'))
                    {
                        answer.Push(st.Pop().ToString());
                        //ShowStack(answer);
                        //calcBoolean(answer);
                        //ShowStack(answer);
                    }
                    st.Push(sybls[index][0]);
                }
                else if (sybls[index] == "!")//preoryty = 1
                {//not
                    while (st.Count > 0 && st.Peek() == '!')
                    {
                        answer.Push(st.Pop().ToString());
                        //ShowStack(answer);
                        //calcBoolean(answer);
                        //ShowStack(answer);
                    }
                    st.Push(sybls[index][0]);
                }
                else if (sybls[index] == "(" || sybls[index] == ")")//preoryty = 0
                {
                    if (sybls[index] == "(")
                        st.Push('(');
                    else if (sybls[index] == ")")
                    {
                        while (st.Peek() != '(')
                        {
                            answer.Push(st.Pop().ToString());
                            //ShowStack(answer);
                            //calcBoolean(answer);
                            //ShowStack(answer);
                        }
                        st.Pop();
                    }
                }
                index++;
            }
            while (st.Count > 0)
            {
                answer.Push(st.Pop().ToString());
                //ShowStack(answer);
                //calcBoolean(answer);
                //ShowStack(answer);
            }
            return answer;
        }
        //bool to string
        private string BTS(bool b)
        {
            return b ? "1" : "0";
        }
        //string to bool
        private bool STB(string s)
        {
            return s == "1" ? true : false;
        }

        /// <summary>
        /// calculates value of the function 
        /// </summary>
        /// <param name="arr"> array of concrete values of variables </param>
        /// <param name="var_names"> list of names of variables respectivly </param>
        /// <returns></returns>
        protected string Calc(bool[] arr)
        {
            string[] temp = func.ToArray();
            Array.Reverse(temp, 0, temp.Length);
            List<string> a = temp.ToList();

            //push values instead of each variable:
            int i = 0;
            for (i = 0; i < a.Count; i++)
                for (int j = 0; j < number_of_variables; j++)
                {
                    if (a[i] == variables[j])
                    {
                        a[i] = BTS(arr[j]);
                    }
                }
            i = 0;
            while (a.Count > 1)
            {
                i++;
                if (a[i] == "&")
                {
                    bool second = STB(a[i - 1]);//second
                    bool first = STB(a[i - 2]);//first
                    a.RemoveRange(i - 2, 3);
                    a.Insert(i - 2, BTS(first && second));
                    i = 0;
                }
                else if (a[i] == "V")
                {
                    bool second = STB(a[i - 1]);//second
                    bool first = STB(a[i - 2]);//first
                    a.RemoveRange(i - 2, 3);
                    a.Insert(i - 2, BTS(first || second));
                    i = 0;
                }
                else if (a[i] == "+")
                {
                    bool second = STB(a[i - 1]);//second
                    bool first = STB(a[i - 2]);//first
                    a.RemoveRange(i - 2, 3);
                    a.Insert(i - 2, BTS((first && !second) || (!first && second)));
                    i = 0;
                }
                else if (a[i] == "=")
                {
                    bool second = STB(a[i - 1]);//second
                    bool first = STB(a[i - 2]);//first
                    a.RemoveRange(i - 2, 3);
                    a.Insert(i - 2, BTS((first && second) || (!first && !second)));
                    i = 0;
                }
                else if (a[i] == ">")
                {
                    bool second = STB(a[i - 1]);//second
                    bool first = STB(a[i - 2]);//first
                    a.RemoveRange(i - 2, 3);
                    a.Insert(i - 2, BTS((!first) || second));
                    i = 0;
                }
                else if (a[i] == "!")
                {
                    bool first = STB(a[i - 1]);//first
                    a.RemoveRange(i - 1, 2);
                    a.Insert(i - 1, BTS(!first));
                    i = 0;
                }
            }
            return a[0];
        }
        /// <summary>
        /// outputs to console table all possible variants of variables
        /// </summary>
        public void CalcForAllValues()
        {
            int k = (int)Math.Pow(2, number_of_variables);
            string aRgUmEnTs = "";
            for (int i = 0; i < number_of_variables; i++)
            {
                Console.Write(variables[i].ToString() + "|");
                aRgUmEnTs += variables[i].ToString() + ", ";
            }
            if (aRgUmEnTs.Length > 1)
                aRgUmEnTs = aRgUmEnTs.Remove(aRgUmEnTs.Length - 2, 2);
            Console.Write("   f(" + aRgUmEnTs + ")\n");
            for (int i = 0; i < 4 + number_of_variables * 6; i++) Console.Write("=");
            Console.WriteLine();
            for (int i = 0; i < k; i++)
            {
                //make array
                bool[] arr = new bool[number_of_variables];
                //for (int j = 0;j<NumOfVariablesInFunction;j++)
                for (int j = number_of_variables - 1; j >= 0; j--)
                {
                    arr[number_of_variables - 1 - j] = Convert.ToBoolean((i >> j) & (0b1));

                    if (variables[number_of_variables - 1 - j].Length % 2 == 0)
                    {
                        for (int ttt = 0; ttt < variables[number_of_variables - 1 - j].Length / 2 - 1; ttt++)
                            Console.Write(" ");
                    }
                    else
                    {
                        for (int ttt = 0; ttt < variables[number_of_variables - 1 - j].Length / 2; ttt++)
                            Console.Write(" ");
                    }
                    Console.Write(BTS(Convert.ToBoolean((i >> j) & (0b1))));
                    for (int ttt = 0; ttt < variables[number_of_variables - 1 - j].Length / 2; ttt++)
                        Console.Write(" ");
                    Console.Write("|");
                }
                //calculate function for this set of arguments
                for (int ttt = 0; ttt < (aRgUmEnTs.Length + 6) / 2; ttt++)
                    Console.Write(" ");
                Console.Write(Calc(arr) + "\n");
            }
        }
    }
}
