using System.Diagnostics;
using System.Security;
using com.bp.remoteservices.file;

namespace com.bp.remoteservices.sybase {
    public class BatchExecutor : FileProperties {
        public static bool executeBatch(string processName, string arguments) {
            Process process = Process.Start(setupStartInfo(processName, arguments));
            process.WaitForExit();
            return true;
        }

        private static ProcessStartInfo setupStartInfo(string processName, string arguments) {
            ProcessStartInfo startInfo = new ProcessStartInfo(processName);
            startInfo.CreateNoWindow = false;
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.UseShellExecute = true;
            
            startInfo.WorkingDirectory = getExecutingDirectory();
            startInfo.Arguments = arguments;
            //startInfo.RedirectStandardInput = true;
            //startInfo.RedirectStandardError = true;
            //startInfo.RedirectStandardOutput = true;
            //startInfo.UserName = SETTINGS.AdminUser;
            //startInfo.Password = createSecurePassword(SETTINGS.AdminPassword);
            return startInfo;
        }

        private static SecureString createSecurePassword(string inPassword) {
            SecureString ss = new SecureString();

            foreach(char c in inPassword) {
                ss.AppendChar(c);
            }
            ss.MakeReadOnly();

            return ss;
        }
    }
}