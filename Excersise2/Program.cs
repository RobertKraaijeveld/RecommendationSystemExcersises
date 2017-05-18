using System;
using System.Linq;
using System.Collections.Generic;

namespace Excersise2
{
    class Program
    {
        static void Main(string[] args)
        {
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

            var predictionTest = computePrediction(allUsers[2], allItems[102], allItems);
            Console.WriteLine("Predicted rating of user 2 for product 102: " + predictionTest);
        }

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

        private static double computeDeviation(int firstItemId, int secondItemId, List<User> allUsersThatRatedBoth)
        {
            double deviation = 0.0; 
            foreach(var user in allUsersThatRatedBoth)
            {
                deviation += (user.ratings[firstItemId] - user.ratings[secondItemId]);
            }
            return deviation / allUsersThatRatedBoth.Count;
        }

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
