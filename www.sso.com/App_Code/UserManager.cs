using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;

/// <summary>
/// Summary description for UserManager
/// </summary>
public class UserManager
{
	public UserManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// Authenticates user from the system. A hard-coded logic is used for demonstration
    /// </summary>
    /// <param name="UserName"></param>
    /// <param name="Password"></param>
    /// <returns></returns>
    public static WebUser AuthenticateUser(string UserName, string Password)
    {
        WebUser user = null;

        DbManager DBConnecttion = new DbManager();
        List<WebUser> z = DBConnecttion.AuthenticateUser(UserName, Password);

        if(z.Count == 1)
        {
            user = z[0];
            if(z[0].Token == "")
            {
              user.Token =  DBConnecttion.GenerateTokenForUser(z[0].UniqueId, double.Parse( System.Configuration.ConfigurationManager.AppSettings["AUTH_COOKIE_TIMEOUT_IN_MINUTES"]));
            }
        }

 
        if (user != null)
        {
          //  user.Token = Utility.GetGuidHash(user.Token);
        }
        return user;
    }

   
    /// <summary>
    /// Retrieves a user form the system. A hard-coded logic is used for demonstration
    /// </summary>
    /// <param name="UniqueId"></param>
    /// <returns></returns>
    public static WebUser GetWebUserByUniqueId(string UniqueId)
    {
        WebUser user = null;

        DbManager DBConnecttion = new DbManager();
        List<WebUser> z = DBConnecttion.GetWebUserByUniqueId(UniqueId);

        if (z.Count == 1)
        {
            user = z[0];
        }

        return user;
    }


}
