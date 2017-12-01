using AbstractionLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureUtils.Feature_Based.Detector
{
    public class Detector
    {
        private NormalizedFeatures mNormalized;
        private SignatureFeatures mTemplate = new SignatureFeatures();


        public Detector(List<SignatureFeatures> aFeatures)
        {
            mNormalized = FeatureNormalizer.NormalizeFeatures(aFeatures);

            foreach(var featureSet in mNormalized.ListOfFeatureSets)
            {
                foreach(var feature in featureSet)
                {
                    mTemplate[feature.Key] += feature.Value;
                }
            }

            foreach(var feature in mTemplate.ToList())
            {
                mTemplate[feature.Key] /= mNormalized.ListOfFeatureSets.Count;
            }
        }

        public virtual double CalculateScores(SignatureFeatures aTrainedFeatures, SignatureFeatures aFeaturesOfSigToVerify)
        {
            throw new NotImplementedException();
        }
        

        public List<double> CalculateScoreList(List<SignatureFeatures> aTrainedFeatures, SignatureFeatures aFeaturesOfSigToVerify)
        {
            List<double> lScoreList = new List<double>();

            foreach (var featureSet in aTrainedFeatures)
            {
                lScoreList.Add(CalculateScores(featureSet, aFeaturesOfSigToVerify));
            }

            return lScoreList;
        }

        public List<double> CompareToTemplate(List<SignatureFeatures> aFeaturesToCompare)
        {
            List<double> lScoreList = new List<double>();

            foreach (var featureSet in aFeaturesToCompare)
            {
                lScoreList.Add(CalculateScores(mTemplate, FeatureNormalizer.NormalizeFeaturesSingleSignature(featureSet, mNormalized.NormalizedWith)));
            }

            return lScoreList;
        }
    }


}
