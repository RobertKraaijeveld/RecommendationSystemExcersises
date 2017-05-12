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

        public List<KeyValuePair<int, double>> getListOfTopPredictedRatings(int recommendationAmount, int minimumAmountOfReviewsPerUser, Dictionary<int, User> sortedUserItems, User subject)
        {
            var allUniqueProducts = getUniqueProducts(sortedUserItems);
            var nearestNeighboursAndSimilarities = subject.getNearestNeighboursAndSimilarities(this.amountOfNeighbours, sortedUserItems.Values.ToList(), this.similarityComputer);

            Console.WriteLine("NEIGHBOURS: ");
            nearestNeighboursAndSimilarities.ToList().ForEach(x => Console.WriteLine("Neighbour with id " + x.Key.userId + " with similarity " + x.Value));


            var allPredictedRatingsPerProduct = new Dictionary<int, double>();
            foreach(var productNo in allUniqueProducts)
            {
                if(allPredictedRatingsPerProduct.ContainsKey(productNo) == false) 
                   //&& isRatedByGivenAmountOfNeighbours(recommendationAmount, productNo, nearestNeighboursAndSimilarities.Keys.ToList()))
                {
                    var predictedRatingForCurrProduct = predicter.predictRating(productNo, subject, nearestNeighboursAndSimilarities);
                    allPredictedRatingsPerProduct.Add(productNo, predictedRatingForCurrProduct);
                }
            }
            return allPredictedRatingsPerProduct.OrderByDescending(key => key.Value).Take(recommendationAmount).OrderByDescending(key => key.Value).ToList();
            
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

        //INCORRECT
        private bool isRatedByGivenAmountOfNeighbours(int amount, int productKey, List<User> neighbours)
        {
            return neighbours.Where(x => x.ratings.ContainsKey(productKey)).ToList().Count >= amount;
        }
    }
}