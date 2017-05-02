using System;

namespace RecommendationSystemExcersises
{
    interface ISimilarity
    {
        double computeSimilarity(Vector x, Vector y);
    }
}