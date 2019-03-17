using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;


namespace GrolTestPoolParser
{
	public class clsGrolDocParser
	{
		/******************* properties **********************/
		private List<string> m_DescriptionsCommands = null;
		public List<string> DescriptionsCommands { get { return m_DescriptionsCommands; } set { m_DescriptionsCommands = value; } }

		private List<string> m_QuestionsCommands = null;
		public List<string> QuestionsCommands { get { return m_QuestionsCommands; } set { m_QuestionsCommands = value; } }

		private string m_ElementNumber = "0";
		public string ElementNumber { get { return m_ElementNumber; } set { m_ElementNumber = value; } }

		private string m_LastError = "";
		public string LastError { get { return m_LastError; } }


		/********************* global vars ********************/
		ToolStripStatusLabel txtStatus = null;
		bool bDBout = true;
		Dictionary<string, string> dicAnswerKey = new Dictionary<string, string>();
		string sDocumentText = "";

		/********************* con/destructors ********************/
		public clsGrolDocParser(ToolStripStatusLabel StatusLabel)
		{ txtStatus = StatusLabel; }


		/********************* public methods ********************/
		public bool ParseTextFile(string DocumentText)
		{
			sDocumentText = DocumentText;
			if (sDocumentText.Length <= 0)
			{
				txtStatus.Text = "ERROR: no document text loaded!";
				return false;
			}
			m_DescriptionsCommands.Clear();
			m_QuestionsCommands.Clear();

			try
			{
				if (m_ElementNumber != "6")
				{
					if (!CreateAnswerKey())
						return false;
				}
				if (!CreateQuestionAndDescriptionCommandLists())
				{
					m_DescriptionsCommands.Clear();
					m_QuestionsCommands.Clear();
					return false;
				}
			}
			catch (Exception u)
			{
				Debug.WriteLine("Error in ParseTextFile() - " + u);
				m_LastError = u.Message + "  stacktrace = " + u.StackTrace;
				throw;
			}
			return true;
		}

