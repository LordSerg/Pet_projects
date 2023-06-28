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
            //Console.WriteLine("Input a math equasion to calculate, for example: '5 * 7 + ( 4 - 1 ) * 9'");
            //Console.Write("input:\n      ");
            //string input = Console.ReadLine(); //"3 + 4 * 2 / ( 1 - 5 ) ^ 2"; //
            ////Console.WriteLine(input);
            //Console.Write("output:\n       ");
            //Console.WriteLine(ToPostfixNotation(input));

            /*
            int nnnn = 6;
            for (int i = 0; i < (int)Math.Pow(2,nnnn); i++)
            {
                for (int j = nnnn-1; j >= 0; j--)
                {
                    Console.Write( BTS(Convert.ToBoolean((i >> j) & (0b1)))+ " ");
                }
                Console.WriteLine();
            }
            */
            /*
            Stack<string> a = new Stack<String>();
            a.Push("1");
            a.Push("2");
            a.Push("3");
            a.Push("4");
            String[] b = a.ToArray();
            */


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
            Console.Write("output:\n");
            int NumOfVariablesInFunction = 0;
            List<char> Variables = new List<char>();
            Stack<string> func = ToPostfixNotationBoolean(input, ref NumOfVariablesInFunction, ref Variables);
            //ShowStack(func);
            int k = (int)Math.Pow(2, NumOfVariablesInFunction);
            string aRgUmEnTs = "";
            for (int i = 0; i < NumOfVariablesInFunction; i++)
            {
                Console.Write(Variables[i].ToString() + "|");
                aRgUmEnTs += Variables[i].ToString() + ", ";
            }
            if(aRgUmEnTs.Length>1)
                aRgUmEnTs = aRgUmEnTs.Remove(aRgUmEnTs.Length-2,2);
            Console.Write("   f(" + aRgUmEnTs + ")\n");
            for (int i = 0; i < 4 + NumOfVariablesInFunction * 6; i++) Console.Write("=");
            Console.WriteLine();
            for (int i = 0; i < k; i++)
            {
                //make array
                bool[] arr = new bool[NumOfVariablesInFunction];
                //for (int j = 0;j<NumOfVariablesInFunction;j++)
                for (int j = NumOfVariablesInFunction-1; j >=0 ; j--)
                {
                    arr[NumOfVariablesInFunction - 1 - j] = Convert.ToBoolean((i >> j) & (0b1));
                    Console.Write(BTS(Convert.ToBoolean((i >> j) & (0b1))) + "|");
                }
                //calculate function for this set of arguments
                Console.Write("     " + calcBoolean(func, arr, Variables) + "\n");
            }

            Console.ReadKey();
        }
        
        //divide string into elements for calculations in boolean algebra
        static string[] SplitStringBoolean(string input, ref int num_of_vars, ref List<char> variables)
        {
            List<string> answer = new List<string>();
            foreach (char ch in input)
            {
                if (ch == '0') answer.Add(ch.ToString());//logical zero
                else if (ch == '1') answer.Add(ch.ToString());//logical one
                else if (ch == '(') answer.Add(ch.ToString());//brackets
                else if (ch == ')') answer.Add(ch.ToString());//brackets
                else if (ch == 'V') answer.Add(ch.ToString());//logical or
                else if (ch == '&') answer.Add(ch.ToString());//logical and
                else if (ch == '!') answer.Add(ch.ToString());//logical not
                else if (ch == '+') answer.Add(ch.ToString());//logical XOR
                else if (ch == '=') answer.Add(ch.ToString());//logical equivalence
                else if (ch == '>') answer.Add(ch.ToString());//logical implication
                //logical variable
                else if (ch >= 'a' && ch <= 'z')
                {
                    answer.Add(ch.ToString());
                    if (!variables.Contains(ch))
                    {
                        num_of_vars++;
                        variables.Add(ch);
                    }
                }
            }
            variables.Sort();
            return answer.ToArray();
        }
        static Stack<string> ToPostfixNotationBoolean(string input, ref int num_of_vars, ref List<char> variables)
        {
            //not <- and <- or
            int index = 0;
            string[] sybls = SplitStringBoolean(input, ref num_of_vars, ref variables);
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
                else if (sybls[index] == "+"|| sybls[index] == "="|| sybls[index] == ">")//preoryty = 3
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
        
        static void calcBoolean(Stack<string> stack)
        {
            if (stack.Peek() == "&")
            {
                stack.Pop();//operation
                bool second = STB(stack.Pop());//second
                bool first =  STB(stack.Pop());//first
                stack.Push(BTS(first&&second));
            }
            if (stack.Peek() == "V")
            {
                stack.Pop();//operation
                bool second = STB(stack.Pop());//second
                bool first = STB(stack.Pop());//first
                stack.Push(BTS(first || second));
            }
            if (stack.Peek() == "!")
            {
                stack.Pop();//operation
                bool first = STB(stack.Pop());//first
                stack.Push(BTS(!first));
            }
        }
        //array is 'arguments' of function "stack"
        static string calcBoolean(Stack<string> stack, bool[] arr, List<char> var_names)
        {
            string[] temp = stack.ToArray();
            Array.Reverse (temp,0,temp.Length);
            List<string> a = temp.ToList();

            //push values instead of each variable:
            int i = 0;
            for (i = 0; i < a.Count; i++)
                if (a[i][0] >= 'a' && a[i][0] <= 'z')//
                {
                    a[i] = BTS(arr[var_names.IndexOf(a[i][0])]);
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
                    a.Insert(i - 2, BTS((!first)|| second));
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
        //bool to string
        static string BTS(bool b)
        {
            return b ? "1" : "0";
        }
        //string to bool
        static bool STB(string s)
        {
            return s == "1" ? true : false;
        }
        static string ToPostfixNotationMinimal(string input)
        {
            int index = 0;
            string[] sybls = input.Split(' ');
            int max_length = sybls.Length;
            Stack<string> answer = new Stack<string>();
            Stack<char> st = new Stack<char>();
            while (index < max_length)
            {
                //read an symbol

                //if number - add to the answer
                if (double.TryParse(sybls[index], out _)) 
                {
                    answer.Push(sybls[index]);
                    //ShowStack(answer);
                }
                if (sybls[index] == "+"|| sybls[index] == "-")//preoryty = 3
                {
                    while (st.Count > 0 && (st.Peek() == '+' || st.Peek() == '-' || st.Peek() == '/' || st.Peek() == '*' || st.Peek() == '^'))
                    {
                        answer.Push(st.Pop().ToString());
                        //ShowStack(answer);
                        answer.Push(calc(answer.Pop(), answer.Pop(), answer.Pop()));
                        //ShowStack(answer);
                    }
                    st.Push(sybls[index][0]);
                }
                else if (sybls[index] == "*" || sybls[index] == "/")//preoryty = 2
                {
                    while (st.Count>0&&(st.Peek() == '/' || st.Peek() == '*' || st.Peek() == '^'))
                    {
                        answer.Push(st.Pop().ToString());
                        //ShowStack(answer);
                        answer.Push(calc(answer.Pop(), answer.Pop(), answer.Pop()));
                        //ShowStack(answer);
                    }
                    st.Push(sybls[index][0]);
                }
                else if (sybls[index] == "^")//preoryty = 1
                {
                    while (st.Count > 0 && st.Peek() == '^')
                    {
                        answer.Push(st.Pop().ToString());
                        //ShowStack(answer);
                        answer.Push(calc(answer.Pop(), answer.Pop(), answer.Pop()));
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
                            answer.Push(calc(answer.Pop(), answer.Pop(), answer.Pop()));
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
                answer.Push(calc(answer.Pop(), answer.Pop(), answer.Pop()));
                //ShowStack(answer);
            }
            return answer.Peek().ToString();
        }
        static string calc(string operation, string second, string first)
        {
            if (operation == "+") return (double.Parse(first) + double.Parse(second)).ToString();
            if (operation == "-") return (double.Parse(first) - double.Parse(second)).ToString();
            if (operation == "*") return (double.Parse(first) * double.Parse(second)).ToString();
            if (operation == "/") return (double.Parse(first) / double.Parse(second)).ToString();
            if (operation == "^") return (Math.Pow(double.Parse(first), double.Parse(second))).ToString();
            return "";
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
