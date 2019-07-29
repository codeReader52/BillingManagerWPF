using System;
using System.Collections.Generic;
using System.Linq;

namespace ApproximationAlgorithms
{
    public class SetCoverAlgorithms<T>
    {
        public HashSet<HashSet<T>> Greedy(IDictionary<HashSet<T>, double> dictionarySetToCost, int numElementsToCover)
        {
            HashSet<HashSet<T>> setRet = new HashSet<HashSet<T>>();
            HashSet<T> setElementAlreadyCovered = new HashSet<T>();
            HashSet<HashSet<T>> setOfRemainingSets = new HashSet<HashSet<T>>();
            foreach (var set in dictionarySetToCost.Keys)
            {
                setOfRemainingSets.Add(set);
            }
            while (setElementAlreadyCovered.Count < numElementsToCover)
            {
                HashSet<T> mostCostEffectiveSet = FindMostCosEffectivenessSet(dictionarySetToCost, setOfRemainingSets, setElementAlreadyCovered);
                if (mostCostEffectiveSet == null)
                {
                    Console.WriteLine("Should not be returning a null object while search for most cost effective set");
                    break;
                }
                setRet.Add(mostCostEffectiveSet);
                setElementAlreadyCovered.UnionWith(mostCostEffectiveSet);
                setOfRemainingSets.Remove(mostCostEffectiveSet);
            }
            return setRet;
        }

        private HashSet<T> FindMostCosEffectivenessSet(IDictionary<HashSet<T>, double> dictionarySetToCost, HashSet<HashSet<T>> setOfRemainingSets, HashSet<T> setElementAlreadyCovered)
        {
            HashSet<T> selectedSet = null;
            double bestCostEffectiveness = double.MaxValue;
            foreach(var candidate in setOfRemainingSets)
            {
                IEnumerable<T> notYetSelectedElementsInCandidate = candidate.Except<T>(setElementAlreadyCovered);
                int numNotYetCoveredelement = notYetSelectedElementsInCandidate.Count<T>();
                if (numNotYetCoveredelement == 0)
                    continue;
                double averageCostPerUnselectedItem = dictionarySetToCost[candidate] / (numNotYetCoveredelement);
                if (averageCostPerUnselectedItem < bestCostEffectiveness)
                {
                    bestCostEffectiveness = averageCostPerUnselectedItem;
                    selectedSet = candidate;
                }
            }
            return selectedSet;
        }
    }

    public class LayerVertexCover<V, E>
    {
        public static HashSet<V> FindVertexCover(Graph<V, E> graph, IDictionary<V, double> vertexCost)
        {
            HashSet<V> setVertRet = new HashSet<V>();

            Func<Tuple<V, V, E>, bool> filterEdgeNotInCurrentVertexCover = 
                edge => !setVertRet.Contains(edge.Item1) && !setVertRet.Contains(edge.Item2);

            Graph<V, E> tempGraph = new Graph<V, E>(graph);

            while (graph.Edges.Where(filterEdgeNotInCurrentVertexCover).Count() > 0)
            {
                IEnumerable<V> setVertexHead = tempGraph.Edges.Select(edge => edge.Item1), setVertexTail = tempGraph.Edges.Select(edge => edge.Item2);
                IEnumerable<V> setVertexWithAtLeastAnEdge = setVertexTail.Union(setVertexHead);
                IEnumerable<V> setVertexZeroDeg = tempGraph.Vertices.Except(setVertexWithAtLeastAnEdge);
                tempGraph = GraphOperation.GetVertexInducedSubGraph<V, E>(tempGraph, vertex => setVertexWithAtLeastAnEdge.Contains(vertex));
            }
            return setVertRet;
        }
    }
}
