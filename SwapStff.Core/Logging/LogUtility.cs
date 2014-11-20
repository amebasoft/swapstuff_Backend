﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SwapStff.Core.Logging
{
    public static class LogUtility
    {
        public static void Log(Exception ex)
        {
            //
        }

        //http://dotnetdarren.wordpress.com/2010/07/29/logging-in-mvc-part-4-log4net/

        /// <summary>
        /// This methods formats an error message so that it is 
        /// in a nice format for the event log or other places
        /// </summary>
        /// <param name="x">The exception</param>
        /// <returns>A formatted error message</returns>
        public static  string BuildExceptionMessage(Exception x)
        {

            Exception logException = x;
            if (x.InnerException != null)
                logException = x.InnerException;

            string strErrorMsg = Environment.NewLine + "Error in Path :" + System.Web.HttpContext.Current.Request.Path;

            // Get the QueryString along with the Virtual Path
            strErrorMsg += Environment.NewLine + "Raw Url :" + System.Web.HttpContext.Current.Request.RawUrl;


            // Get the error message
            strErrorMsg += Environment.NewLine + "Message :" + logException.Message;

            // Source of the message
            strErrorMsg += Environment.NewLine + "Source :" + logException.Source;

            // Stack Trace of the error

            strErrorMsg += Environment.NewLine + "Stack Trace :" + logException.StackTrace;

            // Method where the error occurred
            strErrorMsg += Environment.NewLine + "TargetSite :" + logException.TargetSite;
            return strErrorMsg;
        }
    }
}