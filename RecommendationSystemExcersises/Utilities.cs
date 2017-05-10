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
                

        QUICKSORT (A, p, r)
        if p < r
            q = PARTITION(A, p, r)
            QUICKSORT(A, p, q-1)
            QUICKSORT(A, q+1, r)
         */

         //{5,2,PIVOT,3,1}
        public static void quickSort(List<int> vals, int leftIndex, int rightIndex)
        {
            //if this is false, list is 0 or 1 values so there is no sorting to be done.
            if(leftIndex < rightIndex)
            {
                var newPivotLoc = sort(vals, leftIndex, rightIndex);
                quickSort(vals, leftIndex, newPivotLoc - 1);
                quickSort(vals, newPivotLoc + 1, rightIndex);                
            }   
        }

        /*
        {1,5,3,4}

        pivot = 4;
        lingerer = 0;

        1 <= 4 = true
            swap 1 and 1.
            lingerer = 1;
        --
        {1,5,3,4}
        5 <= 4 == false!
        --
        lingerer = 1;
        currIndex = 2;
        3 <= 4 = true
            swap 3 and 5
            lingerer = 2;
        --
        {1,3,5,4}
        curr = 5
        lingerer val = 3
        5 <= 4 = false
        loop done.
        swap vals[lingerer] which is 5, and the pivot.
        
        RESULT: {1,3,4,5}
        */

        private static int sort(List<int> vals, int leftIndex, int rightIndex)
        {
            var pivot = vals[rightIndex];
            var lingerer = leftIndex;

            //rightmost index is pivot, dont want to compare pivot to pivot:)
            for(int currIndex = leftIndex; currIndex < rightIndex; currIndex++)
            {
                if(vals[currIndex ] <= pivot)
                {
                    //swap the current element, which is smaller than pivot with the lingerer
                    swap(vals, lingerer, currIndex);
                    //lingerer only gets incremented on smaller values!  
                    lingerer++;                    
                }
            }
            //swap lingerer element which apparently is larger than pivot with pivot.
            swap(vals, lingerer, rightIndex);
            
            return lingerer;
        }

        private static void swap(List<int> vals, int indexA, int indexB)
        {
            var tempA = vals[indexA];
            vals[indexA] = vals[indexB];
            vals[indexB] = tempA;
        }
    }
}
