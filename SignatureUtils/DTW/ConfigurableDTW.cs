using AbstractionLayer;
using SignatureUtils.DTW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureUtils
{
    public class ConfigurableDTW
    {
        public ConfigurableDTW()
        {

        }

        public static double DTWDistance<T>(T aSig1, T aSig2, DTWConfiguration aConfiguration) where T : List<Point>
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

            for (int i = 1; i < aSig1.Count; ++i)
            {
                for (int j = 1; j < aSig2.Count; ++j)
                {
                    double lCost = DistanceCalculator.CalculateDistance(aSig1.ElementAt(i), aSig2.ElementAt(j), aConfiguration.GetConfiguration());
                    lDTW[i, j] = lCost + DistanceCalculator.Min(lDTW[i - 1, j], lDTW[i, j - 1], lDTW[i - 1, j - 1]);
                }
            }

            return lDTW[aSig1.Count - 1, aSig2.Count - 1] / (aSig1.Count + aSig2.Count);
        }
    }
}
