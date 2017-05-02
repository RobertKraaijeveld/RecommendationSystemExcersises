using System;
using System.Linq;
using System.Collections.Generic;

namespace RecommendationSystemExcersises
{
    class CosineSimilarity : ISimilarity
    {
        public double computeSimilarity(Vector x, Vector y)
        {
            return x.dotProduct(y) / (x.getLength() * y.getLength());
        }
    }
}