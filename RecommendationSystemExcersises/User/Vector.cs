using System;
using System.Linq;
using System.Collections.Generic;


namespace RecommendationSystemExcersises
{
    class Vector
    {
        public readonly List<double> values;

        public Vector(List<double> values)
        {
            this.values = values;
        }

        public double dotProduct(Vector other)
        {
            double returnValue = 0.0f;

            for(int i = 0; i < this.values.Count; i++)
            {
                if(i <= (other.values.Count - 1))
                    returnValue += this.values[i] * other.values[i];
            }
            return returnValue;
        }

        public double getLength() 
        {
            double squaredVectorValues = 0.0f;
            this.values.ForEach(val => squaredVectorValues += Math.Pow(val,2));

            return Math.Sqrt(squaredVectorValues);
        }

        public override string ToString()
        {
            return string.Join(", ", values);
        }
    }
}
