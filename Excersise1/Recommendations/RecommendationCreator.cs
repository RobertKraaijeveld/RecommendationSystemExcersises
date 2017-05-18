using System;
using System.Linq;
using System.Collections.Generic;

namespace Excersise1
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

        public List<KeyValuePair<int, double>> getListOfTopPredictedRatings(int recommendationAmount, int minimumNeighboursThatRatedProductAmount, Dictionary<int, User> sortedUserItems, User subject)
        {
            var allUniqueProducts = getUniqueProducts(sortedUserItems);
            allUniqueProducts = allUniqueProducts.Where(x => !subject.ratings.ContainsKey(x)).ToList();
            
            var nearestNeighboursAndSimilarities = subject.getNearestNeighboursAndSimilarities(this.amountOfNeighbours, sortedUserItems.Values.ToList(), this.similarityComputer);

            var allPredictedRatingsPerProduct = new Dictionary<int, double>();
            foreach(var productNo in allUniqueProducts)
            {
                if(amountOfNeighboursThatRatedProduct(productNo, nearestNeighboursAndSimilarities.Keys.ToList()) >= minimumNeighboursThatRatedProductAmount)
                {
                    var predictedRatingForCurrProduct = predicter.predictRating(productNo, nearestNeighboursAndSimilarities);
                    allPredictedRatingsPerProduct.Add(productNo, predictedRatingForCurrProduct);
                }
            }
            //Doing this just to see how much predictRating calls filtering on amountOfNeighboursThatRatedProduct saves us:
            Console.WriteLine("Computed " + allPredictedRatingsPerProduct.Count + " ratings predictions with minimum amount of neighbours to have rated a product being " + minimumNeighboursThatRatedProductAmount);
            
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

        private int amountOfNeighboursThatRatedProduct(int productKey, List<User> neighbours)
        {
            return neighbours.Where(x => x.ratings.ContainsKey(productKey)).ToList().Count;
        }
    }
}