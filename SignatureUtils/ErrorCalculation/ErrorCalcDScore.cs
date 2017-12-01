using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureUtils.ErrorCalculation
{
    public class ErrorCalcDScore : Base.ErrorCalculation
    {
        public ErrorCalcDScore() : base()
        {
            
        }

        public override void CalculateErrors(List<double> aOriginalScores, List<double> aImpostorScores, double aNumIntervals)
        {
            List<double> lSOriginalScores = SignatureUtils.GetSScores(aOriginalScores);
            List<double> lSImpostorScores = SignatureUtils.GetSScores(aImpostorScores);

            base.CalculateErrors(lSOriginalScores, lSImpostorScores, aNumIntervals);
        }
    }
}
