using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPN
{
    public class Equasion
    {
        protected string input_string;//the input string
        protected Stack<string> func;//reverse polish notation of input string
        protected List<string> variables;//names of variables
        protected int number_of_variables;//number of variables in function
        public int NumOfVariables { get { return number_of_variables; } }
        /// <summary>
        /// devides all variables, constants and operations between each other and returns them in form of array
        /// </summary>
        /// <param name="input"> pure input data </param>
        /// <param name="num_of_vars"> return the number of variables in function </param>
        /// <param name="variables"> return names of variables, sorted from 'a' to 'z' </param>
        /// <returns></returns>
        protected string[] SplitString(string input, ref int num_of_vars, ref List<string> variables)
        {
            List<string> answer = new List<string>();
            string variable_name = "";
            string number = "";
            bool is_var_now = false;
            int br_count=0;//brackets in variables name, for example A_((c+s)*x)
            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];

                //variables
                if (br_count > 0)
                {
                    variable_name += ch;
                    if (ch == ')')
                    {
                        br_count--;
                    }
                    else if (ch == '(')
                        br_count++;

                }
                else if (is_var_now)
                {
                    if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || ch == '_' || (ch >= '0' && ch <= '9'))
                        variable_name += ch;
                    else if (ch == '(')
                    {
                        variable_name += ch;
                        br_count++;
                    }
                    else
                    {
                        is_var_now = false;
                        answer.Add(variable_name);
                        if (!variables.Contains(variable_name))
                        {
                            num_of_vars++;
                            variables.Add(variable_name);
                        }
                        variable_name = "";
                        i--;
                    }
                }
                else if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || ch == '_')
                {
                    is_var_now = true;
                    variable_name = ch.ToString();
                }
                //constants
                else if ((ch >= '0' && ch <= '9') || (ch == ',' || ch == '.'))
                {
                    number += ch;
                    //if next char is not a number
                    if (!((i + 1 < input.Length) &&
                         ((input[i + 1] >= '0' && input[i + 1] <= '9') ||
                         (input[i + 1] == ',' || input[i + 1] == '.'))))
                    {
                        answer.Add(number);
                        number = "";
                    }
                }
                // operations
                else if (ch == '(') answer.Add(ch.ToString());
                else if (ch == ')') answer.Add(ch.ToString());
                //binary functions
                else if (ch == '+') answer.Add(ch.ToString());
                else if (ch == '-') answer.Add(ch.ToString());
                else if (ch == '*') answer.Add(ch.ToString());
                else if (ch == '/') answer.Add(ch.ToString());
                else if (ch == '^') answer.Add(ch.ToString());
                //unary functions
                else if ((ch == 'S' || ch == 's') &&
                    (i + 3 < input.Length) &&
                    (input[i + 1] == 'i' && input[i + 2] == 'n'))
                {//sin
                    answer.Add("sin");
                }
                else if ((ch == 'C' || ch == 'c') &&
                    (i + 3 < input.Length) &&
                    (input[i + 1] == 'o' && input[i + 2] == 's'))
                {//cos
                    answer.Add("cos");
                }
                //for logical
                else if (ch == '=') answer.Add(ch.ToString());
                else if (ch == '!') answer.Add(ch.ToString());
                else if (ch == '&') answer.Add(ch.ToString());
                else if (ch == 'V') answer.Add(ch.ToString());
                else if (ch == '>') answer.Add(ch.ToString());
            }
            if (is_var_now)
            {
                is_var_now = false;
                answer.Add(variable_name);
                if (!variables.Contains(variable_name))
                {
                    num_of_vars++;
                    variables.Add(variable_name);
                }
                variable_name = "";
            }
            /*
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
                    if (!variables.Contains(ch.ToString()))
                    {
                        num_of_vars++;
                        variables.Add(ch.ToString());
                    }
                }
            }
            */
            variables.Sort();
            return answer.ToArray();
        }
        /// <summary>
        /// Show function in nice form
        /// </summary>
        /// <param name="size"> maximum size of operand</param>
        /// <returns>returns bitmap with function in nice visual form</returns>
        public Bitmap ShowFunction(int size)
        {
            string[] str = func.ToArray();//reverced

            draw(size, str);
            int w = input_string.Length*size, h = 100;//???
            Bitmap bit = new Bitmap(w,h);
            Graphics g = Graphics.FromImage(bit);
            g.Clear(Color.White);
            //
            g.DrawString(input_string,new Font("Arial",size),new SolidBrush(Color.Black),0,0);
            return bit;
        }
        private void draw(int size, string[] str, int i=0)
        {
            if (str[i] == "+" || str[i] == "-" || str[i] == "*")
            {
                if (i + 1 < str.Length)
                    draw(size, str, i + 1);
            }
            else if (str[i] == "/")
            {

            }
            else if (str[i] == "^")
            {

            }
            else// if ((str[i][0] >= 'a' && str[i][0] <= 'z') || (str[i][0] >= 'A' && str[i][0] <= 'Z') || (str[i][0] >= '0' && str[i][0] <= '9') || str[i][0] == '_')
            {//draw an literal or variable
                
            }
        }
        private void draw_variable(string s)
        {
            string []str = s.Split('_');

        }
    }
}
