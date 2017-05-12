using System;
using System.Linq;
using System.Collections.Generic;

namespace RecommendationSystemExcersises
{
    class WeightedAveragePrediction : IRatingPrediction
    {
        public double predictRating(int productKey, User subject, Dictionary<User, double> similaritiesAndNeighbours)
        {
            //filter the dict only on ratings of the productKey.
            var filteredDict = similaritiesAndNeighbours.Where(x => x.Key.ratings.ContainsKey(productKey)).ToDictionary(x => x.Key, x => x.Value);

            var totalSimilarityValue = filteredDict.Values.ToList().Sum();

            //check if these are ordered correctly
            var similaritiesAsPercentages = filteredDict.Values.Select(x => (x / totalSimilarityValue)).ToList();

            var finalPrediction = 0.0;
            var usersList = filteredDict.Keys.ToList();
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