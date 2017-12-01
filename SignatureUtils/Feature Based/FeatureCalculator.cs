using AbstractionLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureUtils.Feature_Based
{
    public static class FeatureCalculator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSig"></param>
        /// <returns></returns>
        public static SignatureFeatures CalculateFeatures(Signature aSig)
        {
            SignatureFeatures lFeatures = new SignatureFeatures();

            lFeatures["TotalSignatureDuration"] = CalculateT(aSig);
            //lFeatures["MeanX"] = aSig.Select(element => element.X).ToList().Average();
            lFeatures["AverageVelocityX"] = aSig.Select(element => element.X1).ToList().Average();
            lFeatures["MaxVelocityX"] = aSig.Select(element => element.X1).ToList().Max();
            //lFeatures["MeanX2"] = aSig.Select(element => element.X2).ToList().Average();
            //lFeatures["MeanY"] = aSig.Select(element => element.Y).ToList().Average();
            lFeatures["AverageVelocityY"] = aSig.Select(element => element.Y1).ToList().Average();
            lFeatures["MaxVelocityY"] = aSig.Select(element => element.Y1).ToList().Max();
            //lFeatures["MeanY2"] = aSig.Select(element => element.Y2).ToList().Average();
            lFeatures["MeanP"] = aSig.Select(element => element.Force).ToList().Average();
            //lFeatures["MeanP1"] = aSig.Select(element => element.Force1).ToList().Average();
            lFeatures["MaxVelocity"] = aSig.Select(element => element.PathVelocity).ToList().Max();
            lFeatures["MaxP"] = aSig.Select(element => element.Force).ToList().Max();
            lFeatures["AveragePointWiseVelocity"] = aSig.Select(element => element.PathVelocity).ToList().Average();
            lFeatures["AveragePointWiseAcceleration"] = aSig.Select(element => element.PathVelocity1).ToList().Average();
            CalculateAdditionalFeatures(aSig, ref lFeatures);

            return lFeatures;
        }

        private static double CalculateT(Signature aSig)
        {
            double lTime = 0;
            lTime = aSig.ElementAt(aSig.Count - 1).Time - aSig.ElementAt(0).Time;
            return lTime/1000;
        }

        private static void CalculateAdditionalFeatures(Signature aSig, ref SignatureFeatures aFeatures)
        {
            int lNumberOfStorkes = 0;
            double lTTouch = aFeatures["TotalSignatureDuration"];

            
            for(int i = 1; i < aSig.Count; ++i)
            {
                //
                double lTimeDiff = aSig.ElementAt(i).Time - aSig.ElementAt(i - 1).Time;
                if ( lTimeDiff > 70 )
                {
                    lNumberOfStorkes++;
                    lTTouch -= lTimeDiff;
                }

                if(i > 2 && i < aSig.Count - 1)
                {
                    if ((aSig.ElementAt(i).X - aSig.ElementAt(i - 1).X) > 0)
                    {
                        if ((aSig.ElementAt(i + 1).X - aSig.ElementAt(i).X) < 0)
                        {
                            aFeatures["SignChangesX"]++;
                        }
                    }
                    else
                    {
                        if ((aSig.ElementAt(i + 1).X - aSig.ElementAt(i).X) > 0)
                        {
                            aFeatures["SignChangesX"]++;
                        }
                    }

                    if ((aSig.ElementAt(i).X1 - aSig.ElementAt(i-1).X1) > 0)
                    {
                        if((aSig.ElementAt(i+1).X1 - aSig.ElementAt(i).X1) < 0)
                        {
                            aFeatures["SignChangesX1"]++;
                        }
                    }
                    else
                    {
                        if ((aSig.ElementAt(i + 1).X1 - aSig.ElementAt(i).X1) > 0)
                        {
                            aFeatures["SignChangesX1"]++;
                        }
                    }


                    if ((aSig.ElementAt(i).X2 - aSig.ElementAt(i - 1).X2) > 0)
                    {
                        if ((aSig.ElementAt(i + 1).X2 - aSig.ElementAt(i).X2) < 0)
                        {
                            aFeatures["SignChangesX2"]++;
                        }
                    }
                    else
                    {
                        if ((aSig.ElementAt(i + 1).X2 - aSig.ElementAt(i).X2) > 0)
                        {
                            aFeatures["SignChangesX2"]++;
                        }
                    }

                    if ((aSig.ElementAt(i).Y - aSig.ElementAt(i - 1).Y) > 0)
                    {
                        if ((aSig.ElementAt(i + 1).Y - aSig.ElementAt(i).Y) < 0)
                        {
                            aFeatures["SignChangesY"]++;
                        }
                    }
                    else
                    {
                        if ((aSig.ElementAt(i + 1).Y1 - aSig.ElementAt(i).Y1) > 0)
                        {
                            aFeatures["SignChangesY1"]++;
                        }
                    }

                    if ((aSig.ElementAt(i).Y1 - aSig.ElementAt(i - 1).Y1) > 0)
                    {
                        if ((aSig.ElementAt(i + 1).Y1 - aSig.ElementAt(i).Y1) < 0)
                        {
                            aFeatures["SignChangesY1"]++;
                        }
                    }
                    else
                    {
                        if ((aSig.ElementAt(i + 1).Y1 - aSig.ElementAt(i).Y1) > 0)
                        {
                            aFeatures["SignChangesY1"]++;
                        }
                    }

                    if ((aSig.ElementAt(i).Y2 - aSig.ElementAt(i - 1).Y2) > 0)
                    {
                        if ((aSig.ElementAt(i + 1).Y2 - aSig.ElementAt(i).Y2) < 0)
                        {
                            aFeatures["SignChangesY2"]++;
                        }
                    }
                    else
                    {
                        if ((aSig.ElementAt(i + 1).Y2 - aSig.ElementAt(i).Y2) > 0)
                        {
                            aFeatures["SignChangesY2"]++;
                        }
                    }


                    if ((aSig.ElementAt(i).Force - aSig.ElementAt(i - 1).Force) > 0)
                    {
                        if ((aSig.ElementAt(i + 1).Force - aSig.ElementAt(i).Force) < 0)
                        {
                            aFeatures["SignChangesP"]++;
                        }
                    }
                    else
                    {
                        if ((aSig.ElementAt(i + 1).Force - aSig.ElementAt(i).Force) > 0)
                        {
                            aFeatures["SignChangesP"]++;
                        }
                    }

                }
            }

            aFeatures["NumberOfStrokes"] = lNumberOfStorkes;
            //aFeatures["TTouch"] = lTTouch;
        }

    }
}
