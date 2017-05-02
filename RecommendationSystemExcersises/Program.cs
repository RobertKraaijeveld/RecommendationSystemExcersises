using System;
using System.Collections.Generic;

namespace RecommendationSystemExcersises
{
    class Program
    {
        static void Main(string[] args)
        {
            Vector v = new Vector(new List<double>(){4.75, 4.5, 5, 4.25, 4});
            Vector k = new Vector(new List<double>(){4, 3, 5, 2 ,1});
            
            Console.WriteLine("Sample vector 1: " + v.ToString());
            Console.WriteLine("Sample vector 2: " + k.ToString());         

            IDistance distanceMeasurer = new EuclideanDistance();
            Console.WriteLine("Euclidean distance between v1 and v2: " + distanceMeasurer.computeDistance(v,k));

            distanceMeasurer = new ManhattanDistance();
            Console.WriteLine("Manhattan distance between v1 and v2: " + distanceMeasurer.computeDistance(v,k));

            ISimilarity similarityMeasurer = new PearsonSimilarity();
            Console.WriteLine("Pearson similarity between v1 and v2: " + similarityMeasurer.computeSimilarity(v,k));  

            similarityMeasurer = new CosineSimilarity();
            Console.WriteLine("Cosine similarity between v1 and v2: " + similarityMeasurer.computeSimilarity(v,k));  
                             
        }
    }
}
