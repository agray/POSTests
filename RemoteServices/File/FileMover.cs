using System;
using System.IO;

namespace com.bp.remoteservices.file {
    public class FileMover : FileProperties {
        public static bool moveFile(string source, string dest) {
            try {
                if(!File.Exists(source)) {
                    log.Error(source + "does not exist, cannot move it to " + dest);
                    return false;
                }

                // Ensure that the target does not exist.
                if(File.Exists(dest)) {
                    FileDeleter.deleteFile(dest, true);
                }
                // Move the file.
                File.Move(source, dest);
                log.Info(source + " was moved to " + dest);

                // See if the original exists now.
                if(File.Exists(source)) {
                    log.Error("The original file still exists, which is unexpected.");
                    return false;
                } else {
                    log.Info("The original file no longer exists, which is expected.");
                    return true;
                }

            } catch(Exception e) {
                log.Error("The process failed:", e);
                return true;
            }
        }
    }
}