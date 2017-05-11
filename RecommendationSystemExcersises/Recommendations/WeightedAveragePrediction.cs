using System;
using System.Linq;
using System.Collections.Generic;

namespace RecommendationSystemExcersises
{
    class WeightedAveragePrediction : IRatingPrediction
    {
        //RENAME STUFF
        public double predictRating(int productKey, User subject, Dictionary<double, User> similaritiesAndNeighbours)
        {
            //filter the dict only on ratings of the productKey.
            var filteredDict = similaritiesAndNeighbours.Where(x => x.Value.ratings.ContainsKey(productKey)).ToDictionary(x => x.Key, x => x.Value);

            var totalSimilarityValue = filteredDict.Keys.ToList().Sum();

            //check if these are ordered correctly
            var similaritiesAsPercentages = filteredDict.Keys.Select(x => (x / totalSimilarityValue)).ToList();

            var finalPrediction = 0.0;
            var usersList = filteredDict.Values.ToList();
            for(int i = 0; i < usersList.Count; i++)
            {
                if(usersList[i].ratings.ContainsKey(productKey))
                {
                    var currentNeighbourProductRating = usersList[i].ratings[productKey];
                    finalPrediction += similaritiesAsPercentages[i] * currentNeighbourProductRating;
                }
            }
            return finalPrediction;
        }
    }
}