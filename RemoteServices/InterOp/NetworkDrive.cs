using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using com.bp.remoteservices.file;
using RemoteServices.Properties;

namespace com.bp.remoteservices.interop {
    public class NetworkDrive : FileProperties {
        private static Settings SETTINGS = Settings.Default;
        #region Enum
        public enum ResourceScope {
            RESOURCE_CONNECTED = 1,
            RESOURCE_GLOBALNET,
            RESOURCE_REMEMBERED,
            RESOURCE_RECENT,
            RESOURCE_CONTEXT
        }

        public enum ResourceType {
            RESOURCETYPE_ANY,
            RESOURCETYPE_DISK,
            RESOURCETYPE_PRINT,
            RESOURCETYPE_RESERVED
        }

        public enum ResourceUsage {
            RESOURCEUSAGE_CONNECTABLE = 0x00000001,
            RESOURCEUSAGE_CONTAINER = 0x00000002,
            RESOURCEUSAGE_NOLOCALDEVICE = 0x00000004,
            RESOURCEUSAGE_SIBLING = 0x00000008,
            RESOURCEUSAGE_ATTACHED = 0x00000010,
            RESOURCEUSAGE_ALL = (RESOURCEUSAGE_CONNECTABLE | RESOURCEUSAGE_CONTAINER | RESOURCEUSAGE_ATTACHED),
        }

        public enum ResourceDisplayType {
            RESOURCEDISPLAYTYPE_GENERIC,
            RESOURCEDISPLAYTYPE_DOMAIN,
            RESOURCEDISPLAYTYPE_SERVER,
            RESOURCEDISPLAYTYPE_SHARE,
            RESOURCEDISPLAYTYPE_FILE,
            RESOURCEDISPLAYTYPE_GROUP,
            RESOURCEDISPLAYTYPE_NETWORK,
            RESOURCEDISPLAYTYPE_ROOT,
            RESOURCEDISPLAYTYPE_SHAREADMIN,
            RESOURCEDISPLAYTYPE_DIRECTORY,
            RESOURCEDISPLAYTYPE_TREE,
            RESOURCEDISPLAYTYPE_NDSCONTAINER
        }
        #endregion

        [StructLayout(LayoutKind.Sequential)]
        private class NETRESOURCE {
            public ResourceScope dwScope = 0;
            public ResourceType dwType = 0;
            public ResourceDisplayType dwDisplayType = 0;
            public ResourceUsage dwUsage = 0;
            public string lpLocalName = null;
            public string lpRemoteName = null;
            public string lpComment = null;
            public string lpProvider = null;
        }

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(NETRESOURCE lpNetResource, string lpPassword, string lpUsername, int dwFlags);

        public static bool DoNetworkDriveMapping() {
            string driveLetter = SETTINGS.DriveLetter;

            if(NetworkDrive.alreadyMapped(driveLetter)) {
                if(NetworkDrive.GetUNCPath(driveLetter + DRIVE_SUFFIX).Equals(UNC_STARTER + SETTINGS.RemotePOS +
                                                                              DIR_SEP + SETTINGS.RemoteFSPath)) {
                    log.Info(driveLetter + ": drive is mapped to the correct location.");
                    return true;
                } else {
                    log.Info(driveLetter + ": drive is incorrectly mapped, remapping...");
                    DropNetworkDrive(driveLetter);
                    //MapNetworkDrive(@"\\THERUINS\DFS\Managed Services", "K:", "andrewg", "Gray89*(and");
                    return MapNetworkDrive();
                }
            } else {
                //MapNetworkDrive("\\\\THERUINS\\DFS\\Managed Services\\MS", "K:", "andrewg", "Gray89*(and");
                return MapNetworkDrive();
            }
        }

        private static bool alreadyMapped(string driveLetter) {
            DriveInfo[] drives = DriveInfo.GetDrives();
            log.Info("Mapped Drive Count: " + drives.Length);

            //Console.WriteLine("Currently Mapped Drives are: " + drives.ToString());
            log.Info("Passed in driveLetter param is: " + driveLetter);

            foreach(DriveInfo drive in drives) {
                log.Info("Mapped Drive: " + drive.ToString()[0].ToString());
                if(drive.ToString()[0].ToString().Equals(driveLetter)) {
                    log.Info(driveLetter + " is already mapped, returning true...");
                    return true;
                }

                //Console.WriteLine("Name: " + drive.Name);
                //if(drive.IsReady) {
                //    Console.WriteLine("AvailableFreeSpace: " + drive.AvailableFreeSpace);
                //    Console.WriteLine("DriveFormat: " + drive.DriveFormat);
                //    Console.WriteLine("DriveType: " + drive.DriveType);
                //    Console.WriteLine("RootDirectory: " + drive.RootDirectory);
                //    Console.WriteLine("TotalFreeSpace: " + drive.TotalFreeSpace);
                //    Console.WriteLine("TotalSize: " + drive.TotalSize);
                //    Console.WriteLine("VolumeLabel: " + drive.VolumeLabel);
                //    Console.WriteLine("ToString: " + drive.ToString());
                //}
                //Console.WriteLine("*****************");
            }
            log.Error(driveLetter + " is not mapped, returned false...");
            return false;
        }

