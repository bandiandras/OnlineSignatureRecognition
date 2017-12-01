using AbstractionLayer;
using SignatureUtils;
using SignatureUtils.Base;
using SignatureUtils.DTW;
using SignatureUtils.Feature_Based;
using SignatureUtils.Feature_Based.Detector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureCompare
{
    public static class SignatureCompareFunctions
    {
        public static double CompareMCYTFeatureBased(string aFolder, int aNrOfTrainingSamples, ref StreamWriter aSWriter)
        {
            List<Signature> lSignatures = SignatureFileUtils.GetAllSignaturesFromFolder(aFolder);

            List<Signature> lTemplate = lSignatures.Skip(25).Take(aNrOfTrainingSamples).ToList();
            List<Signature> lOriginalSignatures = lSignatures.Skip(25 + aNrOfTrainingSamples).Take(25 - aNrOfTrainingSamples).ToList();
            List<Signature> lImpostorSignatures = lSignatures.Take(15).ToList();

            //retrieve feature set
            List<SignatureFeatures> lFeaturesTemplate = new List<SignatureFeatures>();
            List<SignatureFeatures> lFeaturesOriginal = new List<SignatureFeatures>();
            List<SignatureFeatures> lFeaturesImpostor = new List<SignatureFeatures>();

            for (int i = 0; i < lTemplate.Count; ++i)
            {
                var lElement = lTemplate.ElementAt(i);
                var lNewElement = SignatureUtils.SignatureUtils.CalculateCharacteristics(lElement);

                lNewElement = SignatureUtils.SignatureUtils.StandardizeSignature(lNewElement);

                lTemplate.RemoveAt(i);
                lTemplate.Insert(i, lNewElement);

                lFeaturesTemplate.Add(FeatureCalculator.CalculateFeatures(lNewElement));
            }

            for (int i = 0; i < lOriginalSignatures.Count; ++i)
            {
                var lElement = lOriginalSignatures.ElementAt(i);
                var lNewElement = SignatureUtils.SignatureUtils.CalculateCharacteristics(lElement);

                lNewElement = SignatureUtils.SignatureUtils.StandardizeSignature(lNewElement);

                lOriginalSignatures.RemoveAt(i);
                lOriginalSignatures.Insert(i, lNewElement);

                lFeaturesOriginal.Add(FeatureCalculator.CalculateFeatures(lNewElement));
            }

            for (int i = 0; i < lImpostorSignatures.Count; ++i)
            {
                var lElement = lImpostorSignatures.ElementAt(i);
                var lNewElement = SignatureUtils.SignatureUtils.CalculateCharacteristics(lElement);

                lNewElement = SignatureUtils.SignatureUtils.StandardizeSignature(lNewElement);

                lImpostorSignatures.RemoveAt(i);
                lImpostorSignatures.Insert(i, lNewElement);

                lFeaturesImpostor.Add(FeatureCalculator.CalculateFeatures(lNewElement));
            }

            ManhattanDetector lEuclideanDetector = new ManhattanDetector(lFeaturesTemplate);

            //aDetector.CalculateScores()
            List<double> lOriginalScores = lEuclideanDetector.CompareToTemplate(lFeaturesOriginal);
            List<double> lImpostorScores = lEuclideanDetector.CompareToTemplate(lFeaturesImpostor);

            ErrorCalculation lError = ErrorCalculationFactory.GetDScoreErrorCalculator();
            lError.CalculateErrors(lOriginalScores, lImpostorScores, 100);

            //var lFARList = lError.GetFARList();
            //var lFRRList = lError.GetFRRList();
            //var lTresholdList = lError.GetThresholdList();

            //for(int i = 0; i < lFARList.Count; ++i)
            //{
            //    aSWriter.WriteLine(lFARList.ElementAt(i) + "," + lFRRList.ElementAt(i) + ", " + lTresholdList.ElementAt(i));
            //}

            aSWriter.WriteLine(lError.GetERR());

            return (lError.GetERR());
        }

        public static double CompareMobisigFeatureBased(string aFolder, int aNrOfTrainingSamples, ref StreamWriter aSWriter)
        {
            List<Signature> lSignatures = SignatureFileUtils.GetAllSignaturesFromFolder(aFolder);

            List<Signature> lTemplate = lSignatures.Skip(20).Take(aNrOfTrainingSamples).ToList();
            List<Signature> lOriginalSignatures = lSignatures.Skip(20 + aNrOfTrainingSamples).Take(25 - aNrOfTrainingSamples).ToList();
            List<Signature> lImpostorSignatures = lSignatures.Take(15).ToList();

            //retrieve feature set
            List<SignatureFeatures> lFeaturesTemplate = new List<SignatureFeatures>();
            List<SignatureFeatures> lFeaturesOriginal = new List<SignatureFeatures>();
            List<SignatureFeatures> lFeaturesImpostor = new List<SignatureFeatures>();

            for (int i = 0; i < lTemplate.Count; ++i)
            {
                var lElement = lTemplate.ElementAt(i);
                var lNewElement = SignatureUtils.SignatureUtils.CalculateCharacteristics(lElement);

                lNewElement = SignatureUtils.SignatureUtils.StandardizeSignature(lNewElement);

                lTemplate.RemoveAt(i);
                lTemplate.Insert(i, lNewElement);

                lFeaturesTemplate.Add(FeatureCalculator.CalculateFeatures(lElement));
            }

            for (int i = 0; i < lOriginalSignatures.Count; ++i)
            {
                var lElement = lOriginalSignatures.ElementAt(i);
                var lNewElement = SignatureUtils.SignatureUtils.CalculateCharacteristics(lElement);

                lNewElement = SignatureUtils.SignatureUtils.StandardizeSignature(lNewElement);

                lOriginalSignatures.RemoveAt(i);
                lOriginalSignatures.Insert(i, lNewElement);

                lFeaturesOriginal.Add(FeatureCalculator.CalculateFeatures(lElement));
            }

            for (int i = 0; i < lImpostorSignatures.Count; ++i)
            {
                var lElement = lImpostorSignatures.ElementAt(i);
                var lNewElement = SignatureUtils.SignatureUtils.CalculateCharacteristics(lElement);

                lNewElement = SignatureUtils.SignatureUtils.StandardizeSignature(lNewElement);

                lImpostorSignatures.RemoveAt(i);
                lImpostorSignatures.Insert(i, lNewElement);

                lFeaturesImpostor.Add(FeatureCalculator.CalculateFeatures(lElement));
            }

            EuclideanDetector lEuclideanDetector = new EuclideanDetector(lFeaturesTemplate);

            //aDetector.CalculateScores()
            List<double> lOriginalScores = lEuclideanDetector.CompareToTemplate(lFeaturesOriginal);
            List<double> lImpostorScores = lEuclideanDetector.CompareToTemplate(lFeaturesImpostor);

            ErrorCalculation lError = ErrorCalculationFactory.GetDScoreErrorCalculator();
            lError.CalculateErrors(lOriginalScores, lImpostorScores, 100);

            //var lFARList = lError.GetFARList();
            //var lFRRList = lError.GetFRRList();
            //var lTresholdList = lError.GetThresholdList();

            //for(int i = 0; i < lFARList.Count; ++i)
            //{
            //    aSWriter.WriteLine(lFARList.ElementAt(i) + "," + lFRRList.ElementAt(i) + ", " + lTresholdList.ElementAt(i));
            //}

            aSWriter.WriteLine(lError.GetERR());

            return (lError.GetERR());
        }


        public static void CompareMCYT(string aFolder, int aNrOfTrainingSamples, DTWConfiguration aDTWConfig, ref StreamWriter aSWriter)
        {
            List<Signature> lSignatures = SignatureFileUtils.GetAllSignaturesFromFolder(aFolder);

            List<Signature> lTemplate = lSignatures.Skip(25).Take(aNrOfTrainingSamples).ToList();
            List<Signature> lOriginalSignatures = lSignatures.Skip(25 + aNrOfTrainingSamples).Take(25 - aNrOfTrainingSamples).ToList();
            List<Signature> lImpostorSignatures = lSignatures.Take(15).ToList();

            for (int i = 0; i < lTemplate.Count; ++i)
            {
                var lElement = lTemplate.ElementAt(i);
                var lNewElement = SignatureUtils.SignatureUtils.CalculateCharacteristics(lElement);

                lNewElement = SignatureUtils.SignatureUtils.StandardizeSignature(lNewElement);

                lTemplate.RemoveAt(i);
                lTemplate.Insert(i, lNewElement);
            }

            for (int i = 0; i < lOriginalSignatures.Count; ++i)
            {
                var lElement = lOriginalSignatures.ElementAt(i);
                var lNewElement = SignatureUtils.SignatureUtils.CalculateCharacteristics(lElement);

                lNewElement = SignatureUtils.SignatureUtils.StandardizeSignature(lNewElement);

                lOriginalSignatures.RemoveAt(i);
                lOriginalSignatures.Insert(i, lNewElement);
            }

            for (int i = 0; i < lImpostorSignatures.Count; ++i)
            {
                var lElement = lImpostorSignatures.ElementAt(i);
                var lNewElement = SignatureUtils.SignatureUtils.CalculateCharacteristics(lElement);

                lNewElement = SignatureUtils.SignatureUtils.StandardizeSignature(lNewElement);

                lImpostorSignatures.RemoveAt(i);
                lImpostorSignatures.Insert(i, lNewElement);
            }

            List<double> lOriginalScores = SignatureUtils.SignatureUtils.CompareSignaturesDTW(lTemplate, lOriginalSignatures, aDTWConfig);
            List<double> lImpostorScores = SignatureUtils.SignatureUtils.CompareSignaturesDTW(lTemplate, lImpostorSignatures, aDTWConfig);

            ErrorCalculation lError = ErrorCalculationFactory.GetDScoreErrorCalculator();
            lError.CalculateErrors(lOriginalScores, lImpostorScores, 100);

            //var lFARList = lError.GetFARList();
            //var lFRRList = lError.GetFRRList();
            //var lTresholdList = lError.GetThresholdList();

            //for(int i = 0; i < lFARList.Count; ++i)
            //{
            //    aSWriter.WriteLine(lFARList.ElementAt(i) + "," + lFRRList.ElementAt(i) + ", " + lTresholdList.ElementAt(i));
            //}
             
            aSWriter.WriteLine(lError.GetERR());
        }

        public static void CompareMOBISIG(string aFolder, int aNrOfTrainingSamples, DTWConfiguration aDTWConfig, ref StreamWriter aSWriter)
        {
            List<Signature> lSignatures = SignatureFileUtils.GetAllSignaturesFromFolder(aFolder);

            List<Signature> lTemplate = lSignatures.Skip(20).Take(aNrOfTrainingSamples).ToList();
            List<Signature> lOriginalSignatures = lSignatures.Skip(20+aNrOfTrainingSamples).Take(25 - aNrOfTrainingSamples).ToList();
            List<Signature> lImpostorSignatures = lSignatures.Take(15).ToList();

            for (int i = 0; i < lTemplate.Count; ++i)
            {
                var lElement = lTemplate.ElementAt(i);
                var lNewElement = SignatureUtils.SignatureUtils.CalculateCharacteristics(lElement);

                lNewElement = SignatureUtils.SignatureUtils.StandardizeSignature(lNewElement);

                lTemplate.RemoveAt(i);
                lTemplate.Insert(i, lNewElement);
            }

            for (int i = 0; i < lOriginalSignatures.Count; ++i)
            {
                var lElement = lOriginalSignatures.ElementAt(i);
                var lNewElement = SignatureUtils.SignatureUtils.CalculateCharacteristics(lElement);

                lNewElement = SignatureUtils.SignatureUtils.StandardizeSignature(lNewElement);

                lOriginalSignatures.RemoveAt(i);
                lOriginalSignatures.Insert(i, lNewElement);
            }

            for (int i = 0; i < lImpostorSignatures.Count; ++i)
            {
                var lElement = lImpostorSignatures.ElementAt(i);
                var lNewElement = SignatureUtils.SignatureUtils.CalculateCharacteristics(lElement);

                lNewElement = SignatureUtils.SignatureUtils.StandardizeSignature(lNewElement);

                lImpostorSignatures.RemoveAt(i);
                lImpostorSignatures.Insert(i, lNewElement);
            }

            List<double> lOriginalScores = SignatureUtils.SignatureUtils.CompareSignaturesDTW(lTemplate, lOriginalSignatures, aDTWConfig);
            List<double> lImpostorScores = SignatureUtils.SignatureUtils.CompareSignaturesDTW(lTemplate, lImpostorSignatures, aDTWConfig);

            ErrorCalculation lError = ErrorCalculationFactory.GetDScoreErrorCalculator();
            lError.CalculateErrors(lOriginalScores, lImpostorScores, 100);
            aSWriter.WriteLine(lError.GetERR());
        }

        public static void Compare(string aFolder, DTWConfiguration aDTWConfig, ref StreamWriter aSWriter)
        {
            List<Signature> lSignatures = SignatureFileUtils.GetAllSignaturesFromFolder(aFolder);

            List<Signature> lTemplate = lSignatures.Take(5).ToList();
            List<Signature> lOriginalSignatures = lSignatures.Skip(5).Take(13).ToList();
            List<Signature> lImpostorSignatures = lSignatures.Skip(18).Take(9).ToList();

            for (int i = 0; i < lTemplate.Count; ++i)
            {
                var lElement = lTemplate.ElementAt(i);
                var lNewElement = SignatureUtils.SignatureUtils.CalculateCharacteristics(lElement);

                lNewElement = SignatureUtils.SignatureUtils.StandardizeSignature(lNewElement);

                lTemplate.RemoveAt(i);
                lTemplate.Insert(i, lNewElement);
            }

            for (int i = 0; i < lOriginalSignatures.Count; ++i)
            {
                var lElement = lOriginalSignatures.ElementAt(i);
                var lNewElement = SignatureUtils.SignatureUtils.CalculateCharacteristics(lElement);

                lNewElement = SignatureUtils.SignatureUtils.StandardizeSignature(lNewElement);

                lOriginalSignatures.RemoveAt(i);
                lOriginalSignatures.Insert(i, lNewElement);
            }

            for (int i = 0; i < lImpostorSignatures.Count; ++i)
            {
                var lElement = lImpostorSignatures.ElementAt(i);
                var lNewElement = SignatureUtils.SignatureUtils.CalculateCharacteristics(lElement);

                lNewElement = SignatureUtils.SignatureUtils.StandardizeSignature(lNewElement);

                lImpostorSignatures.RemoveAt(i);
                lImpostorSignatures.Insert(i, lNewElement);
            }

            List<double> lOriginalScores = SignatureUtils.SignatureUtils.CompareSignaturesDTW(lTemplate, lOriginalSignatures, aDTWConfig);
            List<double> lImpostorScores = SignatureUtils.SignatureUtils.CompareSignaturesDTW(lTemplate, lImpostorSignatures, aDTWConfig);

            ErrorCalculation lError = ErrorCalculationFactory.GetDScoreErrorCalculator();
            lError.CalculateErrors(lOriginalScores, lImpostorScores, 100);
            aSWriter.WriteLine(lError.GetERR());
        }
    }
}
