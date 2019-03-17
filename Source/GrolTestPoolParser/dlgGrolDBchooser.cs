using System;
using System.Data;
using System.Windows.Forms;

namespace GrolTestPoolParser
{
    public partial class dlgGrolDBchooser : Form
    {

        private DataTable m_Table = null;

        public dlgGrolDBchooser(DataTable oTable)
        {
            InitializeComponent();
            m_Table = oTable;
        }

        private void dlgDBchooser_Shown(object sender, EventArgs e)
        {
            cmbDBnames.Items.Clear();
            foreach (DataRow oRow in m_Table.Rows)
            {
                string sName = oRow.Field<string>(0);
                if (!sName.ToLower().Contains("schema"))
                    cmbDBnames.Items.Add(sName);
            }
            cmbDBnames.Text = cmbDBnames.Items[0].ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }  // end class
} // end namespace
