using System;
using System.Linq;
using System.Collections.Generic;

namespace RecommendationSystemExcersises
{
    class CosineSimilarity : ISimilarity
    {
        public bool canHandleSparseData() { return true; }
        public double computeSimilarity(Vector x, Vector y)
        {
            return x.dotProduct(y) / (x.getLength() * y.getLength());
        }
    }
}