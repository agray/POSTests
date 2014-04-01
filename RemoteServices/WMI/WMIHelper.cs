using System;
using System.Management;
using com.bp.logger;
using RemoteServices.Properties;

namespace com.bp.remoteservices.wmi {
    public class WMIHelper {
        private ManagementScope managementScope = null;
        private static readonly ILogger log = new EventLogger();
        private static Settings SETTINGS = Settings.Default;

        public WMIHelper() {
            try {
                ConnectionOptions connectionOptions = new ConnectionOptions();
                string wmiPath;
                log.Info("ConnectionOptions: BOS: " + SETTINGS.RemotePOS + " AdminUser: " + SETTINGS.AdminUser + " AdminPassword: " + SETTINGS.AdminPassword);
                if(isLocal(SETTINGS.RemotePOS, SETTINGS.AdminUser, SETTINGS.AdminPassword)) {
                    //specify WMI path for local machine 
                    wmiPath = @"root\cimv2";
                } else {
                    //set connection parameters for remote machine
                    connectionOptions.Username = SETTINGS.AdminUser;
                    connectionOptions.Password = SETTINGS.AdminPassword;
                    //specify WMI path for remote machine
                    wmiPath = String.Format(@"\\{0}\root\cimv2", SETTINGS.RemotePOS);
                }
                log.Info("WMIPath: " + wmiPath);
                managementScope = new ManagementScope(wmiPath, connectionOptions);
            } catch(Exception ex) {
                log.Error("WMI Exception occurred", ex);
                //throw new Exception(
                //  String.Format("WMI exception: {0}", ex.Message));
            }
        }
        
        private bool isLocal(string serverName, string adminUser, string adminUserPassword) {
            return serverName == null || 
                   serverName.Length == 0 ||
                   adminUser == null  || 
                   adminUser.Length == 0  ||
                   adminUserPassword == null || 
                   adminUserPassword.Length == 0;
        }
    }
}