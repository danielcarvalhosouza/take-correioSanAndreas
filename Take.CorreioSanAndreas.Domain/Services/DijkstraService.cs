using System;
using System.Collections.Generic;
using System.Linq;
using Take.CorreioSanAndreas.Domain.Entities;
using Take.CorreioSanAndreas.Domain.Interfaces;

namespace Take.CorreioSanAndreas.Domain.Services
{
    public class DijkstraService : IShortestPathFinderService
    {
        private class Weight
        {
            public Node From { get; }
            public int Value { get; }

            public Weight(Node @from, int value)
            {
                From = @from;
                Value = value;
            }
        }

        class VisitingData
        {
            readonly List<Node> _visiteds =
                new List<Node>();

            readonly Dictionary<Node, Weight> _weights =
                new Dictionary<Node, Weight>();

            readonly List<Node> _scheduled =
                new List<Node>();

            public void RegisterVisitTo(Node node)
            {
                if (!_visiteds.Contains(node))
                    _visiteds.Add((node));
            }

            public bool WasVisited(Node node)
            {
                return _visiteds.Contains(node);
            }

            public void UpdateWeight(Node node, Weight newWeight)
            {
                if (!_weights.ContainsKey(node))
                {
                    _weights.Add(node, newWeight);
                }
                else
                {
                    _weights[node] = newWeight;
                }
            }

            public Weight QueryWeight(Node node)
            {
                Weight result;
                if (!_weights.ContainsKey(node))
                {
                    result = new Weight(null, int.MaxValue);
                    _weights.Add(node, result);
                }
                else
                {
                    result = _weights[node];
                }
                return result;
            }

            public void ScheduleVisitTo(Node node)
            {
                _scheduled.Add(node);
            }

            public bool HasScheduledVisits => _scheduled.Count > 0;

            public Node GetNodeToVisit()
            {
                var ordered = from n in _scheduled
                              orderby QueryWeight(n).Value
                              select n;

                var result = ordered.First();
                _scheduled.Remove(result);
                return result;
            }

            public bool HasComputedPathToOrigin(Node node)
            {
                return QueryWeight(node).From != null;
            }

            public IEnumerable<Node> ComputedPathToOrigin(Node node)
            {
                var n = node;
                while (n != null)
                {
                    yield return n;
                    n = QueryWeight(n).From;
                }
            }
        }

        public Node[] FindShortestPath(Node @from, Node to)
        {
            var control = new VisitingData();

            control.UpdateWeight(@from, new Weight(null, 0));
            control.ScheduleVisitTo(@from);

            while (control.HasScheduledVisits)
            {
                var visitingNode = control.GetNodeToVisit();
                var visitingNodeWeight = control.QueryWeight(visitingNode);
                control.RegisterVisitTo(visitingNode);

                foreach (var neighborhoodInfo in visitingNode.Neighbors)
                {
                    if (!control.WasVisited(neighborhoodInfo.Node))
                    {
                        control.ScheduleVisitTo(neighborhoodInfo.Node);
                    }

                    var neighborWeight = control.QueryWeight(neighborhoodInfo.Node);

                    var probableWeight = (visitingNodeWeight.Value + neighborhoodInfo.WeightToNode);
                    if (neighborWeight.Value > probableWeight)
                    {
                        control.UpdateWeight(neighborhoodInfo.Node, new Weight(visitingNode, probableWeight));
                    }
                }
            }

            return control.HasComputedPathToOrigin(to)
                ? control.ComputedPathToOrigin(to).Reverse().ToArray()
                : null;
        }

        public string WriteTxtPath(params Node[][] nodesList)
        {
            string txt = null;

            foreach (var nodes in nodesList)
            {
                int size = 0;
                for (int i = 0; i < nodes.Length - 1; i++)
                {
                    var edge = nodes[i].Edges.First(x => x.Node2.Label == nodes[i + 1].Label);
                    if (i == 0)
                    {
                        txt += edge.Node1.Id + " " + edge.Node2.Id;
                    }
                    else
                    {
                        txt += " " + edge.Node2.Id;
                    }

                    size += edge.Value;
                }
                txt += " " + size + Environment.NewLine;
            }
            return txt;
        }
    }
}
