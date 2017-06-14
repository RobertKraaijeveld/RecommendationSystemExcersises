using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Excersise2
{
    class Parser
    {
        string fileName;
        char[] delimiters;
        List<List<float>> allValuesMatrix;


        public Parser(string fileName, char[] delimiters)
        {
            this.fileName = fileName;
            this.delimiters = delimiters;

            //better to make this controllable by callee?
            allValuesMatrix = parseAllValuesToMatrix();
        }

        private List<List<float>>  parseAllValuesToMatrix()
        {
            var valuesMatrix = File.ReadLines(this.fileName)
                               //.Skip(1)
                               .Select(x => x.Split(this.delimiters)
                                      .Select(float.Parse)
                                      .ToList())
                               .ToList();
            return valuesMatrix;
        }

        public Dictionary<int, User> getParsedUsers()
        {
            var allUsers = new Dictionary<int, User>();

            foreach (var lineValues in allValuesMatrix)
            {
                int userId = (int) lineValues[0];
                int articleNo = (int) lineValues[1];
                float givenRating = lineValues[2];

                if (allUsers.ContainsKey(userId))
                {
                    allUsers[userId].ratings.Add(articleNo, givenRating);
                }
                else
                {
                    var ratingsDict = new Dictionary<int, float>() { { articleNo, givenRating } };
                    allUsers.Add(userId, new User(userId, ratingsDict));
                }
            }
            return allUsers;
        }

        public Dictionary<int, Item> getParsedItems()
        {
            var allItems = new Dictionary<int, Item>();

            foreach (var lineValues in allValuesMatrix)
            {
                //duplication with getParsedUsers
                int articleNo = (int) lineValues[1];

                if(!allItems.ContainsKey(articleNo))
                {
                    allItems[articleNo] = new Item();
                }
            }
            return allItems;
        }
    }
}
