using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BusinessLayer;

namespace DVLD
{
    public partial class frmListApplicationTypes : Form
    {
        
        public frmListApplicationTypes()
        {
            InitializeComponent();
        }

        private void frmListApplicationTypes_Load(object sender, EventArgs e)
        {
            dgvApplicationTypes.DataSource = clsApplicationType.GetAllApplicationTypes();
            lblApplicationTypesNumber.Text = dgvApplicationTypes.Rows.Count.ToString();

            if (dgvApplicationTypes.Rows.Count > 0)
            {
                dgvApplicationTypes.Columns[0].HeaderText = "Type ID";
                dgvApplicationTypes.Columns[0].Width = 70;

                dgvApplicationTypes.Columns[1].HeaderText = "Name";
                dgvApplicationTypes.Columns[1].Width = 300;

                dgvApplicationTypes.Columns[2].HeaderText = "Fees";
                dgvApplicationTypes.Columns[2].Width = 150;
            }
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditApplicationType frm = new frmAddEditApplicationType((int)dgvApplicationTypes.CurrentRow.Cells[0].Value); 
            frm.ShowDialog();

            frmListApplicationTypes_Load(null, null);
        }
    }
}
