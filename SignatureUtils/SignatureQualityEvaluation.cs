using AbstractionLayer;
using SignatureUtils.DTW;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureUtils
{
    public static class SignatureQualityEvaluation
    {
        /// <summary>
        /// Checks the quality of the signature 
        /// Can be used at taking samples, ot at validating a signature
        /// </summary>
        /// <param name="aDirectory"></param>
        /// <param name="aSignature"></param>
        /// <returns></returns>
        public static bool CheckQuality(string aDirectory, Signature aSignature, DTWConfiguration aDTWConfig, double aTolerance = 0.05)
        {
            int lNumberOfRegisteredSamples = SignatureFileUtils.GetNumberOfSignaturesFromFolder(aDirectory);
            if (lNumberOfRegisteredSamples > 1)
            {
                List<Signature> lSignaturesOfUser = SignatureFileUtils.GetAllSignaturesFromFolder(aDirectory, true);
                List<double> lListOfScores = new List<double>();


                //Take the first element from the registered samples
                Signature lSignatureToCompare = lSignaturesOfUser.ElementAt(0);
                List<double> lListOfScoresRegisteredSig = new List<double>();
                //Compare the first element of the registered signature to the rest of the template and save the scores
                for (int i = 1; i < lSignaturesOfUser.Count; i++)
                {
                    var lSig = lSignaturesOfUser.ElementAt(i);
                    lListOfScoresRegisteredSig.Add(ConfigurableDTW.DTWDistance(lSignatureToCompare, lSig, aDTWConfig));
                }

                //Compare the aSignature sample to the template and save the scores
                foreach (Signature lSig in lSignaturesOfUser)
                {
                    lListOfScores.Add(ConfigurableDTW.DTWDistance(aSignature, lSig, aDTWConfig));
                }

                //Compare the average of the two score lists, accept of reject the signature
                if (lListOfScores.Average() > lListOfScoresRegisteredSig.Average() + aTolerance)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public static bool Authenticate(string aDirectory, Signature aSignature, DTWConfiguration aDTWConfig, double aTolerance = 0.05)
        {
            List<Signature> lSignaturesOfUser = SignatureFileUtils.GetAllSignaturesFromFolder(aDirectory, true);
            List<double> lListOfScores = new List<double>();

            //Check for NAN in the signature sample
            //If an attribute is NAN, but is checked for usage by the DTWConfig, uncheck it
            SignatureUtils.CheckForNaN(lSignaturesOfUser.ElementAt(0), ref aDTWConfig);

            //Take the first element from the registered samples
            Signature lSignatureToCompare = lSignaturesOfUser.ElementAt(0);
            List<double> lListOfScoresRegisteredSig = new List<double>();
            //Compare the first element of the registered signature to the rest of the template and save the scores
            for (int i = 1; i < lSignaturesOfUser.Count; i++)
            {
                var lSig = lSignaturesOfUser.ElementAt(i);
                lListOfScoresRegisteredSig.Add(ConfigurableDTW.DTWDistance(lSignatureToCompare, lSig, aDTWConfig));
            }

            //Compare the aSignature sample to the template and save the scores
            foreach (Signature lSig in lSignaturesOfUser)
            {
                lListOfScores.Add(ConfigurableDTW.DTWDistance(aSignature, lSig, aDTWConfig));
            }

            //Compare the average of the two score lists, accept of reject the signature
            if (lListOfScores.Average() > lListOfScoresRegisteredSig.Average() + aTolerance)
            {
                return false;
            }
            else
            {
                return true;
            }       
        }

        /// <summary>
        /// Removes the worst aNrOfSamplesToRemove samples from the template
        /// </summary>
        /// <param name="aDirectory"></param>
        /// <param name="aNrOfSamplesToRemove"></param>
        /// <param name="aDTWConfig"></param>
        /// <param name="aTolerance"></param>
        public static void RemoveWorstSamples(string aDirectory, int aNrOfSamplesToRemove, DTWConfiguration aDTWConfig, double aTolerance = 0.05)
        {         
            List<Signature> lSignaturesOfUser = SignatureFileUtils.GetAllSignaturesFromFolder(aDirectory, true);

            List<double> lListOfAverageOfScores = new List<double>();
            List<List<double>> lListOfScores = new List<List<double>>();

            //Compare all signatures to the rest, one by one and save the scores in a matrix
            for(int i = 0; i < lSignaturesOfUser.Count; ++i)
            {
                var lSigToCompare = lSignaturesOfUser.ElementAt(i);

                List<double> lResults = new List<double>();
                for (int j = 0; j < lSignaturesOfUser.Count; ++j)
                {                  
                    var lSig = lSignaturesOfUser.ElementAt(j);
                    lResults.Add(ConfigurableDTW.DTWDistance(lSigToCompare, lSig, aDTWConfig));
                }
                lListOfScores.Add(lResults);
            }

            //Calculate the average of the scores
            for (int i = 0; i <  lListOfScores.Count; ++i)
            {
                for(int j = 0; j < lListOfScores.ElementAt(i).Count; ++i)
                {
                    if(i == 0)
                    {
                        lListOfAverageOfScores.Add(lListOfScores[i][j]);
                    }
                    else
                    {
                        lListOfAverageOfScores[j] += lListOfScores[i][j];
                    }
                }
            }
            for(int i = 0; i < lListOfAverageOfScores.Count; ++i)
            {
                lListOfAverageOfScores[i] /= lListOfAverageOfScores.Count;
            }

            //Remove the worst samples
            for(int i = 0; i < aNrOfSamplesToRemove; ++i)
            {
                //delete the signature files from disk
                var lIndexToRemove = lListOfAverageOfScores.IndexOf(lListOfAverageOfScores.Max());
                File.Delete(lSignaturesOfUser.ElementAt(lIndexToRemove).Filename);
            }
        }
    }
}
