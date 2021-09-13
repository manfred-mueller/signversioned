using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Deployment.WindowsInstaller;

namespace signversioned
{
    public class MsiProperties
    {
        public static string Get(string msi, string name)
        {
            using (Database db = new Database(msi))
            {
                return db.ExecuteScalar("SELECT `Value` FROM `Property` WHERE `Property` = '{0}'", name) as string;
            }
        }
    }
    class Mainclass
    {
        public static bool ExistsInPath(string fileName)
        {
            return GetFullPath(fileName) != null;
        }

        public static string GetFullPath(string fileName)
        {
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(Path.PathSeparator))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                    return fullPath;
            }
            return null;
        }
        static void Main(string[] args)
        {
            if (!ExistsInPath("signtool.exe"))
            {
                Console.WriteLine(Properties.Resources.ErrorSigntoolExeNotFoundInPath);
                return;
            }
            
            if (args.Length != 1)
            {
                Console.WriteLine(Properties.Resources.UsageSignversionedExeTarget);
                return;
            }
            if (!args[0].Equals("params") && !File.Exists(args[0]))
            {
                Console.WriteLine(Properties.Resources.Error0NotFound, args[0]);
                return;
            }

            if (args[0].Equals("params"))
            {
                Console.WriteLine(Properties.Resources.PleaseKeyInTheSignerSNameN);
                Properties.Settings.Default.SignedBy = Console.ReadLine();
                Properties.Settings.Default.Save();
                Console.WriteLine(Properties.Resources.NPleaseKeyInTheTimeServerN);
                Properties.Settings.Default.TimeServer = Console.ReadLine();
                Properties.Settings.Default.Save();
                Console.WriteLine(Properties.Resources.NPleaseKeyInAdditionalParametersEnterForNoneN);
                Properties.Settings.Default.AdditionalParams = Console.ReadLine();
                Properties.Settings.Default.Save();
                return;
            }
            string path = args[0];
            string vPath = path;
            if (vPath.EndsWith(".exe"))
            {
                string vString = (FileVersionInfo.GetVersionInfo(vPath)).ProductVersion;
                vString.Replace(", ", ".");
                string vEnding = "-" + vString + ".exe";
                vPath = path.Replace(".exe", vEnding);                
            }
            else if (vPath.EndsWith(".msi"))
            {
                string vEnding = "-" + MsiProperties.Get(path, "ProductVersion") + ".msi";
                vPath = path.Replace(".msi", vEnding);
            }

            try
            {
                File.Move(path, vPath);
            }
            catch
            {
                Console.WriteLine(Properties.Resources.ErrorMoving0To1Failed, path, vPath);
            }

            string signString = "sign";
            signString += " /n " + Properties.Settings.Default.SignedBy;
            signString += " /t " + Properties.Settings.Default.TimeServer;
            signString += " " + Properties.Settings.Default.AdditionalParams;
            signString += " " + vPath;
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "signtool";
            startInfo.Arguments = signString;
            Process.Start(startInfo);
        }
    }
}