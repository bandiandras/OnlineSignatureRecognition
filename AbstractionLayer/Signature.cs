using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractionLayer
{
    public class Signature : List<Point>
    {
        private string mFilename;

        #region "Ititialization"
        public Signature() : base()
        {
        }
        #endregion

        public string Filename
        {
            get
            {
                return mFilename;
            }
            set
            {
                mFilename = value;
            }
        }

        #region "Public Functions"
        public List<Point> GetListOfPoints()
        {
            return this;
        }

        public void SetListOfPoints(List<Point> aPoints)
        {
            this.AddRange(aPoints);
        }
        #endregion
    }
}
