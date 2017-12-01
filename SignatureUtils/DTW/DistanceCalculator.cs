using AbstractionLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureUtils
{
    public class DistanceCalculator
    {
        public static double CalculateDistance(Point aPoint1, Point aPoint2, Dictionary<string, bool> aConfiguration)
        {
            double lDistanceOneCoord = 0.0;
            double lDistance = 0.0;

            foreach (var element in aConfiguration)
            {
                if(element.Value == true)
                {
                    switch (element.Key)
                    {
                        case "UseXY":
                            lDistanceOneCoord += Math.Pow((aPoint2.X - aPoint1.X), 2) + Math.Pow((aPoint2.Y - aPoint1.Y), 2);
                            break;
                        case "UseX1Y1":
                            lDistanceOneCoord += Math.Pow((aPoint2.X1 - aPoint1.X1), 2) + Math.Pow((aPoint2.Y1 - aPoint1.Y1), 2);
                            break;
                        case "UseX2Y2":
                            lDistanceOneCoord += Math.Pow((aPoint2.X2 - aPoint1.X2), 2) + Math.Pow((aPoint2.Y2 - aPoint1.Y2), 2);
                            break;
                        case "UseForce":
                            lDistanceOneCoord += Math.Pow((aPoint2.Force - aPoint1.Force), 2);
                            break;
                        case "UseForce1":
                            lDistanceOneCoord += Math.Pow((aPoint2.Force1 - aPoint1.Force1), 2);
                            break;
                        case "UsePathVelocity":
                            lDistanceOneCoord += Math.Pow((aPoint2.PathVelocity - aPoint1.PathVelocity), 2);
                            break;
                        case "UsePathVelocity1":
                            lDistanceOneCoord += Math.Pow((aPoint2.PathVelocity1 - aPoint1.PathVelocity1), 2);
                            break;
                        case "UseTheta":
                            lDistanceOneCoord += Math.Pow((aPoint2.Theta - aPoint1.Theta), 2);
                            break;
                        default:
                            break;
                    }
                }
            }

            lDistance = Math.Sqrt(lDistanceOneCoord);

            return lDistance;
        }

        public static double Min(double x, double y, double z)
        {
            return Math.Min(x, Math.Min(y, z));
        }
    }
}
