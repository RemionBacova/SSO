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
/// A sample User entity class
/// </summary>
public class WebUser
{
    public WebUser()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string UniqueId
    {
        get;
        set;
    }
    public string Token
    {
        get;
        set;
    }

    public string UserName
    {
        get;
        set;
    }

    public string FirstName
    {
        get;
        set;
    }

    public string LastName
    {
        get;
        set;
    }
}

public class WebUserRights
{
    public string CustomerId
    {
        get;
        set;
    }
    public string CustomerName
    {
        get;
        set;
    }

    public List<UserRightsObject> UserRightsVector
    {
        get;
        set;
    }

}

public class UserRightsObject
{
    public string RightType
    {
        get;
        set;
    }
    public string Right
    {
        get;
        set;
    }
}

public class CustomerID
{
    public string ID
    {
        get;
        set;
    }
}
