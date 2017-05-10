using System;
using System.Collections.Generic;

namespace RecommendationSystemExcersises
{
    class Program
    {
        static void Main(string[] args)
        {
            /* 
            var allUsersAndTheirRatings = Parser.parseUserRatings();   

            foreach(var userKey in allUsersAndTheirRatings.Keys)    
            {
                Console.WriteLine("Looking at user " + userKey);
                var currUserProductsAndRatings = allUsersAndTheirRatings[userKey].productsAndRatings;
                foreach(var productKey in currUserProductsAndRatings.Keys)
                {
                     Console.WriteLine(" This user gave product " + productKey + " the rating " 
                                       + currUserProductsAndRatings[productKey]);
                }     
            }    

            //if map is ordered on product its no problem:)
            //if its not, correlation measures wont be right.
            //its not. use quicksort on it.
            //then, get users 3 and 4 with their ordered productratings and do pearson on it.
            */  

            var unordered = new List<int>(){ 1,5,3,2};
            var ordered = Utilities.quickSort(unordered);

            
            //ISimilarity similarityMeasurer = new PearsonSimilarity();
            //Console.WriteLine("Pearson similarity between user 3 and user 4: " + similarityMeasurer.computeSimilarity(v,k)); 
        }
    }
}
