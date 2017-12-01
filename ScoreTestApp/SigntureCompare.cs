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

namespace ScoreTestApp
{
    public static class SigntureCompare
    {
        public static double CompareMCYTFusion(string aFolder, int aNrOfTrainingSamples, DTWConfiguration aDTWConfig, ref StreamWriter aSWriter)
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

            List<double> lListOfScores = new List<double>();

            //aDetector.CalculateScores()
            List<double> lOriginalScoresGlobal = lEuclideanDetector.CompareToTemplate(lFeaturesOriginal);
            List<double> lImpostorScoresGlobal = lEuclideanDetector.CompareToTemplate(lFeaturesImpostor);

            //int lNumberOfOriginalScores;
            //int lNumberOfImpostorScores;

            //lListOfScores.AddRange(lOriginalScoresGlobal);
            //lListOfScores.AddRange(lImpostorScoresGlobal);

            //lListOfScores = SignatureUtils.SignatureUtils.MinMaxNormalization(lListOfScores);

            //lNumberOfOriginalScores = lOriginalScoresGlobal.Count;
            //lNumberOfImpostorScores = lImpostorScoresGlobal.Count;

            //lOriginalScoresGlobal = new List<double>();
            //lOriginalScoresGlobal.AddRange(lListOfScores.Take(lNumberOfOriginalScores));

            //lImpostorScoresGlobal = new List<double>();
            //lImpostorScoresGlobal.AddRange(lListOfScores.Skip(lNumberOfOriginalScores).Take(lNumberOfImpostorScores));

            List<double> lOriginalScoresLocal = SignatureUtils.SignatureUtils.CompareSignaturesDTW(lTemplate, lOriginalSignatures, aDTWConfig);
            List<double> lImpostorScoresLocal = SignatureUtils.SignatureUtils.CompareSignaturesDTW(lTemplate, lImpostorSignatures, aDTWConfig);


            aSWriter.WriteLine("Original Scores Local" + "," + "Original Scores Global");
            for (int i = 0; i < lOriginalScoresLocal.Count; ++i)
            {
                aSWriter.WriteLine(lOriginalScoresLocal.ElementAt(i) + "," + lOriginalScoresGlobal.ElementAt(i));
            }


            //lListOfScores = new List<double>();
            //lListOfScores.AddRange(lOriginalScoresLocal);
            //lListOfScores.AddRange(lImpostorScoresLocal);

            //lListOfScores = SignatureUtils.SignatureUtils.MinMaxNormalization(lListOfScores);

            //lNumberOfOriginalScores = lOriginalScoresLocal.Count;
            //lNumberOfImpostorScores = lImpostorScoresLocal.Count;

            //lOriginalScoresLocal = new List<double>();
            //lOriginalScoresLocal.AddRange(lListOfScores.Take(lNumberOfOriginalScores));

            //lImpostorScoresLocal = new List<double>();
            //lImpostorScoresLocal.AddRange(lListOfScores.Skip(lNumberOfOriginalScores).Take(lNumberOfImpostorScores));

            aSWriter.WriteLine("Impostor Scores Local" + "," + "Impostor Scores Global");
            for (int i = 0; i < lImpostorScoresLocal.Count; ++i)
            {
                aSWriter.WriteLine(lImpostorScoresLocal.ElementAt(i) + "," + lImpostorScoresGlobal.ElementAt(i));
            }

            //List<double> lOriginalScores = SignatureUtils.SignatureUtils.ScoreFusion(lOriginalScoresLocal, lOriginalScoresGlobal);
            //List<double> lImpostorScores = SignatureUtils.SignatureUtils.ScoreFusion(lImpostorScoresLocal, lImpostorScoresGlobal);

            //aSWriter.WriteLine("Impostor Scores");
            //for (int i = 0; i < lImpostorScores.Count; ++i)
            //{
            //    aSWriter.WriteLine(lImpostorScores.ElementAt(i));
            //}

            //aSWriter.WriteLine("Original Scores");
            //for (int i = 0; i < lOriginalScores.Count; ++i)
            //{
            //    aSWriter.WriteLine(lOriginalScores.ElementAt(i));
            //}

            //ErrorCalculation lError = ErrorCalculationFactory.GetDScoreErrorCalculator();
            //lError.CalculateErrors(lOriginalScores, lImpostorScores, 100);

            //var lFARList = lError.GetFARList();
            //var lFRRList = lError.GetFRRList();
            //var lTresholdList = lError.GetThresholdList();

            //for(int i = 0; i < lFARList.Count; ++i)
            //{
            //    aSWriter.WriteLine(lFARList.ElementAt(i) + "," + lFRRList.ElementAt(i) + ", " + lTresholdList.ElementAt(i));
            //}

            //for (int i = 0; i < lOriginalScoresLocal.Count; ++i)
            //{
            //    aSWriter.WriteLine(lOriginalScoresLocal.ElementAt(i) + "," + lOriginalScoresGlobal.ElementAt(i));
            //}

            //aSWriter.WriteLine(lError.GetERR());

            return 1;
        }
    }
}
