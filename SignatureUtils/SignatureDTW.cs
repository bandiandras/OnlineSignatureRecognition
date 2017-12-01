using AbstractionLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureDTW
{
    public static class SignatureDTW
    {
        /// <summary>
        /// Compare two signatures using DTW algorithm
        /// </summary>
        /// <param name="aSig1"></param>
        /// <param name="aSig2"></param>
        /// <returns></returns>
        public static double DTWDistance<T>(T aSig1, T aSig2, int aType = 0) where T : List<Point>
        {
            double[,] lDTW = new double[aSig1.Count, aSig2.Count];

            for (int i = 1; i < aSig1.Count; ++i)
            {
                lDTW[i, 0] = int.MaxValue;
            }

            for (int i = 1; i < aSig2.Count; ++i)
            { 
                lDTW[0, i] = int.MaxValue;
            }

            lDTW[0, 0] = 0;

            switch (aType)
            {
                //use only X, Y
                case 0:
                    {
                        for (int i = 1; i < aSig1.Count; ++i)
                        {
                            for (int j = 1; j < aSig2.Count; ++j)
                            {
                                double lCost = GetDistance(aSig1.ElementAt(i), aSig2.ElementAt(j));
                                lDTW[i, j] = lCost + Min(lDTW[i - 1, j], lDTW[i, j - 1], lDTW[i - 1, j - 1]);
                            }
                        }
                    }
                    break;
                //use only X1, Y1
                case 1:
                    {
                        for (int i = 1; i < aSig1.Count; ++i)
                        {
                            for (int j = 1; j < aSig2.Count; ++j)
                            {
                                double lCost = GetDistanceFOD(aSig1.ElementAt(i), aSig2.ElementAt(j));
                                lDTW[i, j] = lCost + Min(lDTW[i - 1, j], lDTW[i, j - 1], lDTW[i - 1, j - 1]);
                            }
                        }
                    }
                    break;
                //use only X2, Y2
                case 2:
                    {
                        for (int i = 1; i < aSig1.Count; ++i)
                        {
                            for (int j = 1; j < aSig2.Count; ++j)
                            {
                                double lCost = GetDistanceSOD(aSig1.ElementAt(i), aSig2.ElementAt(j));
                                lDTW[i, j] = lCost + Min(lDTW[i - 1, j], lDTW[i, j - 1], lDTW[i - 1, j - 1]);
                            }
                        }
                    }
                    break;
                //use X, Y, X1, Y1, X2, Y2, FORCE, FORCE1, PATHVELOCITY, THETA
                case 3:
                    for (int i = 1; i < aSig1.Count; ++i)
                    {
                        for (int j = 1; j < aSig2.Count; ++j)
                        {
                            double lCost = GetDistanceAll(aSig1.ElementAt(i), aSig2.ElementAt(j));
                            lDTW[i, j] = lCost + Min(lDTW[i - 1, j], lDTW[i, j - 1], lDTW[i - 1, j - 1]);
                        }
                    }
                    break;
                //use X, Y, X1, Y1, X2, Y2
                case 4:
                    for (int i = 1; i < aSig1.Count; ++i)
                    {
                        for (int j = 1; j < aSig2.Count; ++j)
                        {
                            double lCost = GetDistanceCoordinatesAndDifferences(aSig1.ElementAt(i), aSig2.ElementAt(j));
                            lDTW[i, j] = lCost + Min(lDTW[i - 1, j], lDTW[i, j - 1], lDTW[i - 1, j - 1]);
                        }
                    }
                    break;
                //use X, Y, X1, Y1, X2, Y2, FORCE
                case 5:
                    for (int i = 1; i < aSig1.Count; ++i)
                    {
                        for (int j = 1; j < aSig2.Count; ++j)
                        {
                            double lCost = GetDistance5(aSig1.ElementAt(i), aSig2.ElementAt(j));
                            lDTW[i, j] = lCost + Min(lDTW[i - 1, j], lDTW[i, j - 1], lDTW[i - 1, j - 1]);
                        }
                    }
                    break;
            }
            return lDTW[aSig1.Count - 1, aSig2.Count - 1] / (aSig1.Count + aSig2.Count);
            //return lDTW[aSig1.Count-1, aSig2.Count-1];
        }

        /// <summary>
        /// Returns the min from 3 values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        private static double Min(double x, double y, double z)
        {
            return Math.Min(x, Math.Min(y, z));
        }

        /// <summary>
        /// Calculates the distance between 2 two-dimensional points 
        /// </summary>
        /// <param name="aPoint1"></param>
        /// <param name="aPoint2"></param>
        /// <returns></returns>
        private static double GetDistance(Point aPoint1, Point aPoint2)
        { 
            return Math.Sqrt(Math.Pow((aPoint2.X - aPoint1.X), 2) + Math.Pow((aPoint2.Y - aPoint1.Y), 2));
        }

        private static double GetDistanceFOD(Point aPoint1, Point aPoint2)
        {
            return Math.Sqrt(Math.Pow((aPoint2.X1 - aPoint1.X1), 2) + Math.Pow((aPoint2.Y1 - aPoint1.Y1), 2));
        }

        private static double GetDistanceSOD(Point aPoint1, Point aPoint2)
        {
            return Math.Sqrt(Math.Pow((aPoint2.X2 - aPoint1.X2), 2) + Math.Pow((aPoint2.Y2 - aPoint1.Y2), 2));
        }

        private static double GetDistanceAll(Point aPoint1, Point aPoint2)
        {
            return Math.Sqrt(Math.Pow((aPoint2.X - aPoint1.X), 2) 
                + Math.Pow((aPoint2.Y - aPoint1.Y), 2) 
                + Math.Pow((aPoint2.X1 - aPoint1.X1), 2)
                + Math.Pow((aPoint2.Y1 - aPoint1.Y1), 2) 
                + Math.Pow((aPoint2.X2 - aPoint1.X2), 2) 
                + Math.Pow((aPoint2.Y2 - aPoint1.Y2), 2)
                + Math.Pow((aPoint2.Force - aPoint1.Force), 2) 
                + Math.Pow((aPoint2.Force1 - aPoint1.Force1), 2)
                + Math.Pow((aPoint2.Theta - aPoint1.Theta), 2)
                + Math.Pow((aPoint2.PathVelocity - aPoint1.PathVelocity), 2));
        }

        private static double GetDistanceCoordinatesAndDifferences(Point aPoint1, Point aPoint2)
        {
            return Math.Sqrt(Math.Pow((aPoint2.X - aPoint1.X), 2)
                + Math.Pow((aPoint2.Y - aPoint1.Y), 2)
                + Math.Pow((aPoint2.X1 - aPoint1.X1), 2)
                + Math.Pow((aPoint2.Y1 - aPoint1.Y1), 2)
                + Math.Pow((aPoint2.X2 - aPoint1.X2), 2)
                + Math.Pow((aPoint2.Y2 - aPoint1.Y2), 2));
        }

        private static double GetDistance5(Point aPoint1, Point aPoint2)
        {
            return Math.Sqrt(Math.Pow((aPoint2.X - aPoint1.X), 2)
                + Math.Pow((aPoint2.Y - aPoint1.Y), 2)
                + Math.Pow((aPoint2.X1 - aPoint1.X1), 2)
                + Math.Pow((aPoint2.Y1 - aPoint1.Y1), 2)
                + Math.Pow((aPoint2.X2 - aPoint1.X2), 2)
                + Math.Pow((aPoint2.Y2 - aPoint1.Y2), 2)
                + Math.Pow((aPoint2.Force - aPoint1.Force), 2));
        }
    }
}
