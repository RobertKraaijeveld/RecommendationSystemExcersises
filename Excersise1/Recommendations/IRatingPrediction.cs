using System;
using System.Collections.Generic;

namespace Excersise1
{
    interface IRatingPrediction
    {
        double predictRating(int productKey, Dictionary<User, double> similaritiesAndNeighbours);
    }
}