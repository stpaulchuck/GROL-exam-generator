using System;
using System.Windows.Forms;
using System.IO;

namespace GrolTestPoolParser
{
    class clsErrorLogWriter
    {
        public string ErrorLogLocation
        {get;set;}

        public clsErrorLogWriter()
        {
            ErrorLogLocation = "";
        }

        public clsErrorLogWriter(string LogLocation)
        {
            ErrorLogLocation = LogLocation;
        }

        public void WriteErrorLog(string ErrorText)
        {
            if (ErrorLogLocation == "")
            {
                ErrorLogLocation = Application.StartupPath;
            }
            StreamWriter oWriter = new StreamWriter(ErrorLogLocation + "\\ErrorLog.txt", true);
            oWriter.WriteLine("\nError Occured " + DateTime.Now.ToLongDateString());
            oWriter.WriteLine("Error Text: " + ErrorText);
            oWriter.Flush();
            oWriter.Close();
            oWriter = null;
        }

    } // end class
} // end namespace
