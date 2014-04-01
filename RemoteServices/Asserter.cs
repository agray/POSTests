using System;
using System.IO;
using System.Text;
using com.bp.remoteservices.file;
using RemoteServices.Properties;

namespace com.bp.remoteservices {
    public class Asserter : FileProperties {
        private static Settings SETTINGS = Settings.Default;
        public static bool assert(string parameters, string expected) {
            if(setup(SETTINGS.TransactionAssertionTemplateFile, 
                     SETTINGS.TransactionAssertionConcreteFile,
                     parameters + "," + SETTINGS.RawActualsFile)) {
                return processFile(SETTINGS.BoomerangFile, expected);
            } else {
                return false;
            }
        }

        private static bool processFile(string boomerangFile, string expected) {
            try {
                string actual = FileReader.readAllText(boomerangFile, false);
                FileDeleter.deleteFile(boomerangFile, true);
                log.Info("read in BoomerangFile, returning " + actual.Equals(expected));
                return actual.Equals(expected);

            } catch(Exception e) {
                log.Error("Error occurred processing Actuals data", e);
                return false;
            }
        }

        private static bool setup(string sqlTemplateFile, string sqlFile, string parameters) {
            try {
                //Create Notification File - Handball.txt (will pass control to FileMonitor)
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(sqlTemplateFile);
                sb.AppendLine(sqlFile);
                //sb.AppendLine(outputFile);
                sb.AppendLine(parameters);
                //sb.Append(expected);

                if(!FileWriter.appendText(SETTINGS.NotificationFile, false, sb.ToString())) {
                    //something went wrong writing out the notification file.
                    return false;
                }

                //wait for BoomerangFile to exist
                while(!File.Exists(WORKING_DIR + DIR_SEP + SETTINGS.BoomerangFile)) {
                    if(File.Exists(WORKING_DIR + DIR_SEP + SETTINGS.BoomerangFile)) {
                        break;
                    }
                }
                return true;
            } catch(Exception e){
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}