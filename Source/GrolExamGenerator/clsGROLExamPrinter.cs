using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;


namespace GrolExamGenerator
{
	public enum OutputTypeEnum { Desktop, TxtFile, Paper };

	class clsGROLExamPrinter : IDisposable
	{
		/*************** Properties **************/
		private int m_HowMany = 0;
		public int HowMany { get { return m_HowMany; } set { m_HowMany = value; } }

		private bool m_RandomSpread = false;
		public bool RandomSpread { get { return m_RandomSpread; } set { m_RandomSpread = value; } }

		private string m_OutputFolderName = "";
		public string OutputFolderName { get { return m_OutputFolderName; } set { m_OutputFolderName = value; } }

		private string m_OutputFileName = "";
		public string OutputFileName { get { return m_OutputFileName; } set { m_OutputFileName = value; } }

		private int m_KeyTopicCount = 0;  // derived from descriptions db
		public int KeyTopicCount { get { return m_KeyTopicCount; } set { m_KeyTopicCount = value; } }

		private OutputTypeEnum m_OutputType = OutputTypeEnum.Desktop;
		public OutputTypeEnum OutputType { get { return m_OutputType; } set { m_OutputType = value; } }

		private bool m_RandomizeAnswers = true;
		public bool RandomizeAnswers { get { return m_RandomizeAnswers; } set { m_RandomizeAnswers = value; } }

		private ExamTypes m_TestType = ExamTypes.Regular;
		public ExamTypes TestType { get { return m_TestType; } set { m_TestType = value; } }

		private bool m_LearnMode = false;
		public bool LearnMode { get { return m_LearnMode; } set { m_LearnMode = value; } }


		// todo: write code in printer for 'learn mode'

		/*************** global vars **************/
		DataRow[] m_QuestionsPool = new DataRow[0];
		int iPoolSize = 0, iHowMany = 0, iElementNumber = 0;
		const int iElement1 = 24, iElement3 = 100, iElement6 = 100; // in case it changes, just one item to update
		const int iElement7 = 100, iElement8 = 50, iElement9 = 50;

		/*************** slave classes **************/
		frmGROLElectronicExam ElectronicExamForm = new frmGROLElectronicExam();

		/*************** constructor **************/
		public clsGROLExamPrinter(DataRow[] QuestionsPool)
		{
			m_QuestionsPool = QuestionsPool;
			iElementNumber = (int)m_QuestionsPool[0]["ElementNumber"];
		}


