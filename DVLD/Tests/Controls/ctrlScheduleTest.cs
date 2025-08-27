using DVLD.Properties;
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
    public partial class ctrlScheduleTest : UserControl
    {

        private enum enMode { AddNew = 1, Update = 2 }
        private enMode _Mode = enMode.AddNew;

        private int _LocalDrivingLicenseApplicationID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplicationInfo;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private clsTestType _TestTypeInfo;
        private int _TestAppointmentID = -1;
        private clsTestAppointment _TestAppointmentInfo;

        private clsApplication _RetakeTestApplicationInfo;
        private int _RetakeTestApplicationID;

        private enum enCheckRetakeTest { FirstTime = 1,  Retaking = 2}
        private enCheckRetakeTest _CheckRetakeTest = enCheckRetakeTest.FirstTime;

        public ctrlScheduleTest()
        {
            InitializeComponent();
        }

        public clsTestType.enTestType TestTypeID
        {
            get
            {
                return _TestTypeID;
            }
            set
            {
                _TestTypeID = value;

                switch (_TestTypeID)
                {

                    case clsTestType.enTestType.VisionTest:
                        {
                            gbTestType.Text = "Vision Test";
                            pbTestTypeImage.Image = Resources.Vision_512;
                            break;
                        }

                    case clsTestType.enTestType.WrittenTest:
                        {
                            gbTestType.Text = "Written Test";
                            pbTestTypeImage.Image = Resources.Written_Test_512;
                            break;
                        }
                    case clsTestType.enTestType.StreetTest:
                        {
                            gbTestType.Text = "Street Test";
                            pbTestTypeImage.Image = Resources.driving_test_512;
                            break;


                        }
                }
            }
        }

        public void LoadTestAppointmentInfo(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID, int TestAppointmentID = -1)
        {
            if(TestAppointmentID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;


            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestTypeID = TestTypeID;
            _TestAppointmentID = TestAppointmentID;


            if (this._Mode == enMode.AddNew)
                _LoadNewTestAppointmentInfo();
            else
                _LoadOldTestAppointmentInfo();

        }

        private void _LoadNewTestAppointmentInfo()
        {
            _LocalDrivingLicenseApplicationInfo = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);
            _TestTypeInfo = clsTestType.Find(_TestTypeID);
            _TestAppointmentInfo = new clsTestAppointment();

            dtpTestDate.MinDate = DateTime.Now;
            lblFees.Text = _TestTypeInfo.Fees.ToString();

            _FillMainScheduleInfo();

            if (_LocalDrivingLicenseApplicationInfo.HaveAttendedTestType((int)_TestTypeID))
                _CheckRetakeTest = enCheckRetakeTest.Retaking;
            else
                _CheckRetakeTest = enCheckRetakeTest.FirstTime;

            lblRetakeTestAppID.Text = "N/A";

            if (_CheckRetakeTest == enCheckRetakeTest.Retaking)
            {
                lblRetakeAppFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).Fees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                _RetakeTestApplicationInfo = new clsApplication();
            }
            else
            {
                lblRetakeAppFees.Text = "0";
                gbRetakeTestInfo.Enabled = false;
                lblTitle.Text = "Schedule Test";

            }

            lblTotalFees.Text = (Convert.ToSingle(lblFees.Text) + Convert.ToSingle(lblRetakeAppFees.Text)).ToString();

        }

        private void _LoadOldTestAppointmentInfo()
        {
            _TestAppointmentInfo = clsTestAppointment.Find(_TestAppointmentID);
            _LocalDrivingLicenseApplicationID = _TestAppointmentInfo.LocalDrivingLicenseApplicationID;
            _TestTypeID = _TestAppointmentInfo.TestTypeID;

            _LocalDrivingLicenseApplicationInfo = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);
            _TestTypeInfo = clsTestType.Find(_TestTypeID);

            _FillMainScheduleInfo();

            if (_TestAppointmentInfo == null)
            {
                MessageBox.Show("Error: No Appointment with ID = " + _TestAppointmentID.ToString(),
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            lblFees.Text = _TestAppointmentInfo.PaidFees.ToString();

            //we compare the current date with the appointment date to set the min date.
            if (DateTime.Compare(DateTime.Now, _TestAppointmentInfo.AppointmentDate) < 0)
                dtpTestDate.MinDate = DateTime.Now;
            else
                dtpTestDate.MinDate = _TestAppointmentInfo.AppointmentDate;

            dtpTestDate.Value = _TestAppointmentInfo.AppointmentDate;

            if (_TestAppointmentInfo.RetakeTestApplicationID == -1)
            {
                lblRetakeAppFees.Text = "0";
                lblRetakeTestAppID.Text = "N/A";
            }
            else
            {
                lblRetakeAppFees.Text = _TestAppointmentInfo.RetakeTestAppInfo.PaidFees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblRetakeTestAppID.Text = _TestAppointmentInfo.RetakeTestApplicationID.ToString();

            }

            if (_TestAppointmentInfo.IsLocked == true)
            {
                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
            }

            lblTotalFees.Text = (Convert.ToSingle(lblFees.Text) + Convert.ToSingle(lblRetakeAppFees.Text)).ToString();


        }

        private void _FillMainScheduleInfo()
        {
            lblLocalDrivingLicenseAppID.Text = _LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LocalDrivingLicenseApplicationInfo.LicenseClassInfo.ClassName;
            lblFullName.Text = _LocalDrivingLicenseApplicationInfo.ApplicantFullName;
            lblTrial.Text = _LocalDrivingLicenseApplicationInfo.AttendedTestsTypeCount((int)_TestTypeID).ToString();
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.AddNew)
            {
                _TestAppointmentInfo.TestTypeID = _TestTypeID;
                _TestAppointmentInfo.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplicationID;
                _TestAppointmentInfo.AppointmentDate = dtpTestDate.Value;
                _TestAppointmentInfo.PaidFees = Convert.ToSingle(lblFees.Text);
                _TestAppointmentInfo.CreatedByUserID = clsGlobal.CurrentUser.UserID;
                _TestAppointmentInfo.IsLocked = false;
            }
            else
            {
                _TestAppointmentInfo.AppointmentDate = dtpTestDate.Value;
            }


            if(_CheckRetakeTest == enCheckRetakeTest.Retaking)
            {
                
                _RetakeTestApplicationInfo.ApplicantPersonID = _LocalDrivingLicenseApplicationInfo.ApplicantPersonID;
                _RetakeTestApplicationInfo.PaidFees = Convert.ToSingle(lblRetakeAppFees.Text);
                _RetakeTestApplicationInfo.ApplicationDate = DateTime.Now;
                _RetakeTestApplicationInfo.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                _RetakeTestApplicationInfo.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                _RetakeTestApplicationInfo.LastStatusDate = DateTime.Now;
                _RetakeTestApplicationInfo.CreatedByUserID = clsGlobal.CurrentUser.UserID;

                if(!_RetakeTestApplicationInfo.Save())
                {
                    MessageBox.Show("Retake Test Application Saving Error", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnSave.Enabled = false;
                    return;
                }

                lblRetakeTestAppID.Text = _RetakeTestApplicationInfo.ApplicationID.ToString();
                _TestAppointmentInfo.RetakeTestApplicationID = _RetakeTestApplicationInfo.ApplicationID;
            }


            if(_TestAppointmentInfo.Save())
            {
                _Mode = enMode.Update;

                MessageBox.Show("Test Appointment Data Saved successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Test Appointment Data Saving Error", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
    }
}
