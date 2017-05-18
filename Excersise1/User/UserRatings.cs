using System;
using System.Linq;
using System.Collections.Generic;


namespace Excersise1
{
    class UserRatings
    {
        public Dictionary<int, double> productsAndRatings = new Dictionary<int, double>();

        public UserRatings(Dictionary<int, double> productsAndRatings)
        {
            this.productsAndRatings = productsAndRatings;
        }
        public UserRatings(){}

        //make N-dimensional?
        public Tuple<UserRatings, UserRatings> getOverlappingRatings(UserRatings other)
        {
            var myRatings = new UserRatings();
            var otherUsersRatings = new UserRatings();

            foreach(int productKey in this.productsAndRatings.Keys)
            {
                if(other.productsAndRatings.ContainsKey(productKey))
                {
                    myRatings.productsAndRatings.Add(productKey, this.productsAndRatings[productKey]);
                    otherUsersRatings.productsAndRatings.Add(productKey, other.productsAndRatings[productKey]);
                }
            }
            return new Tuple<UserRatings, UserRatings>(myRatings, otherUsersRatings);
        }

        public void addRating(int productKey, double rating)
        {
            productsAndRatings[productKey] = rating;
        }

        public Vector ratingsToVector()
        {
            return new Vector(this.productsAndRatings.Values.ToList());
        }
    }
}
