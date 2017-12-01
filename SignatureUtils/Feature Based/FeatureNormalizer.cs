using AbstractionLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureUtils.Feature_Based
{
    /// <summary>
    /// Class to normalize Signature Features
    /// </summary>
    public static class FeatureNormalizer
    {
        /// <summary>
        /// Applies min-max normalization to the feature set
        /// </summary>
        /// <param name="aFeatureList"></param>
        /// <returns>The return value contains the normalized Feature set, as well the min and max values used for normaziation</returns>
        public static NormalizedFeatures NormalizeFeatures(List<SignatureFeatures> aFeatureList)
        {                

            //Initialize Normalized With list. This will contain the min anf max values used at the normalization of the features
            List<NormalizedWith> lNormalizedWith = new List<NormalizedWith>();
            for(var i = 0; i < aFeatureList.ElementAt(0).Count; ++i)
            {
                lNormalizedWith.Add(new NormalizedWith(9999, -9999));
            }

            for(int i = 0; i < aFeatureList.Count; ++i)
            {
                SignatureFeatures lFeatures = new SignatureFeatures();
                for(int j = 0; j < aFeatureList.ElementAt(i).Count; ++j)
                {
                    var lCurrentFeatureValue = aFeatureList.ElementAt(i).ElementAt(j).Value;
                    if (lNormalizedWith.ElementAt(j).Min > lCurrentFeatureValue)
                    {
                        lNormalizedWith.ElementAt(j).Min = lCurrentFeatureValue;
                    }
                    if (lNormalizedWith.ElementAt(j).Max < lCurrentFeatureValue)
                    {
                        lNormalizedWith.ElementAt(j).Max = lCurrentFeatureValue;
                    }
                }
            }
           
            for(int i = 0; i < aFeatureList.Count; ++i)
            {
                SignatureFeatures lNormalizedFeatures = new SignatureFeatures();
                for (int j = 0; j < aFeatureList.ElementAt(i).Count; ++j)
                {               
                    var lCurrentFeature = aFeatureList.ElementAt(i).ElementAt(j);
                    if (lNormalizedWith.ElementAt(j).Min != lNormalizedWith.ElementAt(j).Max)
                    {
                        lNormalizedFeatures[lCurrentFeature.Key] = 2 * ((lCurrentFeature.Value - lNormalizedWith.ElementAt(j).Min) / (lNormalizedWith.ElementAt(j).Max - lNormalizedWith.ElementAt(j).Min)) - 1;
                    }
                    else
                    {
                        lNormalizedFeatures[lCurrentFeature.Key] = lCurrentFeature.Value;
                    }
                    

                }
                aFeatureList.RemoveAt(i);
                aFeatureList.Insert(i, lNormalizedFeatures);
            }

            NormalizedFeatures lNormalized = new NormalizedFeatures(aFeatureList, lNormalizedWith);

            return lNormalized;
        }

        public static SignatureFeatures NormalizeFeaturesSingleSignature(SignatureFeatures aFeatures, List<NormalizedWith> aNormalizedWith)
        {
            SignatureFeatures lFeatures = new SignatureFeatures();
            int lCounter = 0;

            foreach (var key in lFeatures.Keys.ToList())
            {
                if (aNormalizedWith.ElementAt(lCounter).Max != aNormalizedWith.ElementAt(lCounter).Min)
                {
                    lFeatures[key] = 2 * ((aFeatures[key] - aNormalizedWith.ElementAt(lCounter).Min) / (aNormalizedWith.ElementAt(lCounter).Max - aNormalizedWith.ElementAt(lCounter).Min)) - 1;
                }
                else
                {
                    lFeatures[key] = aFeatures[key];
                }
                
                lCounter++;
            }            

            return lFeatures;
        }
    }
}
