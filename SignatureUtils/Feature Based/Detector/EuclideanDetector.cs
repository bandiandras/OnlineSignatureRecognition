using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractionLayer;

namespace SignatureUtils.Feature_Based.Detector
{
    public class EuclideanDetector : Detector
    {
        public EuclideanDetector(List<SignatureFeatures> aFeatures) : base(aFeatures)
        {
        }

        public override double CalculateScores(SignatureFeatures aTrainedFeatures, SignatureFeatures aFeaturesOfSigToVerify)
        {
            double sum = 0;
            for(int i = 0; i < aTrainedFeatures.Count; ++i)
            {
                sum += Math.Pow(aTrainedFeatures.ElementAt(i).Value - aFeaturesOfSigToVerify.ElementAt(i).Value, 2);
            }

            return Math.Sqrt(sum);
        }
    }
}
