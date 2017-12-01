using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureUtils
{
    public static class ErrorCalculationFactory
    {
        public static Base.ErrorCalculation GetSScoreErrorCalculator()
        {
            return new ErrorCalculation.ErrorCalcSScore();
        }

        public static Base.ErrorCalculation GetDScoreErrorCalculator()
        {
            return new ErrorCalculation.ErrorCalcDScore();
        }
    }
}
