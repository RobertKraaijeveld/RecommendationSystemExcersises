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

        //Use option pattern to call appropriate method for an similarity measuring object
        public Dictionary<int, Vector> getRatingsVectors(User other, bool useSparseData)
        {
            var ratingsVectors = new Dictionary<int, Vector>();

            ratingsVectors[this.userId] = new Vector();
            ratingsVectors[other.userId] = new Vector();

            if (useSparseData)
                setSparseRatingsVectors(ref ratingsVectors, other);
            else
                setNormalRatingsVectors(ref ratingsVectors, other);

            return ratingsVectors;
        }

        private void setSparseRatingsVectors(ref Dictionary<int, Vector> outDict, User otherUser)
        {
            var bothUsersRatedProducts = this.ratings.Keys.ToList().Union(otherUser.ratings.Keys.ToList()).ToList();

            foreach (var productKey in bothUsersRatedProducts)
            {
                if (this.ratings.ContainsKey(productKey))
                {
                    var myRating = this.ratings[productKey];
                    outDict[this.userId].values.Add(myRating);

                    if (otherUser.ratings.ContainsKey(productKey))
                    {
                        var otherUsersRating = otherUser.ratings[productKey];
                        outDict[otherUser.userId].values.Add(otherUsersRating);
                    }
                    else
                        outDict[otherUser.userId].values.Add(0.0);

                }
                else
                {
                    outDict[this.userId].values.Add(0.0);

                    var otherUsersRating = otherUser.ratings[productKey];
                    outDict[otherUser.userId].values.Add(otherUsersRating);
                }
            }
        }

        private void setNormalRatingsVectors(ref Dictionary<int, Vector> outDict, User otherUser)
        {
            foreach(int productKey in this.ratings.Keys)
            {
                if(otherUser.ratings.ContainsKey(productKey))
                {
                    var myRating = ratings[productKey];
                    var otherUsersRating = otherUser.ratings[productKey];

                    outDict[this.userId].values.Add(myRating);
                    outDict[otherUser.userId].values.Add(otherUsersRating);
                }
            }
        }
        
    
        public Dictionary<User, double> getNearestNeighboursAndSimilarities(int maximumNeighboursAmount, List<User> allUsers, ISimilarity similarityMeasurer)
        {
            //make parameter?
            var similarityMinimum = 0.35;

            //change name
            //not sure if nice
            var neighboursAndSimilarities = new Dictionary<User, double>();

            foreach (var currentUser in allUsers)
            {
                if (currentUser.userId != this.userId)
                {
                    var ourRatingsVectors = this.getRatingsVectors(currentUser, similarityMeasurer.canHandleSparseData());
                    var myRatingVector = ourRatingsVectors[this.userId];
                    var otherUserRatingVector = ourRatingsVectors[currentUser.userId];

                    var currentSimilarity = similarityMeasurer.computeSimilarity(myRatingVector, otherUserRatingVector);

                    if(this.userId == 4)
                    {
                        Console.WriteLine("User 4s similarity with other user " + currentUser.userId + " is " + currentSimilarity + " with the minimum being " + similarityMinimum);
                    }

                    if (currentSimilarity > similarityMinimum && hasRatedDifferentProducts(currentUser))
                    {
                        if (neighboursAndSimilarities.Count <= maximumNeighboursAmount)
                        {
                            neighboursAndSimilarities.Add(currentUser, currentSimilarity);
                        }
                        else if (neighboursAndSimilarities.Count == maximumNeighboursAmount)
                        {
                            var lowestSimilarity = getLowestSimilarity(neighboursAndSimilarities);
                            var leastSimilarNeighbour = neighboursAndSimilarities.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;

                            //Ugly extra if
                            if (currentSimilarity > lowestSimilarity)
                            {
                                neighboursAndSimilarities.Remove(leastSimilarNeighbour);
                                neighboursAndSimilarities.Add(currentUser, currentSimilarity);

                                //calling getLowestSimilarity since the neighbours are now updated
                                similarityMinimum = getLowestSimilarity(neighboursAndSimilarities);
                            }
                        }
                    }

                    if (neighboursAndSimilarities.Count == maximumNeighboursAmount)
                        similarityMinimum = getLowestSimilarity(neighboursAndSimilarities);
                }
            }
            return neighboursAndSimilarities;
        }

        private bool neighbourAlreadyPresent(Dictionary<double, User> neighboursAndSimilarities, int userId)
        {
            return neighboursAndSimilarities.Values.Where(x => x.userId == userId).ToList().Count > 1;
        }

        private double getLowestSimilarity(Dictionary<User, double> similaritiesAndNeighbours)
        {
            return similaritiesAndNeighbours.Values.Min();
        }

        private bool hasRatedDifferentProducts(User otherUser)
        {
            return otherUser.ratings.Keys.ToList().Where(x => this.ratings.Keys.Contains(x) == false).ToList().Count > 1;
        }
    }
}