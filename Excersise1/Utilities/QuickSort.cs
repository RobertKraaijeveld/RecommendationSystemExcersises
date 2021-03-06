using System;
using System.Linq;
using System.Collections.Generic;

namespace Excersise1
{
    static class QuickSort
    {
        public static Dictionary<int, double> quickSortDict(Dictionary<int, double> dict)
        {
            var orderedDict = new Dictionary<int, double>();

            var dictKeys = dict.Keys.ToList();
            quickSort(dictKeys, 0, dictKeys.Count-1);

            foreach(var orderedKey in dictKeys)
            {
                var associatedValue = dict[orderedKey];
                orderedDict.Add(orderedKey, associatedValue);
            }
            return orderedDict;
        }

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

        private static int sort(List<int> vals, int leftIndex, int  rightIndex)
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
            //(should all elements be smaller than the pivot, lingerer will be equal to rightindex so nothing will happen.)
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