		private bool CreateQuestionAndDescriptionCommandLists()
		{
			Regex oRegQuestionNum = new Regex(@"(\d-)*\d+[A-Z]\d+", RegexOptions.Singleline | RegexOptions.ExplicitCapture);
			Regex oRegKeyTopicNum = new Regex(@"\d+");
			Regex oRegAnswerLine = new Regex(@"[A-D]\.");
			Regex oRegAnswerKeyLine = new Regex(@"Answers:|Answer Key:");


			string sKeyTopic = "", sSubelement = "", sKeyTopicName = "", sTemp = "";
			string sCorrectAnswer = "";
			string[] splitArray = null;
			int iParseCounter = 0;
			List<string> sQuestionStuff = null;
			//---- parse and send text to proper subroutine
			StringReader sReader = new StringReader(sDocumentText);
			string aLine = sReader.ReadLine().Trim();
			try
			{
				while (aLine != null)
				{
					iParseCounter++;
					if (iParseCounter % 17 == 0)
					{
						txtStatus.Text = "Parsing Line " + iParseCounter.ToString();
					}

					Application.DoEvents();

					// already read in the answer key lines, so drop it
					if (oRegAnswerKeyLine.IsMatch(aLine)) aLine = "";

					if (aLine.Trim() != "")
					{
						//---- determine if entry is desc. or question

						//---- is it a description?
						if (aLine.Length > 10) //----> description?
						{
							aLine = aLine.Trim();

							#region Descriptors
							//----- check if it's a Subelement line
							if ( aLine.Substring(0,10) == "Subelement")
							{
								splitArray = aLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								if (splitArray.Length < 3)
								{
									m_LastError = "ERROR in input file - subelement";
									txtStatus.Text = "ERROR in input file - subelement";
									MessageBox.Show("Error in input file - subelement", "ERROR");
									return false;
								}
								sSubelement = splitArray[1];
								if (sSubelement[0].ToString() == m_ElementNumber)
								{
									if (sSubelement.Length == 3)
										sSubelement = sSubelement.Substring(2);
									else if (sSubelement.Length == 2)
										sSubelement = sSubelement.Substring(1);
								}
								if (Regex.IsMatch(splitArray[2], @"[A-Z]"))
								{
									sTemp = splitArray[2];
								}
								else
								{
									sTemp = splitArray[3];
								}
								sTemp = aLine.Substring(aLine.IndexOf(sTemp));
								CreateDescriptionsCommands(sTemp, sSubelement, "0");
							}
							if (Regex.IsMatch(aLine, "Section", RegexOptions.None)) // elements 7 & 9
							{
								splitArray = aLine.Split(new string[] { "Key Topic" }, StringSplitOptions.RemoveEmptyEntries);
								if (splitArray.Length < 2)
								{
									m_LastError = "ERROR in input file - section: " + aLine ;
									txtStatus.Text = "ERROR in input file - section";
									MessageBox.Show("Error in input file - section: " + aLine, "ERROR");
									return false;
								}
								sTemp = aLine.Substring(8, aLine.IndexOf(':') - 8);
								if (sTemp.Length > 2) // error
								{
									m_LastError = "ERROR in input file - section, can't locate section name: " + aLine;
									txtStatus.Text = "ERROR in input file - section";
									MessageBox.Show("Error in input file - section, can't parse section name: " + aLine, "ERROR");
									return false;
								}
								if (sSubelement != sTemp) // only need the description once
								{
									sSubelement = sTemp;
									sTemp = splitArray[0].Substring(splitArray[0].IndexOf(":"));
									sTemp = sTemp.Replace(":", "");
									CreateDescriptionsCommands(sTemp, sSubelement, "0");
								}
								sKeyTopic = splitArray[1].Trim();
								int iTemp = sKeyTopic.IndexOf(":");
								sKeyTopicName = sKeyTopic.Substring(1, iTemp - 1).Trim();
								sKeyTopic = sKeyTopic.Substring(iTemp).Replace(":","");
								sKeyTopicName = sKeyTopicName.Replace("#", "");
								CreateDescriptionsCommands(sKeyTopic, sSubelement, sKeyTopicName); // the key topic entry
							}

							if (aLine.Substring(0,10) == "Key Topic ") // key topic description
							{
								splitArray = aLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								sKeyTopicName = splitArray[2];
								sKeyTopicName = sKeyTopicName.Replace(":", "");
								if (Regex.IsMatch(splitArray[3], @"[A-Z]", RegexOptions.IgnoreCase))
								{
									sKeyTopic = aLine.Substring(aLine.IndexOf(splitArray[3]));
								}
								else
								{
									sKeyTopic = aLine.Substring(aLine.IndexOf(splitArray[4]));
								}
								CreateDescriptionsCommands(sKeyTopic, sSubelement, sKeyTopicName);
							}
							#endregion // descriptors

						}  // if len > 10

						/* now check for questions */

						/*  Send: QuestionNumber, SubElement, KeyTopicNumber, QuestionText, AnswerA, AnswerB, AnswerC, AnswerD, CorrectAnswer */

						#region questions

						if (oRegQuestionNum.IsMatch(aLine)) // it's a question number
						{
							sQuestionStuff = new List<string>();
							sCorrectAnswer = "";
							if (m_ElementNumber == "6")  // has a simple layout with answer in the text flow
							{
								sQuestionStuff.Add(aLine.Trim()); // question number
								sQuestionStuff.Add(sSubelement);
								sQuestionStuff.Add("6"); // topic
								aLine = sReader.ReadLine().Trim();
								aLine.Replace("\t", " ");
								while (aLine == "")
								{
									aLine = sReader.ReadLine().Trim();
									aLine.Replace("\t", " ");
								}
								sCorrectAnswer = aLine.Trim();
								sCorrectAnswer = sCorrectAnswer.Replace(".", ""); // hold this for a little later on
								aLine = sReader.ReadLine().Trim();
								aLine.Replace("\t", " ");
								while (aLine == "")
								{
									aLine = sReader.ReadLine().Trim();
									aLine.Replace("\t", " ");
								}
								sQuestionStuff.Add(aLine + " "); // question stem
							}  // element number is 6
							else // everyone else
							{
								aLine.Replace("\t", " ");
								splitArray = aLine.Split(new char[] { ' ' }, 2);
								sQuestionStuff.Add(splitArray[0].Trim()); // question number
								sQuestionStuff.Add(sSubelement);
								sQuestionStuff.Add(sKeyTopicName);
								sQuestionStuff.Add(splitArray[1] + " "); // question stem
							}

							// if question stem has orphan second line, grab it and attach
							aLine = sReader.ReadLine().Trim();
							while (aLine == "")
							{
								aLine = sReader.ReadLine().Trim();
								aLine.Replace("\t", " ");
							}
							if (!oRegAnswerLine.IsMatch(aLine)) // look like an orphan
							{
								sQuestionStuff[3] += aLine;
								aLine = sReader.ReadLine().Trim();
								while (aLine.Length < 3)
								{
									aLine = sReader.ReadLine().Trim();
									aLine.Replace("\t", " ");
								}
							}

							// check for orphan answer strings while capturing answers
							while (aLine.Substring(1, 1) == ".") // A. B. C. D.
							{
								aLine = aLine.Substring(2).Trim();
								sQuestionStuff.Add(aLine);
								if (sReader.Peek() >= 0)
								{
									aLine = sReader.ReadLine().Trim();
									if (aLine == null)
										break;
									if (aLine.Length < 2)
									{
										break;
									}
									aLine.Replace("\t", " ");
									if (aLine.Substring(1, 1) != ".") // likely an orphan
									{
										sQuestionStuff[sQuestionStuff.Count - 1] += aLine;
										aLine = sReader.ReadLine().Trim();
										if (aLine == null)
											break;
										if (aLine.Length < 2)
										{
											break;
										}
									}
								}
								else
									break;
							}  //  while()
							if (m_ElementNumber == "6")
							{
								sQuestionStuff.Add(sCorrectAnswer);
							}
							else
							{
								if (dicAnswerKey.ContainsKey(sQuestionStuff[0]))
								{
									sQuestionStuff.Add(dicAnswerKey[sQuestionStuff[0]]);
								}
							}

							if (sQuestionStuff.Count < 9) // missing something, usually a badly formed question/answers
							{
								throw new Exception("Question #" + sQuestionStuff[0] + " has incorrect layout or missing parts!");
							}
							CreateTestQuestionCommands(sQuestionStuff);
							sQuestionStuff.Clear();
						}

						#endregion

					}  // end if aline != "", skip blank lines

					if (sReader.Peek() >= 0)
					{
						aLine = sReader.ReadLine().Trim();
						aLine.Replace("\t", " ");
					}
					else
					{
						aLine = null;
					}
				} // end while()
			}
			catch (Exception e5)
			{
				string errtext = "aLine = " + aLine + "\n";
				if (sQuestionStuff != null)
				{
					if (sQuestionStuff.Count > 0)
						txtStatus.Text = "ERROR: Question " + sQuestionStuff[0] + " has issues!";
				}
				/*
				foreach (string s in sQuestionStuff)
				{
					errtext += s + "\n";
				}
				*/
				errtext += "/************************************************/";
				errtext += "\n" + e5.Message;
#if DEBUG
				errtext += "\nstacktrace = " + e5.StackTrace;
#endif
				m_LastError = errtext;
				//MessageBox.Show(errtext, "Parse Error");
				throw new Exception(errtext);
			}
			sReader.Close();
			txtStatus.Text = "Text Question commands parsed.";
			return true;
		}


