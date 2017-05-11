using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace RecommendationSystemExcersises
{
    class Parser
    {
        public static Dictionary<int, User> parseUserItems(string fileName, char delimiter)
        {
            var allUsers = new Dictionary<int, User>();

            var valuesMatrix = File.ReadLines(fileName)
                               .Select(x => x.Split(delimiter)
                                      .Select(Double.Parse)
                                      .ToList())
                               .ToList();

            foreach (var lineValues in valuesMatrix)
            {
                updateUsersDict(lineValues, allUsers);
            }
            return allUsers;
        }

        private static Dictionary<int, User> updateUsersDict(List<double> values, Dictionary<int, User> allUsersSoFar)
        {
            int userId = (int)values[0];
            int articleNo = (int)values[1];
            double givenRating = values[2];

            //if results already contains this userId, add articleNo and rating to users existing ratings
            if (allUsersSoFar.ContainsKey(userId))
            {
                allUsersSoFar[userId].ratings.Add(articleNo, givenRating);
            }
            else
            {
                var ratingsDict = new Dictionary<int, double>() { { articleNo, givenRating } };
                allUsersSoFar.Add(userId, new User(userId, ratingsDict));
            }
            return allUsersSoFar;
        }
    }
}
