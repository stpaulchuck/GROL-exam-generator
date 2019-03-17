namespace GrolTestPoolParser
{
	partial class frmGrolParserMain
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
			oSqlHandler.Dispose();
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGrolParserMain));
			this.label1 = new System.Windows.Forms.Label();
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.btnFindInputFile = new System.Windows.Forms.Button();
			this.btnExit = new System.Windows.Forms.Button();
			this.btnAbort = new System.Windows.Forms.Button();
			this.btnRun = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.btnSelectDB = new System.Windows.Forms.Button();
			this.txtDBname = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbElementNumber = new System.Windows.Forms.ComboBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.txtStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.btReload = new System.Windows.Forms.Button();
			this.groupBox3.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(289, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Input File";
			// 
			// txtFileName
			// 
			this.txtFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtFileName.Location = new System.Drawing.Point(72, 33);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.Size = new System.Drawing.Size(511, 23);
			this.txtFileName.TabIndex = 1;
			this.txtFileName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// btnFindInputFile
			// 
			this.btnFindInputFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnFindInputFile.Location = new System.Drawing.Point(607, 33);
			this.btnFindInputFile.Name = "btnFindInputFile";
			this.btnFindInputFile.Size = new System.Drawing.Size(75, 23);
			this.btnFindInputFile.TabIndex = 2;
			this.btnFindInputFile.Text = "Find";
			this.btnFindInputFile.UseVisualStyleBackColor = true;
			this.btnFindInputFile.Click += new System.EventHandler(this.btnFindInputFile_Click);
			// 
			// btnExit
			// 
			this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnExit.Location = new System.Drawing.Point(519, 250);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(75, 34);
			this.btnExit.TabIndex = 13;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// btnAbort
			// 
			this.btnAbort.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnAbort.Location = new System.Drawing.Point(364, 250);
			this.btnAbort.Name = "btnAbort";
			this.btnAbort.Size = new System.Drawing.Size(75, 34);
			this.btnAbort.TabIndex = 14;
			this.btnAbort.Text = "Abort";
			this.btnAbort.UseVisualStyleBackColor = true;
			this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
			// 
			// btnRun
			// 
			this.btnRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnRun.Location = new System.Drawing.Point(211, 250);
			this.btnRun.Name = "btnRun";
			this.btnRun.Size = new System.Drawing.Size(75, 34);
			this.btnRun.TabIndex = 15;
			this.btnRun.Text = "Run";
			this.btnRun.UseVisualStyleBackColor = true;
			this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(626, 104);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(113, 17);
			this.label4.TabIndex = 18;
			this.label4.Text = "Element Number";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.btnSelectDB);
			this.groupBox3.Controls.Add(this.txtDBname);
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.Location = new System.Drawing.Point(90, 88);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(467, 89);
			this.groupBox3.TabIndex = 28;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "MySql Server Properties";
			// 
			// btnSelectDB
			// 
			this.btnSelectDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSelectDB.Location = new System.Drawing.Point(361, 46);
			this.btnSelectDB.Name = "btnSelectDB";
			this.btnSelectDB.Size = new System.Drawing.Size(75, 23);
			this.btnSelectDB.TabIndex = 12;
			this.btnSelectDB.Text = "Select";
			this.btnSelectDB.UseVisualStyleBackColor = true;
			this.btnSelectDB.Click += new System.EventHandler(this.btnSelectDB_Click);
			// 
			// txtDBname
			// 
			this.txtDBname.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtDBname.Location = new System.Drawing.Point(68, 46);
			this.txtDBname.Name = "txtDBname";
			this.txtDBname.Size = new System.Drawing.Size(269, 23);
			this.txtDBname.TabIndex = 11;
			this.txtDBname.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(156, 26);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(110, 17);
			this.label3.TabIndex = 10;
			this.label3.Text = "Database Name";
			// 
			// cmbElementNumber
			// 
			this.cmbElementNumber.FormattingEnabled = true;
			this.cmbElementNumber.Items.AddRange(new object[] {
            "1",
            "3",
            "6",
            "7",
            "8",
            "9"});
			this.cmbElementNumber.Location = new System.Drawing.Point(646, 124);
			this.cmbElementNumber.Name = "cmbElementNumber";
			this.cmbElementNumber.Size = new System.Drawing.Size(54, 21);
			this.cmbElementNumber.TabIndex = 29;
			this.cmbElementNumber.Text = "1";
			this.cmbElementNumber.SelectedValueChanged += new System.EventHandler(this.cmbElementNumber_SelectedValueChanged);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtStatus,
            this.toolStripStatusLabel2});
			this.statusStrip1.Location = new System.Drawing.Point(10, 373);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(834, 22);
			this.statusStrip1.TabIndex = 30;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// txtStatus
			// 
			this.txtStatus.Name = "txtStatus";
			this.txtStatus.Size = new System.Drawing.Size(739, 17);
			this.txtStatus.Spring = true;
			this.txtStatus.Text = "Idle ...";
			// 
			// toolStripStatusLabel2
			// 
			this.toolStripStatusLabel2.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.toolStripStatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.toolStripStatusLabel2.Size = new System.Drawing.Size(80, 17);
			this.toolStripStatusLabel2.Text = " Parser V1.0";
			// 
			// btReload
			// 
			this.btReload.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btReload.Location = new System.Drawing.Point(714, 33);
			this.btReload.Name = "btReload";
			this.btReload.Size = new System.Drawing.Size(75, 23);
			this.btReload.TabIndex = 31;
			this.btReload.Text = "Reload";
			this.btReload.UseVisualStyleBackColor = true;
			this.btReload.Click += new System.EventHandler(this.btReload_Click);
			// 
			// frmGrolParserMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(844, 395);
			this.Controls.Add(this.btReload);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.cmbElementNumber);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.btnRun);
			this.Controls.Add(this.btnAbort);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.btnFindInputFile);
			this.Controls.Add(this.txtFileName);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmGrolParserMain";
			this.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.Text = "FCC Test Pool Parser";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmParserMain_FormClosing);
			this.Shown += new System.EventHandler(this.frmParserMain_Shown);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtFileName;
		private System.Windows.Forms.Button btnFindInputFile;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Button btnAbort;
		private System.Windows.Forms.Button btnRun;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button btnSelectDB;
		private System.Windows.Forms.TextBox txtDBname;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cmbElementNumber;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel txtStatus;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
		private System.Windows.Forms.Button btReload;
	}
}

