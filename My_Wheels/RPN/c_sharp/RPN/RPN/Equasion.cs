using System;
using System.Collections.Generic;
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
            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];

                //variables
                if (is_var_now)
                {
                    if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || ch == '_' || (ch >= '0' && ch <= '9'))
                        variable_name += ch;
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
    }
}
