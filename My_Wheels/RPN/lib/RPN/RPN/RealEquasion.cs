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
            Stack<string> st = new Stack<string>();
            while (index < max_length)
            {

                bool temporary_for_checking_if_it_is_variable = true;
                for (int i = 0; i < UnaryFunctions.Count; i++)
                {
                    temporary_for_checking_if_it_is_variable = temporary_for_checking_if_it_is_variable && (sybls[index]!=UnaryFunctions[i]);
                }
                //if number - add to the answer
                if (double.TryParse(sybls[index], out _) || ((sybls[index][0] >= 'a' && sybls[index][0] <= 'z') || (sybls[index][0] >= 'A' && sybls[index][0] <= 'Z') || sybls[index][0] == '_')
                                                           && temporary_for_checking_if_it_is_variable)
                {
                    answer.Push(sybls[index]);
                }
                //unary functions, preority = 0
                bool is_unary_func = false;
                for (int i = 0; i < UnaryFunctions.Count; i++)
                {
                    if (sybls[index] == UnaryFunctions[i])
                    {
                        bool flag = false;
                        for (int t = 0; t < UnaryFunctions.Count && st.Count > 0; t++)
                        {
                            if (st.Peek() == UnaryFunctions[t])
                            {
                                flag = true;
                                break;
                            }
                        }
                        while (st.Count > 0 && flag)
                        {
                            answer.Push(st.Pop());
                            //check for flag
                            flag = false;
                            for (int t = 0; t < UnaryFunctions.Count && st.Count > 0; t++)
                            {
                                if (st.Peek() == UnaryFunctions[t])
                                {
                                    flag = true;
                                    break;
                                }
                            }
                        }
                        st.Push(sybls[index]);
                        is_unary_func = true;
                        break;
                    }
                }

                /*
                if ((sybls[index][0] == 'S' || sybls[index][0] == 's') &&
                    (3 == sybls[index].Length) &&
                    (sybls[index][1] == 'i' && input[2] == 'n'))
                {//sin
                    while (st.Count > 0 && (st.Peek() == "sin" || st.Peek() == "cos"))
                    {
                        answer.Push(st.Pop());
                    }
                    st.Push(sybls[index]);
                }
                else if ((sybls[index][0] == 'C' || sybls[index][0] == 'c') &&
                    (3 == sybls[index].Length) &&
                    (sybls[index][1] == 'o' && input[2] == 's'))
                {//cos
                    while (st.Count > 0 && (st.Peek() == 's' || st.Peek() == 'c' ))
                    {
                        if (st.Peek() == 'c')
                        {
                            answer.Push("cos");
                            st.Pop();
                        }
                        else if (st.Peek() == 's')
                        {
                            answer.Push("sin");
                            st.Pop();
                        }
                        else
                        {
                            answer.Push(st.Pop().ToString());
                        }
                    }
                    st.Push(sybls[index][0]);
                }
                */
                if (is_unary_func)
                {
                }
                else if (sybls[index] == "+" || sybls[index] == "-")//preoryty = 3
                {
                    bool flag = false;
                    for (int t = 0; t < UnaryFunctions.Count && st.Count > 0; t++)
                    {
                        if (st.Peek() == UnaryFunctions[t])
                        {
                            flag = true;
                            break;
                        }
                    }
                    while (st.Count > 0 && (flag || st.Peek() == "+" || st.Peek() == "-" || st.Peek() == "/" || st.Peek() == "*" || st.Peek() == "^"))
                    {
                        answer.Push(st.Pop());
                        flag = false;
                        for (int t = 0; t < UnaryFunctions.Count && st.Count > 0; t++)
                        {
                            if (st.Peek() == UnaryFunctions[t])
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    st.Push(sybls[index]);
                }
                else if (sybls[index] == "*" || sybls[index] == "/")//preoryty = 2
                {
                    bool flag = false;
                    for (int t = 0; t < UnaryFunctions.Count && st.Count > 0; t++)
                    {
                        if (st.Peek() == UnaryFunctions[t])
                        {
                            flag = true;
                            break;
                        }
                    }
                    while (st.Count > 0 && (flag || st.Peek() == "/" || st.Peek() == "*" || st.Peek() == "^"))
                    {
                        answer.Push(st.Pop());
                        flag = false;
                        for (int t = 0; t < UnaryFunctions.Count && st.Count > 0; t++)
                        {
                            if (st.Peek() == UnaryFunctions[t])
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    st.Push(sybls[index]);
                }
                else if (sybls[index] == "^")//preoryty = 1
                {
                    bool flag = false;
                    for (int t = 0; t < UnaryFunctions.Count && st.Count > 0; t++)
                    {
                        if (st.Peek() == UnaryFunctions[t])
                        {
                            flag = true;
                            break;
                        }
                    }
                    while (st.Count > 0 && (flag || st.Peek() == "^"))
                    {
                        answer.Push(st.Pop());
                        flag = false;
                        for (int t = 0; t < UnaryFunctions.Count && st.Count > 0; t++)
                        {
                            if (st.Peek() == UnaryFunctions[t])
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    st.Push(sybls[index]);
                }
                else if (sybls[index] == "(" || sybls[index] == ")")//preoryty = -1
                {
                    if (sybls[index] == "(")
                        st.Push("(");
                    else if (sybls[index] == ")")
                    {
                        while (st.Peek() != "(")
                        {
                            answer.Push(st.Pop());
                        }
                        st.Pop();
                    }
                }
                index++;
            }
            while (st.Count > 0)
            {
                answer.Push(st.Pop());
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
                    double first = Convert.ToDouble(a[i - 2]);//first
                    a.RemoveRange(i - 2, 3);
                    a.Insert(i - 2, (first + second).ToString());
                    i = 0;
                }
                else if (a[i] == "-")
                {
                    double second = Convert.ToDouble(a[i - 1]);//second
                    double first;
                    if (i - 2 >= 0)
                    {
                        first = Convert.ToDouble(a[i - 2]);//first
                        a.RemoveRange(i - 2, 3);
                        a.Insert(i - 2, (first - second).ToString());
                    }
                    else
                    {
                        a.RemoveRange(i - 1, 2);
                        a.Insert(i - 1, (0-second).ToString());
                    }
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
                else if (a[i] == "cos")
                {
                    double first = Convert.ToDouble(a[i - 1]);//first
                    a.RemoveRange(i - 1, 2);
                    a.Insert(i - 1, Math.Cos(first).ToString());
                    i = 0;
                }
                else if (a[i] == "sin")
                {
                    double first = Convert.ToDouble(a[i - 1]);//first
                    a.RemoveRange(i - 1, 2);
                    a.Insert(i - 1, Math.Sin(first).ToString());
                    i = 0;
                }
                else if (a[i] == "tg")
                {
                    double first = Convert.ToDouble(a[i - 1]);//first
                    a.RemoveRange(i - 1, 2);
                    a.Insert(i - 1, Math.Tan(first).ToString());
                    i = 0;
                }
                else if (a[i] == "ctg"|| a[i] == "cot")
                {
                    double first = Convert.ToDouble(a[i - 1]);//first
                    a.RemoveRange(i - 1, 2);
                    a.Insert(i - 1, (1.0/Math.Tan(first)).ToString());
                    i = 0;
                }
                else if (a[i] == "ln")
                {
                    double first = Convert.ToDouble(a[i - 1]);//first
                    a.RemoveRange(i - 1, 2);
                    a.Insert(i - 1, Math.Log(first).ToString());
                    i = 0;
                }
                else if (a[i] == "lg")
                {
                    double first = Convert.ToDouble(a[i - 1]);//first
                    a.RemoveRange(i - 1, 2);
                    a.Insert(i - 1, Math.Log10(first).ToString());
                    i = 0;
                }
                else if (a[i] == "sqrt")
                {
                    double first = Convert.ToDouble(a[i - 1]);//first
                    a.RemoveRange(i - 1, 2);
                    a.Insert(i - 1, Math.Sqrt(first).ToString());
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
                answer += (str[i] + " ");
            //Console.Write(str[i] + " ");
            answer += "\n";
            return answer;
        }
        public override string ToString()
        {
            return ShowStack(func);
        }
        public override string ToJS_function()
        {
            string[] temp = func.ToArray();
            Array.Reverse(temp, 0, temp.Length);
            List<string> a = temp.ToList();
            int i = 0;
            while (a.Count > 1 && i < a.Count)
            {
                i++;
                if (a[i] == "+")
                {
                    string second = a[i - 1];//second
                    string first =  a[i - 2];//first
                    a.RemoveRange(i - 2, 3);
                    a.Insert(i - 2, "("+first +"+"+ second+")");
                    i = 0;
                }
                else if (a[i] == "-")
                {
                    string second = a[i - 1];//second
                    string first;
                    if (i - 2 >= 0)
                    {
                        first = a[i - 2];//first
                        a.RemoveRange(i - 2, 3);
                        a.Insert(i - 2, "(" + first + "-" + second + ")");
                    }
                    else
                    {
                        a.RemoveRange(i - 1, 2);
                        a.Insert(i - 1, "(-" + second + ")");
                    }
                    i = 0;
                }
                else if (a[i] == "*")
                {
                    string second = a[i - 1];//second
                    string first = a[i - 2];//first
                    a.RemoveRange(i - 2, 3);
                    a.Insert(i - 2, "("+first +"*"+ second+")");
                    i = 0;
                }
                else if (a[i] == "/")
                {
                    string second = a[i - 1];//second
                    string first = a[i - 2];//first
                    a.RemoveRange(i - 2, 3);
                    a.Insert(i - 2, "("+first +"/"+ second+")");
                    i = 0;
                }
                else if (a[i] == "^")
                {
                    string second = a[i - 1];//second
                    string first = a[i - 2];//first
                    a.RemoveRange(i - 2, 3);
                    a.Insert(i - 2, "Math.pow("+first +","+ second+")");
                    i = 0;
                }
                else if (a[i] == "cos")
                {
                    string first = a[i - 1];//first
                    a.RemoveRange(i - 1, 2);
                    a.Insert(i - 1, "Math.cos(" + first + ")");
                    i = 0;
                }
                else if (a[i] == "sin")
                {
                    string first = a[i - 1];//first
                    a.RemoveRange(i - 1, 2);
                    a.Insert(i - 1, "Math.sin(" + first + ")");
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
    }
}
