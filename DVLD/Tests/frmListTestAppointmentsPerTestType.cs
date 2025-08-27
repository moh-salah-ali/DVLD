using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD_BusinessLayer.clsTestType;

namespace DVLD
{
    public partial class frmListTestAppointmentsPerTestType : Form
    {
        private int _LocalDrivingLicenseApplicationID;
        private clsTestType.enTestType _TestTypeID;
        private DataTable _TestAppointments;
        public frmListTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, byte PassedTestsCount)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;

            switch(PassedTestsCount)
            {
                case 0:
                    _TestTypeID = clsTestType.enTestType.VisionTest;
                    this.Text = "Schedule Vision Test";
                    lblTitle.Text = "Schedule Vision Test";
                    break;
                case 1:
                    _TestTypeID = clsTestType.enTestType.WrittenTest;
                    this.Text = "Schedule Written Test";
                    lblTitle.Text = "Schedule Written Test";
                    break;
                case 2:
                    _TestTypeID = clsTestType.enTestType.StreetTest;
                    this.Text = "Schedule Street Test";
                    lblTitle.Text = "Schedule Street Test";
                    break;
            }
        }

        private void frmListTestAppointmentsPerTestType_Load(object sender, EventArgs e)
        {

            ctrlLocaiDrivingLicenseApplicationInfo1.LoadLocalDrivingLicenseApplicationInfo(_LocalDrivingLicenseApplicationID);

            _TestAppointments = clsTestAppointment.GetTestAppointmentsForLocalDrivingLicenseApplicationAndTestType(_LocalDrivingLicenseApplicationID, _TestTypeID);
            dgvTestAppointments.DataSource = _TestAppointments;
            lblRecordsCount.Text = dgvTestAppointments.Rows.Count.ToString();

            if (dgvTestAppointments.Rows.Count > 0)
            {
                dgvTestAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvTestAppointments.Columns[0].Width = 150;

                dgvTestAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvTestAppointments.Columns[1].Width = 400;

                dgvTestAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvTestAppointments.Columns[2].Width = 150;

                dgvTestAppointments.Columns[3].HeaderText = "Is Locked";
                dgvTestAppointments.Columns[3].Width = 100;
            }
            
        }

        private void btnNewAppointment_Click(object sender, EventArgs e)
        {
            if(clsTestAppointment.HasActiveTestAppointment(_LocalDrivingLicenseApplicationID, (int)_TestTypeID))
            {
                MessageBox.Show("Can't schedule new Appointment, Person already has an active appointment for this Test", "Schedule Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (clsTestAppointment.HadPassedTest(_LocalDrivingLicenseApplicationID, (int)_TestTypeID))
            {
                MessageBox.Show("Can't schedule new Appointment, Person already passed this test before", "Schedule Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestTypeID);
            frm.ShowDialog();
            frmListTestAppointmentsPerTestType_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
             frmScheduleTest frm = new frmScheduleTest((int)dgvTestAppointments.CurrentRow.Cells["TestAppointmentID"].Value);
            frm.ShowDialog();
            frmListTestAppointmentsPerTestType_Load(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvTestAppointments.CurrentRow.Cells[0].Value;

            frmTakeTest frm = new frmTakeTest(TestAppointmentID, _TestTypeID);
            frm.ShowDialog();
            frmListTestAppointmentsPerTestType_Load(null, null);
        }
    }
}
