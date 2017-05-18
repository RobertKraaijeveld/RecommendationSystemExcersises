using System;

namespace Excersise1
{
    interface ISimilarity
    {
        //Dirty
        bool canHandleSparseData();
        double computeSimilarity(Vector x, Vector y);
    }
}