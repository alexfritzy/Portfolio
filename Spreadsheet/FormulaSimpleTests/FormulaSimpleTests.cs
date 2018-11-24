// Written by Joe Zachary for CS 3500, January 2017.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using System.Text.RegularExpressions;

namespace FormulaTestCases
{
    /// <summary>
    /// These test cases are in no sense comprehensive!  They are intended to show you how
    /// client code can make use of the Formula class, and to show you how to create your
    /// own (which we strongly recommend).  To run them, pull down the Test menu and do
    /// Run > All Tests.
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// This tests that a syntactically incorrect parameter to Formula results
        /// in a FormulaFormatException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct1()
        {
            Formula f = new Formula("_");
        }

        /// <summary>
        /// This is another syntax error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct2()
        {
            Formula f = new Formula("2++3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct3()
        {
            Formula f = new Formula("2 3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct4()
        {
            Formula f = new Formula("  ");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct5()
        {
            Formula f = new Formula("(2 + 3))(");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct6()
        {
            Formula f = new Formula("(((2 + 3))");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct7()
        {
            Formula f = new Formula("*24");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct8()
        {
            Formula f = new Formula("24*");
        }

        /// <summary>
        /// Zero parameters
        /// </summary>
        [TestMethod]
        public void Construct9()
        {
            Formula f = new Formula();
            Assert.AreEqual(f.Evaluate(v => 0), 0);
        }

        /// <summary>
        /// Three parameters
        /// </summary>
        [TestMethod]
        public void Construct10()
        {
            Formula f = new Formula("x2+y3", (s => s.ToUpper()), (s => (s != "x2") ? true : false));
            Assert.AreEqual(f.Evaluate(v => (v == "X2") ? 10 : 1), 11);
        }

        /// <summary>
        /// Null test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct11()
        {
            Formula f = new Formula(null);
        }

        /// <summary>
        /// Null test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct12()
        {
            Formula f = new Formula("5", null, (v => true));
        }

        /// <summary>
        /// Null test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct13()
        {
            Formula f = new Formula("5", (v => v), null);
        }

        /// <summary>
        /// Null test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct14()
        {
            Formula f = new Formula(null, (v => v), (v => true));
        }

        /// <summary>
        /// Get variables test
        /// </summary>
        [TestMethod]
        public void Variables1()
        {
            Formula f = new Formula("x2+y3", (s => s.ToUpper()), (s => (s != "x2") ? true : false));
            Assert.IsTrue(f.GetVariables().Contains("X2"));
            Assert.IsFalse(f.GetVariables().Contains("x2"));
        }

        /// <summary>
        /// Get variables test duplicates
        /// </summary>
        [TestMethod]
        public void Variables2()
        {
            Formula f = new Formula("x2+x2", (s => s.ToUpper()), (s => (s != "x2") ? true : false));
            Assert.IsTrue(f.GetVariables().Contains("X2"));
            Assert.IsFalse(f.GetVariables().Contains("x2"));
        }

        /// <summary>
        /// Get variables test empty
        /// </summary>
        [TestMethod]
        public void Variables3()
        {
            Formula f = new Formula();
            Assert.IsFalse(f.GetVariables().Contains("X2"));
            Assert.IsFalse(f.GetVariables().Contains("x2"));
        }

        /// <summary>
        /// ToString test
        /// </summary>
        [TestMethod]
        public void ToString1()
        {
            Formula f = new Formula("5 + x2");
            Formula f2 = new Formula(f.ToString());
            Assert.IsTrue(f.Evaluate(v => 1) == f2.Evaluate(v => 1) && f.Evaluate(v => 1) == 6);
        }

        /// <summary>
        /// Makes sure that "2+3" evaluates to 5.  Since the Formula
        /// contains no variables, the delegate passed in as the
        /// parameter doesn't matter.  We are passing in one that
        /// maps all variables to zero.
        /// </summary>
        [TestMethod]
        public void Evaluate1()
        {
            Formula f = new Formula("2+3");
            Assert.AreEqual(f.Evaluate(v => 0), 5.0, 1e-6);
        }

        /// <summary>
        /// The Formula consists of a single variable (x5).  The value of
        /// the Formula depends on the value of x5, which is determined by
        /// the delegate passed to Evaluate.  Since this delegate maps all
        /// variables to 22.5, the return value should be 22.5.
        /// </summary>
        [TestMethod]
        public void Evaluate2()
        {
            Formula f = new Formula("x5");
            Assert.AreEqual(f.Evaluate(v => 22.5), 22.5, 1e-6);
        }

        /// <summary>
        /// Here, the delegate passed to Evaluate always throws a
        /// UndefinedVariableException (meaning that no variables have
        /// values).  The test case checks that the result of
        /// evaluating the Formula is a FormulaEvaluationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate3()
        {
            Formula f = new Formula("x + y");
            f.Evaluate(v => { throw new UndefinedVariableException(v); });
        }

        /// <summary>
        /// The delegate passed to Evaluate is defined below.  We check
        /// that evaluating the formula returns in 10.
        /// </summary>
        [TestMethod]
        public void Evaluate4()
        {
            Formula f = new Formula("x + y");
            Assert.AreEqual(f.Evaluate(Lookup4), 10.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void Evaluate5()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0");
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);
        }

        /// <summary>
        /// Dividing by 0.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate6()
        {
            Formula f = new Formula("4 / 0");
            f.Evaluate(v => 0);
        }

        /// <summary>
        /// A Lookup method that maps x to 4.0, y to 6.0, and z to 8.0.
        /// All other variables result in an UndefinedVariableException.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Lookup4(String v)
        {
            switch (v)
            {
                case "x": return 4.0;
                case "y": return 6.0;
                case "z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }

        /// <summary>
        /// Null Evaluate
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Evaluate7()
        {
            Formula f = new Formula("4 / 1");
            f.Evaluate(null);
        }
    }
}

