using System;
using System.Linq;
using System.Collections.Generic;

namespace RecommendationSystemExcersises
{
    class WeightedAveragePrediction : IRatingPrediction
    {
        public double predictRating(int productKey, Dictionary<User, double> similaritiesAndNeighbours)
        {
            var totalSimilarityValue = 0.0;

            //check if these are ordered correctly
            var similaritiesAsPercentages = similaritiesAndNeighbours.Values.Select(x => (x / totalSimilarityValue)).ToList();

            var finalPrediction = 0.0;
            var neighboursAndSimilaritiesList = similaritiesAndNeighbours.ToList();

            for (int i = 0; i < neighboursAndSimilaritiesList.Count; i++)
            {
                if (neighboursAndSimilaritiesList[i].Key.ratings.ContainsKey(productKey))
                {
                    var currentNeighbourProductRating = neighboursAndSimilaritiesList[i].Key.ratings[productKey];
                    var currentNeighbourSimilarity = neighboursAndSimilaritiesList[i].Value;

                    finalPrediction += currentNeighbourProductRating * currentNeighbourSimilarity;
                    totalSimilarityValue += currentNeighbourSimilarity;
                }
            }
            finalPrediction = finalPrediction / totalSimilarityValue;
            return finalPrediction;
        }
    }
}