using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Deployment.WindowsInstaller;
using System.Security.Cryptography;
using System.Text;

namespace signversioned
{
    public class Encryption
    {
    public static string Encrypt(string textToEncrypt)
    {
        try
        {
            string ToReturn = "";
            string publickey = "52830761";
            string secretkey = "Cfg_!7KjH";
            byte[] secretkeyByte = { };
            secretkeyByte = Encoding.UTF8.GetBytes(secretkey);
            byte[] publickeybyte = { };
            publickeybyte = Encoding.UTF8.GetBytes(publickey);
            MemoryStream ms = null;
            CryptoStream cs = null;
            byte[] inputbyteArray = Encoding.UTF8.GetBytes(textToEncrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                ToReturn = Convert.ToBase64String(ms.ToArray());
            }
            return ToReturn;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex.InnerException);
        }
    }

    public static string Decrypt(string textToDecrypt)
    {
        try
        {
            string ToReturn = "";
            string publickey = "52830761";
            string privatekey = "Cfg_!7KjH";
            byte[] privatekeyByte = { };
            privatekeyByte = Encoding.UTF8.GetBytes(privatekey);
            byte[] publickeybyte = { };
            publickeybyte = Encoding.UTF8.GetBytes(publickey);
            MemoryStream ms = null;
            CryptoStream cs = null;
            byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
            inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                ToReturn = encoding.GetString(ms.ToArray());
            }
            return ToReturn;
        }
        catch (Exception ae)
        {
            throw new Exception(ae.Message, ae.InnerException);
        }
    }
}
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

            signString += " /n " + Encryption.Decrypt(Properties.Settings.Default.SignedBy);
            signString += " /t " + Encryption.Decrypt(Properties.Settings.Default.TimeServer);
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