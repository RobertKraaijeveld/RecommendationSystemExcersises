using System;
using System.Linq;
using System.Collections.Generic;

namespace Excersise1
{
    static class Utilities
    {
        public static Dictionary<double, User> getRatingsForSpecificProduct(int productKey, Dictionary<double, User> similaritiesAndNeighbours)
        {
            var filteredDict = new Dictionary<double, User>();
            foreach(var kvPair in similaritiesAndNeighbours)
            {
                if(kvPair.Value.ratings.ContainsKey(productKey))
                    filteredDict.Add(kvPair.Key, kvPair.Value);
            }
            return filteredDict;
        }
    }
}
