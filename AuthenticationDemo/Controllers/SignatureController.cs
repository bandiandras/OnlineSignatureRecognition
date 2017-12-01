using AbstractionLayer;
using AngularJSWebApiEmpty.DTO;
using SignatureUtils;
using SignatureUtils.DTW;
using SignatureUtils.Feature_Based;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace AngularJSWebApiEmpty.Controllers
{
    [RoutePrefix("api/Signature")]
    public class SignatureController : ApiController
    {
        /// <summary>
        /// Used to save signatures to the server.
        /// Processes the encoded string, calculates characterisitcs and saves the signature to the server
        /// </summary>
        /// <param name="aSig"></param>
        [HttpPost]
        [Route("SaveSignature")]
        public int SaveSignature([FromBody]SignatureDTO aSig)
        {
            //Get signature from the Base64Encoded string
            SignatureData lSignatureData = SignatureUtils.SignatureUtils.GetSignatureFromBase64String(aSig.Signature, aSig.Email);

            var lDirectory = "d:\\Signatures\\" + lSignatureData.Email + "\\";

            if (!System.IO.Directory.Exists(lDirectory))
            {
                System.IO.Directory.CreateDirectory(lDirectory);
            }

            var fileName = lDirectory + DateTime.Now.Ticks.ToString() + ".csv";
            var lFeaturesFileName = lDirectory + "\\SignatureFeatures\\" + "SignatureFeatures.csv";

            //Calculate characteristics of the received signature and standardize it
            Signature lSigWithCharacteristics = SignatureUtils.SignatureUtils.CalculateCharacteristics(lSignatureData.Signature);
            lSigWithCharacteristics = SignatureUtils.SignatureUtils.StandardizeSignature(lSigWithCharacteristics);

            //Create DTW configuration for the quality evaluation
            DTWConfiguration lDTWConfig = new DTWConfiguration()
                                                 .UseXY()
                                                 .UseX1Y1();

            //Check the quality of the signature. If it is a poor quality signature, request it once again
            bool lResult = SignatureQualityEvaluation.CheckQuality(lDirectory, lSigWithCharacteristics, lDTWConfig);
            if (lResult)
            {
                //Save function based data to csv file
                SignatureFileUtils.SaveSignatureWithCharacteristicsToFile(lSigWithCharacteristics, fileName);

                try
                {
                    //Save feature based data to csv file
                    SignatureFeatures lFEatures = FeatureCalculator.CalculateFeatures(lSigWithCharacteristics);

                    if (!System.IO.Directory.Exists(lDirectory + "\\SignatureFeatures\\"))
                    {
                        System.IO.Directory.CreateDirectory(lDirectory + "\\SignatureFeatures\\");
                    }

                    SignatureFileUtils.SaveFeatureSetToFile(lFEatures, lFeaturesFileName);

                }
                catch(Exception ex)
                {
                    //If an error occurs, do nothing
                    //An error can occur in case of the pressure related features, beacause some devices don't return the pressure value
                }

                return 1;
            }
            else
            {
                return 0;
            }

        }


        /// <summary>
        /// Compares a signature to the template at logging in
        /// </summary>
        /// <param name="aSig"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CheckSignature")]
        public bool CheckSignature([FromBody]SignatureDTO aSig)
        {
            SignatureData lSignatureData = SignatureUtils.SignatureUtils.GetSignatureFromBase64String(aSig.Signature, aSig.Email); 

            Signature lSigWithCharacteristics = SignatureUtils.SignatureUtils.CalculateCharacteristics(lSignatureData.Signature);
            lSigWithCharacteristics = SignatureUtils.SignatureUtils.StandardizeSignature(lSigWithCharacteristics);

            var lDirectory = "d:\\Signatures\\" + lSignatureData.Email + "\\";

            //Get all signatures from folder. True flag indicates, that all characteristics we are working with are stored in the file, these will be loaded too
            List<Signature> lSignatures = SignatureFileUtils.GetAllSignaturesFromFolder(lDirectory, true);

            DTWConfiguration lDTWConfig = new DTWConfiguration()
                                                .UseXY()
                                                .UseX1Y1()
                                                .UseX2Y2()
                                                .UseForce()
                                                .UseForce1()
                                                .UsePathVelocity();

            //Check for NaN values at Force and Force1 characrteristics.
            SignatureUtils.SignatureUtils.CheckForNaN(lSigWithCharacteristics, ref lDTWConfig);

            return SignatureQualityEvaluation.Authenticate(lDirectory, lSigWithCharacteristics, lDTWConfig, 0.05);        
        }
        
        
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="aUser"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("ResetEnrollment")]
        //public bool ResetEnrollmentProcess([FromBody] string aUser)
        //{
        //    var lDirectory = "d:\\Signatures\\" + aUser + "\\";

        //    //delete the directory with all of its content
        //    //Don't user hardcoded value, get it from config
        //    if(SignatureFileUtils.GetNumberOfSignaturesFromFolder(lDirectory) < 10)
        //    {
        //        try
        //        {
        //            Directory.Delete(lDirectory, true);
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}
