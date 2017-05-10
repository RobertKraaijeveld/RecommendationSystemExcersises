using System;
using System.Collections.Generic;

namespace RecommendationSystemExcersises
{
    //why cant i just make the method signature generic?
    static class Utilities
    {

        /*
        
        Divide by choosing any element in the subarray array[p..r].
         Call this element the pivot. Rearrange the elements in array[p..r] so that all other elements in array[p..r] 
         hat are less than or equal to the pivot are to its left and all elements in array[p..r] are to the pivot's right.
         
          We call this procedure partitioning. 
          At this point, it doesn't matter what order the elements to the left of the pivot are in relative to each other, 
          and the same holds for the elements to the right of the pivot. 
          We just care that each element is somewhere on the correct side of the pivot.


        As a matter of practice, we'll always choose the rightmost element in the subarray, array[r], as the pivot. 
        So, for example, if the subarray consists of [9, 7, 5, 11, 12, 2, 14, 3, 10, 6], then we choose 6 as the pivot. 
        After partitioning, the subarray might look like [5, 2, 3, 6, 12, 7, 14, 9, 10, 11]. 
        Let q be the index of where the pivot ends up.

        Conquer by recursively sorting the subarrays array[p..q-1] (all elements to the left of the pivot, which must be less than or equal to the pivot) 
        and array[q+1..r] (all elements to the right of the pivot, which must be greater than the pivot).

        Combine by doing nothing. Once the conquer step recursively sorts, we are done. 
        Why? All elements to the left of the pivot, in array[p..q-1], are less than or equal to the pivot and are sorted, and all elements to the right of the pivot, 
        in array[q+1..r], are greater than the pivot and are sorted. The elements in array[p..r] can't help but be sorted!
                
        
         */
        public static List<int> quickSort(List<int> vals, int left, int right)
        {
            int pivot = vals[(right + left) / 2];

            while(l <= r)
            {

            }

        
            /*
            2,6,1,3,0

            pivot = 1

            left = 0
            right = 2,6,3
            */

            foreach(var val in l)
            {
                if(val <= pivot)
                    left.Add(val);
                else
                    right.Add(val);                    
            }
            left.AddRange(right);
            return left;
            
            //sort first half from 0 to pivot - 1
            //sort second half from pivot + 1 to end
            //return
        }

        public static List<int> partition(List<int> l, int pivot)
        {
            
        }


    }
}
