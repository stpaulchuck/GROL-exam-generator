using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


namespace GrolTestPoolParser
{
	public partial class frmGrolParserMain : Form
	{
		//******************* global vars **************************
		List<string> QuestionsCommandList = new List<string>(); // for MySQL commands
		List<string> DescriptionsCommandList = new List<string>(); // element descriptions
		string sDocumentText = "", sCurrentInputFilePath = "", sDBfilepath;
		clsSQLiteHandler oSqlHandler = null;
		clsErrorLogWriter oLogWriter = null;
		clsGrolDocParser oDocParser = null;
		bool bInitializing = true;

		//***************** contstructor ***************************
		public frmGrolParserMain()
		{
			InitializeComponent();
			oSqlHandler = new clsSQLiteHandler(txtStatus);
			oDocParser = new clsGrolDocParser(txtStatus);
			oLogWriter = new clsErrorLogWriter(Application.StartupPath);
			bInitializing = false;
		}


		//******************** event handlers **********************
		private void frmParserMain_Shown(object sender, EventArgs e)
		{
			int iPtr = -1;
			//------- load any saved values
			sDBfilepath = Properties.Settings.Default.DataFilePath;
			if (sDBfilepath != "")
			{
				iPtr = sDBfilepath.LastIndexOf("\\") + 1;
				if (!File.Exists(sDBfilepath))
				{
					iPtr = -99;
				}
				if (iPtr > 0)
				{
					txtDBname.Text = sDBfilepath.Substring(iPtr);
				}
				else
				{
					txtDBname.Text = "{ data file location not set! }";
				}
			}
			else
			{
				txtDBname.Text = "{ data file location not set! }";
			}

			if (Properties.Settings.Default.LastInputFile != "")
			{
				sCurrentInputFilePath = Properties.Settings.Default.LastInputFile;
				if (!sCurrentInputFilePath.Contains("\\"))
				{
					sCurrentInputFilePath = "";
					Properties.Settings.Default.LastInputFile = "";
					Properties.Settings.Default.Save();
					txtStatus.Text = "Stored input file path bad.";
					MessageBox.Show(this, "Stored input file path bad. Reselect file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					txtFileName.Text = " { no file selected } ";
				}
				else
				{
					txtFileName.Text = sCurrentInputFilePath.Substring(sCurrentInputFilePath.LastIndexOf("\\") + 1);
				}
			}
			if (File.Exists(sCurrentInputFilePath))
			{
				bInitializing = true;
				if (LoadSourceFile(sCurrentInputFilePath))
				{
					txtStatus.Text = "Input file loaded. Idle...";
				}
				else
				{
					txtFileName.Text = "{ no file loaded }";
				}
				bInitializing = false;
			}
			else
			{
				txtStatus.Text = "Failed to load input file!";
			}
		}

		private void frmParserMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			QuestionsCommandList.Clear();
			DescriptionsCommandList.Clear();
		}

		private void btnFindInputFile_Click(object sender, EventArgs e)
		{
			FindInputFile();
			//MessageBox.Show(this, "Now Select The Output File Or Set Appropriate Values For It.", "Next Step");
		}

		private void btnSelectDB_Click(object sender, EventArgs e)
		{
			txtStatus.Text = "Attempting to locate data file.";
			string sTemp = sDBfilepath;
			if (sTemp.Length > 3) // try to load the folder name
			{
				int iIndexer = sTemp.LastIndexOf('\\');
				sTemp = sTemp.Substring(0, iIndexer);
			}
			else
				sTemp = "\\";
			OpenFileDialog oDlg = new OpenFileDialog();
			if (!Directory.Exists(sTemp))
			{ sTemp = Application.StartupPath; }
			oDlg.InitialDirectory = sTemp;
			oDlg.Multiselect = false;
			oDlg.Filter = "grol_exam_data.db|grol_exam_data.db"; // only fcc_exam_data.db
			if (oDlg.ShowDialog(this) == DialogResult.OK)
			{
				sDBfilepath = oDlg.FileName;
				txtDBname.Text = sDBfilepath.Substring(sDBfilepath.LastIndexOf("\\") + 1);
				Properties.Settings.Default.DataFilePath = oDlg.FileName;
				Properties.Settings.Default.Save();
				txtStatus.Text = "grol_exam_data.db file found";
			}
			else
			{
				sDBfilepath = "";
				txtDBname.Text = "{ no file identfied }";
				Properties.Settings.Default.DataFilePath = "";
				Properties.Settings.Default.Save();
				txtStatus.Text = "grol_exam_data.db file not identified.";
			}
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnAbort_Click(object sender, EventArgs e)
		{
			txtStatus.Text = "User aborted!";
			throw new Exception("abort function not written!");
		}

		private void btnRun_Click(object sender, EventArgs e)
		{
			string udElementNumber = cmbElementNumber.Text;
			txtStatus.Text = "Starting parser run...";
			btnRun.Enabled = false;

			//---- validate MySql connection works
			if (!ValidateMySqlReady())
			{
				txtStatus.Text = "Parse Cancelled. Sql connection failed.";
				MessageBox.Show(this, "Parse is cancelled. Sql connection failed.", "SQL Server Error");
				return;
			}

			//---- parse the file and save the info to the tables
			try
			{
				oDocParser.ElementNumber = cmbElementNumber.Text;
				oDocParser.DescriptionsCommands = DescriptionsCommandList;
				oDocParser.QuestionsCommands = QuestionsCommandList;
				if (!oDocParser.ParseTextFile(sDocumentText))
				{
					oLogWriter.WriteErrorLog(oDocParser.LastError);
				}

				SaveQuestionsToMySQL();
			}
			catch (Exception e5)
			{
				oLogWriter.WriteErrorLog("Error creating or saving questions!!  " + e5.Message + "\n  stacktrace: " + e5.StackTrace);
				txtStatus.Text = "Error creating or saving questions!!";
				MessageBox.Show(this, "Error creating or saving questions: " + e5.Message, "ERROR");
				btnRun.Enabled = true;
				return;
			}

			//-------- if successful, save input property values
			if (sCurrentInputFilePath != "")
				Properties.Settings.Default.LastInputFile = sCurrentInputFilePath;
			Properties.Settings.Default.LastExamElement = int.Parse(cmbElementNumber.Text);
			Properties.Settings.Default.Save();
			btnRun.Enabled = true;
			txtStatus.Text = "Completed parsing run successfully.";
		}

		private void cmbElementNumber_SelectedValueChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.LastExamElement = int.Parse(cmbElementNumber.Text);
			Properties.Settings.Default.Save();
		}


