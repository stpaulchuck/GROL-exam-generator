using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace GrolExamGenerator
{
	public class clsGROLSQLiteHandler : IDisposable
	{
		/************************* properties ******************************/
		private string m_DBpath = "";
		public string DBpath { get { return m_DBpath; } set { m_DBpath = value; } }

		private int m_ElementNumber = 0;
		public int ElementNumber { get { return m_ElementNumber; } set { m_ElementNumber = value; } }

		private bool m_Abort = false;
		public bool Abort { get { return m_Abort; } set { m_Abort = value; } }

		/************************ global vars *****************************/
		private SQLiteConnection oConn = null;
		private SQLiteCommand oCmd = null;
		private ToolStripStatusLabel txtStatus = null;

		/*********************** con/destructor ***************************/
		public clsGROLSQLiteHandler(ToolStripStatusLabel StatusText)
		{ txtStatus = StatusText; }

		/*********************** public methods ***************************/
		public bool TestSqlConnection(string DBpath)
		{
			if (!File.Exists(DBpath))
			{ return false; }

			// all good, so -
			m_DBpath = DBpath;
			return true;
		}

		public DataTable GetDescriptions()
		{
			if (oConn == null)
				MakeSqlConnection();
			if (oConn.State != ConnectionState.Open)
			{
				oConn.Open();
			}
			//---- get all questions for the master table
			oCmd.CommandText = "select * from grol_element_descriptions"; // where elementnumber = " + ElementNumber.ToString();
			SQLiteDataAdapter oDA = new SQLiteDataAdapter(oCmd);
			DataTable oTable = new DataTable();
			oDA.Fill(oTable);
			return oTable;
		}

		public DataTable GetQuestions()
		{
			DataTable oTable = new DataTable();
			DataSet oDS = new DataSet();
			oDS.Tables.Add(oTable);

			try
			{
				if (oConn == null)
				{
					MakeSqlConnection();
				}
				if (oConn.State != ConnectionState.Open)
				{
					oConn.Open();
				}
				//---- get all questions for the master table
				if (oCmd == null)
					oCmd = oConn.CreateCommand();
				oCmd.CommandText = "select * from grol_exam_questions"; // where questionnumber = '1-1A2' ";
				oCmd.CommandType = CommandType.Text;
				SQLiteDataAdapter oDA = new SQLiteDataAdapter(oCmd);
				
				oDA.Fill(oDS.Tables[0]);
			}
			catch (Exception e)
			{
				Debug.WriteLine("error reading GetQuestions: " + e.Message + "   stacktrace = " + e.StackTrace);
				throw;
			}

			return oTable;
		}

		/********************** private methods ***************************/
		private bool MakeSqlConnection()
		{
			oConn = new SQLiteConnection("Data Source=" + DBpath + ";Version=3;");
			if (oConn == null)
			{
				txtStatus.Text = "failed to open database";
				return false;
			}
			if (oConn.State != System.Data.ConnectionState.Open)
			{
				oConn.Open();
			}
			if (oConn.State != System.Data.ConnectionState.Open)
			{
				txtStatus.Text = "failed to open the database";
				return false;
			}
			return true;
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
				if (oConn != null)
					oConn.Dispose();
			}
		}

	}  // end class

}  //  end namespace
