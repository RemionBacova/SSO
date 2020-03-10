using SSOLib.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSOLib
{
    class UserRights
    {
        AuthService service = new AuthService();

        public static AuthUtil Instance
        {
            get
            {
                return new AuthUtil();
            }
        }
        public String GetUsrRights(string Token,string platformId)
        {

            string json = service.GetUserRights(Token,platformId);

            return json;
        }
    }
}
