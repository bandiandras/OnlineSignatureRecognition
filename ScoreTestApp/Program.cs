using AbstractionLayer;
using SignatureUtils;
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
    class Program
    {
        static void Main(string[] args)
        {
            StreamWriter file1 = new StreamWriter("d:\\" + "Result_MOBISIG_Scores_" + DateTime.Now.Ticks.ToString() + ".csv");
            DTWConfiguration lDTWConfig = new DTWConfiguration();

            List<double> lAllOriginalScoresGlobal = new List<double>();
            List<double> lAllOriginalScoresGlobal2 = new List<double>();
            List<double> lAllOriginalScoresLocal = new List<double>();

            List<double> lAllImpostorScoresGlobal = new List<double>();
            List<double> lAllImpostorScoresGlobal2 = new List<double>();
            List<double> lAllImpostorScoresLocal = new List<double>();


            Console.Write("Path to directory: ");
            string lPath = Console.ReadLine();

            string[] lSubdirectoryEntries = Directory.GetDirectories(lPath);


            lDTWConfig = new DTWConfiguration()
                                    .UseXY()
                                    .UseX1Y1()
                                    .UseX2Y2();

            int aNrOfTrainingSamples = 10;

            foreach (var directory in lSubdirectoryEntries)
            {
                Console.WriteLine("Processing " + directory + " directory");

                List<Signature> lSignatures = SignatureFileUtils.GetAllSignaturesFromFolder(directory);

                //MCYT
                //List<Signature> lTemplate = lSignatures.Skip(25).Take(aNrOfTrainingSamples).ToList();
                //List<Signature> lOriginalSignatures = lSignatures.Skip(25 + aNrOfTrainingSamples).Take(25 - aNrOfTrainingSamples).ToList();
                //List<Signature> lImpostorSignatures = lSignatures.Take(15).ToList();

                //MOBISIG
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

                ManhattanDetector lManhattanDetector = new ManhattanDetector(lFeaturesTemplate);
                EuclideanDetector lEuclideanDetector = new EuclideanDetector(lFeaturesTemplate);

                List<double> lListOfScores = new List<double>();

                //aDetector.CalculateScores()
                List<double> lOriginalScoresGlobal = lManhattanDetector.CompareToTemplate(lFeaturesOriginal);
                List<double> lImpostorScoresGlobal = lManhattanDetector.CompareToTemplate(lFeaturesImpostor);

                List<double> lOriginalScoresGlobal2 = lEuclideanDetector.CompareToTemplate(lFeaturesOriginal);
                List<double> lImpostorScoresGlobal2 = lEuclideanDetector.CompareToTemplate(lFeaturesImpostor);

                List<double> lOriginalScoresLocal = SignatureUtils.SignatureUtils.CompareSignaturesDTW(lTemplate, lOriginalSignatures, lDTWConfig);
                List<double> lImpostorScoresLocal = SignatureUtils.SignatureUtils.CompareSignaturesDTW(lTemplate, lImpostorSignatures, lDTWConfig);


                lAllOriginalScoresGlobal.AddRange(lOriginalScoresGlobal);
                lAllOriginalScoresGlobal2.AddRange(lOriginalScoresGlobal2);
                lAllOriginalScoresLocal.AddRange(lOriginalScoresLocal);
                lAllImpostorScoresGlobal.AddRange(lImpostorScoresGlobal);
                lAllImpostorScoresGlobal2.AddRange(lImpostorScoresGlobal2);
                lAllImpostorScoresLocal.AddRange(lImpostorScoresLocal);
            }

            file1.WriteLine("Original Scores Local" + "," + "Original Scores Manhattan" + "," + "Original Scores Euclidean");
            for (int i = 0; i < lAllOriginalScoresLocal.Count; ++i)
            {
                file1.WriteLine(lAllOriginalScoresLocal.ElementAt(i) + "," + lAllOriginalScoresGlobal.ElementAt(i) + "," + lAllOriginalScoresGlobal2.ElementAt(i));
            }

            file1.WriteLine("Impostor Scores Local" + "," + "Impostor Scores Manhattan" + "," + "Impostor Scores Euclidean");
            for (int i = 0; i < lAllImpostorScoresLocal.Count; ++i)
            {
                file1.WriteLine(lAllImpostorScoresLocal.ElementAt(i) + "," + lAllImpostorScoresGlobal.ElementAt(i) + "," + lAllImpostorScoresGlobal2.ElementAt(i));
            }

            file1.Close();

        }
    }
}