		//********************** private methods ***********************

		private void FindInputFile()
		{
			txtStatus.Text = "Attempting to locate file to parse.";
			string sTemp = sCurrentInputFilePath;
			if (sTemp.Length > 3) // try to load the folder name
			{
				int iIndexer = sTemp.LastIndexOf('\\');
				sTemp = sTemp.Substring(0, iIndexer);
			}
			else
				sTemp = "";
			OpenFileDialog oDlg = new OpenFileDialog();
			if (!Directory.Exists(sTemp))
			{ sTemp = Application.StartupPath; }
			oDlg.InitialDirectory = sTemp;
			oDlg.Multiselect = false;
			oDlg.Filter = "*.txt|*.txt"; // only plain text files (for now)
			if (oDlg.ShowDialog(this) == DialogResult.OK)
			{
				sCurrentInputFilePath = oDlg.FileName;
				txtFileName.Text = sCurrentInputFilePath.Substring(sCurrentInputFilePath.LastIndexOf("\\") + 1);
				if (!LoadSourceFile(oDlg.FileName))
				{
					txtFileName.Text = "{ no file loaded }";
					return;
				}
				Properties.Settings.Default.LastInputFile = oDlg.FileName;
				Properties.Settings.Default.Save();
			}
			txtStatus.Text = "File Found";
		}

		private bool ValidateMySqlReady()
		{
			bool bResults = oSqlHandler.TestMySqlConnection(sDBfilepath);
			if (!bResults) btnRun.Enabled = true;
			return bResults;
		}

		private void SaveQuestionsToMySQL()
		{
			txtStatus.Text = "Saving Data.";
			//-------- set working values
			oSqlHandler.QuestionsCommandList = QuestionsCommandList;
			oSqlHandler.DescriptionsCommandList = DescriptionsCommandList;
			oSqlHandler.DBpath = sDBfilepath;
			oSqlHandler.ElementNumber = cmbElementNumber.Text;
			//-------- now do it
			oSqlHandler.SaveData();
		}

		private void btReload_Click(object sender, EventArgs e)
		{
			LoadSourceFile(sCurrentInputFilePath, true);
		}

		private bool LoadSourceFile(string FullPath, bool IsReload = false)
		{
			bool bResults = true;
			Application.DoEvents();
			sDocumentText = "";
			StreamReader oReader = null;
			try
			{
				oReader = new StreamReader(FullPath, System.Text.Encoding.UTF8, false);
				sDocumentText = oReader.ReadToEnd();
			}
			catch (Exception d)
			{
				MessageBox.Show(this, "Failed to load input file! - " + d.Message);
				txtStatus.Text = "ERROR: Failed to load input file.";
				bResults = false;
			}
			finally
			{
				if (oReader != null)
				{
					oReader.Close();
					oReader.Dispose();
				}
			}
			if (!bResults) return bResults;

			//------- clean up odd characters if any in text file
			char cRightSingleQuote = Convert.ToChar(146);
			char cLeftSingleQuote = Convert.ToChar(145);
			char cEnDash = Convert.ToChar(150);
			char cEmDash = Convert.ToChar(151);
			char LQuote = Convert.ToChar(147);
			char RQuote = Convert.ToChar(148);
			string cOddness = Convert.ToChar(65533).ToString();
			sDocumentText = sDocumentText.Replace(cRightSingleQuote, '\'');
			sDocumentText = sDocumentText.Replace(cLeftSingleQuote, '\'');
			sDocumentText = sDocumentText.Replace(cEnDash, '-');
			sDocumentText = sDocumentText.Replace(cEmDash, '-');
			sDocumentText = sDocumentText.Replace(LQuote, '"');
			sDocumentText = sDocumentText.Replace(RQuote, '"');
			sDocumentText = sDocumentText.Replace(cOddness, "-");
			Application.DoEvents();
			//----- done
			if (!IsReload)
			{
				if (bInitializing)
				{
					int iTestVal = Properties.Settings.Default.LastExamElement;
					cmbElementNumber.Text = iTestVal.ToString("0");
					Application.DoEvents();
				}
				else
				{
					cmbElementNumber.Text = "1";
					Application.DoEvents();
					MessageBox.Show(this, "Please set the Element Number to proceed.", "Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					Application.DoEvents();
				}
			}
			txtStatus.Text = "File to parse loaded. Idle...";
			return true;
		}

	}  // end class
} // end namespace
