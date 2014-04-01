using System;
using System.IO;

namespace com.bp.remoteservices.file {
    public class FileDeleter : FileProperties {
        public static bool deleteFile(string fileToDelete, bool isFullPath) {
            string fullPath = getFullPath(fileToDelete, isFullPath);
            if(!File.Exists(fullPath)) {
                log.Info("File " + fullPath + " does not exist, nothing to delete.");
                return true;
            } else {
                return doDelete(fullPath);
            }
        }

        private static bool doDelete(string fileToDelete) {
            try {
                File.Delete(fileToDelete);
            } catch(Exception e) {
                log.Error("Error deleting " + fileToDelete, e);
                return false;
            }
            log.Info("Deleted " + fileToDelete);
            return true;
        }
    }
}