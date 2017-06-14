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
        static Dictionary<int, User> allUsers;
        static Dictionary<int, Item> allItems;
        static Dictionary<int, HashSet<int>> usersWhoRatedItem;

        static void Main(string[] args)
        {
            var parser = new Parser("docs/userItem.data", new char[1] { ',' });

            allUsers = parser.getParsedUsers();
            allItems = parser.getParsedItems();
            usersWhoRatedItem = Utilities.getUsersWhoRatedItems(allUsers, allItems);

            computeAllDeviations();


            var user7 = allUsers[7];
            var user7sPredictions = computeMultiplePredictions(user7, new List<int>() { 101, 103, 106 });

            var user3 = allUsers[3];
            var user3sPredictions = computeMultiplePredictions(user3, new List<int>() { 103, 105 });


            Console.WriteLine("User 7 predictions: ");
            user7sPredictions.ForEach(kv => Console.WriteLine("Item no. " + kv.Item1 + " predicted rating: " + kv.Item2));

            Console.WriteLine("");
            Console.WriteLine("User 3 predictions: ");
            user3sPredictions.ForEach(kv => Console.WriteLine("Item no. " + kv.Item1 + " predicted rating: " + kv.Item2));


            Console.WriteLine("");
            Console.WriteLine("Updating users 3 rating for item 105 to 4.0...");
            setRating(3, 105, 4.0f);
            allItems = updateDeviations(105);

            Console.WriteLine("");
            Console.WriteLine("NEW User 7 predictions: ");
            user7sPredictions = computeMultiplePredictions(user7, new List<int>() { 101, 103, 106 });
            user7sPredictions.ForEach(kv => Console.WriteLine("Item no. " + kv.Item1 + " predicted rating: " + kv.Item2));


            Console.WriteLine("");
            Console.WriteLine("IMPORTING MOVIE100K DATA");
            Console.WriteLine("");



            parser = new Parser("docs/u.data", new char[1] { '	' });

            allUsers = parser.getParsedUsers();
            allItems = parser.getParsedItems();
            usersWhoRatedItem = Utilities.getUsersWhoRatedItems(allUsers, allItems);


            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            computeAllDeviations();

            stopWatch.Stop();
            Console.WriteLine("Deviation creation time: " + stopWatch.ElapsedMilliseconds / 1000 + "s");


            var user186 = allUsers[186];
            var user186sPrediction = computePrediction(user186, 1599);
            Console.WriteLine("user186sPrediction = " + user186sPrediction);

            var user186TopPredictions = computeMultiplePredictions(user186, allItems.Keys.ToList());

            Console.WriteLine("");
            Console.WriteLine("NEW User 186 predictions: ");

            user186TopPredictions
                        .OrderByDescending(x => x.Item2)
                        .Take(5)
                        .ToList()
                        .ForEach(kv => Console.WriteLine("Item no. " + kv.Item1 + " predicted rating: " + kv.Item2));
        }



        /*
            DEVIATION COMPUTATION
        */
        private static void setRating(int userId, int itemId, float newRating)
        {
            allUsers[userId].ratings[itemId] = newRating;
            usersWhoRatedItem[itemId].Add(userId);
        }

        private static Dictionary<int, Item> updateDeviations(int toBeUpdatedItemId)
        {
            var itemObjectForId = allItems[toBeUpdatedItemId];

            //computing the new deviations of toBeUpdatedItemId to all other items and vice versa            
            foreach (var otherItemDeviationKV in itemObjectForId.deviations.ToList())
            {
                var otherItemId = otherItemDeviationKV.Key;

                if (otherItemId != toBeUpdatedItemId)
                {
                    var otherItem = itemObjectForId;

                    var usersWhoRatedBoth = getListOfUsersThatRatedBoth(toBeUpdatedItemId, otherItemId);

                    var newDeviationOfUpdatedItemToOtherItem = computeDeviation(toBeUpdatedItemId, otherItemId, usersWhoRatedBoth);
                    var newDeviationOfOtherItemToUpdatedItem = computeDeviation(otherItemId, toBeUpdatedItemId, usersWhoRatedBoth);

                    allItems[toBeUpdatedItemId].deviations[otherItemId] = newDeviationOfUpdatedItemToOtherItem;
                    allItems[otherItemId].deviations[toBeUpdatedItemId] = newDeviationOfOtherItemToUpdatedItem;
                }
            }
            return allItems;
        }


        private static void computeAllDeviations()
        {
            foreach (var itemKV in allItems)
            {
                var firstItemId = itemKV.Key;

                foreach (var otherItemKV in allItems)
                {
                    var secondItemId = otherItemKV.Key;

                    var allUsersThatRatedBoth = getListOfUsersThatRatedBoth(firstItemId, secondItemId);
                    setCardinality(firstItemId, secondItemId, allUsersThatRatedBoth);

                    if (firstItemId == secondItemId)
                    {
                        itemKV.Value.deviations[firstItemId] = 0;
                        itemKV.Value.deviations[secondItemId] = 0;
                    }
                    else if (otherItemKV.Value.deviations.ContainsKey(firstItemId))
                    {
                        var otherItemsDeviationWithCurrentItem = otherItemKV.Value.deviations[firstItemId];
                        itemKV.Value.deviations[secondItemId] = -otherItemsDeviationWithCurrentItem;
                    }
                    else
                    {
                        itemKV.Value.deviations[secondItemId] = computeDeviation(firstItemId, secondItemId, allUsersThatRatedBoth);
                    }
                }
            }
        }

        private static List<User> getListOfUsersThatRatedBoth(int firstItemId, int secondItemId)
        {
            var usersThatRatedBothItems = new List<User>();

            var usersWhoRatedFirstItem = new HashSet<int>(usersWhoRatedItem[firstItemId]);
            var usersWhoRatedSecondItem = new HashSet<int>(usersWhoRatedItem[secondItemId]);

            usersWhoRatedFirstItem.IntersectWith(usersWhoRatedSecondItem);

            var usersWhoRatedBothIds = usersWhoRatedFirstItem.ToList();
            for (int i = 0; i < usersWhoRatedBothIds.Count; i++)
            {
                usersThatRatedBothItems.Add(allUsers[usersWhoRatedBothIds[i]]);
            }
            return usersThatRatedBothItems;
        }


        private static void setCardinality(int firstItemId, int secondItemId, List<User> allUsersThatRatedBoth)
        {
            allItems[firstItemId].cardinalities[secondItemId] = allUsersThatRatedBoth.Count;
            allItems[secondItemId].cardinalities[firstItemId] = allUsersThatRatedBoth.Count;
        }

        private static float computeDeviation(int firstItemId, int secondItemId, List<User> allUsersThatRatedBoth)
        {
            float deviation = 0.0f;

            foreach (var user in allUsersThatRatedBoth)
            {
                deviation += (user.ratings[firstItemId] - user.ratings[secondItemId]);
            }
            float finalDeviation;


            if (allUsersThatRatedBoth.Count > 0)
            {
                finalDeviation = deviation / allUsersThatRatedBoth.Count;
            }
            else
                finalDeviation = deviation / 1;

            return finalDeviation;
        }



        /*
            PREDICTION COMPUTATION
        */

        private static List<Tuple<int, float>> computeMultiplePredictions(User subject, List<int> itemsIdsToBePredicted)
        {
            var productRatingPredictions = new List<Tuple<int, float>>();

            foreach (var itemId in itemsIdsToBePredicted)
            {
                var item = allItems[itemId];
                var predictionForThisItem = computePrediction(subject, itemId);

                productRatingPredictions.Add(new Tuple<int, float>(itemId, predictionForThisItem));
            }
            return productRatingPredictions;
        }

        private static float computePrediction(User subject, int itemToBePredictedId)
        {
            int totalCardinality = 0;

            float sumOfUsersRatingsAndDeviations = 0.0f;
            foreach (var ratingKV in subject.ratings)
            {
                var currProductNo = ratingKV.Key;
                var currProductRating = ratingKV.Value;

                float currProductAndItemToBePredictedDeviation = allItems[itemToBePredictedId].deviations[currProductNo];
                int currProductCardinality = allItems[itemToBePredictedId].cardinalities[currProductNo];

                sumOfUsersRatingsAndDeviations += ((currProductRating + currProductAndItemToBePredictedDeviation) * currProductCardinality);

                totalCardinality += currProductCardinality;
            }
            return sumOfUsersRatingsAndDeviations / totalCardinality;
        }
    }
}
