using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using AsserterService.Properties;
using com.bp.remoteservices.file;
using com.bp.remoteservices.service;
using com.bp.remoteservices.sybase;

namespace AsserterService {
    public partial class AsserterService : ServiceBase {
        private static Settings SETTINGS = Settings.Default;

        private const string SERVICENAME = "BPAsserterService";
        protected const string DIR_SEP = @"\";
        protected const string UNC_STARTER = @"\\";
        protected static string WORKING_DIR = UNC_STARTER + SETTINGS.RemotePOS + DIR_SEP + SETTINGS.RemoteFSPath;
        protected const int CORRECT_HANDBALL_LINES = 5;

        // Keep track of worker thread.
        private Thread m_oAsserterThread = null;

        public AsserterService() {
            InitializeComponent();
        }

        protected override void OnStart(string[] args) {
            // Add code here to start your service. This method should set things
            // in motion so your service can do its work.
            eventLog1.WriteEntry(SERVICENAME + " is starting.");

            try {
                if(Directory.Exists(WORKING_DIR)) {
                    // Start the thread.
                    if(m_oAsserterThread == null) {
                        m_oAsserterThread = new Thread(new ThreadStart(FSWRun));
                    }
                    m_oAsserterThread.Start();
                } else {
                    eventLog1.WriteEntry("Directory" + WORKING_DIR + " does not exist. " + SERVICENAME + " cannot start");
                    Stop();
                    Environment.Exit(1);
                }
            } catch(Exception e) {
                Console.WriteLine(e.Message);
                Stop();
                Environment.Exit(1);
            }
        }

        protected override void OnStop() {
            // Add code here to perform any tear-down necessary to stop your service.
            eventLog1.WriteEntry(SERVICENAME + " is stopping.");

            // Stop the thread.
            if(m_oAsserterThread != null) {
                m_oAsserterThread.Abort();
            }
        }

        private void FSWRun() {
            // Loops, until killed by OnStop.
            eventLog1.WriteEntry(SERVICENAME + " polling thread started.");
            
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = WORKING_DIR;
            
            /* Watch for new files. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*.txt";

            // Add event handlers.
            watcher.Created += new FileSystemEventHandler(OnCreated);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        // Define the event handlers.
        private void OnCreated(object source, FileSystemEventArgs e) {
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);

            try {
                // Do Stuff Here...
                //read in file with details of which SQL to execute
                string[] lines = FileReader.readAllLines(e.FullPath, true);

                if(lines == null || lines.Length != CORRECT_HANDBALL_LINES) {
                    eventLog1.WriteEntry(e.FullPath + " has " + lines.Length + " lines, it should contain " + CORRECT_HANDBALL_LINES + " lines.", EventLogEntryType.Error);
                } else {
                    //capture lines
                    string sqlTemplateFile = lines[0];
                    string sqlFile = lines[1];
                    string outputFile = lines[2];
                    string[] parameters = lines[3].Split(',');
                    string[] expected = lines[4].Split(',');
                    eventLog1.WriteEntry("Lines read: " + sqlTemplateFile + DIR_SEP +
                                                          sqlFile + DIR_SEP +
                                                          outputFile + DIR_SEP +
                                                          parameters.ToString() + DIR_SEP +
                                                          expected.ToString());

                    //Delete file that started this polling pass
                    if(FileDeleter.deleteFile(e.FullPath, true)) {
                        //Ensure Service is started
                        if(BPServiceController.StartService()) {
                            //Prepare SQL File
                            if(ParameterReplacer.replaceParameters(sqlTemplateFile, sqlFile, parameters)) {
                                //Execute SQL
                                if(!SQLRunner.run(sqlFile, outputFile)) {
                                    //problem occurred running SQL
                                    eventLog1.WriteEntry("Problem occurred running SQL file, cannot continue processing this pass", EventLogEntryType.Error);
                                }
                            } else {
                                //problem occurred replacing parameters
                                eventLog1.WriteEntry("SQL file could not be prepared with parameters, cannot continue processing this pass", EventLogEntryType.Error);
                            }
                        } else {
                            //problem occurred starting service
                            eventLog1.WriteEntry("Service could not be started, cannot continue processing this pass", EventLogEntryType.Error);
                        }
                    } else {
                        //couldn't delete file
                        eventLog1.WriteEntry(e.FullPath + " could not be deleted", EventLogEntryType.Error);
                    }
                }
                eventLog1.WriteEntry(SERVICENAME + " pass executed.");
            } catch(Exception ex) {
                eventLog1.WriteEntry(SERVICENAME + " encountered an error '" +
                                     ex.Message + "'", EventLogEntryType.Error);
                eventLog1.WriteEntry(SERVICENAME + " Stack Trace: " +
                                     ex.StackTrace, EventLogEntryType.Error);
            }
        }

        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e) {

        }
    }
}