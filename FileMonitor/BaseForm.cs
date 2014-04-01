using System.Windows.Forms;
using com.bp.logger;
using FileMonitor.Properties;

namespace FileMonitor {
    public class BaseForm : Form {
        protected static readonly ILogger log = new EventLogger();
        private static Settings SETTINGS = Settings.Default;
        protected const string DIR_SEP = @"\";
        protected const string UNC_STARTER = @"\\";
        protected static string WORKING_DIR = UNC_STARTER + SETTINGS.RemotePOS + DIR_SEP + SETTINGS.RemoteFSPath;
        //protected static string WORKING_DIR = @"C:\temp";
        protected const int CORRECT_HANDBALL_LINES = 3;
    }
}