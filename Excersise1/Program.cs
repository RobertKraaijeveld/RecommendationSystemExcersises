﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace Excersise1
{
    class Program
    {
        static void Main(string[] args)
        {
            var allUsers = getParsedAndSortedUserItems("docs/userItem.data", new char[1] { ',' });
            
            Console.WriteLine("RUNNING SIMILARITY MEASURE DIAGNOSTIC");
            Console.WriteLine("-------------------------------------");
            similarityMeasuresDiagnostic(allUsers);

            Console.WriteLine("");
            Console.WriteLine("RUNNING NEAREST NEIGHBOURS CHECK USING ALL 3 MEASURES");
            Console.WriteLine("-------------------------------------");
            showNeighboursAndSimilaritiesOfUserSeven(allUsers);

            Console.WriteLine("");
            Console.WriteLine("RUNNING SINGLE RATING PREDICTION DIAGNOSTIC");
            Console.WriteLine("-------------------------------------");
            singlePredictionDiagnostic(allUsers);

            Console.WriteLine("");
            Console.WriteLine("RUNNING MULTIPLE RECOMMENDATIONS ON MOVIE100K DATA DIAGNOSTIC");
            Console.WriteLine("-------------------------------------");

            var sortedUserItems = getParsedAndSortedUserItems("docs/u.data", new char[2] { ' ', '\t' });            
            nDimensionalRecommendationDiagnostic(sortedUserItems);
        }

        private static void similarityMeasuresDiagnostic(Dictionary<int, User> allUsers)
        {
            ISimilarity similarityMeasurer = new PearsonSimilarity();

            var testUser1 = allUsers[1];
            var testUser3 = allUsers[3];

            var overlappingProductsVectors = testUser1.getRatingsVectors(testUser3, similarityMeasurer.canHandleSparseData());

            var testUser1Vector = overlappingProductsVectors[testUser1.userId];
            var testUser3Vector = overlappingProductsVectors[testUser3.userId];

            Console.WriteLine("User 1 ratings vector: " + testUser1Vector .ToString());
            Console.WriteLine("User 3 ratings vector: " + testUser3Vector.ToString());

            Console.WriteLine("Pearson similarity between user 1 and user 3: "
                               + similarityMeasurer.computeSimilarity(testUser1Vector , testUser3Vector));
        }


        private static void outputNeighbours(Dictionary<User, double> neighboursAndSimilarities)
        {
            neighboursAndSimilarities.ToList().ForEach(kv => Console.WriteLine("Neighbour with user no. " + kv.Key.userId + " has similarity " + kv.Value));                                                
        }
        private static void showNeighboursAndSimilaritiesOfUserSeven(Dictionary<int, User> allUsers)
        {
            var userSeven = allUsers[7];

            ISimilarity similarityMeasure = new EuclideanSimilarity();
            var userSevenNeighboursAndSimilarities = userSeven.getNearestNeighboursAndSimilarities(3, allUsers.Values.ToList(), similarityMeasure);
            
            Console.WriteLine("");
            Console.WriteLine("Neighbours and similarities using EUCLIDEAN: ");
            outputNeighbours(userSevenNeighboursAndSimilarities);
            

            similarityMeasure = new PearsonSimilarity();
            userSevenNeighboursAndSimilarities = userSeven.getNearestNeighboursAndSimilarities(3, allUsers.Values.ToList(), similarityMeasure);
            
            Console.WriteLine("");
            Console.WriteLine("Neighbours and similarities using PEARSON: ");
            outputNeighbours(userSevenNeighboursAndSimilarities);
            

            similarityMeasure = new CosineSimilarity();
            userSevenNeighboursAndSimilarities = userSeven.getNearestNeighboursAndSimilarities(3, allUsers.Values.ToList(), similarityMeasure);
            
            Console.WriteLine("");
            Console.WriteLine("Neighbours and similarities using COSINE: ");
            outputNeighbours(userSevenNeighboursAndSimilarities);
        }

        private static void singlePredictionDiagnostic(Dictionary<int, User> allUsers)
        {
            var userSeven = allUsers[7];

            ISimilarity similarityMeasure = new PearsonSimilarity();

            var userSevenNeighboursAndSimilarities = userSeven.getNearestNeighboursAndSimilarities(3, allUsers.Values.ToList(), similarityMeasure);

            Console.WriteLine("");            
            IRatingPrediction weightedAvgPrediction = new WeightedAveragePrediction();
            Console.WriteLine("User no. 7 predicted rating of product no. 101  using pearson is "
                               + weightedAvgPrediction.predictRating(101, userSevenNeighboursAndSimilarities));
            Console.WriteLine("User no. 7 predicted rating of product no. 103 using pearson is "
                               + weightedAvgPrediction.predictRating(103, userSevenNeighboursAndSimilarities));
            Console.WriteLine("User no. 7 predicted rating of product no. 106 using pearson is "
                               + weightedAvgPrediction.predictRating(106, userSevenNeighboursAndSimilarities));

            //INCORRECT, CHECK THIS ONE
            var userFour = allUsers[4];
            var userFourNeighboursAndSimilarities = userFour.getNearestNeighboursAndSimilarities(3, allUsers.Values.ToList(), similarityMeasure);

            Console.WriteLine("");
            Console.WriteLine("User no. 4 predicted rating of product no. 101 using pearson is "
                               + weightedAvgPrediction.predictRating(101, userFourNeighboursAndSimilarities));


            Console.WriteLine("");
            Console.WriteLine("USER NO. 7 RATED ITEM 106 AS A 2.8: REDOING PREDICTIONS OF ITEMS 101 and 103");
            userSeven.ratings[106] = 2.8;
            userSevenNeighboursAndSimilarities = userSeven.getNearestNeighboursAndSimilarities(3, allUsers.Values.ToList(), similarityMeasure);

            Console.WriteLine("User no. 7 predicted rating of product no. 101, with rating of product no. 106 set to 2.8 using pearson is "
                               + weightedAvgPrediction.predictRating(101, userSevenNeighboursAndSimilarities));
            Console.WriteLine("User no. 7 predicted rating of product no. 103, with rating of product no. 106 set to 2.8 using pearson is "
                               + weightedAvgPrediction.predictRating(103, userSevenNeighboursAndSimilarities));


            Console.WriteLine("");
            Console.WriteLine("USER NO. 7 NOW RATED ITEM 106 AS A 5: REDOING PREDICTIONS OF ITEMS 101 and 103");
            userSeven.ratings[106] = 5;
            userSevenNeighboursAndSimilarities = userSeven.getNearestNeighboursAndSimilarities(3, allUsers.Values.ToList(), similarityMeasure);

            Console.WriteLine("User no. 7 predicted rating of product no. 101, with rating of product no. 106 updated to 5 using pearson is "
                               + weightedAvgPrediction.predictRating(101, userSevenNeighboursAndSimilarities));
            Console.WriteLine("User no. 7 predicted rating of product no. 103, with rating of product no. 106 updated to 5 using pearson is "
                               + weightedAvgPrediction.predictRating(103, userSevenNeighboursAndSimilarities));

        }

        private static void nDimensionalRecommendationDiagnostic(Dictionary<int, User> sortedUserItems)
        {
            var user186 = sortedUserItems[186];

            ISimilarity pearsonSimilarity = new PearsonSimilarity();
            IRatingPrediction weightedAvgPrediction = new WeightedAveragePrediction();
            
            int neighboursAmount = 25;
            RecommendationCreator recommendationCreator = new RecommendationCreator(pearsonSimilarity, weightedAvgPrediction, neighboursAmount);

            int recommendationAmount = 8;
            int minimumAmountOfNeighbourProductOccurences = 0;
            var top8Recommendations = recommendationCreator.getListOfTopPredictedRatings(recommendationAmount, minimumAmountOfNeighbourProductOccurences, sortedUserItems, user186);
            
            Console.WriteLine("");            
            Console.WriteLine("Top 8 recommendations for user 186 with "+neighboursAmount+" neighbours and minimumAmountOfNeighbourProductOccurences = 0: ");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("");            

            int counter = 0;
            top8Recommendations.ForEach(x => Console.WriteLine("Top " + (++counter) + " product: Movie no. " + x.Key + ", predicted rating " + x.Value));
            Console.WriteLine("");            

            recommendationAmount = 8;
            minimumAmountOfNeighbourProductOccurences = 3;
            top8Recommendations = recommendationCreator.getListOfTopPredictedRatings(recommendationAmount, minimumAmountOfNeighbourProductOccurences, sortedUserItems, user186);

            Console.WriteLine("");                         
            Console.WriteLine("Top 8 recommendations for user 186 with "+neighboursAmount+" neighbours and minimumAmountOfNeighbourProductOccurences = 3: ");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("");            

            counter = 0;
            top8Recommendations.ForEach(x => Console.WriteLine("Top " + (++counter) + " product: Movie no. " + x.Key + ", predicted rating " + x.Value));
            
        }

        /*
        HELPERS
        */

        private static Dictionary<int, User> getParsedAndSortedUserItems(string fileName, char[] delimiters)
        {
            var usersAndRatings = Parser.parseUserItems(fileName, delimiters);
            var vals = usersAndRatings.Values.ToList();

            //quicksorting keys by product numbers
            foreach (var userKey in usersAndRatings.Keys)
            {
                var currUserProductsAndRatings = usersAndRatings[userKey].ratings;
                usersAndRatings[userKey].ratings = QuickSort.quickSortDict(currUserProductsAndRatings);
            } 
            return usersAndRatings;
        }

        private static List<User> getUsersForNeighbourUserIds(List<int> neighbourUserIds, Dictionary<int, User> allUsers)
        {
            var users = new List<User>();
            foreach(var userId in neighbourUserIds)
            {
                users.Add(allUsers[userId]);
            }
            return users;
        }

    }
}
