using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace RecommendationSystemExcersises
{
    class Parser
    {
        public static Dictionary<double, UserRatings> parseUserRatings()
        {
            var allUsersRatings = new Dictionary<double, UserRatings>();

            var valuesMatrix = File.ReadLines("userItem.data")
                               .Select(x => x.Split(',')
                                      .Select(Double.Parse)
                                      .ToList())
                               .ToList();

            foreach(var line in valuesMatrix)
            {
                //implement option pattern for this type of stuff?
                double userId = line[0];
                double articleNo = line[1];
                double givenRating = line[2];                

                //if results already contains this userId, add articleNo and rating to users existing dict
                if(allUsersRatings.ContainsKey(userId))
                    allUsersRatings[userId].productsAndRatings.Add(articleNo, givenRating);
                else
                {
                    var productsAndRatingsForThisUser = new Dictionary<double, double>();
                    productsAndRatingsForThisUser.Add(articleNo, givenRating);
                    allUsersRatings.Add(userId, new UserRatings(productsAndRatingsForThisUser));
                }
            }
            return allUsersRatings;
        }

        public static 
    }
}
