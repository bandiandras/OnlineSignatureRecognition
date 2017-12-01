using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularJSWebApiEmpty.DTO
{
    public class SignatureDTO
    {
        private string mEmail;
        private string mSignature;

        public SignatureDTO()
        {

        }

        public SignatureDTO(string aEmail, string aSignature)
        {
            mEmail = aEmail;
            mSignature = aSignature;
        }

        public string Signature
        {
            get
            {
                return mSignature;
            }
            set
            {
                mSignature = value;
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