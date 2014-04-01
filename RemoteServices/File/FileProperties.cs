using System;
using System.IO;
using System.Reflection;
using com.bp.logger;
using RemoteServices.Properties;

namespace com.bp.remoteservices.file {
    public class FileProperties {
        protected static readonly ILogger log = new EventLogger();
        private static Settings SETTINGS = Settings.Default;
        protected const string DIR_SEP = @"\";
        protected const string UNC_STARTER = @"\\";
        protected const string PARENT_STEPS = "../../../";
        protected const string DRIVE_SUFFIX = @":\";
        protected static string WORKING_DIR = UNC_STARTER + SETTINGS.RemotePOS + DIR_SEP + SETTINGS.RemoteFSPath;

        protected static string getFullPath(string filename, bool isFullPath) {
            if(isFullPath) {
                return filename;
            } else {
                return WORKING_DIR + DIR_SEP + filename;
            }
        }

        protected static string getExecutingDirectory() {
            String path = Assembly.GetExecutingAssembly().Location;
            return Path.GetDirectoryName(path);
        }
    }
}