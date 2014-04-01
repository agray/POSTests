using System;
using System.Collections;
using System.ServiceProcess;
using System.Windows.Forms;

namespace WindowsApplication3
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class Form1 : System.Windows.Forms.Form {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DataGrid dataGrid1;
        private System.Windows.Forms.Button button1;
        private ArrayList ALServiceInfo;
        private System.Windows.Forms.Button button2;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public Form1() {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            ALServiceInfo = new ArrayList();

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if(disposing) {
                if(components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server Name:";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(112, 8);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(152, 20);
            this.txtServer.TabIndex = 1;
            this.txtServer.Text = "Local";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(16, 40);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "Get Services";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // dataGrid1
            // 
            this.dataGrid1.CaptionText = "Services List";
            this.dataGrid1.DataMember = "";
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(16, 72);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(336, 248);
            this.dataGrid1.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(144, 40);
            this.button1.Name = "button1";
            this.button1.TabIndex = 4;
            this.button1.Text = "Stop";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(224, 40);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 24);
            this.button2.TabIndex = 5;
            this.button2.Text = "Start";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(384, 333);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.button2,
																		  this.button1,
																		  this.dataGrid1,
																		  this.btnOK,
																		  this.txtServer,
																		  this.label1});
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        //static void Main() {
        //    Application.Run(new Form1());
        //}

        private void btnOK_Click(object sender, System.EventArgs e) {
            DisplayServices();
        }

        private void button1_Click(object sender, System.EventArgs e) {
            ProcessRequest(true);
        }
        private void DisplayServices() {
            //string strServerName;
            ServiceController[] oArrServices;
            try {

                if((txtServer.Text.Length == 0) || (txtServer.Text == "Local")) {
                    oArrServices = ServiceController.GetServices();
                } else
                    oArrServices = ServiceController.GetServices(txtServer.Text);

                ALServiceInfo.Clear();
                foreach(ServiceController oSC in oArrServices) {
                    ALServiceInfo.Add(new ServiceInfo(oSC.ServiceName, oSC.Status.ToString()));
                }
                dataGrid1.DataSource = ALServiceInfo;
                dataGrid1.Refresh();
            } catch(Exception ex) {
                MessageBox.Show("Error Occured: " + ex.Message);
            }
        }

        private void ProcessRequest(Boolean bStop) {
            ServiceController oSC;
            for(int i = 0; i < ALServiceInfo.Count; i++) {
                if(dataGrid1.IsSelected(i)) {

                    if((txtServer.Text.Length == 0) || (txtServer.Text == "Local")) {
                        oSC = new ServiceController(dataGrid1[i, 0].ToString());
                    } else {
                        oSC = new ServiceController(dataGrid1[i, 0].ToString(), txtServer.Text);
                    }

                    if(bStop) {
                        if(oSC.CanStop) {
                            oSC.Stop();
                        }
                    } else {
                        if((oSC.Status.ToString() != "Started") & (oSC.Status.ToString() != "Pending Start")) {
                            oSC.Start();
                        }
                    }

                }
            }
            DisplayServices();
        }

        private void button2_Click(object sender, System.EventArgs e) {
            ProcessRequest(false);
        }
    }

	class ServiceInfo
	{
		private string strName, strStatus;
		public ServiceInfo(string sName, string sStatus)
		{
			strName = sName ;
			strStatus = sStatus;
		}
		public string ServiceName
		{
			get {return strName;}
		}
		public string ServiceStatus
		{
			get {return strStatus;}
		}	

	}

}
