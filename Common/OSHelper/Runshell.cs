using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Common.FileOSHelpersystem
{
    public static class Runshell
    {
        public static string RunScript(string filename)
        {
            ProcessStartInfo processiinfo = new ProcessStartInfo();
            processiinfo.FileName = filename;
            processiinfo.UseShellExecute = false;
            processiinfo.RedirectStandardOutput = true;
            Process process = Process.Start(processiinfo);
            string strOutput = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            Console.WriteLine(strOutput);
            return strOutput;
        }

    }    
}
