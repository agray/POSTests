using System.Text;

namespace com.bp.remoteservices.file {
    public class ParameterReplacer : FileProperties {
        public static bool replaceParameters(string sourceFile, string destinationFile, string[] parameters) {
            if(!FileDeleter.deleteFile(getExecutingDirectory() + DIR_SEP + destinationFile, true)) {
                return false;
            }

            StringBuilder allText = new StringBuilder(FileReader.readAllText(getExecutingDirectory() + DIR_SEP + sourceFile, true), 5000);
            if(allText.Equals(string.Empty)) {
                return false;
            }

            for(int i = 1; i <= parameters.Length; i++) {
                allText.Replace("@" + i, parameters[i - 1]);
                //log.Info("Replaced " + "@" + i + " for " + parameters[i - 1]);
            }

            if(FileWriter.writeAllText(getExecutingDirectory() + DIR_SEP + destinationFile, true, allText.ToString())) {
                log.Info("Parameters replaced in " + destinationFile);
                return true;
            } else {
                return false;
            }
        }
    }
}