        [DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int WNetGetConnection([MarshalAs(UnmanagedType.LPTStr)] string localName,
                                                   [MarshalAs(UnmanagedType.LPTStr)] StringBuilder remoteName,
                                                   ref int length);
        /// <summary>
        /// Given a path, returns the UNC path or the original. (No exceptions
        /// are raised by this function directly). For example, "P:\2008-02-29"
        /// might return: "\\networkserver\Shares\Photos\2008-02-09"
        /// </summary>
        /// <param name="originalPath">The path to convert to a UNC Path</param>
        /// <returns>A UNC path. If a network drive letter is specified, the
        /// drive letter is converted to a UNC or network path. If the
        /// originalPath cannot be converted, it is returned unchanged.</returns>
        public static string GetUNCPath(string originalPath) {
            StringBuilder sb = new StringBuilder(512);
            int size = sb.Capacity;

            // look for the {LETTER}: combination ...
            if(originalPath.Length > 2 && originalPath[1] == ':') {
                // don't use char.IsLetter here - as that can be misleading
                // the only valid drive letters are a-z && A-Z.
                char c = originalPath[0];
                if((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')) {
                    int error = WNetGetConnection(originalPath.Substring(0, 2),
                        sb, ref size);
                    if(error == 0) {
                        DirectoryInfo dir = new DirectoryInfo(originalPath);

                        string path = Path.GetFullPath(originalPath)
                            .Substring(Path.GetPathRoot(originalPath).Length);
                        return Path.Combine(sb.ToString().TrimEnd(), path);
                    }
                }
            }

            return originalPath;
        }

        private static bool MapNetworkDrive() {
            NETRESOURCE myNetResource = new NETRESOURCE();
            myNetResource.lpLocalName = SETTINGS.DriveLetter + ":";
            myNetResource.lpRemoteName = UNC_STARTER + SETTINGS.RemotePOS +
                                         DIR_SEP + SETTINGS.RemoteFSPath;
            myNetResource.lpProvider = null;
            int result = WNetAddConnection2(myNetResource, SETTINGS.AdminPassword,
                                            SETTINGS.AdminUser, 0);

            if(result == 0) {
                log.Info("Successfully mapped " + SETTINGS.DriveLetter + ": drive.");
                return true;
            } else {
                log.Error("Failed to map " + SETTINGS.DriveLetter + ": to network location " + myNetResource.lpRemoteName + " using username " + SETTINGS.AdminUser + " and password " + SETTINGS.AdminPassword + ".  Result code is " + result);
                return false;
            }
        }

        /// <summary>
        /// API Call to Drop existing Network Connection
        /// </summary>
        /// <param name="lpName">Pointer to a constant null-terminated string that specifies the name of either the redirected local device or the remote network resource to disconnect from.</param>
        /// <param name="dwFlags">Connection type.  0 to retain only for current session or CONNECT_UPDATE_PROFILE to persist on session being logged out.</param>
        /// <param name="fForce">Specifies whether the disconnection should occur if there are open files or jobs on the connection. If this parameter is FALSE, the function fails if there are open files or jobs.</param>
        /// <returns></returns>
        [DllImport("Mpr.dll", EntryPoint = "WNetCancelConnection2A", CharSet = CharSet.Ansi)]
        private static extern int DropNetworkConnection(string lpName, int dwFlags, bool fForce);
        internal const int NO_ERROR = 0x000;
        internal const int ERROR_BAD_PROFILE = 0x4B6;
        internal const int ERROR_CANNOT_OPEN_PROFILE = 0x4B5;
        internal const int ERROR_DEVICE_IN_USE = 0x964;
        internal const int ERROR_EXTENDED_ERROR = 0x4B8;
        internal const int ERROR_NOT_CONNECTED = 0x8CA;
        internal const int ERROR_OPEN_FILES = 0x961;

        private static int DropNetworkDrive(string driveLetter) {

            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach(DriveInfo drive in drives) {
                if(drive.ToString()[0].ToString().Equals(driveLetter)) {
                    switch(DropNetworkConnection(drive.Name.Substring(0, 2), 0, false)) {
                        case NO_ERROR:
                            log.Info("Successfully dropped " + driveLetter + ": drive");
                            return NO_ERROR;
                        case ERROR_BAD_PROFILE:
                            log.Error("Bad Profile error occurred while dropping " + driveLetter);
                            return ERROR_BAD_PROFILE;
                        case ERROR_CANNOT_OPEN_PROFILE:
                            log.Error("Cannot Open Profile error occurred while dropping " + driveLetter);
                            return ERROR_CANNOT_OPEN_PROFILE;
                        case ERROR_DEVICE_IN_USE:           //TODO: Handle
                            log.Error("Device in Use error occurred while dropping " + driveLetter);
                            return ERROR_DEVICE_IN_USE;
                        case ERROR_EXTENDED_ERROR:          //TODO: Handle
                            log.Error("Extended error occurred while dropping " + driveLetter);
                            return ERROR_EXTENDED_ERROR;
                        case ERROR_NOT_CONNECTED:           //TODO: Handle
                            log.Error("Not Connected error occurred while dropping " + driveLetter);
                            return ERROR_NOT_CONNECTED;
                        case ERROR_OPEN_FILES:              //TODO: Handle
                            log.Error("Open Files error occurred while dropping " + driveLetter);
                            return ERROR_OPEN_FILES;
                        default:                            //TODO: Handle Unsupported Error Code
                            log.Error("Unknown error occurred while dropping " + driveLetter);
                            return 1;
                    }
                }
            }
            return 1;
        }
    }
}