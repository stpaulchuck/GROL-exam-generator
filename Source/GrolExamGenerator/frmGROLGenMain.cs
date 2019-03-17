using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace GrolExamGenerator
{
	public enum ExamTypes { Regular, Subelement, KeyTopic };  // underlying values: 0, 1, 2

	public partial class frmGROLGenMain : Form
	{
		/**************************** vars ***************************************/
		bool bInitializing = true;
		DataTable oDescTable = new DataTable(), oQTable = new DataTable(); // holds selected element data
		DataTable oDescMasterTable = new DataTable(), oQMasterTable = new DataTable(); // holds all data
		DataTable oKeyTopicDescTable = new DataTable(); // holds the key topic descriptions for any given subelement
		Timer oTimeDelay = new Timer();
		frmGROLOutputFileName OutputFileForm = new frmGROLOutputFileName();
		Dictionary<string, int> dicElementQamt = new Dictionary<string, int>();
		string sDBfilepath = "";
		clsGROLSQLiteHandler SqlHandler = null;
		ExamTypes enCurrentExamType = ExamTypes.Regular;

		/**************************** constructor ********************************/
		public frmGROLGenMain()
		{
			InitializeComponent();
			OutputFileForm.Hide();
			SqlHandler = new clsGROLSQLiteHandler(this.txtStatus);
		}


		/****************************** events ************************************/
		private void frmGenMain_Shown(object sender, EventArgs e)
		{
			bInitializing = true;

			//------- load any saved values
			LoadSettings();

			//------ input intial data
			FetchMasterData();

			/*timer is used to prevent racing conditions loading UI objects with data*/
			//----- timer setups
			oTimeDelay.Enabled = true;
			oTimeDelay.Stop();
			oTimeDelay.Interval = 1500;
			oTimeDelay.Tick += oTimeDelay_Tick;
			//string sLastTest = Enum.GetName(typeof(ExamTypes), Properties.Settings.Default.LastTestType);
			int iLastTestType = Properties.Settings.Default.LastTestType;
			switch (iLastTestType)
			{
				case (int)ExamTypes.Regular:
					txtNumberOfQuestions.Text = dicElementQamt[cmbElementNumber.Text].ToString("0");
					break;
				case (int)ExamTypes.Subelement:
					txtNumberOfQuestions.Text = GetSubelementQcount().ToString();
					break;
				case (int)ExamTypes.KeyTopic:
					txtNumberOfQuestions.Text = GetKeyTopicQcount().ToString();
					break;
				default:
					throw new Exception("Unknown exam type! LoadSettings()");
			}
			cmbKeyTopic.Text = Properties.Settings.Default.LastKeyTopic;
			cmbSubelements.Text = Properties.Settings.Default.LastSubElement;
			//------ done
			bInitializing = false;
		}

		private void frmGenMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			Application.DoEvents();
			SaveSettings();
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnRun_Click(object sender, EventArgs e)
		{
			btnRun.Enabled = false;
			if (oQTable.Rows.Count == 0)
			{
				txtStatus.Text = "No questions available for this Element";
				MessageBox.Show(this, "There are no questions in the file for Element " + cmbElementNumber.Text);
				return;
			}
			bool retVal = CreateExam();
			this.BringToFront();
			this.Focus();
			btnRun.Enabled = true;
			if (!retVal)
				return;
			SaveSettings();
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
			oDlg.Filter = "grol_exam_data.db|grol_exam_data.db"; // only grol_exam_data.db
			if (oDlg.ShowDialog(this) == DialogResult.OK)
			{
				sDBfilepath = oDlg.FileName;
				txtDBpath.Text = sDBfilepath;  //.Substring(sDBfilepath.LastIndexOf("\\") + 1);
				Properties.Settings.Default.DataFilePath = oDlg.FileName;
				Properties.Settings.Default.Save();
				txtStatus.Text = "File Found";
				FetchMasterData(); // new file so load it
			}
		}

		private void rbTestType_CheckedChanged(object sender, EventArgs e)
		{
			RadioButton rb = (RadioButton)sender;
			if (rb.Checked)
			{
				int iTemp = (int)Enum.Parse(typeof(ExamTypes), (string)rb.Tag);
				Properties.Settings.Default.LastTestType = iTemp;
				Properties.Settings.Default.Save();
				switch ((string)rb.Tag)
				{
					case "Regular":
						enCurrentExamType = ExamTypes.Regular;
						txtNumberOfQuestions.Text = dicElementQamt[cmbElementNumber.Text].ToString();
						break;
					case "Subelement":
						enCurrentExamType = ExamTypes.Subelement;
						txtNumberOfQuestions.Text = GetSubelementQcount().ToString();
						break;
					case "KeyTopic":
						enCurrentExamType = ExamTypes.KeyTopic;
						txtNumberOfQuestions.Text = GetKeyTopicQcount().ToString();
						break;
					default:
						throw new Exception("Unknown test type in rbTestType_CheckedChanged");
				}
			}
		}

		private void cmbElementNumber_SelectedValueChanged(object sender, EventArgs e)
		{
			if (bInitializing) return;
			DisplaySubelementDescription();
			oTimeDelay.Stop();
			btnRun.Enabled = false;
			Application.DoEvents();
			oTimeDelay.Start();
			Properties.Settings.Default.LastExamElement = cmbElementNumber.Text;
			Properties.Settings.Default.Save();
			Application.DoEvents();
		}

		private void cmbKeyTopic_SelectedValueChanged(object sender, EventArgs e)
		{
			if (bInitializing) return;
			DisplayKeyTopicDescription();
			Properties.Settings.Default.LastKeyTopic = cmbKeyTopic.Text;
			Properties.Settings.Default.Save();
		}

		void oTimeDelay_Tick(object sender, EventArgs e)
		{
			bInitializing = true;
			oTimeDelay.Stop();
			FetchElementQuestions();
			FetchSubElementDescriptions();
			if (oQTable.Rows.Count > 0 && oDescTable.Rows.Count > 0)
			{
				txtNumberOfQuestions.Text = dicElementQamt[cmbElementNumber.Text].ToString("0");
				btnRun.Enabled = true;
			}
			else
			{
				txtNumberOfQuestions.Text = "0";
				btnRun.Enabled = false;
				txtStatus.Text = "Insufficient data for Element " + cmbElementNumber.Text;
			}
			bInitializing = false;
			Application.DoEvents();
		}

		private void cmbSubelements_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (bInitializing) return;

			DisplaySubelementDescription();
			FetchKeyTopicDescriptions();
			DisplayKeyTopicDescription();
			Properties.Settings.Default.LastSubElement = cmbSubelements.Text;
			Properties.Settings.Default.Save();
		}

		private void btnNormalQamt_Click(object sender, EventArgs e)
		{
			int iTemp = Properties.Settings.Default.LastTestType;
			switch (iTemp)
			{
				case (int)ExamTypes.Regular:
					txtNumberOfQuestions.Text = dicElementQamt[cmbElementNumber.Text].ToString("0");
					break;
				case (int)ExamTypes.Subelement:
					txtNumberOfQuestions.Text = GetSubelementQcount().ToString();
					break;
				case (int)ExamTypes.KeyTopic:
					txtNumberOfQuestions.Text = GetKeyTopicQcount().ToString();
					break;
				default:
					throw new Exception("Unknown exam type! btnNormalQamt_Click");
			} // end switch
		}

		private void btnAllQuestions_Click(object sender, EventArgs e)
		{
			string sWhereClause = "";
			var iCount = oQTable.Select(sWhereClause).Length;

			if (MessageBox.Show(this, "Are you sure you want all " + iCount.ToString() + " questions?", "Warning!", MessageBoxButtons.OKCancel
				 , MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
			{
				txtNumberOfQuestions.Text = iCount.ToString("0");
			}
		}

		//********************** private methods ***********************

		private bool FetchElementQuestions()
		{
			string sElementNumber = cmbElementNumber.Text.ToString();
			txtStatus.Text = "fetching questions for element " + sElementNumber;
			Application.DoEvents();
			oQTable.Clear();
			try
			{
				if (oQMasterTable.Rows.Count > 0)
				{
					DataRow[] oResults = oQMasterTable.Select("ElementNumber = " + sElementNumber);
					if (oResults.Length > 0)
					{
						oQTable = oResults.CopyToDataTable();
					}
				}
				if (oQTable.Rows.Count == 0)
				{
					MessageBox.Show(this, "No questions found for Element Number " + sElementNumber);
					return false;
				}
			}
			catch (Exception e7)
			{
				txtStatus.Text = "Error reading questions.";
				MessageBox.Show(this, "Error fetching questions: " + e7, "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			txtStatus.Text = "completed fetching questions for element " + cmbElementNumber.Text.ToString();
			return true; // default for good fetch
		}

		private void LoadSettings()
		{
			sDBfilepath = Properties.Settings.Default.DataFilePath;
			txtDBpath.Text = sDBfilepath; //.Substring(sDBfilepath.LastIndexOf("\\") + 1);

			dicElementQamt.Clear();
			char[] splitVal = new char[] { ',' };
			foreach (string s in Properties.Settings.Default.QsPerElement)
			{
				string[] splitArray = s.Split(splitVal);
				dicElementQamt.Add(splitArray[0], int.Parse(splitArray[1]));
			}
			if (Properties.Settings.Default.LastOutputFolder != "")
				OutputFileForm.txtOutputFolder.Text = Properties.Settings.Default.LastOutputFolder;
			if (Properties.Settings.Default.LastOutputFile != "")
				OutputFileForm.txtOutputFileName.Text = Properties.Settings.Default.LastOutputFile;
			chkRandomDist.Checked = Properties.Settings.Default.RandomDistrubtion;
			cmbElementNumber.Text = Properties.Settings.Default.LastExamElement.ToString();
			cmbSubelements.Text = Properties.Settings.Default.LastSubElement;
			cmbKeyTopic.Text = Properties.Settings.Default.LastKeyTopic;
			int iLastTest = Properties.Settings.Default.LastTestType;
			switch (iLastTest)
			{
				case (int)ExamTypes.Regular:
					enCurrentExamType = ExamTypes.Regular;
					rbRegularTest.Checked = true;
					break;
				case (int)ExamTypes.Subelement:
					enCurrentExamType = ExamTypes.Subelement;
					rbSubelementTest.Checked = true;
					break;
				case (int)ExamTypes.KeyTopic:
					enCurrentExamType = ExamTypes.KeyTopic;
					rbKeyTopicTest.Checked = true;
					break;
				default:
					throw new Exception("Unknown exam type! LoadSettings()");
			}
			chkRandomAnswers.Checked = Properties.Settings.Default.RandomizeAnswers;
			chkLearnMode.Checked = Properties.Settings.Default.LearnModeOn;
		}

		private void SaveSettings()
		{
			dicElementQamt[cmbElementNumber.Text] = int.Parse(txtNumberOfQuestions.Text);
			StringCollection oColl = new StringCollection();
			foreach (KeyValuePair<string, int> kvp in dicElementQamt)
			{
				oColl.Add(kvp.Key + "," + kvp.Value.ToString("0"));
			}
			Properties.Settings.Default.LastExamElement = cmbElementNumber.Text;
			Properties.Settings.Default.LastOutputFile = OutputFileForm.txtOutputFileName.Text;
			Properties.Settings.Default.LastOutputFolder = OutputFileForm.txtOutputFolder.Text;
			Properties.Settings.Default.LastSubElement = cmbSubelements.Text;
			Properties.Settings.Default.DataFilePath = sDBfilepath;
			Properties.Settings.Default.RandomDistrubtion = chkRandomDist.Checked;
			Properties.Settings.Default.RandomizeAnswers = chkRandomAnswers.Checked;
			Properties.Settings.Default.Save();
		}

		private bool CreateExam()
		{
			DataRow[] oSubnames = { };
			int iHowMany = -1;
			if (!int.TryParse(txtNumberOfQuestions.Text, out iHowMany))
			{
				MessageBox.Show(this, "Improper value for number of questions!", "Input Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			List<DataRow> oQlist = new List<DataRow>();
			string sFilter = cmbSubelements.Text;
			//---- first filter
			if (rbSubelementTest.Checked)
			{
				// sFilter = "QuestionNumber like '" + cmbSubelements.Text + "*'";
				foreach (DataRow dr in oQTable.Rows)
				{
					if (dr.Field<string>("QuestionNumber").Contains(sFilter))
					{ oQlist.Add(dr); }
				}
			}
			if (rbKeyTopicTest.Checked)
			{
				sFilter = cmbKeyTopic.Text;
				foreach (DataRow dr in oQTable.Rows)
				{
					string sTemp = dr.Field<string>("QuestionNumber");
					if (sTemp.Contains("-"))
					{ sTemp = sTemp.Substring(sTemp.IndexOf("-") + 1); }
					sTemp = sTemp.Substring(0, sFilter.Length);
					if (sTemp == sFilter)
					{ oQlist.Add(dr); }
				}
			}

			if (rbRegularTest.Checked)
			{
				foreach (DataRow dr in oQTable.Rows)
				{
					oQlist.Add(dr);
				}
			}

			//            DataRow[] oQRows = oQTable.Select(sFilter);
			DataRow[] oQRows = new DataRow[oQlist.Count];
			oQlist.CopyTo(oQRows);
			if (iHowMany > oQRows.Length)
			{
				if (MessageBox.Show(this, "You have asked for more questions than I can generate. If you want me to truncate "
					 + "the number to " + oQRows.Length.ToString() + " then click OK, or Cancel to stop.", "Size Error",
					 MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
				{
					iHowMany = oQRows.Length;
					txtNumberOfQuestions.Text = iHowMany.ToString();
				}
				else
				{
					return false;
				}
			}
			//---- second filter set up for descriptions
			sFilter = "";
			if (rbSubelementTest.Checked)
			{
				sFilter = "SubelementName = '" + cmbSubelements.Text + "' and ";
			}
			sFilter += "KeyTopic = '0'";
			if (oDescTable.Rows.Count > 0)
			{
				oSubnames = oDescTable.Select(sFilter);
			}
			else
			{
				oSubnames = new DataRow[] { };
			}
			//---- set up the printer
			if (rbTextFileExam.Checked)
			{
				if (Properties.Settings.Default.LastOutputFolder != "")
					OutputFileForm.txtOutputFolder.Text = Properties.Settings.Default.LastOutputFolder;
				if (Properties.Settings.Default.LastOutputFile != "")
					OutputFileForm.txtOutputFileName.Text = Properties.Settings.Default.LastOutputFile;
				if (OutputFileForm.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
				{
					MessageBox.Show(this, "User cancelled. Unable to save to file. Aborting.", "File path error.");
					return false;
				}
			}

			int iKeyTaskCount = oDescTable.AsEnumerable().Count(row => row.Field<string>("KeyTopic") != "0");

			clsGROLExamPrinter oPrinter = new clsGROLExamPrinter(oQRows);
			oPrinter.HowMany = iHowMany;
			oPrinter.KeyTopicCount = iKeyTaskCount;
			oPrinter.OutputFileName = OutputFileForm.txtOutputFileName.Text;
			oPrinter.OutputFolderName = OutputFileForm.txtOutputFolder.Text;
			oPrinter.RandomSpread = chkRandomDist.Checked;
			oPrinter.RandomizeAnswers = chkRandomAnswers.Checked;
			if (rbElectronicExam.Checked) oPrinter.OutputType = OutputTypeEnum.Desktop;
			else if (rbTextFileExam.Checked) oPrinter.OutputType = OutputTypeEnum.TxtFile;
			oPrinter.TestType = enCurrentExamType;
			oPrinter.LearnMode = chkLearnMode.Checked;

			//---- do it!
			return oPrinter.PrintExam();
		}

		private bool FetchMasterData()
		{
			oQMasterTable.Clear();
			oDescMasterTable.Clear();
			if (sDBfilepath == "")
			{
				MessageBox.Show(this, "No database file name entered. Please select one.", "Data File Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
			else
			{
				if (SqlHandler.TestSqlConnection(sDBfilepath))
				{
					oQMasterTable = SqlHandler.GetQuestions();
					if (oQMasterTable.Rows.Count > 0)
					{
						oDescMasterTable = SqlHandler.GetDescriptions();
						if (oDescMasterTable.Rows.Count > 0)
							btnRun.Enabled = true;
					}

				}
				else
				{
					MessageBox.Show(this, "Database file won't load. Reselect.", "DB File Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}
			}

			if (FetchElementQuestions())
			{
				FetchSubElementDescriptions();
			}
			txtNumberOfQuestions.Text = dicElementQamt[cmbElementNumber.Text].ToString("0");
			FetchKeyTopicDescriptions();
			DisplayKeyTopicDescription();
			return true;
		}

		private int GetSubelementQcount()
		{
			int retVal = oQTable.AsEnumerable().Count(row => row.Field<string>("Subelement") == cmbSubelements.Text);
			return retVal;
		}

		private int GetKeyTopicQcount()
		{
			int retVal = oQTable.AsEnumerable().Count(row => row.Field<string>("Subelement") == cmbSubelements.Text
				 && row.Field<string>("KeyTopicNumber") == cmbKeyTopic.Text);
			return retVal;
		}

		private void FetchSubElementDescriptions()
		{
			bInitializing = true;
			cmbSubelements.Items.Clear();
			cmbSubelements.Text = "";
			txtSubelementDesc.Text = "";
			Application.DoEvents();
			oDescTable.Clear();

			DataRow[] oResults = oDescMasterTable.Select("ElementNumber = " + cmbElementNumber.Text);
			if (oResults.Length > 0)
			{
				oDescTable = oResults.CopyToDataTable();
			}
			else
			{
				MessageBox.Show(this, "No Element Descriptions were found!", "Data Error");
				return;
			}
			foreach (DataRow oRow in oDescTable.Rows)
			{
				if (oRow.Field<string>("KeyTopic") == "0")
					cmbSubelements.Items.Add(oRow.Field<string>("SubElementName"));
			}
			cmbSubelements.Text = cmbSubelements.Items[0].ToString();
			if (Properties.Settings.Default.LastExamElement == cmbElementNumber.Text)
			{
				if (Properties.Settings.Default.LastSubElement != "")
					cmbSubelements.Text = Properties.Settings.Default.LastSubElement;
			}
			bInitializing = false;
			if (!cmbSubelements.Items.Contains(Properties.Settings.Default.LastSubElement))
			{
				cmbSubelements.Text = cmbSubelements.Items[0].ToString();
				Properties.Settings.Default.LastSubElement = cmbSubelements.Text;
				Properties.Settings.Default.Save();
			}
			DisplaySubelementDescription();
			txtStatus.Text = "completed fetching element " + cmbElementNumber.Text + " descriptions";
		}

		private void DisplaySubelementDescription()
		{
			DataRow[] oRows = oDescTable.Select("SubElementName = '" + cmbSubelements.Text + "' and KeyTopic = '0'");
			txtSubelementDesc.Text = oRows[0].Field<string>("DescriptiveText");
		}

		private void chkLearnMode_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.LearnModeOn = chkLearnMode.Checked;
			Properties.Settings.Default.Save();
		}

		private void FetchKeyTopicDescriptions()
		{
			oKeyTopicDescTable.Clear();
			cmbKeyTopic.Items.Clear();

			DataRow[] oRows = (from q in oDescTable.AsEnumerable()
									 where q.Field<string>("KeyTopic") != "0"
										  && q.Field<string>("SubelementName") == cmbSubelements.Text
									 select q).ToArray<DataRow>();

			oKeyTopicDescTable = oRows.CopyToDataTable();
			txtKeyTopic.Text = oRows[0].Field<string>("DescriptiveText");
			foreach (DataRow r in oRows)
			{
				cmbKeyTopic.Items.Add(r.Field<string>("KeyTopic"));
			}
			string s = Properties.Settings.Default.LastKeyTopic;
			if (cmbKeyTopic.Items.Contains(s))
			{ cmbKeyTopic.Text = s; }
			else
			{
				cmbKeyTopic.Text = cmbKeyTopic.Items[0].ToString();
				Properties.Settings.Default.LastKeyTopic = s;
			}
		}

		private void DisplayKeyTopicDescription()
		{
			DataRow[] oRows = oKeyTopicDescTable.Select("KeyTopic = '" + cmbKeyTopic.Text + "'");
			txtKeyTopic.Text = oRows[0].Field<string>("DescriptiveText");
		}

	}  // end class
}  // end namespace
