// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Web.Profile;

public partial class Header : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if( Context.Profile.IsAnonymous )
        {
            LoginLinkButton.Visible = true;
            LogoutLinkButton.Visible = false;
            AccountLinkButton.Visible = false;
        }
        else
        {
            LoginLinkButton.Visible = false;
            LogoutLinkButton.Visible = true;
            AccountLinkButton.Visible = true;
            UserNameLabel.Text = Profile.UserName + " | ";
            UserNameLabel.Visible = true;
        }
    }
}