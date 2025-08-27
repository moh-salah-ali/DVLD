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
    public partial class ctrlLocaiDrivingLicenseApplicationInfo : UserControl
    {
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplicationInfo;
        private int _LicenseID = -1;

        public ctrlLocaiDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        private void _FillApplicationInfo()
        {
           
            ctrlBaseApplicationInfo2.LoadApplicationInfo(_LocalDrivingLicenseApplicationInfo.ApplicationID);
            lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplicationID.ToString();
            lblAppliedFor.Text = clsLicenseClass.Find(_LocalDrivingLicenseApplicationInfo.LicenseClassID).ClassName;
            lblPassedTests.Text = clsLocalDrivingLicenseApplication.GetPassedTestsCount(_LocalDrivingLicenseApplicationID).ToString() + "/3";
            llShowLicenceInfo.Enabled = (_LicenseID != -1);
        }


        public void LoadLocalDrivingLicenseApplicationInfo(int LocalDrivingLicenseApplicationID)
        {
            
            _LocalDrivingLicenseApplicationInfo = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);

            if(_LocalDrivingLicenseApplicationInfo == null)
            {
                MessageBox.Show("There is no Local Driving license App with ID " + LocalDrivingLicenseApplicationID,
                    "App Doesn't Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _FillApplicationInfo();

        }
    }
}
