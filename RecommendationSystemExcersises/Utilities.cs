using System;
using System.Collections.Generic;

namespace RecommendationSystemExcersises
{
    //why cant i just make the method signature generic?
    static class Utilities<int>
    {
        public static List<int> quickSort(List<int> l)
        {
            Random rand = new Random();
            int pivot = rand.Next(0, l.Count);

            return partition(l, pivot);
        }

        public static List<int> partition(List<int> l, int pivot)
        {
            var left = new List<int>();
            var right = new List<int>();
        
            foreach(var val in l)
            {
                if(val <= pivot)
                    left.Add(val);
                else
                    right.Add(val);                    
            }
            left.AddRange(right);
            return left;
        }


    }
}
