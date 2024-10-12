using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPN
{
    
    public class RealEquasion : Equasion
    {
        public RealEquasion(string input)
        {
            input_string = input;
            variables = new List<string>();
            number_of_variables = 0;
            func = ToPostfixNotation(input, ref number_of_variables, ref variables);
        }
        protected Stack<string> ToPostfixNotation(string input, ref int number_of_variables, ref List<string> variables)
        {
            int index = 0;
            string[] sybls = SplitString(input, ref number_of_variables, ref variables);//input.Split(' ');
            int max_length = sybls.Length;
            Stack<string> answer = new Stack<string>();
            Stack<char> st = new Stack<char>();
            while (index < max_length)
            {
                //if number - add to the answer
                if (double.TryParse(sybls[index], out _) || (sybls[index][0] >= 'a' && sybls[index][0] <= 'z') || (sybls[index][0] >= 'A' && sybls[index][0] <= 'Z') || sybls[index][0] == '_')
                {
                    answer.Push(sybls[index]);
                }
                if (sybls[index] == "+" || sybls[index] == "-")//preoryty = 3
                {
                    while (st.Count > 0 && (st.Peek() == '+' || st.Peek() == '-' || st.Peek() == '/' || st.Peek() == '*' || st.Peek() == '^'))
                    {
                        answer.Push(st.Pop().ToString());
                    }
                    st.Push(sybls[index][0]);
                }
                else if (sybls[index] == "*" || sybls[index] == "/")//preoryty = 2
                {
                    while (st.Count > 0 && (st.Peek() == '/' || st.Peek() == '*' || st.Peek() == '^'))
                    {
                        answer.Push(st.Pop().ToString());
                    }
                    st.Push(sybls[index][0]);
                }
                else if (sybls[index] == "^")//preoryty = 1
                {
                    while (st.Count > 0 && st.Peek() == '^')
                    {
                        answer.Push(st.Pop().ToString());
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
                        }
                        st.Pop();
                    }
                }
                index++;
            }
            while (st.Count > 0)
            {
                answer.Push(st.Pop().ToString());
            }
            return answer;
        }

        public string Calc(double[] arr)
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
                        a[i] = arr[j].ToString();
                    }
                }
            i = 0;
            while (a.Count > 1 && i < a.Count)
            {
                i++;
                if (a[i] == "+")
                {
                    double second = Convert.ToDouble(a[i - 1]);//second
                    double first =  Convert.ToDouble(a[i - 2]);//first
                    a.RemoveRange(i - 2, 3);
                    a.Insert(i - 2, (first + second).ToString());
                    i = 0;
                }
                else if (a[i] == "-")
                {
                    double second = Convert.ToDouble(a[i - 1]);//second
                    double first = Convert.ToDouble(a[i - 2]);//first
                    a.RemoveRange(i - 2, 3);
                    a.Insert(i - 2, (first - second).ToString());
                    i = 0;
                }
                else if (a[i] == "*")
                {
                    double second = Convert.ToDouble(a[i - 1]);//second
                    double first = Convert.ToDouble(a[i - 2]);//first
                    a.RemoveRange(i - 2, 3);
                    a.Insert(i - 2, (first * second).ToString());
                    i = 0;
                }
                else if (a[i] == "/")
                {
                    double second = Convert.ToDouble(a[i - 1]);//second
                    double first = Convert.ToDouble(a[i - 2]);//first
                    a.RemoveRange(i - 2, 3);
                    a.Insert(i - 2, (first / second).ToString());
                    i = 0;
                }
                else if (a[i] == "^")
                {
                    double second = Convert.ToDouble(a[i - 1]);//second
                    double first = Convert.ToDouble(a[i - 2]);//first
                    a.RemoveRange(i - 2, 3);
                    a.Insert(i - 2, Math.Pow(first, second).ToString());
                    i = 0;
                }
                //else if (a[i] == "!")
                //{
                //    bool first = STB(a[i - 1]);//first
                //    a.RemoveRange(i - 1, 2);
                //    a.Insert(i - 1, BTS(!first));
                //    i = 0;
                //}
            }
            return a[0];
        }
        private string ShowStack(Stack<string> stack)
        {
            string answer = "";
            string[] str = stack.ToArray();
            for (int i = str.Length - 1; i >= 0; i--)
                answer+=(str[i] + " ");
            //Console.Write(str[i] + " ");
            answer += "\n";
            return answer;
        }
        public override string ToString()
        {
            return ShowStack(func);
        }
    }
}
