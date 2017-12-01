using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractionLayer
{
    public class NormalizedFeatures
    {
        private List<SignatureFeatures> mListOfFeatureSets;
        private List<NormalizedWith> mNormalizedWith;

        public List<NormalizedWith> NormalizedWith
        {
            get
            {
                return mNormalizedWith;
            }
            set
            {
                mNormalizedWith = value;
            }
        }

        public List<SignatureFeatures> ListOfFeatureSets
        {
            get
            {
                return mListOfFeatureSets;
            }
        }

        public void AddMinMaxPairToNormalizedWith(double aMin, double aMax)
        {
            mNormalizedWith.Add(new NormalizedWith(aMin, aMax));
        }

        public NormalizedFeatures()
        {
            mNormalizedWith = new List<NormalizedWith>();
        }

        public NormalizedFeatures(List<SignatureFeatures> aListOfFeatureSets, List<NormalizedWith> aNormalizedWith)
        {
            mNormalizedWith = aNormalizedWith;
            mListOfFeatureSets = aListOfFeatureSets;
        }
    }
}
