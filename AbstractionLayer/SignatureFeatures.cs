using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractionLayer
{
    public class SignatureFeatures : Dictionary<string, double>
    {
        public SignatureFeatures() : base()
        {
            this.Add("TotalSignatureDuration", 0);
            this.Add("NumberOfStrokes", 0);
            //this.Add("TTouch", 0);
            //this.Add("MeanX", 0);
            this.Add("AverageVelocityX", 0);
            this.Add("MaxVelocityX", 0);
            //this.Add("MeanX2", 0);
            //this.Add("MeanY", 0);
            this.Add("AverageVelocityY", 0);
            this.Add("MaxVelocityY", 0);
            //this.Add("MeanY2", 0);
            this.Add("MeanP", 0);
            //this.Add("MeanP1", 0);
            this.Add("SignChangesX", 0);
            this.Add("SignChangesX1", 0);
            this.Add("SignChangesX2", 0);
            this.Add("SignChangesY", 0);
            this.Add("SignChangesY1", 0);
            this.Add("SignChangesY2", 0);
            this.Add("SignChangesP", 0);
            this.Add("MaxVelocity", 0);
            this.Add("MaxP", 0);
            this.Add("AveragePointWiseVelocity", 0);
            this.Add("AveragePointWiseAcceleration", 0);
        }
    }
}
