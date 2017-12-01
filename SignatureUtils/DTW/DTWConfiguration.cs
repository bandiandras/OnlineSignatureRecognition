using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureUtils.DTW
{
    public class DTWConfiguration
    {
        private Dictionary<string, bool> mConfiguration;

        public DTWConfiguration()
        {
            mConfiguration = new Dictionary<string, bool>();
            mConfiguration.Add("UseXY", false);
            mConfiguration.Add("UseX1Y1", false);
            mConfiguration.Add("UseX2Y2", false);
            mConfiguration.Add("UseForce", false);
            mConfiguration.Add("UseForce1", false);
            mConfiguration.Add("UsePathVelocity", false);
            mConfiguration.Add("UsePathVelocity1", false);
            mConfiguration.Add("UseTheta", false);
        }

        public DTWConfiguration UseXY()
        {
            mConfiguration["UseXY"] = true;
            return this;
        }

        public DTWConfiguration UseX1Y1()
        {
            mConfiguration["UseX1Y1"] = true;
            return this;
        }

        public DTWConfiguration UseX2Y2()
        {
            mConfiguration["UseX2Y2"] = true;
            return this;
        }

        public DTWConfiguration UseForce()
        {
            mConfiguration["UseForce"] = true;
            return this;
        }

        public DTWConfiguration UseForce1()
        {
            mConfiguration["UseForce1"] = true;
            return this;
        }

        public DTWConfiguration UsePathVelocity()
        {
            mConfiguration["UsePathVelocity"] = true;
            return this;
        }

        public DTWConfiguration UsePathVelocity1()
        {
            mConfiguration["UsePathVelocity1"] = true;
            return this;
        }

        public DTWConfiguration UseTheta()
        {
            mConfiguration["UseTheta"] = true;
            return this;
        }

        public Dictionary<string, bool> GetConfiguration()
        {
            return mConfiguration;
        }

        public void SetConfiguration(Dictionary<string, bool> aConfig)
        {
            mConfiguration = aConfig;
        }
    }
}
