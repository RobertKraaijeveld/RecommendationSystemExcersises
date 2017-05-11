using System;
using System.Linq;
using System.Collections.Generic;


namespace RecommendationSystemExcersises
{
    class Vector
    {
        public readonly List<double> values = new List<double>();

        public Vector(){}
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
            return Math.Sqrt(this.values.Sum(x => Math.Pow(x, 2)));
        }

        public override string ToString()
        {
            return "(" + string.Join(", ", values) + ")";
        }
    }
}