		/// <summary>
		/// creates a batch file of commands for descriptions to be saved
		/// </summary>
		/// <param name="InputText"></param>
		private void CreateDescriptionsCommands(string InputText, string SubElement, string KeyTopic)
		{
			//string[] splitArray = null;
			string sDataFormatString = "Insert into grol_element_descriptions (ElementNumber, SubElementName, KeyTopic, DescriptiveText) "
				 + "Values (" + m_ElementNumber + ", '{0}', '{1}', '{2}')";
			string sTextFormatString = m_ElementNumber + "//{0}//{1}//{2}";
			string SubelementName = SubElement, KeyTopicNumber = KeyTopic, CommentText = "";  //, sFinder = "";

			InputText = InputText.Trim();
			InputText = InputText.Replace("--", "-");
			CommentText = InputText;

			#region OldCode
			/*
			splitArray = InputText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (splitArray[0].ToUpper() == "SUBELEMENT")
			{

				if (splitArray[2] == "-")
				{
					sFinder = splitArray[3];
				}
				else
				{
					sFinder = splitArray[2];
				}
				CommentText = InputText.Substring(InputText.IndexOf(sFinder));
			}
			else if (splitArray[0].ToUpper() == "KEY") // it's a key topic def.
			{
				CommentText = "";
				try
				{
					if (splitArray[3] == "-")
					{
						sFinder = splitArray[4];
					}
					else
					{
						sFinder = splitArray[3];
					}
				}
				catch (Exception x)
				{
					m_LastError = x.Message + "  stacktrace = " + x.StackTrace;
					throw;
				}
				CommentText = InputText.Substring(InputText.IndexOf(sFinder));
			}
			else if (InputText.Substring(0, 7).ToUpper() == "SECTION")
			{
				Regex oReg = new Regex(@"Key Topic-? *#?\d");
				Match oMatch = oReg.Match(InputText);
				// see if it's Key Topic 1
				if (oMatch.ToString().Contains("Key")) // found proper layout in header string
				{
					// split on the colons
					splitArray = InputText.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
					// if this is Topic 0 then send the header/subelement after the key topic
					if (KeyTopic == "0")
					{
						CommentText = splitArray[1];
						CommentText = CommentText.Replace(":", "").Trim();
					}
					else
					{
						CommentText = splitArray[3].Replace(":", "").Trim();
						if (splitArray.Length > 4) // extra stuff behind another colon character
						{
							CommentText += " : " + splitArray[4].Trim();
						}
					}
				}
			}
			*/
			#endregion

			if (bDBout)
			{
				CommentText = MySqlHelper(CommentText);
			}
			string sOutput = "";
			if (bDBout)
			{
				sOutput = String.Format(sDataFormatString, SubelementName, KeyTopicNumber, CommentText);
			}
			else
			{
				sOutput = String.Format(sTextFormatString, SubelementName, KeyTopicNumber, CommentText);
			}
			m_DescriptionsCommands.Add(sOutput);
		}

