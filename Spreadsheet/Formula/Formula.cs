// Skeleton written by Joe Zachary for CS 3500, January 2017

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public struct Formula
    {
        private List<string> tokensList;

        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// Examples of a valid parameter to this constructor are:
        ///     "2.5e9 + x5 / 17"
        ///     "(5 * 2) + 8"
        ///     "x*y-2+35/9"
        ///     
        /// Examples of invalid parameters are:
        ///     "_"
        ///     "-5.3"
        ///     "2 5 + 3"
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        public Formula(String formula)
        {
            if (formula == null)
            {
                throw new ArgumentNullException("formula");
            }
            IEnumerable<string> tokens = GetTokens(formula);
            tokensList = new List<string>();
            Normalizer N = (s => s);
            Validator V = (s => true);
            foreach (string s in tokens)
            {
                tokensList.Add(s);
            }
            if (tokensList.Count == 0)
            {
                throw new FormulaFormatException("The formula is empty.");
            }
            foreach (string s in tokensList)
            {
                if (!Regex.IsMatch(N(s), @"\(") && !Regex.IsMatch(N(s), @"\)") && !Regex.IsMatch(N(s), @"[\+\-*/]") && !Regex.IsMatch(N(s), @"[a-zA-Z][0-9a-zA-Z]*") && !Regex.IsMatch(N(s), @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?", RegexOptions.IgnorePatternWhitespace) && V(N(s)))
                {
                    throw new FormulaFormatException("There is an invalid character in the formula.");
                }
            }
            int left = 0;
            int right = 0;
            foreach (string s in tokensList)
            {
                if (s == "(")
                {
                    left++;
                }
                if (s == ")")
                {
                    right++;
                    if (right > left)
                    {
                        throw new FormulaFormatException("Closing parenthesis with no opening parenthesis.");
                    }
                }
            }
            if (left != right)
            {
                throw new FormulaFormatException("Parentheses imbalance.");
            }
            if (Regex.IsMatch(tokensList[0], @"[\+\-*/]") && !double.TryParse(tokensList[0], out double n))
            {
                throw new FormulaFormatException("Formula cannot start with an operator");
            }
            if (Regex.IsMatch(tokensList[tokensList.Count - 1], @"[\+\-*/]") && !double.TryParse(tokensList[tokensList.Count - 1], out n))
            {
                throw new FormulaFormatException("Formula cannot end with an operator");
            }
            for (int x = 0; x < tokensList.Count - 1; x++)
            {
                if (Regex.IsMatch(tokensList[x], @"[\+\-*/]") | Regex.IsMatch(tokensList[x], @"\(") && !double.TryParse(tokensList[x], out n))
                {
                    if ((Regex.IsMatch(tokensList[x + 1], @"[\+\-*/]") | Regex.IsMatch(tokensList[x + 1], @"\)")) && !double.TryParse(tokensList[x + 1], out n))
                    {
                        throw new FormulaFormatException("There is an operator or closing parenthesis immediately following an operator or opening parenthesis.");
                    }
                }
                if (!Regex.IsMatch(tokensList[x], @"[\+\-*/]") && !Regex.IsMatch(tokensList[x], @"\("))
                {
                    if (!Regex.IsMatch(tokensList[x + 1], @"[\+\-*/]") && !Regex.IsMatch(tokensList[x + 1], @"\)"))
                    {
                        throw new FormulaFormatException("There is a number, variable, or opening parenthesis immediately following a number, variable, or closing parenthesis.");
                    }
                }
            }
        }

        /// <summary>
        /// Formula constructor with extra parameters which are rules for the variables.
        /// </summary>
        public Formula(string formula, Normalizer N, Validator V)
        {
            if (N == null)
            {
                throw new ArgumentNullException("Normalizer");
            }
            if (V == null)
            {
                throw new ArgumentNullException("Validator");
            }
            string temp = "";
            double n = 0;
            IEnumerable<string> tokens = GetTokens(formula);
            foreach (string s in tokens)
            {
                if (Regex.IsMatch(s, @"[a-zA-Z][0-9a-zA-Z]*") && !double.TryParse(s, out n))
                {
                    temp = temp + N(s);
                }
                else
                {
                    temp = temp + s;
                }
            }
            this = new Formula(temp);
            foreach (string s in tokensList)
            {
                if (Regex.IsMatch(s, @"[a-zA-Z][0-9a-zA-Z]*") && !double.TryParse(s, out n))
                {
                    if (!V(N(s)))
                    {
                        throw new FormulaFormatException("The validator is not passed.");
                    }
                }
            }
        }

        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            if (lookup == null)
            {
                throw new ArgumentNullException("lookup");
            }
            ConstructHelper();
            Stack<double> value = new Stack<double>();
            Stack<string> operatorS = new Stack<string>();
            double n = 0;
            foreach (string s in tokensList)
            {
                if (double.TryParse(s, out n))
                {
                    if (operatorS.Count == 0)
                    {
                        value.Push(Convert.ToDouble(s));
                    }
                    else if (operatorS.Peek() == "*")
                    {
                        operatorS.Pop();
                        value.Push(value.Pop() * Convert.ToDouble(s));
                    }
                    else if (operatorS.Peek() == "/")
                    {
                        operatorS.Pop();
                        if (Convert.ToDouble(s) == 0)
                        {
                            throw new FormulaEvaluationException("Cannot divide by 0.");
                        }
                        value.Push(value.Pop() / Convert.ToDouble(s));
                    }
                    else
                    {
                        value.Push(Convert.ToDouble(s));
                    }
                }
                else if (Regex.IsMatch(s, @"[a-zA-Z][0-9a-zA-Z]*"))
                {
                    try
                    {
                        lookup(s);
                    }
                    catch
                    {
                        throw new FormulaEvaluationException("The variable is undefined.");
                    }
                    double variable = lookup(s);
                    if (operatorS.Count == 0)
                    {
                        value.Push(variable);
                    }
                    else if (operatorS.Peek() == "*")
                    {
                        operatorS.Pop();
                        value.Push(value.Pop() * variable);
                    }
                    else if (operatorS.Peek() == "/")
                    {
                        operatorS.Pop();
                        if (variable == 0)
                        {
                            throw new FormulaEvaluationException("Cannot divide by 0.");
                        }
                        value.Push(value.Pop() / variable);
                    }
                    else
                    {
                        value.Push(variable);
                    }
                }
                else if (s == "+" || s == "-")
                {
                    bool empty = false;
                    if (operatorS.Count == 0)
                    {
                        operatorS.Push(s);
                        empty = true;
                    }
                    else if (operatorS.Peek() == "+")
                    {
                        operatorS.Pop();
                        value.Push(value.Pop() + value.Pop());
                    }
                    else if (operatorS.Peek() == "-")
                    {
                        operatorS.Pop();
                        double temp = value.Pop();
                        value.Push(value.Pop() - temp);
                    }
                    if (!empty)
                    {
                        operatorS.Push(s);
                    }
                }
                else if (s == "*" || s == "/" || s == "(")
                {
                    operatorS.Push(s);
                }
                else if (s == ")")
                {
                    if (operatorS.Count != 0)
                    {
                        if (operatorS.Peek() == "+")
                        {
                            operatorS.Pop();
                            value.Push(value.Pop() + value.Pop());
                        }
                        else if (operatorS.Peek() == "-")
                        {
                            operatorS.Pop();
                            double temp = value.Pop();
                            value.Push(value.Pop() - temp);
                        }
                    }
                    operatorS.Pop();
                    if (operatorS.Count != 0)
                    {
                        if (operatorS.Peek() == "*")
                        {
                            operatorS.Pop();
                            value.Push(value.Pop() * value.Pop());
                        }
                        else if (operatorS.Peek() == "/")
                        {
                            operatorS.Pop();
                            double temp = value.Pop();
                            if (temp == 0)
                            {
                                throw new FormulaEvaluationException("Cannot divide by 0.");
                            }
                            value.Push(temp / value.Pop());
                        }
                    }
                }
            }
            if (operatorS.Count == 0)
            {
                return value.Pop();
            }
            else
            {
                if (operatorS.Peek() == "+")
                {
                    operatorS.Pop();
                    return value.Pop() + value.Pop();
                }
                else
                {
                    operatorS.Pop();
                    double temp = value.Pop();
                    return value.Pop() - temp;
                }
            }
        }

        /// <summary>
        /// Returns each distinct variable in the formula.
        /// </summary>
        public ISet<string> GetVariables()
        {
            ConstructHelper();
            HashSet<string> set = new HashSet<string>();
            foreach (string s in tokensList)
            {
                if (Regex.IsMatch(s, @"[a-zA-Z][0-9a-zA-Z]*") && !double.TryParse(s, out double n))
                {
                    set.Add(s);
                }
            }
            return set;
        }

        /// <summary>
        /// Returns the original string the formula was constructed from.
        /// </summary>
        public override string ToString()
        {
            ConstructHelper();
            string s = "";
            foreach (string t in tokensList)
            {
                s = s + t;
            }
            return s;
        }

        /// <summary>
        /// Helper for zero parameter construct.
        /// </summary>
        private void ConstructHelper()
        {
            if (tokensList == null)
            {
                this = new Formula("0");
            }
        }

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of a letter followed by
        /// zero or more digits and/or letters, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens.
            // NOTE:  These patterns are designed to be used to create a pattern to split a string into tokens.
            // For example, the opPattern will match any string that contains an operator symbol, such as
            // "abc+def".  If you want to use one of these patterns to match an entire string (e.g., make it so
            // the opPattern will match "+" but not "abc+def", you need to add ^ to the beginning of the pattern
            // and $ to the end (e.g., opPattern would need to be @"^[\+\-*/]$".)
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";

            // PLEASE NOTE:  I have added white space to this regex to make it more readable.
            // When the regex is used, it is necessary to include a parameter that says
            // embedded white space should be ignored.  See below for an example of this.
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern.  It contains embedded white space that must be ignored when
            // it is used.  See below for an example of this.  This pattern is useful for 
            // splitting a string into tokens.
            String splittingPattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            // PLEASE NOTE:  Notice the second parameter to Split, which says to ignore embedded white space
            /// in the pattern.
            foreach (String s in Regex.Split(formula, splittingPattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
    }

    /// <summary>
    /// A Lookup method is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
    /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
    /// don't is up to the implementation of the method.
    /// </summary>
    public delegate double Lookup(string var);

    /// <summary>
    /// Normalizes strings that pass through it.
    /// </summary>
    public delegate string Normalizer(string s);

    /// <summary>
    /// Validates that a string passes a certain rule.
    /// </summary>
    public delegate bool Validator(string s);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    [Serializable]
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>
        public UndefinedVariableException(String variable)
            : base(variable)
        {
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>
    [Serializable]
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    [Serializable]
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}