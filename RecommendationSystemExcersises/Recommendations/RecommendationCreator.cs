using System;
using System.Linq;
using System.Collections.Generic;

namespace RecommendationSystemExcersises
{
    class RecommendationCreator
    {
        private ISimilarity similarityComputer;
        private IRatingPrediction predicter;
        int amountOfNeighbours;
        

        public RecommendationCreator(ISimilarity similarityComputer, IRatingPrediction predicter, int amountOfNeighbours)
        {
            this.similarityComputer = similarityComputer;
            this.predicter = predicter;
            this.amountOfNeighbours = amountOfNeighbours;
        }

        public List<KeyValuePair<int, double>> getListOfTopPredictedRatings(int recommendationAmount, Dictionary<int, User> sortedUserItems, User subject)
        {
            var allUniqueProducts = getUniqueProducts(sortedUserItems);
            var similaritiesAndNearestNeighbours = subject.getNearestNeighboursAndSimilarities(this.amountOfNeighbours, sortedUserItems.Values.ToList(), this.similarityComputer);

            var allPredictedRatingsPerProduct = new Dictionary<int, double>();
            foreach(var productNo in allUniqueProducts)
            {
                if(allPredictedRatingsPerProduct.ContainsKey(productNo) == false)
                {
                    var predictedRatingForCurrProduct = predicter.predictRating(productNo, subject, similaritiesAndNearestNeighbours);
                    allPredictedRatingsPerProduct.Add(productNo, predictedRatingForCurrProduct);
                }
            }

            allPredictedRatingsPerProduct.OrderByDescending(x => x.Value);
            return allPredictedRatingsPerProduct.Take(recommendationAmount).OrderByDescending(x => x.Value).ToList();
            /*
                Using the MovieLens dataset, consider user 186 and create a list of
                the 8 top recommendations for him, together with their predicted
                rating.

                o Based on the results you get, do you think it could be better to
                compute the predicted rating only for movies which were rated by
                more than one nearest neighbour (i.e., at least two or three)? Why?

                o Modify your algorithm to compute the predicted ratings
                considering only products rated by at least 3 neighbours.
                Execute again the program to create the list of 8 top
                recommendations for user 186.
             */
        }

        //Inefficient AF
        private List<int> getUniqueProducts(Dictionary<int, User> sortedUserItems)
        {
            var allUniqueProducts = new List<int>();
            var allProductsForAllUsersMatrix = sortedUserItems.Values.ToList().Select(x => x.ratings.Keys.ToList()).ToList();

            //LINQ THIS BOI
            foreach (var usersProducts in allProductsForAllUsersMatrix)
            {
                foreach(var product in usersProducts)
                {
                    if(allUniqueProducts.Contains(product) == false)
                        allUniqueProducts.Add(product);
                }
            }
            return allUniqueProducts;
        }
    }
}