using System;
using System.IO;

namespace com.bp.remoteservices.file {
    public class FileWriter : FileProperties {
        public static bool appendText(string filename, bool isFullPath, string contents) {

            try {
                string fullName = getFullPath(filename, isFullPath);
                File.AppendAllText(fullName, contents);
                log.Info("Appended \n" + contents + "\n to " + fullName);
                return true;
            } catch(Exception e) {
                log.Error("Error appending text to " + filename, e);
                return false;
            }
        }

        public static bool writeAllText(string filename, bool isFullPath, string contents) {
            try {
                string fullName = getFullPath(filename, isFullPath);
                File.WriteAllText(fullName, contents);
                log.Info("Wrote \n" + contents + "\n to " + fullName);
                return true;
            } catch(Exception e){
                log.Error("Error writing all text to " + filename, e);
                return false;
            }
        }
    }
}