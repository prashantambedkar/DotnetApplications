<%@ Application Language="C#" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        log4net.Config.XmlConfigurator.Configure();
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
        try
        {
            if (Session["userid"] != null)
            {
                Response.Redirect("Login.aspx");
            }

            else
            {
                Page p = new Page();
                //p.ClientScript.RegisterClientScriptBlock(this.GetType(), "Error", "javascript:alert('Session Timeout!')", true);
                //p.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No records found.');", true);
                Response.Redirect("Login.aspx");

            }
        }
        catch (Exception ex)
        {

            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Global.asax", "DeleteOldData", Server.MapPath("~/ErrorLog.txt").ToString());
            Response.Redirect("Login.aspx");
        }

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.        

        //System.Web.Security.FormsAuthentication.SignOut();
        //System.Web.Security.FormsAuthentication.RedirectToLoginPage();
    }

</script>
