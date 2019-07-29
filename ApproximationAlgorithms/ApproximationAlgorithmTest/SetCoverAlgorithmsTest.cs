using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApproximationAlgorithms;
using System.Collections.Generic;
using System.Linq;

namespace ApproximationAlgorithmTest
{
    [TestClass]
    public class TestSetCoverAlgorithm
    {
        private IDictionary<HashSet<int>, double> priceMap;

        [TestInitialize]
        public void Init()
        {
            priceMap = new Dictionary<HashSet<int>, double>()
            {
                { new HashSet<int>(){ 1, 2, 3}, 3 },
                { new HashSet<int>(){ 4, 5, 3}, 1 },
                { new HashSet<int>(){ 5, 3, 4}, 3 },
                { new HashSet<int>(){ 1, 6, 3}, 2 },
            };
        }

        private bool SetEqual(HashSet<int> a, HashSet<int>b)
        {
            return a.IsSubsetOf(b) && b.IsSubsetOf(a);
        }

        [TestMethod]
        public void TestGreedyCoveringCanCoverEverElements()
        {
            SetCoverAlgorithms<int> algorithm = new SetCoverAlgorithms<int>();
            HashSet<HashSet<int>> result = algorithm.Greedy(priceMap, 6);
            HashSet<int> uionAll = new HashSet<int>();
            foreach (var set in result)
                uionAll.UnionWith(set);
            foreach (var value in new List<int>(){ 1, 2, 3, 4, 5, 6 })
                Assert.IsTrue(uionAll.Contains(value));
        }

        [TestMethod]
        public void TestGreedyCoveringCanReturnCorrectSequence()
        {
            SetCoverAlgorithms<int> algorithm = new SetCoverAlgorithms<int>();
            HashSet<HashSet<int>> result = algorithm.Greedy(priceMap, 6);
            HashSet<HashSet<int>> expected = new HashSet<HashSet<int>>()
            {
                new HashSet<int>(){ 1, 2, 3},
                new HashSet<int>(){ 4, 5, 3},
                new HashSet<int>(){ 1, 6, 3}
            };

            double actualCost = 0;
            foreach (var set in result)
            {
                actualCost += priceMap[set];
                bool currentSetIsOneOfSetsInExpected = false;
                foreach(var setExpected in expected)
                {
                    currentSetIsOneOfSetsInExpected = currentSetIsOneOfSetsInExpected || SetEqual(set, setExpected);
                }
                Assert.IsTrue(currentSetIsOneOfSetsInExpected);
            }
            Assert.AreEqual(result.Count, expected.Count);
            Assert.AreEqual(6, actualCost);
        }

        [TestMethod]
        public void TestGreedyWillPickMostCostEffectiveSetFirst()
        {
            priceMap = new Dictionary<HashSet<int>, double>()
            {
                { new HashSet<int>(){1}, 1 },
                { new HashSet<int>(){2}, 1 },
                { new HashSet<int>(){3}, 2 },
                { new HashSet<int>(){1, 2, 3}, 3.5 },
            };
            SetCoverAlgorithms<int> algorithm = new SetCoverAlgorithms<int>();
            HashSet<HashSet<int>> result =  algorithm.Greedy(priceMap, 3);
            HashSet<HashSet<int>> expected = new HashSet<HashSet<int>>()
            {
                new HashSet<int>(){1},
                new HashSet<int>(){2},
                new HashSet<int>(){3}
            };
            double actualCost = 0;
            foreach (var set in result)
            {
                actualCost += priceMap[set];
                bool currentSetIsOneOfSetsInExpected = false;
                foreach (var setExpected in expected)
                {
                    currentSetIsOneOfSetsInExpected = currentSetIsOneOfSetsInExpected || SetEqual(set, setExpected);
                }
                Assert.IsTrue(currentSetIsOneOfSetsInExpected);
            }
            Assert.AreEqual(result.Count, expected.Count);
            Assert.AreEqual(4, actualCost);
        }
    }
}
