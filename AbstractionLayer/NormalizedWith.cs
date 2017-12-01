using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractionLayer
{
    public class NormalizedWith
    {
        private double mMin;
        private double mMax;

        public double Min
        {
            get
            {
                return mMin;
            }
            set
            {
                mMin = value;
            }
        }

        public double Max
        {
            get
            {
                return mMax;
            }
            set
            {
                mMax = value;
            }
        }

        public NormalizedWith()
        {

        }

        public NormalizedWith(double aMin, double aMax)
        {
            mMin = aMin;
            mMax = aMax;
        }
    }
}