		/*************** methods **************/
		public bool PrintExam()
		{
			iPoolSize = m_QuestionsPool.Length;
			iHowMany = HowMany;
			if (HowMany == -1)
				iHowMany = iPoolSize; // "all"

			if (m_TestType != ExamTypes.Regular || iElementNumber == 6)
			{ m_RandomSpread = true; } // for Key Topic and Subelement, just pick randomly

			List<DataRow> lstSelectedQuestions = null;
			try
			{
				if (m_RandomSpread) lstSelectedQuestions = RandomSelection();
				else lstSelectedQuestions = EvenSelection();

				if (!m_LearnMode) // learn mode has it's own pattern
				{
					if (m_RandomizeAnswers)
					{
						RandomizeTheAnswers(lstSelectedQuestions);
					}
				}
				else
					LearnModeTheAnswers(lstSelectedQuestions);

				return PrintTheFile(lstSelectedQuestions);
			}
			catch (Exception e)
			{
				Debug.WriteLine("Error generating output file in PrintExam()! " + e.Message + "   stacktrace = " + e.StackTrace);
				MessageBox.Show("Error generating output file in PrintExam()! " + e.Message, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
		}

		private List<DataRow> RandomSelection()
		{
			Random oRnd = new Random();
			List<int> PickedList = new List<int>();
			int iPtr = -1; // random pointer into m_QuestionsPool
			PickedList.Add(-1);

			oRnd.Next(HowMany); // just a dummy to light it up

			DataRow[] retVal = new DataRow[HowMany];

			for (int iIndexer = 0; iIndexer < iHowMany; iIndexer++)
			{
				while (PickedList.Contains(iPtr))
				{
					iPtr = oRnd.Next(iPoolSize);
				}
				PickedList.Add(iPtr);
				retVal[iIndexer] = m_QuestionsPool[iPtr];
			}

			return retVal.ToList<DataRow>();
		}

		private List<DataRow> EvenSelection()
		{
			List<DataRow> retVal = new List<DataRow>();

			// we'll have to split this up by Key Task
			List<string> QnumList = new List<string>();
			QnumList = new List<string>();
			foreach (DataRow r in m_QuestionsPool)
			{
				QnumList.Add(r.Field<string>("QuestionNumber"));
			}
			// now use the ratio to pull evenly from the question list by key topic
			Double fRatio = (double)iHowMany / (double)m_KeyTopicCount;
			if (fRatio >= 1)
			{
				retVal = GetIntegral((int)fRatio, QnumList);
			}
			double fRemainder = fRatio % 1;
			if (fRemainder > 0.0)
			{
				int iCount = (int)Math.Round(fRemainder * (double)m_KeyTopicCount);
				List<DataRow> lRemainder = GetRemainder(iCount, QnumList);
				foreach (DataRow r in lRemainder)
				{ retVal.Add(r); }
			}

			//---------
			return retVal;
		}

		private List<DataRow> GetIntegral(int QperKeyTopic, List<string> QuestionNumList)
		{
			List<DataRow> retVal = new List<DataRow>(); // question pick list

			Random oRnd = new Random();

			int iQcount = 0;
			string qName = "";
			List<string> groupQnumList = null;
			try
			{
				for (int iIndexer = 1; iIndexer <= m_KeyTopicCount; iIndexer++)
				{
					//------ now get the questions for each group
					groupQnumList = (from DataRow sAnyPick in m_QuestionsPool
										  where sAnyPick.Field<string>("KeyTopicNumber") == iIndexer.ToString()
										  select sAnyPick.Field<string>("QuestionNumber")).ToList<string>();

					//------ then randomly pick the QperGroup number of question numbers
					iQcount = groupQnumList.Count;
					for (int jdex = 0; jdex < QperKeyTopic; jdex++)
					{
						qName = groupQnumList[oRnd.Next(0, iQcount)];
						// linq query on m_questionspool where quest. number = random select from numlist
						List<DataRow> oSelectedRow = (from DataRow oRow in m_QuestionsPool
																where oRow.Field<string>("QuestionNumber") == qName
																select oRow).ToList<DataRow>();
						foreach (DataRow r in oSelectedRow)
						{
							retVal.Add(r);
						}
						// now bleep out the num list item we chose
						groupQnumList.Remove(qName);
						groupQnumList.TrimExcess();
						QuestionNumList.Remove(qName);
						QuestionNumList.TrimExcess();
						iQcount = groupQnumList.Count;
						if (groupQnumList.Count <= 0) // underflow test
							break;
					}
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine("error in GetIntegral() - " + e.Message + "   stacktrace = " + e.StackTrace);
				throw;
			}
			//------ finally return the pick list

			return retVal;
		}

		private List<DataRow> GetRemainder(int Qcount, List<string> QuestionNumList)
		{
			Random oRnd = new Random();

			List<DataRow> retVal = new List<DataRow>();
			for (int iIndexer = 0; iIndexer < Qcount; iIndexer++)
			{
				string qName = QuestionNumList[oRnd.Next(0, QuestionNumList.Count - 1)];
				// linq query on m_questionspool where quest. number = random select from numlist
				List<DataRow> oSelectedRow = (from DataRow oRow in m_QuestionsPool
														where oRow.Field<string>("QuestionNumber") == qName
														select oRow).ToList<DataRow>();
				foreach (DataRow r in oSelectedRow)
				{
					retVal.Add(r);
				}
				// now bleep out the num list item we chose
				QuestionNumList.Remove(qName);
			}

			return retVal;
		}

		private bool PrintTheFile(List<DataRow> OutputPool)
		{
			switch (m_OutputType)
			{
				case OutputTypeEnum.Desktop:
					return UseElectronicForm(OutputPool);
				case OutputTypeEnum.TxtFile:
					return SaveToTxtFile(OutputPool);
				case OutputTypeEnum.Paper:
					MessageBox.Show("Output Exam to paper not yet impletmented.", "Output choice Error");
					break;
				default:
					MessageBox.Show("Output type " + m_OutputType.ToString() + " not impletmented yet!", "Error");
					break;
			}
			return false;
		}

		private bool SaveToTxtFile(List<DataRow> Questions)
		{
			string sKeyFileName = m_OutputFileName;
			sKeyFileName = sKeyFileName.ToLower().Replace(".txt", "");
			sKeyFileName = sKeyFileName + " - answer key.txt";
			if (m_OutputFileName.Length < 5 || m_OutputFileName.Substring(m_OutputFileName.Length - 4) != ".txt")
			{
				m_OutputFileName = m_OutputFileName + ".txt";
			}
			StreamWriter oWriter = new StreamWriter(m_OutputFolderName + "\\" + m_OutputFileName, false);
			string sOutputText = "";
			int iElement = Questions[0].Field<int>("ElementNumber");
			try
			{
				switch (iElement.ToString("0"))
				{
					case "1":
						sOutputText = "Element 1: Basic radio law and operating practice";
						break;
					case "3":
						sOutputText = "Element 3: General Radiotelephone. Electronic fundamentals and techniques.";
						break;
					case "6":
						sOutputText = "Element 6: Advanced Radiotelegraph.";
						break;
					case "7":
						sOutputText = "Element 7: GMDSS Radio Operating Practices.";
						break;
					case "8":
						sOutputText = "Element 8: Ship Radar Techniques.";
						break;
					case "9":
						sOutputText = "Element 9: GMDSS Radio Maintenance Practices and Procedures.";
						break;
					default:
						sOutputText = "Unknown Test Element";
						break;
				}
				oWriter.WriteLine(sOutputText); // write the title
				oWriter.WriteLine(""); // add a line space
				oWriter.WriteLine("[Circle the correct answer.]");
				oWriter.WriteLine("");
				int iQuestionNumber = 1; // for numbering and answer key
				List<string> AnswerKey = new List<string>();
				foreach (DataRow oRow in Questions)
				{
					sOutputText = iQuestionNumber.ToString() + ". " + oRow.Field<string>("QuestionText")
						 + " [" + oRow.Field<string>("QuestionNumber") + "]";
					oWriter.WriteLine(sOutputText);
					sOutputText = "\tA. " + oRow.Field<string>("AnswerA");
					oWriter.WriteLine(sOutputText);
					sOutputText = "\tB. " + oRow.Field<string>("AnswerB");
					oWriter.WriteLine(sOutputText);
					sOutputText = "\tC. " + oRow.Field<string>("AnswerC");
					oWriter.WriteLine(sOutputText);
					sOutputText = "\tD. " + oRow.Field<string>("AnswerD");
					oWriter.WriteLine(sOutputText);
					oWriter.WriteLine("");
					AnswerKey.Add(iQuestionNumber.ToString("0") + ". " + oRow.Field<string>("CorrectAnswer"));
					iQuestionNumber++;
				}
				oWriter.Flush();
				oWriter.Close();
				//---- now the answer key
				if (!m_LearnMode)
				{
					oWriter = new StreamWriter(m_OutputFolderName + "\\" + sKeyFileName, false);
					oWriter.WriteLine("Exam Answer Key for - " + m_OutputFileName);
					oWriter.WriteLine("");
					foreach (string s in AnswerKey)
					{
						oWriter.WriteLine(s);
					}
					oWriter.Flush();
					oWriter.Close();
				}
			}
			catch (Exception e)
			{
				if (oWriter != null)
				{
					oWriter.Flush();
					oWriter.Close();
				}
				Debug.WriteLine("error in SaveTextToFile(): " + e.Message + "   stacktrace = " + e.StackTrace);
				throw;
			}
			return true;
		}

		private bool UseElectronicForm(List<DataRow> Questions)
		{
			try
			{
				ElectronicExamForm.QuestionPool = Questions;
				ElectronicExamForm.LearnMode = m_LearnMode;
				if (ElectronicExamForm.ShowDialog() == DialogResult.OK)
					return true;
				else
					return false;
			}
			catch (Exception e)
			{
				MessageBox.Show("Publishing the exam failed!! Error: " + e.Message);
				return false;
			}
		}

		private void RandomizeTheAnswers(List<DataRow> UnsortedList)
		{
			Random rnd = new Random();
			string[] cAnsList = { "A", "B", "C", "D" };
			for (int iIndexer = 0; iIndexer < UnsortedList.Count; iIndexer++)
			{
				DataRow dr = UnsortedList[iIndexer];
				string sOldCorrectAnsLetter = dr.Field<string>("CorrectAnswer");
				//---------
				string sNewAnsPtr = "Answer" + sOldCorrectAnsLetter;
				string sCorrAnsString = dr.Field<string>(sNewAnsPtr); // now holds the correct answer string value

				string cSwapAnsLetter = cAnsList[rnd.Next(0, 3)];
				if (cSwapAnsLetter == sOldCorrectAnsLetter) continue;
				string sNewCorrAnsPtr = "Answer" + cSwapAnsLetter; // random place to put the correct answer

				string sOldAnsString = dr.Field<string>(sNewCorrAnsPtr);  // now holds the previous answer
				dr.SetField<string>(sNewCorrAnsPtr, sCorrAnsString);

				dr.SetField<string>(sNewAnsPtr, sOldAnsString);
				dr.SetField<string>("CorrectAnswer", cSwapAnsLetter);
			}
		}

		private void LearnModeTheAnswers(List<DataRow> UnsortedList)
		{
			for (int iIndexer = 0; iIndexer < UnsortedList.Count; iIndexer++)
			{
				DataRow dr = UnsortedList[iIndexer];
				string sOldCorrectAnsLetter = dr.Field<string>("CorrectAnswer");
				string sCorrAnsText = dr.Field<string>("Answer" + sOldCorrectAnsLetter);
				//---------
				/*
				dr.SetField<string>("CorrectAnswer", "A");
				dr.SetField<string>("AnswerA", "**| " + sCorrAnsText);
				dr.SetField<string>("AnswerB", "-");
				dr.SetField<string>("AnswerC", "-");
				dr.SetField<string>("AnswerD", "-");
				*/
				//------- alternate output
				dr.SetField<string>("Answer" + sOldCorrectAnsLetter, "** " + sCorrAnsText);
			}
		}


		// Public implementation of Dispose pattern callable by consumers.
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Protected implementation of Dispose pattern.
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (ElectronicExamForm != null)
					ElectronicExamForm.Dispose();
			}
		}


	}  // end class
}  // end namespace
