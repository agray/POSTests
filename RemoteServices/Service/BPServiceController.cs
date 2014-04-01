using System;
using System.ServiceProcess;
using com.bp.logger;
using RemoteServices.Properties;

namespace com.bp.remoteservices.service {
    public class BPServiceController {
        private static readonly ILogger log = new EventLogger();
        private static Settings SETTINGS = Settings.Default;

        public static bool StartService() {
            ServiceController sc;
            if(SETTINGS.LocalOnly) {
                sc = new ServiceController(SETTINGS.SyBaseServiceName);
            } else {
                sc = new ServiceController(SETTINGS.SyBaseServiceName, SETTINGS.RemotePOS);
            }

            switch(sc.Status) {
                case ServiceControllerStatus.Running:
                    log.Info("Service already started, nothing to do");
                    return true;
                case ServiceControllerStatus.Stopped:
                    return doStart(sc);
                case ServiceControllerStatus.StartPending:
                    sc.WaitForStatus(ServiceControllerStatus.Running);
                    return true;
                case ServiceControllerStatus.StopPending:
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                    return doStart(sc);
                case ServiceControllerStatus.Paused:
                    return doContinue(sc);
                case ServiceControllerStatus.ContinuePending:
                    sc.WaitForStatus(ServiceControllerStatus.Running);
                    return true;
                case ServiceControllerStatus.PausePending:
                    sc.WaitForStatus(ServiceControllerStatus.Paused);
                    return doContinue(sc);
                default:
                    return false;
            }
        }

        private static bool doStart(ServiceController sc) {
            try {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running);
                log.Info("Service started successfully");
                return true;
            } catch(Exception e) {
                log.Error("Start() call failed", e);
                return false;
            }
        }

        private static bool doContinue(ServiceController sc) {
            try {
                sc.Continue();
                sc.WaitForStatus(ServiceControllerStatus.Running);
                log.Info("Service continued successfully");
                return true;
            } catch(Exception e) {
                log.Error("Continue() call failed", e);
                return false;
            }
        }
    }
}