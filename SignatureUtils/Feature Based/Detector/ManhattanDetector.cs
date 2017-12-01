using AbstractionLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureUtils.Feature_Based.Detector
{
    public class ManhattanDetector : Detector
    {
        public ManhattanDetector(List<SignatureFeatures> aFeatures) : base(aFeatures)
        {
            
        }

        public override double CalculateScores(SignatureFeatures aTrainedFeatures, SignatureFeatures aFeaturesOfSigToVerify)
        {
            double score = 0;
            for (int i = 0; i < aTrainedFeatures.Count; ++i)
            {
                score += Math.Abs(aTrainedFeatures.ElementAt(i).Value - aFeaturesOfSigToVerify.ElementAt(i).Value);
            }

            return score;
        }
    }
}
