using System;
using System.Linq;
using System.Collections.Generic;

namespace RecommendationSystemExcersises
{
    class Program
    {
        static void Main(string[] args)
        {
            var allUsersAndTheirRatings = Parser.parseUserRatings();   

            foreach(var userKey in allUsersAndTheirRatings.Keys)    
            {
                Console.WriteLine("Looking at user " + userKey);

                var currUserProductsAndRatings = allUsersAndTheirRatings[userKey].productsAndRatings;
                var sortedProductsAndRatings = QuickSort.quickSortDict(currUserProductsAndRatings);

                foreach(var productKey in sortedProductsAndRatings.Keys)
                {
                     Console.WriteLine(" This user gave product " + productKey + " the rating " 
                                       + currUserProductsAndRatings[productKey]);
                }     
            }    
            //Only wanna look at products they both rated.
            //

            //ISimilarity similarityMeasurer = new PearsonSimilarity();
            //Console.WriteLine("Pearson similarity between user 3 and user 4: " + similarityMeasurer.computeSimilarity(v,k)); 
        }
    }
}
