using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

/// <summary>
/// Summary description for UserRights
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class UserRights : System.Web.Services.WebService
{

    public UserRights()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]//Specify return format.
    public string GetUserRights(string token, string platformId)
    {
        JavaScriptSerializer ser = new JavaScriptSerializer();
        DbManager z = new DbManager();
        List<WebUserRights> returnObject = z.GetUserRightsOnPlatform(token, platformId);
        string returnjson= ser.Serialize(returnObject);
        return returnjson;
    }

}
