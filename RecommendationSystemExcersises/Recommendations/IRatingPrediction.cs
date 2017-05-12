using System;
using System.Collections.Generic;

namespace RecommendationSystemExcersises
{
    interface IRatingPrediction
    {
        double predictRating(int productKey, User subject, Dictionary<User, double> similaritiesAndNeighbours);
    }
}