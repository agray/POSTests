using System;
using System.ServiceProcess;
using BPServiceController.Properties;

namespace com.bp.controllers.service {
    public class BPServiceController {
        private static Settings SETTINGS = Settings.Default;

        public static bool StartService() {
            ServiceController sc;
            sc = new ServiceController(SETTINGS.SyBaseServiceName, SETTINGS.RemoteBOS);

            switch(sc.Status) {
                case ServiceControllerStatus.Running:
                    Console.WriteLine("Service already started, nothing to do");
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
                Console.WriteLine("Service started successfully");
                return true;
            } catch(Exception e) {
                Console.WriteLine("Start() call failed");
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private static bool doContinue(ServiceController sc) {
            try {
                sc.Continue();
                sc.WaitForStatus(ServiceControllerStatus.Running);
                Console.WriteLine("Service continued successfully");
                return true;
            } catch(Exception e) {
                Console.WriteLine("Continue() call failed");
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}