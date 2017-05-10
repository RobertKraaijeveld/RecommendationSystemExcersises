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
            */
            var list = new List<int>(){ 1,5,3,2};

            Console.WriteLine("UNORDERED: ");
            list.ForEach(x => Console.WriteLine(x));
                
            Utilities.quickSort(list, 0, list.Count - 1);

            Console.WriteLine("ORDERED: ");
            list.ForEach(x => Console.WriteLine(x));            
            
            //ISimilarity similarityMeasurer = new PearsonSimilarity();
            //Console.WriteLine("Pearson similarity between user 3 and user 4: " + similarityMeasurer.computeSimilarity(v,k)); 
        }
    }
}
