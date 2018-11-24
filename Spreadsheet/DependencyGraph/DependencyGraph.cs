// Skeleton implementation written by Joe Zachary for CS 3500, January 2018.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {
        private int count;
        // There are two dictionaries, one of dependees and one of dependents.
        //Any action that takes place in one gets mirrored on the other. This way, all accesses are in constant time.
        private Dictionary<string, HashSet<string>> dependees;
        private Dictionary<string, HashSet<string>> dependents;
        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            count = 0;
            dependees = new Dictionary<string, HashSet<string>>();
            dependents = new Dictionary<string, HashSet<string>>();
        }

        /// <summary>
        /// Creates a DependencyGraph which is a copy of the graph parameter but independent.
        /// </summary>
        public DependencyGraph(DependencyGraph graph)
        {
            count = graph.Size;
            dependees = new Dictionary<string, HashSet<string>>();
            dependents = new Dictionary<string, HashSet<string>>();
            HashSet<string> temp;
            foreach (KeyValuePair<string, HashSet<string>> k in graph.dependees)
            {
                temp = new HashSet<string>();
                foreach (string v in k.Value)
                {
                    temp.Add(v);
                }
                dependees.Add(k.Key, temp);
            }
            foreach (KeyValuePair<string, HashSet<string>> k in graph.dependents)
            {
                temp = new HashSet<string>();
                foreach (string v in k.Value)
                {
                    temp.Add(v);
                }
                dependents.Add(k.Key, temp);
            }
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return count; }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null or will throw an ArgumentNullException.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (dependees.ContainsKey(s))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null or will throw an ArgumentNullException.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (dependents.ContainsKey(s))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// To help GetDependents() and GetDependees().
        /// </summary>
        private IEnumerable<string> StringHelper(string s, Dictionary<string, HashSet<string>> dictionary)
        {
            foreach (string t in dictionary[s])
            {
                yield return t;
            }
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null or will throw an ArgumentNullException.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            else if (dependees.ContainsKey(s))
            {
                return StringHelper(s, dependees);
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null or will throw an ArgumentNullException.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            else if (dependents.ContainsKey(s))
            {
                return StringHelper(s, dependents);
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null or will throw an ArgumentNullException.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (t == null)
            {
                throw new ArgumentNullException("t");
            }
            if (dependees.ContainsKey(s))
            {
                if (dependees[s].Contains(t))
                {
                    return;
                }
                else
                {
                    dependees[s].Add(t);
                    if (dependents.ContainsKey(t))
                    {
                        dependents[t].Add(s);
                    }
                    else
                    {
                        dependents.Add(t, new HashSet<string> { { s } });
                    }
                    count++;
                }
            }
            else
            {
                dependees.Add(s, new HashSet<string> { { t } });
                if (dependents.ContainsKey(t))
                {
                    dependents[t].Add(s);
                }
                else
                {
                    dependents.Add(t, new HashSet<string> { { s } });
                }
                count++;
            }
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null or will throw an ArgumentNullException.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (t == null)
            {
                throw new ArgumentNullException("t");
            }
            if (dependees.ContainsKey(s))
            {
                if (dependees[s].Contains(t))
                {
                    if (dependees[s].Count == 1)
                    {
                        dependees.Remove(s);
                        if (dependents[t].Count == 1)
                        {
                            dependents.Remove(t);
                        }
                        else
                        {
                            dependents[t].Remove(s);
                        }
                    }
                    else
                    {
                        dependees[s].Remove(t);
                        if (dependents[t].Count == 1)
                        {
                            dependents.Remove(t);
                        }
                        else
                        {
                            dependents[t].Remove(s);
                        }
                    }
                    count--;
                }
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null or will throw an ArgumentNullException.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (newDependents == null)
            {
                throw new ArgumentNullException("newDependents");
            }
            List<string> temp = new List<string>();
            foreach (string t in GetDependents(s))
            {
                if (t == null)
                {
                    throw new ArgumentNullException("t");
                }
                temp.Add(t);
            }
            foreach (string t in temp)
            {
                RemoveDependency(s, t);
            }
            foreach (string t in newDependents)
            {
                AddDependency(s, t);
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null or will throw an ArgumentNullException.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if (t == null)
            {
                throw new ArgumentNullException("t");
            }
            if (newDependees == null)
            {
                throw new ArgumentNullException("newDependees");
            }
            List<string> temp = new List<string>();
            foreach (string s in GetDependees(t))
            {
                if (s == null)
                {
                    throw new ArgumentNullException("s");
                }
                temp.Add(s);
            }
            foreach (string s in temp)
            {
                RemoveDependency(s, t);
            }
            foreach (string s in newDependees)
            {
                AddDependency(s, t);
            }
        }
    }
}