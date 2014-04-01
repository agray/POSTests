namespace FileMonitor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fileWatcher = new System.IO.FileSystemWatcher();
            this.lblFilter = new System.Windows.Forms.Label();
            this.btnMonitor = new System.Windows.Forms.Button();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.chkSubdirectories = new System.Windows.Forms.CheckBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblMonitoredFolderPath = new System.Windows.Forms.Label();
            this.pnlTop = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.fileWatcher)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileWatcher
            // 
            this.fileWatcher.EnableRaisingEvents = true;
            this.fileWatcher.IncludeSubdirectories = true;
            this.fileWatcher.SynchronizingObject = this;
            this.fileWatcher.Created += new System.IO.FileSystemEventHandler(this.fileWatcher_Created);
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(34, 42);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(51, 13);
            this.lblFilter.TabIndex = 5;
            this.lblFilter.Text = "File Filter:";
            // 
            // btnMonitor
            // 
            this.btnMonitor.Location = new System.Drawing.Point(255, 87);
            this.btnMonitor.Name = "btnMonitor";
            this.btnMonitor.Size = new System.Drawing.Size(75, 23);
            this.btnMonitor.TabIndex = 4;
            this.btnMonitor.Text = "Start";
            this.btnMonitor.UseVisualStyleBackColor = true;
            this.btnMonitor.Click += new System.EventHandler(this.btnMonitor_Click);
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(82, 39);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(129, 20);
            this.txtFilter.TabIndex = 2;
            this.txtFilter.Text = "*.*";
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(3, 16);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(82, 13);
            this.lblPath.TabIndex = 2;
            this.lblPath.Text = "Monitored Path:";
            // 
            // chkSubdirectories
            // 
            this.chkSubdirectories.AutoSize = true;
            this.chkSubdirectories.Checked = true;
            this.chkSubdirectories.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSubdirectories.Location = new System.Drawing.Point(82, 65);
            this.chkSubdirectories.Name = "chkSubdirectories";
            this.chkSubdirectories.Size = new System.Drawing.Size(129, 17);
            this.chkSubdirectories.TabIndex = 3;
            this.chkSubdirectories.Text = "Include subdirectories";
            this.chkSubdirectories.UseVisualStyleBackColor = true;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(336, 87);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lblMonitoredFolderPath
            // 
            this.lblMonitoredFolderPath.AutoSize = true;
            this.lblMonitoredFolderPath.Location = new System.Drawing.Point(91, 16);
            this.lblMonitoredFolderPath.Name = "lblMonitoredFolderPath";
            this.lblMonitoredFolderPath.Size = new System.Drawing.Size(0, 13);
            this.lblMonitoredFolderPath.TabIndex = 7;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.lblMonitoredFolderPath);
            this.pnlTop.Controls.Add(this.btnStop);
            this.pnlTop.Controls.Add(this.chkSubdirectories);
            this.pnlTop.Controls.Add(this.lblPath);
            this.pnlTop.Controls.Add(this.txtFilter);
            this.pnlTop.Controls.Add(this.btnMonitor);
            this.pnlTop.Controls.Add(this.lblFilter);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(423, 122);
            this.pnlTop.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 122);
            this.Controls.Add(this.pnlTop);
            this.Name = "Form1";
            this.Text = "File Monitor";
            ((System.ComponentModel.ISupportInitialize)(this.fileWatcher)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.FileSystemWatcher fileWatcher;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblMonitoredFolderPath;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.CheckBox chkSubdirectories;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Button btnMonitor;
        private System.Windows.Forms.Label lblFilter;
    }
}

