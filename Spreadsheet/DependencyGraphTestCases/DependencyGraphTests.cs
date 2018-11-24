using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;
using System.Linq;
using System.Collections.Generic;

namespace DependencyGraphTestCases
{
    /// <summary>
    /// Test cases for DependencyGraph
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// Testing Size
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            Assert.IsTrue(dependencyGraph.Size == 0);
            dependencyGraph.AddDependency("A", "1");
            Assert.IsTrue(dependencyGraph.Size == 1);
        }
        /// <summary>
        /// Testing nulls in AddDependency()
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest1()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency(null, "1");
        }
        /// <summary>
        /// Testing nulls in AddDependency()
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest2()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("A", null);
        }
        /// <summary>
        /// Testing AddDependency() repetitions
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            Assert.IsTrue(dependencyGraph.Size == 0);
            dependencyGraph.AddDependency("A", "1");
            Assert.IsTrue(dependencyGraph.Size == 1);
            dependencyGraph.AddDependency("A", "1");
            Assert.IsTrue(dependencyGraph.Size == 1);
            dependencyGraph.AddDependency("A", "2");
            Assert.IsTrue(dependencyGraph.Size == 2);
            dependencyGraph.AddDependency("B", "2");
            Assert.IsTrue(dependencyGraph.Size == 3);
            dependencyGraph.AddDependency("B", "3");
            Assert.IsTrue(dependencyGraph.Size == 4);
        }
        /// <summary>
        /// Testing HasDependents()
        /// </summary>
        [TestMethod]
        public void TestMethod3()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("A", "1");
            Assert.IsTrue(dependencyGraph.HasDependents("A"));
            Assert.IsFalse(dependencyGraph.HasDependents("B"));
            Assert.IsFalse(dependencyGraph.HasDependents("1"));
        }
        /// <summary>
        /// Testing nulls in HasDependents()
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest3()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("A", "1");
            Assert.IsFalse(dependencyGraph.HasDependents(null));
        }
        /// <summary>
        /// Testing HasDependees()
        /// </summary>
        [TestMethod]
        public void TestMethod4()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("A", "1");
            Assert.IsTrue(dependencyGraph.HasDependees("1"));
            Assert.IsFalse(dependencyGraph.HasDependees("2"));
            Assert.IsFalse(dependencyGraph.HasDependees("A"));
        }
        /// <summary>
        /// Testing nulls in HasDependees()
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest4()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("A", "1");
            Assert.IsFalse(dependencyGraph.HasDependees(null));
        }
        /// <summary>
        /// Testing GetDependents()
        /// </summary>
        [TestMethod]
        public void TestMethod5()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("A", "1");
            dependencyGraph.AddDependency("A", "2");
            int i = 0;
            foreach (string s in dependencyGraph.GetDependents("A"))
            {
                i++;
            }
            Assert.IsTrue(i == 2);
            Assert.IsTrue(dependencyGraph.GetDependents("1").Equals(Enumerable.Empty<string>()));
        }
        /// <summary>
        /// Testing nulls in GetDependents()
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest5()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.GetDependents(null);
        }
        /// <summary>
        /// Testing GetDependees()
        /// </summary>
        [TestMethod]
        public void TestMethod6()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("A", "1");
            dependencyGraph.AddDependency("B", "1");
            int i = 0;
            foreach (string s in dependencyGraph.GetDependees("1"))
            {
                i++;
            }
            Assert.IsTrue(i == 2);
            Assert.IsTrue(dependencyGraph.GetDependees("A").Equals(Enumerable.Empty<string>()));
        }
        /// <summary>
        /// Testing nulls in GetDependees()
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest6()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.GetDependees(null);
        }
        /// <summary>
        /// Testing RemoveDependency()
        /// </summary>
        [TestMethod]
        public void TestMethod7()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("A", "1");
            dependencyGraph.RemoveDependency("A", "1");
            Assert.IsTrue(dependencyGraph.Size == 0);
            dependencyGraph.AddDependency("A", "1");
            dependencyGraph.AddDependency("A", "2");
            dependencyGraph.AddDependency("A", "3");
            dependencyGraph.AddDependency("B", "3");
            dependencyGraph.AddDependency("B", "4");
            dependencyGraph.AddDependency("B", "5");
            Assert.IsTrue(dependencyGraph.Size == 6);
            dependencyGraph.RemoveDependency("A", "1");
            Assert.IsTrue(dependencyGraph.Size == 5);
            dependencyGraph.RemoveDependency("A", "1");
            Assert.IsTrue(dependencyGraph.Size == 5);
            dependencyGraph.RemoveDependency("B", "3");
            Assert.IsTrue(dependencyGraph.Size == 4);
            dependencyGraph.RemoveDependency("A", "3");
            Assert.IsTrue(dependencyGraph.Size == 3);
            Assert.IsFalse(dependencyGraph.HasDependees("3"));
        }
        /// <summary>
        /// Testing nulls in RemoveDependency()
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest7()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.RemoveDependency(null, "1");
        }
        /// <summary>
        /// Testing nulls in RemoveDependency()
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest8()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.RemoveDependency("A", null);
        }
        /// <summary>
        /// Testing ReplaceDependents()
        /// </summary>
        [TestMethod]
        public void TestMethod8()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("A", "1");
            dependencyGraph.AddDependency("A", "2");
            dependencyGraph.AddDependency("B", "2");
            dependencyGraph.AddDependency("B", "3");
            dependencyGraph.AddDependency("B", "4");
            Assert.IsTrue(dependencyGraph.Size == 5);
            dependencyGraph.ReplaceDependents("A", dependencyGraph.GetDependents("B"));
            Assert.IsTrue(dependencyGraph.Size == 6);
        }
        /// <summary>
        /// Testing nulls in ReplaceDependents()
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest9()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.ReplaceDependents(null, dependencyGraph.GetDependents("B"));
        }
        /// <summary>
        /// Testing nulls in ReplaceDependents()
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest10()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.ReplaceDependents("A", null);
        }
        /// <summary>
        /// Testing nulls in ReplaceDependents()
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest11()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            List<string> list = new List<string> { "a", null, "b" };
            dependencyGraph.ReplaceDependents("A", list);
        }
        /// <summary>
        /// Testing ReplaceDependees()
        /// </summary>
        [TestMethod]
        public void TestMethod9()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("A", "1");
            dependencyGraph.AddDependency("B", "1");
            dependencyGraph.AddDependency("A", "2");
            dependencyGraph.AddDependency("B", "2");
            dependencyGraph.AddDependency("C", "2");
            Assert.IsTrue(dependencyGraph.Size == 5);
            dependencyGraph.ReplaceDependees("1", dependencyGraph.GetDependees("2"));
            Assert.IsTrue(dependencyGraph.Size == 6);
        }
        /// <summary>
        /// Testing nulls in ReplaceDependees()
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest12()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.ReplaceDependees(null, dependencyGraph.GetDependees("B"));
        }
        /// <summary>
        /// Testing nulls in ReplaceDependees()
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest13()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.ReplaceDependees("A", null);
        }
        /// <summary>
        /// Testing nulls in ReplaceDependees()
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest14()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            List<string> list = new List<string> { "a", null, "b" };
            dependencyGraph.ReplaceDependees("A", list);
        }
        /// <summary>
        /// Testing 100,000 different dependees and dependents
        /// </summary>
        [TestMethod]
        public void TestMethod10()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            for (int i = 0; i < 100000; i++)
            {
                dependencyGraph.AddDependency(i.ToString(), i.ToString());
            }
            Assert.IsTrue(dependencyGraph.Size == 100000);
            Assert.IsTrue(dependencyGraph.HasDependees("0"));
            Assert.IsTrue(dependencyGraph.HasDependees("99999"));
            Assert.IsTrue(dependencyGraph.HasDependents("0"));
            Assert.IsTrue(dependencyGraph.HasDependents("99999"));
        }
        /// <summary>
        /// Testing 100,000 different dependents
        /// </summary>
        [TestMethod]
        public void TestMethod11()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            for (int i = 0; i < 100000; i++)
            {
                dependencyGraph.AddDependency("0", i.ToString());
            }
            Assert.IsTrue(dependencyGraph.Size == 100000);
            Assert.IsTrue(dependencyGraph.HasDependees("0"));
            Assert.IsTrue(dependencyGraph.HasDependees("99999"));
            Assert.IsTrue(dependencyGraph.HasDependents("0"));
        }
        /// <summary>
        /// Testing 100,000 different dependees
        /// </summary>
        [TestMethod]
        public void TestMethod12()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            for (int i = 0; i < 100000; i++)
            {
                dependencyGraph.AddDependency(i.ToString(), "0");
            }
            Assert.IsTrue(dependencyGraph.Size == 100000);
            Assert.IsTrue(dependencyGraph.HasDependees("0"));
            Assert.IsTrue(dependencyGraph.HasDependents("0"));
            Assert.IsTrue(dependencyGraph.HasDependents("99999"));
        }
        /// <summary>
        /// Testing secondary constructor
        /// </summary>
        [TestMethod]
        public void TestMethod13()
        {
            DependencyGraph dependencyGraph = new DependencyGraph();
            dependencyGraph.AddDependency("A", "1");
            dependencyGraph.AddDependency("A", "2");
            dependencyGraph.AddDependency("B", "2");
            dependencyGraph.AddDependency("B", "3");
            Assert.IsTrue(dependencyGraph.Size == 4);
            DependencyGraph dependencyGraph2 = new DependencyGraph(dependencyGraph);
            dependencyGraph2.AddDependency("C", "3");
            Assert.IsTrue(dependencyGraph.Size == 4);
            Assert.IsTrue(dependencyGraph2.Size == 5);
        }
    }
}
