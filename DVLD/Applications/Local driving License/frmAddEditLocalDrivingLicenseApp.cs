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

namespace DVLD
{
    public partial class frmAddEditLocalDrivingLicenseApp : Form
    {

        private int _LocalDrivingLicenseApplicationID = -1;

        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        private int _SelectedPersonID = -1;

        private enum enMode {AddNew = 1, Update = 2}
        private enMode _Mode = enMode.AddNew;


        public frmAddEditLocalDrivingLicenseApp()
        {
            InitializeComponent();
            this._Mode = enMode.AddNew;
        }

        public frmAddEditLocalDrivingLicenseApp(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();

            this._Mode = enMode.Update;
            this._LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(this._LocalDrivingLicenseApplicationID);
        }

        private void _FillLicenseClassesInCombobox()
        {
            DataTable dt = clsLicenseClass.GetAllLicenseClasses();

            foreach (DataRow dr in dt.Rows)
            {
                cbLicenseClass.Items.Add(dr["ClassName"]);
            }
        }

        private void _SetValues()
        {

            _FillLicenseClassesInCombobox();

            if (_Mode == enMode.AddNew)
            {
                this.Text = "New Local Driving license Application";
                lblTitle.Text = "New Local Driving license Application";

                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
                lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewDrivingLicense).Fees.ToString();
                lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;

                cbLicenseClass.SelectedIndex = 2;
                ctrlPersonCardWithFilter1.Focus();
                tpAppInfo.Enabled = false;
                btnSave.Enabled = false;
            }
            else
            {
                this.Text = "Update Local Driving license Application";
                lblTitle.Text = "Update Local Driving license Application";
                tpAppInfo.Enabled = true;
                btnSave.Enabled = true;
            }
        }

        private void _LoadApplicationInfo()
        {
            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Application with ID = " + _LocalDrivingLicenseApplicationID, "Application Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();

                return;


            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicantPersonID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;
           
            lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = clsFormat.DateToShort(_LocalDrivingLicenseApplication.ApplicationDate);
            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName);
            lblApplicationFees.Text = _LocalDrivingLicenseApplication.PaidFees.ToString();
            lblCreatedBy.Text = clsUser.FindByUserID(_LocalDrivingLicenseApplication.CreatedByUserID).UserName;
        }

        private void frmAddEditLocalDrivingLicenseApp_Load(object sender, EventArgs e)
        {
            _SetValues();
            
            if(_Mode == enMode.Update)
            {
                _LoadApplicationInfo();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                tcLocalDrivingLicenseApplicationInfo.SelectedTab = tcLocalDrivingLicenseApplicationInfo.TabPages["tpAppInfo"];
                return;
            }

            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {
                btnSave.Enabled = true;
                tpAppInfo.Enabled = true;
                tcLocalDrivingLicenseApplicationInfo.SelectedTab = tcLocalDrivingLicenseApplicationInfo.TabPages["tpAppInfo"];

            }
            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int LicenseClassID = clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;

            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(_SelectedPersonID, clsApplication.enApplicationType.NewDrivingLicense, LicenseClassID);

            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            if (ActiveApplicationID != -1)
            {
                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected License class with id=" 
                    + ActiveApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbLicenseClass.Focus();
                return;
            }

            //Check if there is a completed app with the same license class
            int CompletedApplicationID = clsApplication.GetCompletedApplicationIDForLicenseClass(_SelectedPersonID, clsApplication.enApplicationType.NewDrivingLicense, LicenseClassID);
 
            if (CompletedApplicationID != -1)
            {

                MessageBox.Show("Person already have a Completed Application with the same applied driving class, Choose diffrent driving class",
                    "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LocalDrivingLicenseApplication.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;
            _LocalDrivingLicenseApplication.ApplicationDate = DateTime.Now;
            _LocalDrivingLicenseApplication.ApplicationTypeID = 1;
            _LocalDrivingLicenseApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _LocalDrivingLicenseApplication.LastStatusDate = DateTime.Now;
            _LocalDrivingLicenseApplication.PaidFees = Convert.ToSingle(lblApplicationFees.Text);
            _LocalDrivingLicenseApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _LocalDrivingLicenseApplication.LicenseClassID = LicenseClassID;


            if (_LocalDrivingLicenseApplication.Save())
            {
                lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
                
                _Mode = enMode.Update;
                lblTitle.Text = "Update Local Driving License Application";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
