using AbstractionLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureUtils
{
    public static class SignatureFileUtils
    {
        public static Signature GetSignatureFromFile(string aPath)
        {
            Signature lSig = new Signature();

            using (StreamReader lReader = new StreamReader(aPath))
            {
                string lCurrentLine = lReader.ReadLine();

                while ((lCurrentLine = lReader.ReadLine()) != null)
                {
                    String[] lValues = lCurrentLine.Split(',');
                    Point lPoint = new Point(double.Parse(lValues[0], System.Globalization.CultureInfo.InvariantCulture),
                                             double.Parse(lValues[1], System.Globalization.CultureInfo.InvariantCulture),
                                             double.Parse(lValues[2], System.Globalization.CultureInfo.InvariantCulture),
                                             double.Parse(lValues[3], System.Globalization.CultureInfo.InvariantCulture));

                    lSig.Add(lPoint);
                }
            }

            return lSig;
        }

        public static Signature GetSignatureFromFileWithCharacteristics(string aPath)
        {
            Signature lSig = new Signature();

            using (StreamReader lReader = new StreamReader(aPath))
            {
                string lCurrentLine = lReader.ReadLine();

                while ((lCurrentLine = lReader.ReadLine()) != null)
                {
                    String[] lValues = lCurrentLine.Split(',');
                    Point lPoint = new Point(double.Parse(lValues[0], System.Globalization.CultureInfo.InvariantCulture),
                                             double.Parse(lValues[1], System.Globalization.CultureInfo.InvariantCulture),
                                             double.Parse(lValues[2], System.Globalization.CultureInfo.InvariantCulture),
                                             double.Parse(lValues[3], System.Globalization.CultureInfo.InvariantCulture),
                                             double.Parse(lValues[4], System.Globalization.CultureInfo.InvariantCulture),
                                             double.Parse(lValues[5], System.Globalization.CultureInfo.InvariantCulture),
                                             double.Parse(lValues[6], System.Globalization.CultureInfo.InvariantCulture),
                                             double.Parse(lValues[7], System.Globalization.CultureInfo.InvariantCulture),
                                             double.Parse(lValues[8], System.Globalization.CultureInfo.InvariantCulture),
                                             double.Parse(lValues[9], System.Globalization.CultureInfo.InvariantCulture));

                    lSig.Add(lPoint);
                }
            }

            return lSig;
        }

        public static int GetNumberOfSignaturesFromFolder(string aPath)
        {
            var fileCount = (from file in Directory.EnumerateFiles(aPath, "*.csv", SearchOption.TopDirectoryOnly)
                             select file).Count();
            return fileCount;
        }

        public static List<Signature> GetAllSignaturesFromFolder(string aFolderName, bool aAllCharacteristics = false)
        {
            List<Signature> lSignatures = new List<Signature>();

            if (aAllCharacteristics)
            {
                try
                {
                    foreach (string fileName in Directory.GetFiles(aFolderName, "*.csv"))
                    {
                        var lSignature = GetSignatureFromFileWithCharacteristics(fileName);
                        lSignature.Filename = fileName;
                        lSignatures.Add(lSignature);
                    }
                }
                catch(Exception ex)
                {

                }
               
            }
            else
            {
                try
                {
                    foreach (string fileName in Directory.GetFiles(aFolderName, "*.csv"))
                    {
                        var lSignature = GetSignatureFromFile(fileName);
                        lSignatures.Add(lSignature);
                    }
                }
                catch(Exception ex)
                {

                }
               
            }
            

            return lSignatures;
        }

        public static void SaveSignatureToFile(Signature aSig, string aFileName)
        {
            //TODO: if folders do not exist, create them
            StreamWriter file = new StreamWriter(aFileName);

            file.WriteLine("X" + "," + "Y" + "," + "Time" + "," + "Force");

            foreach (var element in aSig)
            {
                file.WriteLine(element.X + ", " + element.Y + ", " + element.Time + "," + element.Force);
            }

            file.Close();
        }

        public static void SaveSignatureWithCharacteristicsToFile(Signature aSig, string aFileName)
        {
            //TODO: if folders do not exist, create them
            StreamWriter file = new StreamWriter(aFileName);

            file.WriteLine("X" + "," + "Y" + "," + "X1" + "," + "Y1" + "," + "X2" + "," + "Y2" + "," + "Time" + "," + "Force" + "," + "Force1" + "," + "PathVelocity");

            foreach (var element in aSig)
            {
                file.WriteLine(element.X + ", " + element.Y + ", " + element.X1 + ", " + element.Y1 + ", " + element.X2 + ", " +element.Y2 + ", " + element.Time + "," + element.Force + ", " + element.Force1 + ", " + element.PathVelocity);
            }

            file.Close();
        }

        public static void SaveDTWResultsToFile(List<double> aListOfScores, StreamWriter aFile)
        {
            aFile.WriteLine("Score");

            foreach (var element in aListOfScores)
            {
                aFile.WriteLine(element);
            }

            aFile.Close();
        }


        public static void SaveFeatureSetToFile(SignatureFeatures aFeatures, string aFileName)
        {
            if (! File.Exists(aFileName))
            {
                StreamWriter file = new StreamWriter(aFileName);

                file.WriteLine("T" + "," + "NumberOfStrokes" + "," + "TTouch" + "," + "MeanX" + "," + "MeanX1" + "," + "MeanY" + "," + "MeanY1" + "," + "MeanP" + "," + "MeanP1");
                file.WriteLine(aFeatures["TotalSignatureDuration"] + "," + aFeatures["NumberOfStrokes"] + "," + aFeatures["TTouch"]
                                + "," + aFeatures["MeanX"] + "," + aFeatures["MeanX1"] + "," + aFeatures["MeanY"] + "," + aFeatures["MeanY1"]
                                + "," + aFeatures["MeanP"] + "," + aFeatures["MeanP1"]) ;

                file.Close();
            }
            else
            {
                StreamWriter file = new StreamWriter(aFileName, append: true);
                file.WriteLine(aFeatures["TotalSignatureDuration"] + "," + aFeatures["NumberOfStrokes"] + "," + aFeatures["TTouch"]
                                + "," + aFeatures["MeanX"] + "," + aFeatures["MeanX1"] + "," + aFeatures["MeanY"] + "," + aFeatures["MeanY1"]
                                + "," + aFeatures["MeanP"] + "," + aFeatures["MeanP1"]);
                file.Close();
            }
            
        }


        public static List<SignatureFeatures> GetFeaturesFromFile(string aFileName)
        {
            List<SignatureFeatures> lFeaturesList = new List<SignatureFeatures>();

            using (StreamReader lReader = new StreamReader(aFileName))
            {
                SignatureFeatures lFeatures = new SignatureFeatures();

                string lLine = lReader.ReadLine();

                while ((lLine = lReader.ReadLine()) != null)
                {
                    var lSplittedLine = lLine.Split(',');

                    lFeatures["TotalSignatureDuration"] = double.Parse(lSplittedLine[0], System.Globalization.CultureInfo.InvariantCulture);
                    lFeatures["NumberOfStrokes"] = double.Parse(lSplittedLine[1], System.Globalization.CultureInfo.InvariantCulture);
                    lFeatures["TTouch"] = double.Parse(lSplittedLine[2], System.Globalization.CultureInfo.InvariantCulture);
                    lFeatures["MeanX"] = double.Parse(lSplittedLine[3], System.Globalization.CultureInfo.InvariantCulture);
                    lFeatures["MeanX1"] = double.Parse(lSplittedLine[4], System.Globalization.CultureInfo.InvariantCulture);
                    lFeatures["MeanY"] = double.Parse(lSplittedLine[5], System.Globalization.CultureInfo.InvariantCulture);
                    lFeatures["MeanY1"] = double.Parse(lSplittedLine[6], System.Globalization.CultureInfo.InvariantCulture);
                    lFeatures["MeanP"] = double.Parse(lSplittedLine[7], System.Globalization.CultureInfo.InvariantCulture);
                    lFeatures["MeanP1"] = double.Parse(lSplittedLine[8], System.Globalization.CultureInfo.InvariantCulture);

                    lFeaturesList.Add(lFeatures);
                }                            
            }
            return lFeaturesList;
        }
    }
}
