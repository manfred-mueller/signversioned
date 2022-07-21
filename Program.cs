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
                Properties.Settings.Default.SignedBy = Encryption.Encrypt(Console.ReadLine());
                Properties.Settings.Default.Save();
                Console.WriteLine(Properties.Resources.NPleaseKeyInTheTimeServerN);
                Properties.Settings.Default.TimeServer = Encryption.Encrypt(Console.ReadLine());
                Properties.Settings.Default.Save();
                Console.WriteLine(Properties.Resources.NPleaseKeyInAdditionalParametersEnterForNoneN);
                Properties.Settings.Default.AdditionalParams = Encryption.Encrypt(Console.ReadLine());
                Properties.Settings.Default.Save();
                return;
            }

            if (string.IsNullOrEmpty(Properties.Settings.Default.SignedBy) || string.IsNullOrEmpty(Properties.Settings.Default.TimeServer))
            {
                Console.WriteLine(Properties.Resources.ErrorIssuerAndOrTimeServerSettingsNotFound);
                return;
            }

            string path = args[0];
            string vPath = path;
            string signString = "sign";

            signString += " /n \"" + Encryption.Decrypt(Properties.Settings.Default.SignedBy) + "\"";
            signString += " /t \"" + Encryption.Decrypt(Properties.Settings.Default.TimeServer) + "\"";
            signString += " " + Encryption.Decrypt(Properties.Settings.Default.AdditionalParams);
            signString += " " + args[0];
            Process signProc = new Process();
            ProcessStartInfo signProcStartInfo = new ProcessStartInfo();
            signProcStartInfo.FileName = "signtool";
            signProcStartInfo.Arguments = signString;
            signProcStartInfo.UseShellExecute = false;
            signProcStartInfo.RedirectStandardError = true;
            signProc.StartInfo = signProcStartInfo;
            signProc.Start();
            signProc.WaitForExit();
            if (signProc.ExitCode == 0)
            {
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
                    Console.WriteLine(Properties.Resources.Moving0To1Successful, path, vPath);
                }
                catch
                {
                    Console.WriteLine(Properties.Resources.ErrorMoving0To1Failed, path, vPath);
                }
            }
            else
            {
                Console.WriteLine(Properties.Resources.ErrorSigning0Failed, path);
            }
        }
    }
}