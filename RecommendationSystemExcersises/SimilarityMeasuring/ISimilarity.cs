using System;

namespace RecommendationSystemExcersises
{
    interface ISimilarity
    {
        //Dirty
        bool canHandleSparseData();
        double computeSimilarity(Vector x, Vector y);
    }
}