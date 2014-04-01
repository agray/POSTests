using System;
using com.bp.remoteservices.wmi.process;

namespace com.bp.test {
    public class WMIProcessTester {
        public static void test() {
            //Local Machine
            ProcessLocal process = new ProcessLocal();

            //Remote Machine
            //ProcessRemote process = new ProcessRemote("neal.bailey",
            //                                          "Cla$$ified",
            //                                          "BAILEYSOFT",
            //                                          "192.168.2.1");

            string processPath = "C:\\Personal\\Visual Studio 2008\\SVNWorkingCopies\\POSTests\\";
            string processName = "testbatch.bat";
            string arguments = "C:\\temp\\testfile.txt";

            Console.WriteLine("Creating Process: " + processPath + processName + " " + arguments);
            Console.WriteLine(process.CreateProcess(processPath, processName, arguments));
        }
    }
}