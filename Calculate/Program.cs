using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Calculate
{
    class Program
    {
        static void Main(string[] args)
        {
            string sum = "";
            while(true)
            {
                Console.WriteLine("Key in the sum, else key in 'exit' to exit");
                sum = Console.ReadLine();

                if(sum == "exit")
                {
                    break;
                }
                //Checking for unmatch amount of parentheses
                else if (sum.Count(x => x == '(') != sum.Count(x => x == ')'))
                {
                    Console.WriteLine("Number of '(' & ')' does not match");
                }
                else
                {
                    Console.WriteLine("Result - " + Calculate(sum));
                }
            }
        }

        //Assuming the input string is always seperated by whitespace
        public static double Calculate(string sum)
        {
            List<string> words = sum.Split(' ').ToList();

            //Always operate on operations with parentheses first
            while(words.Contains("("))
            {
                List<string> chars = new List<string>{ "*","/"};
                int startIndex = words.LastIndexOf("(");
                int endIndex = words.IndexOf(")",startIndex);
                List<string> tempVal = words.GetRange(startIndex, endIndex - startIndex + 1);
                var evaluatedVal = Evaluator(tempVal);
                evaluatedVal.Remove("(");
                evaluatedVal.Remove(")");
                words.RemoveRange(startIndex, endIndex - startIndex + 1);
                words.Insert(startIndex, evaluatedVal.First());
            }

            //Perform remaining operations without parenthses
            Evaluator(words);

            return Math.Round(Convert.ToDouble(words.First()),2);
        }

        //Performs expression evaluation, loops until it gets a single numeric value
        public static List<string> Evaluator(List<string> values)
        {
            //Multiplication and division is performed first
            if (values.Contains("*") || values.Contains("/"))
            {
                int firstOpr = 0;

                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i] == "*" || values[i] == "/")
                    {
                        firstOpr = i;
                        break;
                    }
                }

                string oprVal = "";

                if(values[firstOpr] == "*")
                {
                    oprVal = Multiply(values[firstOpr - 1], values[firstOpr + 1]).ToString();
                    values.RemoveRange(firstOpr - 1, 3);
                    values.Insert(firstOpr -1,oprVal);
                }
                else
                {
                    oprVal = Divide(values[firstOpr - 1], values[firstOpr + 1]).ToString();
                    values.RemoveRange(firstOpr - 1, 3);
                    values.Insert(firstOpr - 1, oprVal);
                }
            }
            else
            //Summation and subtraction performed later
            {
                int firstOpr = 0;

                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i] == "+" || values[i] == "-")
                    {
                        firstOpr = i;
                        break;
                    }
                }

                string oprVal = "";

                if (values[firstOpr] == "+")
                {
                    oprVal = Sum(values[firstOpr - 1], values[firstOpr + 1]).ToString();
                    values.RemoveRange(firstOpr - 1, 3);
                    values.Insert(firstOpr - 1, oprVal);
                }
                else
                {
                    oprVal = Minus(values[firstOpr - 1], values[firstOpr + 1]).ToString();
                    values.RemoveRange(firstOpr - 1, 3);
                    values.Insert(firstOpr - 1, oprVal);
                }
            }

            if(values.Contains("+") || values.Contains("-") || values.Contains("*") || values.Contains("/"))
            {
                Evaluator(values);
            }
            return values;
        }

        public static double Sum(string a, string b)
        {
            double result = Convert.ToDouble(a) + Convert.ToDouble(b);
            return result;
        }

        public static double Minus(string a, string b)
        {
            double result = Convert.ToDouble(a) - Convert.ToDouble(b);
            return result;
        }

        public static double Multiply(string a, string b)
        {
            double result = Convert.ToDouble(a) * Convert.ToDouble(b);
            return result;
        }

        public static double Divide(string a, string b)
        {
            double result = Convert.ToDouble(a) / Convert.ToDouble(b);
            return result;
        }
    }
}
