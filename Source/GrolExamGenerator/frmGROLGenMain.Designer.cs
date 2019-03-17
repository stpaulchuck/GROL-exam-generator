namespace GrolExamGenerator
{
    partial class frmGROLGenMain
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
			if (disposing)
			{
				if (SqlHandler != null)
				{
					SqlHandler.Dispose();
				}
				if (components != null)
				{
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
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGROLGenMain));
			this.btnRun = new System.Windows.Forms.Button();
			this.btnExit = new System.Windows.Forms.Button();
			this.btnSelectDB = new System.Windows.Forms.Button();
			this.txtDBpath = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.txtStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.txtSubelementDesc = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.rbTextFileExam = new System.Windows.Forms.RadioButton();
			this.rbElectronicExam = new System.Windows.Forms.RadioButton();
			this.cmbSubelements = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkLearnMode = new System.Windows.Forms.CheckBox();
			this.rbKeyTopicTest = new System.Windows.Forms.RadioButton();
			this.rbSubelementTest = new System.Windows.Forms.RadioButton();
			this.rbRegularTest = new System.Windows.Forms.RadioButton();
			this.cmbElementNumber = new System.Windows.Forms.ComboBox();
			this.cmbKeyTopic = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.txtKeyTopic = new System.Windows.Forms.TextBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.btnNormalQamt = new System.Windows.Forms.Button();
			this.btnAllQuestions = new System.Windows.Forms.Button();
			this.txtNumberOfQuestions = new System.Windows.Forms.TextBox();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.chkRandomDist = new System.Windows.Forms.CheckBox();
			this.chkRandomAnswers = new System.Windows.Forms.CheckBox();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnRun
			// 
			this.btnRun.Enabled = false;
			this.btnRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnRun.Location = new System.Drawing.Point(309, 426);
			this.btnRun.Name = "btnRun";
			this.btnRun.Size = new System.Drawing.Size(75, 34);
			this.btnRun.TabIndex = 28;
			this.btnRun.Text = "Run";
			this.btnRun.UseVisualStyleBackColor = true;
			this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
			// 
			// btnExit
			// 
			this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnExit.Location = new System.Drawing.Point(416, 426);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(75, 34);
			this.btnExit.TabIndex = 26;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// btnSelectDB
			// 
			this.btnSelectDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSelectDB.Location = new System.Drawing.Point(680, 54);
			this.btnSelectDB.Name = "btnSelectDB";
			this.btnSelectDB.Size = new System.Drawing.Size(75, 23);
			this.btnSelectDB.TabIndex = 20;
			this.btnSelectDB.Text = "Find";
			this.btnSelectDB.UseVisualStyleBackColor = true;
			this.btnSelectDB.Click += new System.EventHandler(this.btnSelectDB_Click);
			// 
			// txtDBpath
			// 
			this.txtDBpath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtDBpath.Location = new System.Drawing.Point(72, 54);
			this.txtDBpath.Name = "txtDBpath";
			this.txtDBpath.Size = new System.Drawing.Size(583, 23);
			this.txtDBpath.TabIndex = 19;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(307, 34);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(127, 17);
			this.label3.TabIndex = 18;
			this.label3.Text = "Database Location";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(128, 332);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 17);
			this.label1.TabIndex = 30;
			this.label1.Text = "Exam Element";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 532);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(826, 22);
			this.statusStrip1.TabIndex = 32;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// txtStatus
			// 
			this.txtStatus.Name = "txtStatus";
			this.txtStatus.Size = new System.Drawing.Size(708, 17);
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
			this.toolStripStatusLabel2.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
			this.toolStripStatusLabel2.Size = new System.Drawing.Size(103, 17);
			this.toolStripStatusLabel2.Text = "Test Gen. V2.0";
			// 
			// txtSubelementDesc
			// 
			this.txtSubelementDesc.Location = new System.Drawing.Point(72, 187);
			this.txtSubelementDesc.Multiline = true;
			this.txtSubelementDesc.Name = "txtSubelementDesc";
			this.txtSubelementDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtSubelementDesc.Size = new System.Drawing.Size(556, 44);
			this.txtSubelementDesc.TabIndex = 34;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(244, 167);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(158, 17);
			this.label7.TabIndex = 38;
			this.label7.Text = "Subelement Description";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.rbTextFileExam);
			this.groupBox3.Controls.Add(this.rbElectronicExam);
			this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox3.Location = new System.Drawing.Point(291, 335);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(424, 50);
			this.groupBox3.TabIndex = 34;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Output Type";
			// 
			// rbTextFileExam
			// 
			this.rbTextFileExam.AutoSize = true;
			this.rbTextFileExam.Location = new System.Drawing.Point(231, 20);
			this.rbTextFileExam.Name = "rbTextFileExam";
			this.rbTextFileExam.Size = new System.Drawing.Size(154, 21);
			this.rbTextFileExam.TabIndex = 2;
			this.rbTextFileExam.Text = "Text File Exam (*.txt)";
			this.rbTextFileExam.UseVisualStyleBackColor = true;
			// 
			// rbElectronicExam
			// 
			this.rbElectronicExam.AutoSize = true;
			this.rbElectronicExam.Checked = true;
			this.rbElectronicExam.Location = new System.Drawing.Point(51, 20);
			this.rbElectronicExam.Name = "rbElectronicExam";
			this.rbElectronicExam.Size = new System.Drawing.Size(126, 21);
			this.rbElectronicExam.TabIndex = 0;
			this.rbElectronicExam.TabStop = true;
			this.rbElectronicExam.Text = "Electronic Exam";
			this.rbElectronicExam.UseVisualStyleBackColor = true;
			// 
			// cmbSubelements
			// 
			this.cmbSubelements.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbSubelements.FormattingEnabled = true;
			this.cmbSubelements.Location = new System.Drawing.Point(661, 204);
			this.cmbSubelements.Name = "cmbSubelements";
			this.cmbSubelements.Size = new System.Drawing.Size(79, 24);
			this.cmbSubelements.Sorted = true;
			this.cmbSubelements.TabIndex = 47;
			this.cmbSubelements.SelectedIndexChanged += new System.EventHandler(this.cmbSubelements_SelectedIndexChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(659, 183);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(83, 17);
			this.label6.TabIndex = 46;
			this.label6.Text = "Subelement";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkLearnMode);
			this.groupBox1.Controls.Add(this.rbKeyTopicTest);
			this.groupBox1.Controls.Add(this.rbSubelementTest);
			this.groupBox1.Controls.Add(this.rbRegularTest);
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(177, 103);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(446, 42);
			this.groupBox1.TabIndex = 54;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Test Type";
			// 
			// chkLearnMode
			// 
			this.chkLearnMode.AutoSize = true;
			this.chkLearnMode.Location = new System.Drawing.Point(333, 17);
			this.chkLearnMode.Name = "chkLearnMode";
			this.chkLearnMode.Size = new System.Drawing.Size(103, 21);
			this.chkLearnMode.TabIndex = 3;
			this.chkLearnMode.Text = "Learn Mode";
			this.chkLearnMode.UseVisualStyleBackColor = true;
			this.chkLearnMode.CheckedChanged += new System.EventHandler(this.chkLearnMode_CheckedChanged);
			// 
			// rbKeyTopicTest
			// 
			this.rbKeyTopicTest.AutoSize = true;
			this.rbKeyTopicTest.Location = new System.Drawing.Point(226, 15);
			this.rbKeyTopicTest.Name = "rbKeyTopicTest";
			this.rbKeyTopicTest.Size = new System.Drawing.Size(89, 21);
			this.rbKeyTopicTest.TabIndex = 2;
			this.rbKeyTopicTest.TabStop = true;
			this.rbKeyTopicTest.Tag = "KeyTopic";
			this.rbKeyTopicTest.Text = "Key Topic";
			this.rbKeyTopicTest.UseVisualStyleBackColor = true;
			this.rbKeyTopicTest.CheckedChanged += new System.EventHandler(this.rbTestType_CheckedChanged);
			// 
			// rbSubelementTest
			// 
			this.rbSubelementTest.AutoSize = true;
			this.rbSubelementTest.Location = new System.Drawing.Point(113, 16);
			this.rbSubelementTest.Name = "rbSubelementTest";
			this.rbSubelementTest.Size = new System.Drawing.Size(101, 21);
			this.rbSubelementTest.TabIndex = 1;
			this.rbSubelementTest.Tag = "Subelement";
			this.rbSubelementTest.Text = "Subelement";
			this.rbSubelementTest.UseVisualStyleBackColor = true;
			this.rbSubelementTest.CheckedChanged += new System.EventHandler(this.rbTestType_CheckedChanged);
			// 
			// rbRegularTest
			// 
			this.rbRegularTest.AutoSize = true;
			this.rbRegularTest.Checked = true;
			this.rbRegularTest.Location = new System.Drawing.Point(22, 16);
			this.rbRegularTest.Name = "rbRegularTest";
			this.rbRegularTest.Size = new System.Drawing.Size(76, 21);
			this.rbRegularTest.TabIndex = 0;
			this.rbRegularTest.TabStop = true;
			this.rbRegularTest.Tag = "Regular";
			this.rbRegularTest.Text = "Regular";
			this.rbRegularTest.UseVisualStyleBackColor = true;
			this.rbRegularTest.CheckedChanged += new System.EventHandler(this.rbTestType_CheckedChanged);
			// 
			// cmbElementNumber
			// 
			this.cmbElementNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbElementNumber.FormattingEnabled = true;
			this.cmbElementNumber.Items.AddRange(new object[] {
            "1",
            "3",
            "6",
            "7",
            "8",
            "9"});
			this.cmbElementNumber.Location = new System.Drawing.Point(136, 352);
			this.cmbElementNumber.Name = "cmbElementNumber";
			this.cmbElementNumber.Size = new System.Drawing.Size(68, 24);
			this.cmbElementNumber.TabIndex = 56;
			this.cmbElementNumber.Text = "1";
			this.cmbElementNumber.SelectedValueChanged += new System.EventHandler(this.cmbElementNumber_SelectedValueChanged);
			// 
			// cmbKeyTopic
			// 
			this.cmbKeyTopic.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbKeyTopic.FormattingEnabled = true;
			this.cmbKeyTopic.Location = new System.Drawing.Point(661, 288);
			this.cmbKeyTopic.Name = "cmbKeyTopic";
			this.cmbKeyTopic.Size = new System.Drawing.Size(79, 24);
			this.cmbKeyTopic.TabIndex = 60;
			this.cmbKeyTopic.SelectedValueChanged += new System.EventHandler(this.cmbKeyTopic_SelectedValueChanged);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(659, 267);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(71, 17);
			this.label8.TabIndex = 59;
			this.label8.Text = "Key Topic";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.Location = new System.Drawing.Point(256, 251);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(146, 17);
			this.label10.TabIndex = 58;
			this.label10.Text = "Key Topic Description";
			// 
			// txtKeyTopic
			// 
			this.txtKeyTopic.Location = new System.Drawing.Point(72, 271);
			this.txtKeyTopic.Multiline = true;
			this.txtKeyTopic.Name = "txtKeyTopic";
			this.txtKeyTopic.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtKeyTopic.Size = new System.Drawing.Size(556, 44);
			this.txtKeyTopic.TabIndex = 57;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.btnNormalQamt);
			this.groupBox5.Controls.Add(this.btnAllQuestions);
			this.groupBox5.Controls.Add(this.txtNumberOfQuestions);
			this.groupBox5.Location = new System.Drawing.Point(12, 416);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(256, 54);
			this.groupBox5.TabIndex = 61;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Number Of Questions";
			// 
			// btnNormalQamt
			// 
			this.btnNormalQamt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnNormalQamt.Location = new System.Drawing.Point(165, 23);
			this.btnNormalQamt.Name = "btnNormalQamt";
			this.btnNormalQamt.Size = new System.Drawing.Size(71, 23);
			this.btnNormalQamt.TabIndex = 48;
			this.btnNormalQamt.Text = "Normal";
			this.btnNormalQamt.UseVisualStyleBackColor = true;
			this.btnNormalQamt.Click += new System.EventHandler(this.btnNormalQamt_Click);
			// 
			// btnAllQuestions
			// 
			this.btnAllQuestions.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnAllQuestions.Location = new System.Drawing.Point(107, 23);
			this.btnAllQuestions.Name = "btnAllQuestions";
			this.btnAllQuestions.Size = new System.Drawing.Size(39, 23);
			this.btnAllQuestions.TabIndex = 47;
			this.btnAllQuestions.Text = "All";
			this.btnAllQuestions.UseVisualStyleBackColor = true;
			this.btnAllQuestions.Click += new System.EventHandler(this.btnAllQuestions_Click);
			// 
			// txtNumberOfQuestions
			// 
			this.txtNumberOfQuestions.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtNumberOfQuestions.Location = new System.Drawing.Point(23, 23);
			this.txtNumberOfQuestions.Name = "txtNumberOfQuestions";
			this.txtNumberOfQuestions.Size = new System.Drawing.Size(69, 23);
			this.txtNumberOfQuestions.TabIndex = 46;
			this.txtNumberOfQuestions.Text = "24";
			this.txtNumberOfQuestions.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.chkRandomDist);
			this.groupBox6.Controls.Add(this.chkRandomAnswers);
			this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox6.Location = new System.Drawing.Point(547, 410);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(222, 50);
			this.groupBox6.TabIndex = 62;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Randomize";
			// 
			// chkRandomDist
			// 
			this.chkRandomDist.AutoSize = true;
			this.chkRandomDist.Location = new System.Drawing.Point(102, 24);
			this.chkRandomDist.Name = "chkRandomDist";
			this.chkRandomDist.Size = new System.Drawing.Size(98, 21);
			this.chkRandomDist.TabIndex = 5;
			this.chkRandomDist.Text = "Distribution";
			this.chkRandomDist.UseVisualStyleBackColor = true;
			// 
			// chkRandomAnswers
			// 
			this.chkRandomAnswers.AutoSize = true;
			this.chkRandomAnswers.Location = new System.Drawing.Point(17, 24);
			this.chkRandomAnswers.Name = "chkRandomAnswers";
			this.chkRandomAnswers.Size = new System.Drawing.Size(80, 21);
			this.chkRandomAnswers.TabIndex = 4;
			this.chkRandomAnswers.Text = "Answers";
			this.chkRandomAnswers.UseVisualStyleBackColor = true;
			// 
			// frmGROLGenMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(826, 554);
			this.Controls.Add(this.groupBox6);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.cmbKeyTopic);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.txtKeyTopic);
			this.Controls.Add(this.cmbElementNumber);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cmbSubelements);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.txtSubelementDesc);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnRun);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.btnSelectDB);
			this.Controls.Add(this.txtDBpath);
			this.Controls.Add(this.label3);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmGROLGenMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "GROL Test Generator";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGenMain_FormClosing);
			this.Shown += new System.EventHandler(this.frmGenMain_Shown);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSelectDB;
        private System.Windows.Forms.TextBox txtDBpath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel txtStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.TextBox txtSubelementDesc;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbTextFileExam;
        private System.Windows.Forms.RadioButton rbElectronicExam;
        private System.Windows.Forms.ComboBox cmbSubelements;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbSubelementTest;
        private System.Windows.Forms.RadioButton rbRegularTest;
        private System.Windows.Forms.ComboBox cmbElementNumber;
        private System.Windows.Forms.ComboBox cmbKeyTopic;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtKeyTopic;
        private System.Windows.Forms.RadioButton rbKeyTopicTest;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnNormalQamt;
        private System.Windows.Forms.Button btnAllQuestions;
        private System.Windows.Forms.TextBox txtNumberOfQuestions;
        private System.Windows.Forms.CheckBox chkLearnMode;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.CheckBox chkRandomDist;
		private System.Windows.Forms.CheckBox chkRandomAnswers;
	}
}

