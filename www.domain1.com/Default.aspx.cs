﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SSOLib;

public partial class _Default : PrivatePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CurrentUser != null)
        {
            lblUser.Text = string.Format("'{0} {1}'", CurrentUser.FirstName, CurrentUser.LastName);
            lblServer.Text = Request.Url.Host;
        }
    }

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        Logout();
    }
}
