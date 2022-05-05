using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator
{
    public class Program
    {
        static double result = new double();
        public static void Main(string[] args)
        {
            ConsoleKeyInfo key;

            Console.WriteLine("Welcome to Calculator.");

            do
            {
                string sum = UserInput();

                Regex regex = new Regex(@"^[0-9.*-/+() ]+$");
                Match match = regex.Match(sum);
                if (!match.Success)
                {
                    Console.WriteLine("\nInvalid formula.");                    
                }
                else
                {
                    result = Calculate(sum);
                    Console.WriteLine("\nResult: {0}", result);
                }
                Console.WriteLine("\nPress Esc to exit, any key to continue.");
                key = Console.ReadKey();
                
            } while (key.Key != ConsoleKey.Escape);
        }

        /// <summary>
        /// Determine if formula contains brackets
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        public static double Calculate(string sum)
        {
            List<string> formulaList = new List<string>();
            string[] sumArray = sum.Split(' ');

            foreach (var s in sumArray)
            {
                formulaList.Add(s);
            }

            if (formulaList.Contains("("))
            {
                result = BracketCalculation(formulaList);
            }
            else
            {
                result = NormalCalculation(formulaList);
            }

            return result;
        }

        /// <summary>
        /// Do calculation without brackets, if there is  * /, do * / first.
        /// </summary>
        /// <param name="formulaList"></param>
        /// <returns></returns>
        private static double NormalCalculation(List<string> formulaList)
        {          
            for (int i = 0; i < formulaList.Count; i++)
            {
                if (formulaList[i] == "*" || formulaList[i] == "/")
                {
                    result = DoOperatorsCalc(formulaList[i - 1], formulaList[i], formulaList[i + 1]);
                    formulaList.RemoveRange(i - 1, 3);
                    formulaList.Insert(i - 1, result.ToString());
                    i = 0;
                }
            }

            for (int i = 0; i < formulaList.Count; i++)
            {
                if (formulaList[i] == "+")
                {
                    result = DoOperatorsCalc(formulaList[i - 1], formulaList[i], formulaList[i + 1]);
                    formulaList.RemoveRange(i - 1, 3);
                    formulaList.Insert(i - 1, result.ToString());
                    i = 0;
                }
                if (formulaList[i] == "-")
                {
                    result = DoOperatorsCalc(formulaList[i - 1], formulaList[i], formulaList[i + 1]);
                    formulaList.RemoveRange(i - 1, 3);
                    formulaList.Insert(i - 1, result.ToString());
                    i = 0;
                }
            }

            return result;
        }

        /// <summary>
        /// Do specific operators calculation
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="fOperator"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        private static double DoOperatorsCalc(string s1, string fOperator, string s2)
        {
            try
            {
                double d1 = Convert.ToDouble(s1);
                double d2 = Convert.ToDouble(s2);

                switch (fOperator)
                {
                    case "*":
                        return d1 * d2;
                    case "/":
                        return d1 / d2;
                    case "+":
                        return d1 + d2;
                    case "-":
                        return d1 - d2;
                    default:
                        throw new Exception("Operators not found.");
                }
            }
            catch
            {
                Console.WriteLine("\nPlease key in valid formula.");
                throw new Exception("\nPlease key in valid formula.");
            }            
        }

        /// <summary>
        /// Formulate formula with brakets
        /// </summary>
        /// <param name="formulaList"></param>
        /// <returns></returns>
        private static double BracketCalculation(List<string> formulaList)
        {
            do
            {
                int? closeBracket = null;
                int? openBracket = null;

                for (int i = 0; i < formulaList.Count; i++)
                {
                    if (formulaList[i].ToString() == "(")
                    {
                        openBracket = i;
                    }

                    if (formulaList[i].ToString() == ")")
                    {
                        if (!closeBracket.HasValue)
                        {
                            closeBracket = i;
                        }
                    }
                }

                List<string> subFormula = formulaList.GetRange(openBracket.Value + 1, closeBracket.Value - openBracket.Value - 1);
                if (subFormula.Count < 3)
                {
                    throw new Exception("Please key in valid formula.");
                }
                result = NormalCalculation(subFormula);
                formulaList.RemoveRange(openBracket.Value, closeBracket.Value - openBracket.Value + 1);
                formulaList.Insert(openBracket.Value, result.ToString());
            } while (formulaList.Contains("("));

            result = NormalCalculation(formulaList);

            return result;
        }

        /// <summary>
        /// Get user input
        /// </summary>
        /// <returns></returns>
        private static string UserInput()
        {
           
            Console.WriteLine("\nPlease key in your formula: ");
            string sum = Console.ReadLine();
            //Regex regex = new Regex(@"1234567890\+-\*/\.\(\)");
            //Match match = regex.Match(sum);
            //if (match.Success)
            //{
            //    Console.WriteLine("nmatch");
            //}
            //else
            //{
            //    Console.WriteLine("Please key in valid formula.");
                

            //}

            return sum;

        }

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}