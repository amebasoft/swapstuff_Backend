using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwapStff.Infrastructure
{
    public static class SaveLog
    {
        public static string serverpath;
        //write the log in file
        public static void LogError(string ErrorMsg)
        {
            if (ErrorMsg != null)
            {

                System.IO.StreamWriter oWriter = new System.IO.StreamWriter(serverpath, true);
                oWriter.WriteLine(System.DateTime.Now.ToString() + ": " + ErrorMsg);
                oWriter.Close();
                oWriter = null;

            }
        }

    }
}