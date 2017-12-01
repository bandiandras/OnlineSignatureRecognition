using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractionLayer
{
    public class Point
    {
        private double mX;
        private double mY;
        private double mX1;
        private double mY1;
        private double mX2;
        private double mY2;
        private double mTime;
        private double mForce;
        private double mForce1;
        private double mPathVelocity;
        private double mPathVelocity1;
        private double mTheta;

        public double X
        {
            get
            {
                return mX;
            }
            set
            {
                mX = value;
            }
        }

        public double Y
        {
            get
            {
                return mY;
            }
            set
            {
                mY = value;
            }
        }


        public double X1
        {
            get
            {
                return mX1;
            }
            set
            {
                mX1 = value;
            }
        }

        public double Y1
        {
            get
            {
                return mY1;
            }
            set
            {
                mY1 = value;
            }
        }


        public double X2
        {
            get
            {
                return mX2;
            }
            set
            {
                mX2 = value;
            }
        }

        public double Y2
        {
            get
            {
                return mY2;
            }
            set
            {
                mY2 = value;
            }
        }

        public double Time
        {
            get
            {
                return mTime;
            }
            set
            {
                mTime = value;
            }
        }

        public double Force
        {
            get
            {
                return mForce;
            }
            set
            {
                mForce = value;
            }
        }

        public double Force1
        {
            get
            {
                return mForce1;
            }
            set
            {
                mForce1 = value;
            }
        }

        public double PathVelocity
        {
            get
            {
                return mPathVelocity;
            }
            set
            {
                mPathVelocity = value;
            }
        }

        public double PathVelocity1
        {
            get
            {
                return mPathVelocity1;
            }
            set
            {
                mPathVelocity1 = value;
            }
        }

        public double Theta
        {
            get
            {
                return mTheta;
            }
            set
            {
                mTheta = value;
            }
        }
        public Point()
        {

        }


        public Point(double aX, double aY, double aTime)
        {
            mX = aX;
            mY = aY;
            mTime = aTime;
            mForce = 1;
        }

        public Point(double aX, double aY, double aTime, double aForce)
        {
            mX = aX;
            mY = aY;
            mTime = aTime;
            mForce = aForce;
        }

        public Point(double aX, double aY, double aTime, double aForce, double aPathVelocity)
        {
            mX = aX;
            mY = aY;
            mTime = aTime;
            mForce = aForce;
            mPathVelocity = aPathVelocity;
        }

        public Point(double aX, double aY, double aX1, double aY1, double aX2, double aY2, double aTime, double aForce, double aForce1, double aPathVelocity)
        {
            mX = aX;
            mY = aY;
            mX1 = aX1;
            mY1 = aY1;
            mX2 = aX2;
            mY2 = aY2;
            mTime = aTime;
            mForce = aForce;
            mForce1 = aForce1;
            mPathVelocity = aPathVelocity;
        }

        public Point(double aX, double aY, double aX1, double aY1, double aX2, double aY2, double aTime, double aForce, double aForce1, double aPathVelocity, double aPatheVelocity1)
        {
            mX = aX;
            mY = aY;
            mX1 = aX1;
            mY1 = aY1;
            mX2 = aX2;
            mY2 = aY2;
            mTime = aTime;
            mForce = aForce;
            mForce1 = aForce1;
            mPathVelocity = aPathVelocity;
            mPathVelocity1 = aPatheVelocity1;
        }
    }
}
