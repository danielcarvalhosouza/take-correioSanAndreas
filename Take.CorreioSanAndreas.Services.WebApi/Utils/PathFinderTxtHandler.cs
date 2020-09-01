using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Take.CorreioSanAndreas.Domain.Entities;
using Take.CorreioSanAndreas.Domain.Interfaces;

namespace Take.CorreioSanAndreas.Services.WebApi.Utils
{
    internal static class PathFinderTxtHandler
    {
        static Dictionary<string, Node> nodes = new Dictionary<string, Node>();
        static object lockObj = new object();
        static List<string> pathTextlines = new List<string>();

        private static string GeneratePathTextLine(params Node[] nodes)
        {
            string txt = null;
            int size = 0;

            for (int i = 0; i < nodes.Length - 1; i++)
            {
                var edge = nodes[i].Edges.First(x => x.Node2.Id == nodes[i + 1].Id);
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
            return $"{txt} {size}"; //txt + "" +;
        }

        public static void RegisterPaths(string trechos)
        
        {
            lock(lockObj)
            {
                nodes.Clear();
                pathTextlines = trechos.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();

                foreach (var line in pathTextlines)
                {
                    var nodeIds = line.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                    .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                    var node1Id = nodeIds[0];
                    var node2Id = nodeIds[1];

                    var connectionValue = int.Parse(nodeIds[2]);

                    if (!nodes.TryGetValue(node1Id, out Node node1))
                    {
                        node1 = new Node(node1Id);
                        nodes.Add(node1Id, node1);
                    }

                    if (!nodes.TryGetValue(node2Id, out Node node2))
                    {
                        node2 = new Node(node2Id);
                        nodes.Add(node2Id, node2);
                    }

                    node1.ConnectTo(node2, connectionValue);
                }
            }
        }

        public static IEnumerable<string> ListPaths()

        {
            return pathTextlines;
        }

        public static string GeneratePathsText(string encomendas, IShortestPathFinderService shortestPathFinder)

        {
            var lines = new List<string>();

            lock (lockObj)
            {
                foreach (var line in encomendas.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                {
                    var nodeIds = line.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                    .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                    var node1Id = nodeIds[0];
                    var node2Id = nodeIds[1];

                    var nodesPath = shortestPathFinder.FindShortestPath(nodes[node1Id], nodes[node2Id]);

                    lines.Add(GeneratePathTextLine(nodesPath));
                }
            }

            //return await Task.FromResult(string.Join(Environment.NewLine, lines));
            return string.Join(Environment.NewLine, lines);
        }
    }
}