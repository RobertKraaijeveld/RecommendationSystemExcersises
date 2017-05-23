using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace Excersise2
{
    /*
    Given a dataset of ratings, implement the computation of the deviations
    between all pairs of items and store them in a specific data structure.
    Note: given two items, remember to store (together with their deviation
    value) also the number of persons which rated both items. This way,
    updating deviations will be easier and quicker. DONE

    Given a user and an item, implement the computation of the predicted
    rating of such item for that user. DONE

    Apply the algorithm to the small dataset already used in Part 1
    (userItem.data) and compute the predicted ratings for user 7 (items 101,
    103, 106) and for user 3 (items 103, 105).

    Given a new item rating (user-id, item-id, rating), implement the execution
    of the needed updates to the deviations between items. Think about which
    deviations you need to update: all of them or only a subset?

    Suppose that user 3 rates item 105 with 4.0. Update the deviations and
    compute again the predicted ratings for user 7. Which of the three
    predicted ratings (items 101, 103, 106) change and which stay the same?
    Explain why that happens.
    Given a user, implement the computation of the set of 𝑛 top
    recommendations for him/her.

    Apply the algorithm to the MovieLens dataset 100K and compute the 5 top
    recommendations for user 186 and display their predicted rating.
    How much time does it take to create a recommendation for a user
    (excluding the computation of the deviations)?
     */
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser("docs/userItem.data", new char[1] { ',' });

            var allUsers = parser.getParsedUsers();
            var allItems = parser.getParsedItemsWithAmountOfTimesRated();
            computeAllDeviations(allItems, allUsers);

            var user7 = allUsers[7];
            var user7sPredictions = computeMultiplePredictions(user7, new List<int>() { 101, 103, 106 }, allItems);

            var user3 = allUsers[3];
            var user3sPredictions = computeMultiplePredictions(user7, new List<int>() { 103, 105 }, allItems);

            Console.WriteLine("User 7 predictions: ");
            user7sPredictions.ForEach(kv => Console.WriteLine("Item no. " + kv.Item1 + " predicted rating: " + kv.Item2));

            Console.WriteLine("");
            Console.WriteLine("User 3 predictions: ");
            user3sPredictions.ForEach(kv => Console.WriteLine("Item no. " + kv.Item1 + " predicted rating: " + kv.Item2));

            Console.WriteLine("");
            Console.WriteLine("Updating users 3 rating for item 105 to 4.0: ");
            allItems = updateDeviations(user3, 105, 4.0, allItems, allUsers);

            Console.WriteLine("NEW User 7 predictions: ");
            user7sPredictions = computeMultiplePredictions(user7, new List<int>() { 101, 103, 106 }, allItems);
            user7sPredictions.ForEach(kv => Console.WriteLine("Item no. " + kv.Item1 + " predicted rating: " + kv.Item2));



            Console.WriteLine("");
            Console.WriteLine("IMPORTING MOVIE100K DATA");
            Console.WriteLine("");

            parser = new Parser("docs/u.data", new char[1] { '	' });

            allUsers = parser.getParsedUsers();
            allItems = parser.getParsedItemsWithAmountOfTimesRated();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //OPTIMIZE THE F**K OUT OF THIS PLS
            computeAllDeviations(allItems, allUsers);

            stopWatch.Stop();
            Console.WriteLine("Deviation creation time: " + stopWatch.ElapsedMilliseconds / 1000);


        }

        //on update alleen dat item zn row en column.


        /*
            DEVIATION COMPUTATION
        */

        //lot of params bruh
        private static Dictionary<int, Item> updateDeviations(User user, int toBeUpdatedItemId, double newRating, Dictionary<int, Item> allItems, Dictionary<int, User> allUsers)
        {
            user.ratings[toBeUpdatedItemId] = newRating;
            var itemObjectForId = allItems[toBeUpdatedItemId];

            //computing the new deviations of toBeUpdatedItemId to all other items and vice versa            
            foreach (var otherItemDeviationKV in itemObjectForId.deviations.ToList())
            {
                var otherItemId = otherItemDeviationKV.Key;

                if (otherItemId != toBeUpdatedItemId)
                {
                    var otherItem = itemObjectForId;

                    var usersWhoRatedBoth = getListOfUsersThatRatedBoth(toBeUpdatedItemId, otherItemId, allUsers);

                    var newDeviationOfUpdatedItemToOtherItem = computeDeviation(toBeUpdatedItemId, otherItemId, usersWhoRatedBoth);
                    var newDeviationOfOtherItemToUpdatedItem = computeDeviation(otherItemId, toBeUpdatedItemId, usersWhoRatedBoth);


                    Console.WriteLine("Item " + toBeUpdatedItemId + " gets new deviation " + newDeviationOfUpdatedItemToOtherItem + " with other item " + otherItemId 
                                    + " old was " + allItems[toBeUpdatedItemId].deviations[otherItemId]);

                    Console.WriteLine("OTHER Item " + otherItemId + " gets new deviation " + newDeviationOfOtherItemToUpdatedItem + " with other item " + toBeUpdatedItemId
                                    + " old was " + allItems[otherItemId].deviations[toBeUpdatedItemId] + "\n");

                    allItems[toBeUpdatedItemId].deviations[otherItemId] = newDeviationOfUpdatedItemToOtherItem;
                    allItems[otherItemId].deviations[toBeUpdatedItemId] = newDeviationOfOtherItemToUpdatedItem;
                }
            }
            return allItems;
        }

        private static void computeAllDeviations(Dictionary<int, Item> allItems, Dictionary<int, User> allUsers)
        {
            foreach (var itemKV in allItems)
            {
                var firstItemId = itemKV.Key;

                foreach (var otherItemKV in allItems)
                {
                    var secondItemId = otherItemKV.Key;

                    //saving a computation
                    if (firstItemId == secondItemId)
                    {
                        itemKV.Value.deviations[secondItemId] = 0;
                    }
                    else
                    {
                        var usersThatRatedBoth = getListOfUsersThatRatedBoth(firstItemId, secondItemId, allUsers);
                        itemKV.Value.deviations[secondItemId] = computeDeviation(firstItemId, secondItemId, usersThatRatedBoth);
                    }
                }
            }
        }

        //This method and the one above only get used right at the beginning.
        private static List<User> getListOfUsersThatRatedBoth(int firstItemId, int secondItemId, Dictionary<int, User> allUsers)
        {
            var usersThatRatedBothItems = new List<User>();
            foreach (var user in allUsers.Values)
            {
                if (user.ratings.ContainsKey(firstItemId) && user.ratings.ContainsKey(secondItemId))
                    usersThatRatedBothItems.Add(user);
            }
            return usersThatRatedBothItems;
        }

        private static double computeDeviation(int firstItemId, int secondItemId, List<User> allUsersThatRatedBoth)
        {
            double deviation = 0.0;
            foreach (var user in allUsersThatRatedBoth)
            {
                deviation += (user.ratings[firstItemId] - user.ratings[secondItemId]);
            }
            return deviation / allUsersThatRatedBoth.Count;
        }



        /*
            PREDICTION COMPUTATION
        */

        private static List<Tuple<int, double>> computeMultiplePredictions(User subject, List<int> itemsIdsToBePredicted, Dictionary<int, Item> allItems)
        {
            var productRatingPredictions = new List<Tuple<int, double>>();

            foreach (var itemId in itemsIdsToBePredicted)
            {
                var item = allItems[itemId];
                var predictionForThisItem = computePrediction(subject, item, allItems);

                productRatingPredictions.Add(new Tuple<int, double>(itemId, predictionForThisItem));
            }
            return productRatingPredictions;
        }

        private static double computePrediction(User subject, Item itemToBePredicted, Dictionary<int, Item> allItems)
        {
            int totalCardinality = 0;

            double sumOfUsersRatingsAndDeviations = 0.0;
            foreach (var ratingKV in subject.ratings)
            {
                var currProductNo = ratingKV.Key;
                var currProductRating = ratingKV.Value;

                double currProductAndItemToBePredictedDeviation = itemToBePredicted.deviations[ratingKV.Key];
                int currProductCardinality = allItems[currProductNo].amountOfTimesRated;

                sumOfUsersRatingsAndDeviations += ((ratingKV.Value + currProductAndItemToBePredictedDeviation) * currProductCardinality);
                totalCardinality += currProductCardinality;
            }
            return sumOfUsersRatingsAndDeviations / totalCardinality;
        }
    }
}
