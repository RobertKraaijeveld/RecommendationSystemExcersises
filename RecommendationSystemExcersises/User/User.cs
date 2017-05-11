using System;
using System.Linq;
using System.Collections.Generic;


namespace RecommendationSystemExcersises
{
    class User
    {
        public readonly int userId;
        public Dictionary<int, double> ratings;

        public User(int userId, Dictionary<int, double> ratings)
        {
            this.userId = userId;
            this.ratings = ratings;
        }

        public Dictionary<int, Vector> getOverlappingRatingsVectors(User other)
        {
            var overlappingRatings = new Dictionary<int, Vector>();

            overlappingRatings[userId] = new Vector();
            overlappingRatings[other.userId] = new Vector();
            
            foreach(int productKey in this.ratings.Keys)
            {
                if(other.ratings.ContainsKey(productKey))
                {
                    var myRating = ratings[productKey];
                    var otherUsersRating = other.ratings[productKey];

                    overlappingRatings[userId].values.Add(myRating);
                    overlappingRatings[other.userId].values.Add(otherUsersRating);
                    
                }
            }
            return overlappingRatings;
        }


        //TODO: CHANGE THESE NAMES
        public Dictionary<double, User> getNearestNeighboursAndSimilarities(int maximumNeighboursAmount, List<User> allUsers, ISimilarity similarityMeasurer)
        {
            //make parameter?
            var similarityMinimum = 0.35;

            //change name
            //not sure if nice
            var neighboursAndSimilarities = new Dictionary<double, User>();

            foreach(var currentUser in allUsers)
            {
                if(currentUser.userId != this.userId)
                {
                    var ourOverlappingRatingsVectors = this.getOverlappingRatingsVectors(currentUser);
                    var myRatingVector = ourOverlappingRatingsVectors[this.userId];
                    var otherUserRatingVector = ourOverlappingRatingsVectors[currentUser.userId];                    

                    var currentSimilarity = similarityMeasurer.computeSimilarity(myRatingVector, otherUserRatingVector);

                    if(currentSimilarity > similarityMinimum && hasRatedDifferentProducts(currentUser))
                    {
                        if(neighboursAndSimilarities.Count < maximumNeighboursAmount)
                        {
                            neighboursAndSimilarities.Add(currentSimilarity, currentUser);
                        }
                        else if(neighboursAndSimilarities.Count == maximumNeighboursAmount)
                        {
                            var lowestSimilarity = getLowestSimilarity(neighboursAndSimilarities);
                            var leastSimilarNeighbour = neighboursAndSimilarities[lowestSimilarity];

                            //Ugly extra if
                            if(currentSimilarity > lowestSimilarity)
                            {
                                neighboursAndSimilarities.Remove(lowestSimilarity);
                                neighboursAndSimilarities.Add(currentSimilarity, currentUser);

                                //calling getLowestSimilarity since the neighbours are now updated
                                similarityMinimum = getLowestSimilarity(neighboursAndSimilarities);
                            }
                        }
                    }

                    if(neighboursAndSimilarities.Count == maximumNeighboursAmount)
                        similarityMinimum = getLowestSimilarity(neighboursAndSimilarities);
                }
            }
            return neighboursAndSimilarities;
        }

        private double getLowestSimilarity(Dictionary<double, User> similaritiesAndNeighbours)
        {
            return similaritiesAndNeighbours.Keys.Min();
        }

        private bool hasRatedDifferentProducts(User otherUser)
        {
            return otherUser.ratings.Keys.ToList().Where(x => this.ratings.Keys.Contains(x) == false).ToList().Count > 1;
        }
    }
}