		/// <summary>
		/// creates a batch file of insert commands for the questions to be saved
		/// </summary>
		/// <param name="InputText"></param>
		private void CreateTestQuestionCommands(List<string> InputText)
		{
			/*  ElementNumber, QuestionNumber, Subelement, KeyTopicNumber, QuestionText, AnswerA, AnswerB, AnswerC, AnswerD, CorrectAnswer*/
			string sSQLFormatString = "Insert into grol_exam_questions Values ( " + m_ElementNumber + ", '{0}', '{1}', "
				 + "'{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')";
			string sTextFormatString = m_ElementNumber + "//{0}//{1}//{2}//{3}//{4}//{5}//{6}//{7}//{8}";

			try
			{
				string QuestionNumber = InputText[0];
				string SubelementNumber = InputText[1];
				string KeyTopicNumber = InputText[2];
				string sCorrectAnswer = InputText[8];
				string QuestionText = "";
				if (bDBout)
				{
					QuestionText = MySqlHelper(InputText[3]);
				}
				else
				{
					QuestionText = InputText[3];
				}
				string[] Answers = new string[4];
				int iStop = InputText.Count - 1;
				for (int iIndexer = 4; iIndexer < iStop; iIndexer++)
				{
					if (bDBout)
					{
						Answers[iIndexer - 4] = MySqlHelper(InputText[iIndexer]);
					}
					else
					{
						Answers[iIndexer - 4] = InputText[iIndexer];
					}
				}
				/*  QuestionNumber, Subelement, KeyTopicNumber, QuestionText, AnswerA, AnswerB, AnswerC, AnswerD, CorrectAnswer*/
				object[] Params = new object[9];
				Params[0] = QuestionNumber;
				Params[1] = SubelementNumber;
				Params[2] = KeyTopicNumber;
				Params[3] = QuestionText;
				Params[4] = Answers[0];
				Params[5] = Answers[1];
				Params[6] = Answers[2];
				Params[7] = Answers[3];
				Params[8] = sCorrectAnswer;
				string sOutput = "";
				if (bDBout)
					sOutput = String.Format(sSQLFormatString, Params);
				else
					sOutput = string.Format(sTextFormatString, Params);
				m_QuestionsCommands.Add(sOutput);
			}
			catch (Exception e8)
			{
				m_LastError = e8.Message + "   stacktrace = " + e8.StackTrace;
				throw;
			}
		}

