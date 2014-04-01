using System;
using System.Diagnostics;
using Logger.Properties;

namespace com.bp.logger {
    public class EventLogger : ILogger {
        private Settings SETTINGS = Settings.Default;

        public void Debug(string text) {
            EventLog.WriteEntry(SETTINGS.ApplicationName, text, EventLogEntryType.Information);
        }

        public void Warn(string text) {
            EventLog.WriteEntry(SETTINGS.ApplicationName, text, EventLogEntryType.Warning);
        }

        public void Info(string text) {
            EventLog.WriteEntry(SETTINGS.ApplicationName, text, EventLogEntryType.Information);
        }

        public void Error(string text) {
            EventLog.WriteEntry(SETTINGS.ApplicationName, text, EventLogEntryType.Error);
        }

        public void Error(string text, Exception ex) {
            Error(text + "\n" + 
                  "Message: " + ex.Message + "\n" +
                  "StackTrace: " + ex.StackTrace);
        }
    }
}