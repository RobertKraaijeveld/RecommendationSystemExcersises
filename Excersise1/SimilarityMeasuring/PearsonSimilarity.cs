using System;
using System.Linq;
using System.Collections.Generic;

namespace Excersise1
{
    class PearsonSimilarity : ISimilarity
    {
        public bool canHandleSparseData() { return false; }

        public double computeSimilarity(Vector x, Vector y)
        {
            var xyMultiplication = x.dotProduct(y);
            var sumsDividedByDimension = (x.values.Sum() * y.values.Sum()) / x.values.Count; 

            var xValuesSquared = x.values.Select((xValue) => Math.Pow(xValue, 2)).ToList().Sum();
            var yValuesSquared = y.values.Select((yValue) => Math.Pow(yValue, 2)).ToList().Sum();

            //make dimension a constant since it has to be the same
            var xValuesSumSquaredDividedByDimension = Math.Pow(x.values.Sum(), 2) / x.values.Count;
            var yValuesSumSquaredDividedByDimension = Math.Pow(y.values.Sum(), 2) / y.values.Count;

            var dividend = xyMultiplication - sumsDividedByDimension;
            var divisorFirstHalf = Math.Sqrt(xValuesSquared - xValuesSumSquaredDividedByDimension);
            var divisorSecondHalf = Math.Sqrt(yValuesSquared - yValuesSumSquaredDividedByDimension);

            return (dividend / (divisorFirstHalf * divisorSecondHalf));
        }
    }
}