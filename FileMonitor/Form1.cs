using System;
using System.IO;
using com.bp.remoteservices;
using com.bp.remoteservices.file;
using com.bp.remoteservices.service;
using com.bp.remoteservices.sybase;
using FileMonitor.Properties;

namespace FileMonitor {
    public partial class Form1 : BaseForm {
        private static Settings SETTINGS = Settings.Default;

        public Form1() {
            InitializeComponent();
            lblMonitoredFolderPath.Text = "Press Start button to start monitoring";
            chkSubdirectories.Enabled = true;
            chkSubdirectories.Checked = false;
            txtFilter.Enabled = true;
            txtFilter.Text = SETTINGS.NotificationFile;
        }

        private void fileWatcher_Created(object sender, FileSystemEventArgs e) {
            log.Info(e.ChangeType + ": " + e.FullPath);
            string sqlTemplateFile, sqlFile, outputFile, expected;
            string[] parameters;
            try {
                //read in file with details of which SQL to execute
                string[] lines = FileReader.readAllLines(e.FullPath, true);

                if(lines == null || lines.Length != CORRECT_HANDBALL_LINES) {
                    log.Error(e.FullPath + " has " + lines.Length + " lines, it should contain " + CORRECT_HANDBALL_LINES + " lines.");
                } else {
                    //capture lines
                    sqlTemplateFile = lines[0];
                    sqlFile = lines[1];
                    //outputFile = lines[2];
                    parameters = lines[2].Split(',');
                    expected = lines[3];
                    outputFile = parameters[1];
                    log.Info("Lines read: " + sqlTemplateFile + Environment.NewLine +
                                              sqlFile + Environment.NewLine +
                                              outputFile + Environment.NewLine +
                                              parameters.ToString() + Environment.NewLine +
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
                                    log.Error("Problem occurred running SQL file, cannot continue processing this pass");
                                }
                            } else {
                                //problem occurred replacing parameters
                                log.Error("SQL file could not be prepared with parameters, cannot continue processing this pass");
                            }
                        } else {
                            //problem occurred starting service
                            log.Error("Service could not be started, cannot continue processing this pass");
                        }
                    } else {
                        //couldn't delete file
                        log.Error(e.FullPath + " could not be deleted");
                    }

                    processActualsFile(outputFile);
                }
                log.Error("Form Monitor completed processing file event successfully.");
            } catch(Exception ex) {
                log.Error("Encountered an error", ex);
            }
        }

        private void processActualsFile(string sqlOutputFile) {
            try {
                //Cleanse File
                string[] lines = FileReader.readAllLines(sqlOutputFile, true);
                FileDeleter.deleteFile(sqlOutputFile, true);
                FileWriter.writeAllText(WORKING_DIR + DIR_SEP + SETTINGS.BoomerangFile, true, CSV.cleanseCSV(lines[4]));
            } catch(Exception e) {
                log.Error("Error occurred processing Actuals data", e);
            }
        }

        private void btnMonitor_Click(object sender, EventArgs e) {
            fileWatcher.Path = WORKING_DIR;
            //watch only for new files
            fileWatcher.NotifyFilter = NotifyFilters.LastAccess |
                                       NotifyFilters.LastWrite |
                                       NotifyFilters.FileName |
                                       NotifyFilters.DirectoryName;
            fileWatcher.Filter = txtFilter.Text;
            fileWatcher.IncludeSubdirectories = chkSubdirectories.Checked;
            fileWatcher.EnableRaisingEvents = true;
            lblMonitoredFolderPath.Text = WORKING_DIR;
            chkSubdirectories.Enabled = false;
            txtFilter.Enabled = false;
            log.Info("Monitoring started - " + "Filter(" + txtFilter.Text + ") " + getSDIncludedMessage());
        }

        private void btnStop_Click(object sender, EventArgs e) {
            fileWatcher.EnableRaisingEvents = false;
            lblMonitoredFolderPath.Text = "Press Start button to start monitoring";
            log.Info("Monitoring stopped");
            chkSubdirectories.Enabled = true;
            txtFilter.Enabled = true;
        }

        private string getSDIncludedMessage() {
            if(chkSubdirectories.Checked) {
                return "Subdirectories(Included)";
            } else {
                return "Subdirectories(Excluded)";
            }
        }
    }
}