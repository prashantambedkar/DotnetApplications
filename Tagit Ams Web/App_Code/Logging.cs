using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Logging
/// </summary>
/// 
public class Logging
{

    public enum LogLevelL4N
    {
        DEBUG = 1,
        ERROR,
        FATAL,
        INFO,
        WARN
    }

    //string path = Server.MapPath("~/ErrorLog.txt");
    #region Members
    private static readonly ILog logger = LogManager.GetLogger(typeof(Logging));
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion

    #region Constructors
    static Logging()
    {
        log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("C:\\log4nett.txt"));
        ////XmlConfigurator.Configure();
        //    log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo
        //                                        (System.IO.Path.GetDirectoryName

        //(System.Reflection.Assembly.GetExecutingAssembly().Location)));
    }
    #endregion

    #region Methods

    public static void WriteLog(LogLevelL4N logLevel, String log, String Method, String Class)
    {
        try
        {

            if (logLevel.Equals(LogLevelL4N.DEBUG))
            {
                logger.Debug(Method + ", " + Class + ", " + log);
            }

            else if (logLevel.Equals(LogLevelL4N.ERROR))
            {
                logger.Error(Method + ", " + Class + ", " + log);
            }

            else if (logLevel.Equals(LogLevelL4N.FATAL))
            {
                logger.Fatal(Method + ", " + Class + ", " + log);
            }

            else if (logLevel.Equals(LogLevelL4N.INFO))
            {
                logger.Info(Method + ", " + Class + ", " + log);
            }

            else if (logLevel.Equals(LogLevelL4N.WARN))
            {
                logger.Warn(Method + ", " + Class + ", " + log);
            }
        }
        catch
        {
        }
    }

    public static void WriteErrorLog(string Message, string StackTrace, string SourcePage, string function, string path)
    {
        try
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", Message);
            message += Environment.NewLine;
            message += string.Format("StackTrace: {0}", StackTrace);
            message += Environment.NewLine;
            message += string.Format("SourcePage: {0}", SourcePage);
            message += Environment.NewLine;
            message += string.Format("Targetfunction: {0}", function);
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            //path=
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }
        catch
        {
        }
    }

    #endregion
}