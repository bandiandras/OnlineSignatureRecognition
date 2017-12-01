using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractionLayer
{
    public class SignatureFile
    {
        Signature mSig;
        string mFileName;

        public SignatureFile()
        {

        }

        public SignatureFile(Signature aSig, string aFileName)
        {
            mSig = aSig;
            mFileName = aFileName;
        }

        public Signature Signature
        {
            get
            {
                return mSig;
            }
            set
            {
                mSig = value;
            }
        }

        public string FileName
        {
            get
            {
                return mFileName;
            }
            set
            {
                mFileName = value;
            }
        }
    }
}
