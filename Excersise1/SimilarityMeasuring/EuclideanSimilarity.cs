using System;
using System.Linq;
using System.Collections.Generic;

namespace Excersise1
{
    class EuclideanSimilarity : ISimilarity
    {
        public bool canHandleSparseData() { return false; }

        public double computeSimilarity(Vector x, Vector y)
        {
            double euclideanDistance = 0.0f;

            //if value missing, skip it and dont add it to the distance.
            for (int i = 0; i < x.values.Count; i++)
            {
                if(i <= (y.values.Count - 1))
                {
                    euclideanDistance += Math.Pow((x.values[i] - y.values[i]), 2);
                }
            }
            return 1 / (Math.Sqrt(euclideanDistance) + 1);
        }
    }
}