		private bool CreateAnswerKey()
		{
			StringReader sReader = new StringReader(sDocumentText);
			string aLine = "";
			string[] splitArray = null;
			int iStartPos = 0, iInterval = 2;

			dicAnswerKey.Clear();
			try
			{
				aLine = sReader.ReadLine().Trim();
				while (aLine != null)
				{
					if (aLine.Length >= 10)
					{
						if (aLine.Substring(0, 10).ToUpper() == "ANSWER KEY" || aLine.Substring(0, 7).ToUpper() == "ANSWERS")  // for all elements except six
						{
							aLine = aLine.Replace(":", "");
							aLine = aLine.Replace("\t", " ");
							aLine = Regex.Replace(aLine, @" - ", " ");
							splitArray = aLine.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
							if (splitArray[1].ToUpper() == "KEY") // sometimes "Answers:" sometimes "Answer Key:"
							{
								iStartPos = 2;
							}
							else
							{
								iStartPos = 1;
							}
							iInterval = 2;
							int iLen = splitArray.Length - 1;
							string sAns = "";
							for (int iIndexer = iStartPos; iIndexer < iLen; iIndexer += iInterval)
							{
									sAns = splitArray[iIndexer + 1];
								if (sAns.Length > 1)
								{
									foreach (char c in sAns)
									{
										if ("ABCD".Contains(c.ToString()))
										{
											sAns = c.ToString();
										}
									}
								}
								try
								{
									dicAnswerKey.Add(splitArray[iIndexer], sAns);
								}
								catch (Exception e)
								{
									Debug.WriteLine(e);
								}
							}
							aLine = sReader.ReadLine().Trim();
							aLine = aLine.Replace("\t", " ");
						}
					}
					if (sReader.Peek() >= 0)
						aLine = sReader.ReadLine().Trim();
					else
						aLine = null;
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				m_LastError = "Malformed answer key: " + aLine + "  error message = " + e.Message;
				throw new Exception("Answer Key string is malformed: " + aLine);
			}

			sReader.Close();
			return true;
		}

		private string MySqlHelper(string TextIn)
		{
			TextIn = TextIn.Replace("'", "''");
			TextIn = TextIn.Replace("’", "''");
			TextIn = TextIn.Replace("‘", "''");
			TextIn = TextIn.Replace("\u2014", "-"); // em dash
			TextIn = TextIn.Replace("\u2013", "-"); // en dash
			return TextIn;
		}

	}  //  end class

}  //  end namespace
