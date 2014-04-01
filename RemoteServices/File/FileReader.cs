using System;
using System.IO;

namespace com.bp.remoteservices.file {
    public class FileReader : FileProperties {
        public static string readAllText(string filename, bool isFullPath) {
            string fullPath = getFullPath(filename, isFullPath);
            try {
                if(File.Exists(fullPath)) {
                    return File.ReadAllText(fullPath);
                } else {
                    return null;
                }
            } catch(Exception e) {
                log.Error("Error reading all text from " + filename, e);
                return "";
            }
        }

        public static string[] readAllLines(string filename, bool isFullPath) {
            string fullPath = getFullPath(filename, isFullPath);
            try {
                if(File.Exists(fullPath)) {
                    return File.ReadAllLines(fullPath);
                } else {
                    return null;
                }
            } catch(Exception e) {
                log.Error("Error reading all lines from " + filename, e);
                return new string[0];
            }
        }
    }
}