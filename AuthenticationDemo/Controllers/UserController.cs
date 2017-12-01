using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AngularJSWebApiEmpty.Controllers
{
    public class UserController : ApiController
    {
        [HttpGet]
        public int CheckIfUserExists([FromUri] string Email)
        {
            var lDirectory = "d:\\Signatures\\" + Email + "\\";

            if (!Directory.Exists(lDirectory))
            {
                return 0;
            }
            return 1;
        }
    }
}
