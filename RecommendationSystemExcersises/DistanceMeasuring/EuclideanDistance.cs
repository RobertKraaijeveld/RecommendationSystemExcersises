using System;

namespace RecommendationSystemExcersises
{
    class EuclideanDistance : IDistance
    {
        public double computeDistance(Vector x, Vector y)
        {
            double euclideanDistance = 0.0f;

            //if value missing, skip it and dont add it to the distance.
            for(int i = 0; i < x.values.Count; i++)
            {
                if(i <= y.values.Count)
                {
                    euclideanDistance += Math.Pow((x.values[i] - y.values[i]), 2);
                }
            }
            return Math.Sqrt(euclideanDistance);
        }
    }
}