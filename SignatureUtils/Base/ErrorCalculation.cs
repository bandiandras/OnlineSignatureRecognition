using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureUtils.Base
{
    public class ErrorCalculation
    {
        private double mFAR = 1.00;
        private double mFRR = 1.00;
        private double mThreshold; 

        private List<double> mFRRList;
        private List<double> mFARList;
        private List<double> mThresholdList;
        private List<double> mSubList;

        public ErrorCalculation()
        {
            mFARList = new List<double>();
            mFRRList = new List<double>();
            mThresholdList = new List<double>();
            mSubList = new List<double>();
        }

        #region "Public Functions"

        public double GetFAR()
        {
            return mFAR;
        }

        public double GetFRR()
        {
            return mFRR;
        }

        public List<double> GetFARList()
        {
            return mFARList;
        }

        public List<double> GetFRRList()
        {
            return mFRRList;
        }

        public List<double> GetThresholdList()
        {
            return mThresholdList;
        }

        public double GetERR()
        {
            return (mFAR + mFRR) / 2;
        }

        /// <summary>
        /// Calculates FRR and FAR based on the original scores list, and impostor scores list (SSCORE) 
        /// </summary>
        /// <param name="aOriginalScores"></param>
        /// <param name="aImpostorScores"></param>
        /// <param name="aNumIntervals"></param>
        public virtual void CalculateErrors(List<double> aOriginalScores, List<double> aImpostorScores, double aNumIntervals)
        {
            double lMin = (aOriginalScores.Min() < aImpostorScores.Min()) ? aOriginalScores.Min() : aImpostorScores.Min();
            double lMax = (aOriginalScores.Max() > aImpostorScores.Max()) ? aOriginalScores.Max() : aImpostorScores.Max();

            double lStep = (double)((lMax - lMin) / aNumIntervals);
            double lDecisionThreshold = lMin;

            double lFRR = (double)aOriginalScores.Count(x => x < lDecisionThreshold) / aOriginalScores.Count;
            double lFAR = (double)aImpostorScores.Count(x => x >= lDecisionThreshold) / aImpostorScores.Count;

            while (lDecisionThreshold < lMax)
            {
                if (lFAR == 1 && lFRR == 1)
                {
                    mSubList.Add(99);
                }
                else
                {
                    mSubList.Add(Math.Abs(lFRR - lFAR));
                }

                double lOrig = aOriginalScores.Count(x => x < lDecisionThreshold);
                double lImpostor = aImpostorScores.Count(x => x >= lDecisionThreshold);

                lFRR = (double)lOrig / aOriginalScores.Count;
                lFAR = (double)lImpostor / aImpostorScores.Count;

                mFRRList.Add(lFRR);
                mFARList.Add(lFAR);
                mThresholdList.Add(lDecisionThreshold);


                lDecisionThreshold += lStep;
            }

            var lIntersectionIndex = mSubList.IndexOf(mSubList.Min()) - 1;

            mFRR = mFRRList.ElementAt(lIntersectionIndex);
            mFAR = mFARList.ElementAt(lIntersectionIndex);
            mThreshold = mThresholdList.ElementAt(lIntersectionIndex);
        }
    }
    #endregion
}
