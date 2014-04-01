
namespace com.bp.remoteservices.wmi.process {
    public enum WMIProcessExitCode : int {
        SUCCESSFULLY_COMPLETED = 0,
        ACCESS_DENIED = 2,
        INSUFFICIENT_PRIVILEDGES = 3,
        UNKNOWN_FAILURE = 8,
        PATH_NOT_FOUND = 9,
        INVALID_PARAMETER = 21
    }
}