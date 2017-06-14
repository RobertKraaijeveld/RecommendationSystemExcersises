using System;
using System.Collections.Generic;

namespace Excersise2
{
    class User  
    {   
        public int id;
        public Dictionary<int, float> ratings;

        public User(int id, Dictionary<int, float> ratings)
        {
            this.id = id;
            this.ratings = ratings;
        }
    }
}
