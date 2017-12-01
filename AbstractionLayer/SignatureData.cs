using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractionLayer
{
    public class SignatureData
    {
        Signature mSig;
        string mEmail;

        public SignatureData()
        {

        }

        public SignatureData(Signature aSig, string aEmail)
        {
            mSig = aSig;
            mEmail = aEmail;
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

        public string Email
        {
            get
            {
                return mEmail;
            }
            set
            {
                mEmail = value;
            }
        }
    }
}
