using System;
using System.Collections.Generic;
using System.Linq;

namespace ApproximationAlgorithms
{
    public class Graph<Vertex, EdgeData>
    {
        public Graph()
        {
        }

        public Graph(Graph<Vertex, EdgeData> copy)
        {
            Vertices = new HashSet<Vertex>(copy.Vertices);
            Edges = new List<Tuple<Vertex, Vertex, EdgeData>>(copy.Edges);
        }
        public HashSet<Vertex> Vertices { get; set; } = new HashSet<Vertex>();
        public IList<Tuple<Vertex, Vertex, EdgeData>> Edges { get; set; } = new List<Tuple<Vertex, Vertex, EdgeData>>();
    }

    public class GraphOperation
    {
        public static Graph<V, E> GetVertexInducedSubGraph<V, E>(Graph<V, E> originalGraph, Func<V, bool> vertexFilter)
        {
            Graph<V, E> graphRet = new Graph<V, E>();

            graphRet.Vertices = new HashSet<V>(originalGraph.Vertices.Where(vertexFilter));
            graphRet.Edges = new List<Tuple<V, V, E>>(
                originalGraph.Edges.Where(
                    edge => graphRet.Vertices.Contains(edge.Item1) && graphRet.Vertices.Contains(edge.Item2)
                ));
            return graphRet;
        }
    }
}
