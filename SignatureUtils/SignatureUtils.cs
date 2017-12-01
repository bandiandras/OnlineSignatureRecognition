using AbstractionLayer;
using SignatureUtils.DTW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace SignatureUtils
{
    /// <summary>
    /// 
    /// </summary>
    public static class SignatureUtils
    {
        #region "Public Functions"

        public static SignatureData GetSignatureFromBase64String(string aEncodedSignature, string aEncodedEmail)
        {
            SignatureData lSignatureToReturn = new SignatureData();

            //decode string
            byte[] lData = Convert.FromBase64String(aEncodedSignature);
            string lDecodedString = Encoding.UTF8.GetString(lData);

            //parse json
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            dynamic lDecodedPoints =
                   json_serializer.DeserializeObject(lDecodedString);
            lData = Convert.FromBase64String(aEncodedEmail);

            Signature lDecodedSignature = new Signature();
            foreach (var point in lDecodedPoints)
            {
                try
                {
                    lDecodedSignature.Add(new Point(Convert.ToDouble(point["x"].ToString()), Convert.ToDouble(point["y"].ToString()), Convert.ToDouble(point["time"].ToString()), Convert.ToDouble(point["force"].ToString())));
                }
                catch (Exception ex)
                {
                    lDecodedSignature.Add(new Point(Convert.ToDouble(point["x"].ToString()), Convert.ToDouble(point["y"].ToString()), Convert.ToDouble(point["time"].ToString())));
                }
            }

            lSignatureToReturn.Signature = lDecodedSignature;
            lSignatureToReturn.Email = (string)json_serializer.DeserializeObject(Encoding.UTF8.GetString(lData));

            return lSignatureToReturn;
        }

        /// <summary>
        /// Force can be NaN in case of some device and browser combinations, becase it is not implemented in the Touch API.
        /// If there is a NaN value, and Force is present inthe DTW configuration, remove is.
        /// </summary>
        /// <param name="aSig"></param>
        /// <param name="aDTWConfig"></param>
        public static void CheckForNaN(Signature aSig, ref DTWConfiguration aDTWConfig)
        {
            Dictionary<string, bool> lConfig = aDTWConfig.GetConfiguration();
            if ((Double.IsNaN(aSig[3].Force) || Double.IsInfinity(aSig[3].Force)) && lConfig["UseForce"] == true)
            {
                lConfig["UseForce"] = false;
                lConfig["UseForce1"] = false;
                aDTWConfig.SetConfiguration(lConfig);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aSignature"></param>
        /// <returns></returns>
        public static T StandardizeSignature<T>(T aSignature) where T : List<Point>
        {
            List<double> lXofSignature = aSignature.Select(element => element.X).ToList();
            List<double> lYofSignature = aSignature.Select(element => element.Y).ToList();
            List<double> lX1ofSignature = aSignature.Select(element => element.X1).ToList();
            List<double> lY1ofSignature = aSignature.Select(element => element.Y1).ToList();
            List<double> lX2ofSignature = aSignature.Select(element => element.X2).ToList();
            List<double> lY2ofSignature = aSignature.Select(element => element.Y2).ToList();
            //List<double> lForceOfSignature = aSignature.Select(element => element.Force).ToList();
            //List<double> lForce1OfSignature = aSignature.Select(element => element.Force1).ToList();
            List<double> lPathVelocitySignature = aSignature.Select(element => element.PathVelocity).ToList();
            List<double> lPathVelocitySignature1 = aSignature.Select(element => element.PathVelocity1).ToList();

            lXofSignature = Standardize(lXofSignature);
            lX1ofSignature = Standardize(lX1ofSignature);
            lX2ofSignature = Standardize(lX2ofSignature);
            lYofSignature = Standardize(lYofSignature);
            lY1ofSignature = Standardize(lY1ofSignature);
            lY2ofSignature = Standardize(lY2ofSignature);
            //lForceOfSignature = Standardize(lForceOfSignature);
            //lForce1OfSignature = Standardize(lForce1OfSignature);
            lPathVelocitySignature = Standardize(lPathVelocitySignature);
            lPathVelocitySignature1 = Standardize(lPathVelocitySignature1);

            for (var j = 0; j < lXofSignature.Count; ++j)
            {
                var lStandardizedElement = aSignature.ElementAt(j);
                lStandardizedElement.X = lXofSignature.ElementAt(j);
                lStandardizedElement.X1 = lX1ofSignature.ElementAt(j);
                lStandardizedElement.X2 = lX2ofSignature.ElementAt(j);
                lStandardizedElement.Y = lYofSignature.ElementAt(j);
                lStandardizedElement.Y1 = lY1ofSignature.ElementAt(j);
                lStandardizedElement.Y2 = lY2ofSignature.ElementAt(j);
                //lStandardizedElement.Force = lForceOfSignature.ElementAt(j);
                //lStandardizedElement.Force1 = lForce1OfSignature.ElementAt(j);
                lStandardizedElement.PathVelocity = lPathVelocitySignature.ElementAt(j);
                lStandardizedElement.PathVelocity1 = lPathVelocitySignature1.ElementAt(j);

                aSignature.RemoveAt(j);
                aSignature.Insert(j, lStandardizedElement);
            }

            return aSignature;
        }

        public static Signature CalculateCharacteristics(Signature aSignature)
        {
            Signature lSignatureToReturn = new Signature();

            //Calculate first and second order differences
            var lElement = aSignature.ElementAt(1);
            var lElementBefore = aSignature.ElementAt(0);
            lElement.X1 = lElement.X - lElementBefore.X;
            lElement.Y1 = lElement.Y - lElementBefore.Y;
            lElement.Force1 = lElement.Force - lElementBefore.Force;
            lElement.Theta = Math.Atan2(lElement.X1, lElement.Y1);
            lElement.PathVelocity = Math.Sqrt(Math.Pow(lElement.X1, 2) + Math.Pow(lElement.Y1, 2));
            lElement.PathVelocity1 = 0;

            for (int i = 2; i < aSignature.Count; ++i)
            {
                lElement = aSignature.ElementAt(i);
                lElementBefore = aSignature.ElementAt(i-1);
                lElement.X1 = lElement.X - lElementBefore.X;
                lElement.Y1 = lElement.Y - lElementBefore.Y;
                lElement.X2 = lElement.X1 - lElementBefore.X1;
                lElement.Y2 = lElement.Y1 - lElementBefore.Y1;
                lElement.Force1 = lElement.Force - lElementBefore.Force;
                lElement.Theta = Math.Atan2(lElement.X1, lElement.Y1);
                lElement.PathVelocity = Math.Sqrt(Math.Pow(lElement.X1, 2) + Math.Pow(lElement.Y1, 2));
                lElement.PathVelocity1 = lElement.PathVelocity - lElementBefore.PathVelocity;
                lSignatureToReturn.Add(lElement);
            }

            return lSignatureToReturn;
        }

        /// <summary>
        /// Standardize a list specified in the parameter
        /// </summary>
        /// <param name="aList"></param>
        /// <returns></returns>
        public static List<double> Standardize(List<double> aList)
        {
            List<double> lListToReturn = new List<double>();
            double lAvg = aList.Average();

            //calculate standard deviations of x and y values
            double lStandardDeviation = CalculateStandardDeviation(aList);

            foreach (var point in aList)
            {
                double lElement = (point - lAvg) / lStandardDeviation;
                lListToReturn.Add(lElement);
            }

            return lListToReturn;
        }

        /// <summary>
        /// Applies the min/max normalization to a list
        /// </summary>
        /// <param name="aList"></param>
        /// <returns></returns>
        public static List<double> MinMaxNormalization(List<double> aList)
        {
            List<double> lListAfterNormalization = new List<double>();
            double lMin = aList.Min();
            double lMax = aList.Max();


            for (int i = 0; i < aList.Count; ++i)
            {
                if (lMin != lMax)
                {
                    lListAfterNormalization.Add(2 * ((aList.ElementAt(i) - lMin) / (lMax - lMin)) - 1);
                }
                else
                {
                    lListAfterNormalization.Add(lMax);
                }
                
            }

            return lListAfterNormalization;
        }
                

        /// <summary>
        /// Compares each element of the aSignatures parameter to each element of aTemplate. 
        /// One signature gets compared to all elements of the template, then an average will be calcutated
        /// </summary>
        /// <param name="aTemplate"></param>
        /// <param name="aSignatures"></param>
        /// <param name="aDTWConfig"></param>
        public static List<double> CompareSignaturesDTW(List<Signature> aTemplate, List<Signature> aSignatures, DTWConfiguration aDTWConfig)
        {
            List<double> lScores = new List<double>();

            foreach (Signature sig in aSignatures)
            {
                int lCounter = 0;
                double lScore = 0;
                foreach (Signature refSig in aTemplate)
                {
                    lScore += ConfigurableDTW.DTWDistance(refSig, sig, aDTWConfig);
                    ++lCounter;
                }
                lScore /= lCounter;
                lScores.Add(lScore);
            }
            return lScores;
        }

        /// <summary>
        /// Compares one signature to all signatures of a template
        /// </summary>
        /// <param name="aTemplate"></param>
        /// <param name="aSignature"></param>
        /// <param name="aDTWConfig"></param>
        /// <returns></returns>
        public static double CompareSignatureDTW(List<Signature> aTemplate, Signature aSignature, DTWConfiguration aDTWConfig)
        {
            int lCounter = 0;
            double lScore = 0;
            foreach (Signature refSig in aTemplate)
            {
                lScore += ConfigurableDTW.DTWDistance(refSig, aSignature, aDTWConfig);
                ++lCounter;
            }
            lScore /= lCounter;

            return lScore;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aDScores"></param>
        /// <returns></returns>
        public static List<double> GetSScores(List<double> aDScores)
        {
            List<double> lSScores = new List<double>();
            foreach (double score in aDScores)
            {
                lSScores.Add(1/(1 + score));
            }
            return lSScores;
        }

        public static double CalculateStandardDeviation(List<double> valueList)
        {
            double ret = 0;
            if (valueList.Count() > 0)
            {
                //Compute the Average      
                double avg = valueList.Average();
                //Perform the Sum of (value-avg)_2_2      
                double sum = valueList.Sum(d => Math.Pow(d - avg, 2));
                //Put it all together      
                ret = Math.Sqrt((sum) / (valueList.Count() - 1));
            }
            return ret;
        }
        #endregion
    }
}