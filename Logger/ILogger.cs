using System;

namespace com.bp.logger {
    public interface ILogger {
        void Debug(string text);
        void Warn(string text);
        void Info(string text);
        void Error(string text);
        void Error(string text, Exception ex);
    }
}