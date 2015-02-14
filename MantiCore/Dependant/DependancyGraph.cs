using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MantiCore.Dependant
{
    public class DependancyGraph
    {
        private class DependancyNode
        {
            public DependancyNode(string nodeName)
            {
                ChildNodes = new List<DependancyNode>();
                NodeName = nodeName;
            }

            public string NodeName { get; private set; }
            public List<DependancyNode> ChildNodes { get; private set; }
            public bool Visited { get; set; }
            public int Checked { get; set; }
            public int Round { get; set; }

            public override string ToString()
            {
                return NodeName + ": {" + string.Join(", ", ChildNodes) + "}";
            }
        }

        public DependancyGraph(List<DependsOn> dependancies)
        {
            dependancies.Sort((d1, d2) => string.CompareOrdinal(d1.BundleIdentifier, d2.BundleIdentifier));
            BuildDependancyGraph(ref dependancies);
        }

        private void BuildDependancyGraph(ref List<DependsOn> dependancies)
        {
            DependancyNode node = null;
            for (var i = 0; i < dependancies.Count; i++)
            {
                if (dependancies[i] != null)
                {
                    BuildDependancyNode(ref node, dependancies[i].BundleIdentifier, ref dependancies);
                }
            }
        }

        private readonly Dictionary<string, DependancyNode> _dependancyNodeMap = new Dictionary<string, DependancyNode>();
        private readonly List<DependancyNode> _rootNodes = new List<DependancyNode>();
        private void BuildDependancyNode(ref DependancyNode parentNode, string dependancyName, ref List<DependsOn> dependancies)
        {
            if (_dependancyNodeMap.ContainsKey(dependancyName))
            {
                var childNode = _dependancyNodeMap[dependancyName];
                if (!childNode.ChildNodes.Contains(parentNode))
                {
                    _rootNodes.Remove(childNode);
                }
                else
                {
                    Debug.WriteLine("Circular dependancy detected!");
                }
                parentNode.ChildNodes.Add(childNode);
                return;
            }

            var ind = dependancies.BinarySearch(new DependsOn {BundleIdentifier = dependancyName});
            if (ind < 0 || ind >= dependancies.Count)
            {
                throw new MissingDependancyException(dependancyName, parentNode.NodeName);
            }
            var dependsOn = dependancies[ind];
            dependancies[ind] = null;

            var node = new DependancyNode(dependancyName);
            if (parentNode != null)
            {
                parentNode.ChildNodes.Add(node);
            }
            else
            {
                _rootNodes.Add(node);
            }
            _dependancyNodeMap[dependancyName] = node;

            foreach (var dependancy in dependsOn.Dependancies)
            {
                BuildDependancyNode(ref node, dependancy, ref dependancies);
            }
        }

        public List<string> GetNextStartupGroup()
        {
            var list = new List<string>();
            foreach (var dfs in _rootNodes.Where(t => !t.Visited).Select(t => DepthFirstSearch(t, _dfsRound)).Where(dfs => dfs != null))
            {
                list.AddRange(dfs);
            }
            _dfsRound++;
            return list;
        }

        private int _dfsRound = 1;
        private static List<string> DepthFirstSearch(DependancyNode rootNode, int i)
        {
            var nodes = new List<string>();
            DependancyNode node;
            while ((node = FindSink(rootNode, i)) != null)
            {
                nodes.Add(node.NodeName);
            }
            return nodes;
        }

        private static DependancyNode FindSink(DependancyNode rootNode, int round)
        {
            if (rootNode.Visited)
            {
                return null;
            }

            while (true)
            {
                if (rootNode.ChildNodes.Exists(n => n.Round == round))
                {
                    rootNode.Checked = 0;
                    return null;
                }

                if (!rootNode.Visited && !rootNode.ChildNodes.Exists(n => n.Visited == false))
                {
                    rootNode.Checked = 0;
                    rootNode.Visited = true;
                    rootNode.Round = round;
                    return rootNode;
                }

                foreach (var t in rootNode.ChildNodes.Where(t => !t.Visited))
                {
                    rootNode = t;
                    break;
                }

                if (rootNode.Checked == 3)
                {
                    rootNode.Checked = 0;
                    throw new CircularDependancyException(rootNode.NodeName);
                }
                rootNode.Checked++;
            }
        }
    }

    public class DependancyException : Exception
    {
        private const string DependancyText = "A problem was encountered creating a dependancy graph: ";
        public DependancyException(string message, Exception innerException = null)
            : base(DependancyText + message, innerException){}
    }

    public class CircularDependancyException : DependancyException
    {
        private const string CircularDependancyText = "A circular dependancy was detected on {0}.";
        public CircularDependancyException(string dependancy, Exception innerException = null)
            : base(string.Format(CircularDependancyText, dependancy), innerException) {}
    }

    public class MissingDependancyException : DependancyException
    {
        private const string MissingDependancyText = "The dependancy {0} could not be found, as required by {1}.";
        public MissingDependancyException(string dependancy, string bundleName, Exception innerException = null)
            : base(string.Format(MissingDependancyText, dependancy, bundleName), innerException) {}
    }
}
