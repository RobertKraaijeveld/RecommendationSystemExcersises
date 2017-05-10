using System;
using System.Linq;
using System.Collections.Generic;


namespace RecommendationSystemExcersises
{
    class UserRatings
    {
        //Needs to be a double because of LINQ-esque parsing
        public Dictionary<double, double> productsAndRatings;

        public UserRatings(Dictionary<double, double> productsAndRatings)
        {
            this.productsAndRatings = productsAndRatings;
        }

        public Dictionary<double, double> getOverlappingRatings(UserRatings other)
        {
            var overlappingRatings = new Dictionary<double, double>();
            foreach(double product in this.productsAndRatings.Keys)
            {

            }
        }
    }
}
