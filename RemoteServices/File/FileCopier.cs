using System;
using System.IO;

namespace com.bp.remoteservices.file {
    public class FileCopier : FileProperties {
        public static bool copyFile(string sourceFile, string destinationFile, bool overwrite) {
            string confirmedSourceLocation = findSourceFile(sourceFile);

            if(!confirmedSourceLocation.Equals(string.Empty)) {
                return doCopy(confirmedSourceLocation, destinationFile, overwrite);
            } else {
                log.Error("Source File does not exist is search scope.");
                return false;
            }
        }

        private static string findSourceFile(string sourceFile) {
            log.Info("findSourceFile received parameter " + sourceFile);
            if(!File.Exists(sourceFile)) {
                if(!File.Exists(PARENT_STEPS + sourceFile)) {
                    log.Info("findSourceFile returning empty string");
                    return string.Empty;
                } else {
                    log.Info("findSourceFile returning: " + PARENT_STEPS + sourceFile);
                    return PARENT_STEPS + sourceFile;
                }
            } else {
                log.Info("findSourceFile returning: " + sourceFile);
                return sourceFile;
            }
        }

        private static bool doCopy(string sourceFile, string destinationFile, bool overwrite) {
            try {
                File.Copy(sourceFile, destinationFile, overwrite);
            } catch(Exception e) {
                log.Error("Error copying " + sourceFile, e);
                return false;
            }
            log.Info("Copied " + sourceFile + " to " + destinationFile);
            return true;
        }
    }
}