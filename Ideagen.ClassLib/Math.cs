using System;

namespace Ideagen.ClassLib
{
    public static class Math
    {
        /// <summary>
        /// Computes a math expression
        /// </summary>
        /// <param name="sum">Math expression (must be seperated by spaces)</param>
        /// <returns>Computed result. Null if error occured</returns>
        public static double? Calculate(string sum)
        {
            char separator = ' ';
            string[] exp = sum.Trim().Split(separator);
            double operand = 0;
            char? op = null;
            double? accumulator = 0;
            string nested = string.Empty;
            int precedenceOperatorIdx = -1;
            int multiplyOpIdx = -1;
            int divOpIdx = -1;

            try
            {
                //Handle nested expressions first
                while (sum.IndexOf('(') > -1 || sum.IndexOf(')') > -1)
                {
                    nested = sum.Substring(sum.LastIndexOf('('), sum.IndexOf(')') - sum.LastIndexOf('(') + 1);
                    accumulator = Calculate(nested.Replace("( ", "").Replace(" )", ""));
                    if (accumulator == null)
                        break;

                    sum = sum.Replace(nested, accumulator.ToString());
                    exp = sum.Trim().Split(' ');
                }

                //Handle operator precedence for multiply (*) and division (/)
                while (exp.Length > 3 && (Array.Exists(exp, e => e == "*") || Array.Exists(exp, e => e == "/")))
                {
                    multiplyOpIdx = Array.IndexOf(exp, "*");
                    divOpIdx = Array.IndexOf(exp, "/");

                    precedenceOperatorIdx = (multiplyOpIdx < divOpIdx && multiplyOpIdx > -1 || divOpIdx == -1) ?
                                            multiplyOpIdx :
                                            divOpIdx;

                    nested = exp[precedenceOperatorIdx - 1] + " " + exp[precedenceOperatorIdx] + " " + exp[precedenceOperatorIdx + 1];
                    accumulator = Calculate(nested);
                    if (accumulator == null)
                        break;

                    sum = sum.Replace(nested, accumulator.ToString());
                    exp = sum.Trim().Split(' ');
                }

                //Eexpression main handler
                foreach (string element in exp)
                {
                    //Extract operands and operators
                    if (Double.TryParse(element, out operand))
                    {
                        //Extract Operand
                        //If there's an operator, compute operand with accumulator
                        if (op.HasValue)
                        {
                            switch (op.Value)
                            {
                                case '+': accumulator += operand; break;
                                case '-': accumulator -= operand; break;
                                case '*': accumulator *= operand; break;
                                case '/': accumulator /= operand; break;
                            }
                        }
                        else
                        {
                            //First run, set accumulator as operand
                            accumulator = operand;
                        }
                    }
                    else
                    {
                        //Extract operator
                        op = Convert.ToChar(element);
                    }
                }
            }
            catch (Exception ex)
            {
                //Error occured, return null
                Console.WriteLine($"Exception occured: {ex.Message}\n{ex.StackTrace}");
                accumulator = null;
            }

            return accumulator;
        }
    }
}
