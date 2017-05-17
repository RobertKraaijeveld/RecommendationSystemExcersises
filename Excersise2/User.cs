using System;
using System.Collections.Generic;

namespace Excersise2
{
    class User  
    {   
        public int id;
        public Dictionary<int, double> ratings;

        public User(int id, Dictionary<int, double> ratings)
        {
            this.id = id;
            this.ratings = ratings;
        }
    }
}
