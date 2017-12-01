using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureUtils.ErrorCalculation
{
    public class ErrorCalcSScore : Base.ErrorCalculation
    {
        public ErrorCalcSScore() : base()
        {

        }

        public override void CalculateErrors(List<double> aOriginalScores, List<double> aImpostorScores, double aNumIntervals)
        {
            base.CalculateErrors(aOriginalScores, aImpostorScores, aNumIntervals);
        } 
    }
}
