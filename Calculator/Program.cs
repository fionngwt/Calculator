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
            Console.WriteLine("\nYour formula should consist only numbers, operators and/or brackets, and all shall be separated by spaces.");

            do
            {
                result = new double();
                Console.WriteLine("\nPlease key in your formula: ");
                string sum = Console.ReadLine();

                Regex regex = new Regex(@"^[0-9.*-/+() ]+$");
                Match match = regex.Match(sum);
                if (!match.Success)
                {
                    Console.WriteLine("\nInvalid formula.");                    
                }
                else
                {
                    result = Calculate(sum);
                    if (double.IsInfinity(result) || double.IsNaN(result) || double.IsNegativeInfinity(result) ||
                    double.IsPositiveInfinity(result))
                    {
                        Console.WriteLine("\nResult is not valid.");
                    }
                    else
                    {
                        Console.WriteLine("\nResult: {0}", Math.Round(result,2));
                    }                    
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
            List<string> formulaList = new List<string>(); // List<string> formulaList = sum.Trim().Split(' ').ToList();
            string[] sumArray = sum.Trim().Split(' ');

            foreach (var s in sumArray) 
            {
                formulaList.Add(s);
            }

            if (formulaList.Contains("(") || formulaList.Contains(")"))
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
        /// Do calculation without brackets, do * / first, and follow by + -
        /// </summary>
        /// <param name="formulaList"></param>
        /// <returns></returns>
        private static double NormalCalculation(List<string> formulaList)
        {
            try
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

                if (formulaList.Count == 2)
                //if (formulaList.Count <= 2)
                {
                    return double.NaN;
                }
                else
                {
                    return result;
                }
            }
            catch
            {
                return double.NaN;
            }
            
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
                        return double.NaN;
                }
            }
            catch
            {
                return double.NaN;
            }            
        }

        /// <summary>
        /// Formulate formula with brackets
        /// </summary>
        /// <param name="formulaList"></param>
        /// <returns></returns>
        private static double BracketCalculation(List<string> formulaList)
        {
            try
            {
                do
                {
                    int? closeBracket = null;
                    int? openBracket = null;
                        
                    for (int i = 0; i < formulaList.Count; i++)
                    {
                        if (openBracket == null && closeBracket == null && formulaList[i] == ")") //invalid formula checking eg. 1 ) 2
                        {
                            return double.NaN;
                        }

                        if (formulaList[i] == "(")
                        {
                            openBracket = i;
                        }

                        if (formulaList[i] == ")")
                        {
                            if (!closeBracket.HasValue)
                            {
                                closeBracket = i;
                                break;
                            }
                        }
                    }                   

                    List<string> subFormula = formulaList.GetRange(openBracket.Value + 1, closeBracket.Value - openBracket.Value - 1);
                   
                    if (subFormula.Count == 1 && Double.TryParse(subFormula[0], out result))
                    {
                        result = Convert.ToDouble(subFormula[0]);
                    }
                    else if (subFormula.Count < 3)
                    {
                        return result = double.NaN;
                    }
                    else
                    {
                        result = NormalCalculation(subFormula);
                    }
                    formulaList.RemoveRange(openBracket.Value, closeBracket.Value - openBracket.Value + 1);
                    formulaList.Insert(openBracket.Value, result.ToString());
                } while (formulaList.Contains("("));

                
                result = NormalCalculation(formulaList);

                return result;
            }
            catch
            {
                return result = double.NaN;
            }
        }       
    }
}