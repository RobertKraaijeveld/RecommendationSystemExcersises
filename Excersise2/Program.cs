using System;
using System.Linq;
using System.Collections.Generic;

namespace Excersise2
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Compute devitations between all items;
            
            - AB needs to be computed, BA is just the inverse of AB.
            -Meaning that if A is already in the dictionary of deviations, Bs deviation to A is simply As deviation to B * -1.

            THIS CAN ALL BE CACHED! Only needs to be updated when new items are added, and even then you only need to do the new one and all the other ones (1 * N) instead of N^2.
            */

            //Increment usersCounter per item when we come across a rating of it during parsing.
            //This is correct since an items cardinality is just the amount of users that rated an item.
            var parser = new Parser("docs/ratings.csv", new char[1]{','});
            var allUsers = parser.getParsedUsers();
            var allItems = parser.getParsedItemsWithAmountOfTimesRated();
            
            computeAllDeviations(ref allItems, allUsers.Values.ToList());
            foreach (var item in allItems)
            {
                var currItemId = item.Key;
                foreach (var otherItem in item.Value.deviations)
                {
                    Console.WriteLine("Item " + currItemId + " has deviation of " + otherItem.Value + " with other item " + otherItem.Key);
                }
            }
        }

        //Works:)       
        private static void computeAllDeviations(ref Dictionary<int, Item> allItems, List<User> allUsers)
        {
            foreach (var itemKV in allItems)
            {
                var firstItemId = itemKV.Key;
	
                //WHAT TO DO WHEN THERES NO USERS THAT RATED BOTH?
                foreach (var otherItemKV in allItems)
                {
                    var secondItemId = otherItemKV.Key;
                    if(firstItemId == secondItemId)
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

        private static List<User> getListOfUsersThatRatedBoth(int firstItemId, int secondItemId, List<User> allUsers)
        {
            var usersThatRatedBothItems = new List<User>();
            foreach (var user in allUsers)
            {
                if(user.ratings.ContainsKey(firstItemId) && user.ratings.ContainsKey(secondItemId))
                    usersThatRatedBothItems.Add(user);
            }
            return usersThatRatedBothItems;
        }

        //Check if rated both 
        //Check if can be inverted
        private static double computeDeviation(int firstItemId, int secondItemId, List<User> allUsersThatRatedBoth)
        {
            double deviation = 0.0; 
            foreach(var user in allUsersThatRatedBoth)
            {
                deviation += (user.ratings[firstItemId] - user.ratings[secondItemId]);
            }
            return deviation / allUsersThatRatedBoth.Count;
        }

        private static void updateDeviation(ref Dictionary<int, Item> deviations, int productId, double newRating)
        {}

        private static double computePrediction(User subject, Item itemToBePredicted, Dictionary<int, Item> allItems)
        {
            int totalCardinality = 0;            

            double sumOfUsersRatingsAndDeviations = 0.0;
            foreach(var ratingKV in subject.ratings)
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
