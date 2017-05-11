using System;
using System.Linq;
using System.Collections.Generic;

namespace RecommendationSystemExcersises
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("RUNNING SIMILARITY MEASURE DIAGNOSTIC");
            Console.WriteLine("-------------------------------------");            
            similarityMeasuresDiagnostic();

            Console.WriteLine("");    
            Console.WriteLine("RUNNING SINGLE RATING PREDICTION DIAGNOSTIC");   
            Console.WriteLine("-------------------------------------");                                 
            singlePredictionDiagnostic();

            Console.WriteLine("");    
            Console.WriteLine("RUNNING MULTIPLE RECOMMENDATIONS ON MOVIE100K DATA DIAGNOSTIC");   
            Console.WriteLine("-------------------------------------");                                 
            nDimensionalRecommendationDiagnostic();
        }

        private static void similarityMeasuresDiagnostic()
        {
            var allUsers = Parser.parseUserItems("docs/userItem.data", ',');

            foreach (var userKey in allUsers.Keys)
            {
                var currUserProductsAndRatings = allUsers[userKey].ratings;
                currUserProductsAndRatings = QuickSort.quickSortDict(currUserProductsAndRatings);
            }

            //check if still ordered correctly
            var testUser3 = allUsers[3];
            var testUser4 = allUsers[4];

            //ALSO: Make N instead of 2-Dimensional
            var overlappingProductsVectors = testUser3.getOverlappingRatingsVectors(testUser4);

            var testUser3Vector = overlappingProductsVectors[testUser3.userId];
            var testUser4Vector = overlappingProductsVectors[testUser4.userId];

            Console.WriteLine("User 3 ratings vector: " + testUser3Vector.ToString());
            Console.WriteLine("User 4 ratings vector: " + testUser4Vector.ToString());

            ISimilarity pearsonSimilarityMeasurer = new PearsonSimilarity();
            Console.WriteLine("Pearson similarity between user 3 and user 4: "
                               + pearsonSimilarityMeasurer.computeSimilarity(testUser3Vector, testUser4Vector));

        }

        private static void singlePredictionDiagnostic()
        {
            var allUsers = Parser.parseUserItems("docs/userItem.data", ',');

            foreach (var userKey in allUsers.Keys)
            {
                var currUserProductsAndRatings = allUsers[userKey].ratings;
                currUserProductsAndRatings = QuickSort.quickSortDict(currUserProductsAndRatings);
            }

            var userSeven = allUsers[7];

            ISimilarity pearsonSimilarityMeasurer = new PearsonSimilarity();

            //Change returnType.            
            var userSevenNeighbours = userSeven.getNearestNeighboursAndSimilarities(3, allUsers.Values.ToList(), pearsonSimilarityMeasurer);

            IRatingPrediction weightedAvgPrediction = new WeightedAveragePrediction();
            Console.WriteLine("User no. 7 predicted rating of product no. 101 is "
                               + weightedAvgPrediction.predictRating(101, userSeven, userSevenNeighbours));
            Console.WriteLine("User no. 7 predicted rating of product no. 103 is "
                               + weightedAvgPrediction.predictRating(103, userSeven, userSevenNeighbours));                               
            Console.WriteLine("User no. 7 predicted rating of product no. 106 is "
                               + weightedAvgPrediction.predictRating(106, userSeven, userSevenNeighbours));


            var userFour = allUsers[4];      
            var userFourNeighbours = userFour.getNearestNeighboursAndSimilarities(3, allUsers.Values.ToList(), pearsonSimilarityMeasurer);
               
            Console.WriteLine("");   
            Console.WriteLine("User no. 4 predicted rating of product no. 101 is "
                               + weightedAvgPrediction.predictRating(101, userFour, userFourNeighbours));   

            
            Console.WriteLine(""); 
            Console.WriteLine("USER NO. 7 RATED ITEM 106 AS A 2.8: REDOING PREDICTIONS OF ITEMS 101 and 103");
            userSeven.ratings[106] = 2.8;
            userSevenNeighbours = userSeven.getNearestNeighboursAndSimilarities(3, allUsers.Values.ToList(), pearsonSimilarityMeasurer);
            
            Console.WriteLine("User no. 7 predicted rating of product no. 101, with rating of product no. 106 set to 2.8 is "
                               + weightedAvgPrediction.predictRating(101, userSeven, userSevenNeighbours));  
            Console.WriteLine("User no. 7 predicted rating of product no. 103, with rating of product no. 106 set to 2.8 is "
                               + weightedAvgPrediction.predictRating(103, userSeven, userSevenNeighbours));


            Console.WriteLine(""); 
            Console.WriteLine("USER NO. 7 NOW RATED ITEM 106 AS A 5: REDOING PREDICTIONS OF ITEMS 101 and 103");
            userSeven.ratings[106] = 5;
            userSevenNeighbours = userSeven.getNearestNeighboursAndSimilarities(3, allUsers.Values.ToList(), pearsonSimilarityMeasurer);
            
            Console.WriteLine("User no. 7 predicted rating of product no. 101, with rating of product no. 106 updated to 5 is "
                               + weightedAvgPrediction.predictRating(101, userSeven, userSevenNeighbours));  
            Console.WriteLine("User no. 7 predicted rating of product no. 103, with rating of product no. 106 updated to 5 is "
                               + weightedAvgPrediction.predictRating(103, userSeven, userSevenNeighbours));
            
        }

        private static void nDimensionalRecommendationDiagnostic()
        {
            /*
            To create a LIST of 𝑛 recommendations?
            Compute the predicted rating on as many items as possible,
            using the 𝑘 nearest neighbours
            Sort the items by descending predicted rating
            Take the first 𝑛 items and suggest them to the user

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

            

    }
}
