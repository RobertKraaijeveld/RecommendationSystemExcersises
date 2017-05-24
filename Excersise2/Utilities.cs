using System;
using System.Collections.Generic;

namespace Excersise2
{
    static class Utilities
    {   
        public static Dictionary<int, HashSet<int>> getUsersWhoRatedItems(Dictionary<int, User> allUsers, Dictionary<int, Item> allItems)
        {
            var usersThatRatedPerItem = new Dictionary<int, HashSet<int>>();
            foreach (var itemKV in allItems)
            {
                foreach (var userKV in allUsers)
                {
                    if(userKV.Value.ratings.ContainsKey(itemKV.Key))
                    {
                        if(usersThatRatedPerItem.ContainsKey(itemKV.Key) == false)
                            usersThatRatedPerItem.Add(itemKV.Key, new HashSet<int>());

                        usersThatRatedPerItem[itemKV.Key].Add(userKV.Key);
                    }
                }
            }
            return usersThatRatedPerItem;
        }
    }
}