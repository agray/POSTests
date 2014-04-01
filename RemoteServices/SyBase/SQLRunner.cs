using System.IO;
using com.bp.remoteservices.file;
using RemoteServices.Properties;

namespace com.bp.remoteservices.sybase {
    public class SQLRunner : FileProperties {
        private static Settings SETTINGS = Settings.Default;
        public static bool run(string inputFile, string outputFile) {
            string processName = "ExecuteSQL.bat";
            string arguments = SETTINGS.SQLUser + " " + 
                               SETTINGS.SQLPassword + " " + 
                               inputFile + " " +
                               outputFile;

            //Make sure batch file exists in executing directory
            if(File.Exists(getExecutingDirectory() + DIR_SEP + processName)) {
                return createProcess(getExecutingDirectory(), processName, arguments, outputFile);
            } else {
                log.Error(getExecutingDirectory() + DIR_SEP + processName + " does not exist.  Cannot continue.");
                return false;
            }
        }

        private static bool createProcess(string directory, string processName, string arguments, string outputFile) {
            log.Info("Calling executeBatch with " + directory + DIR_SEP + processName + " and " + arguments);
            return BatchExecutor.executeBatch(directory + DIR_SEP + processName, arguments);
        }
    }
}