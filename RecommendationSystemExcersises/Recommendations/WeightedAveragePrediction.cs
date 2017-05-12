using System;
using System.Linq;
using System.Collections.Generic;

namespace RecommendationSystemExcersises
{
    class WeightedAveragePrediction : IRatingPrediction
    {
        public double predictRating(int productKey, User subject, Dictionary<User, double> similaritiesAndNeighbours)
        {
            var totalSimilarityValue = similaritiesAndNeighbours.Values.ToList().Sum();

            //check if these are ordered correctly
            var similaritiesAsPercentages = similaritiesAndNeighbours.Values.Select(x => (x / totalSimilarityValue)).ToList();

            var finalPrediction = 0.0;
            var usersList = similaritiesAndNeighbours.Keys.ToList();
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