using System;

namespace RecommendationSystemExcersises
{
    class ManhattanDistance : IDistance
    {
        public double computeDistance(Vector x, Vector y)
        {
            double manhattanDistance = 0.0f;

            //if value missing, skip it and dont add it to the distance.
            for(int i = 0; i < x.values.Count; i++)
            {
                if(i <= y.values.Count)
                {
                    manhattanDistance += Math.Abs(x.values[i] - y.values[i]);
                }
            }
            return manhattanDistance;
        }
    }
}