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
    public partial class ctrlUserCard : UserControl
    {

        private clsUser _User;

        private int _UserID;

        public int UserID
        {
            get { return _UserID; }
        }

        public clsUser SelectedUserInfo
        {
            get { return _User; }
        }

        public ctrlUserCard()
        {
            InitializeComponent();
        }

        private void _FillUserInfo()
        {
            _UserID = _User.UserID;

            ctrlPersonCard1.LoadPersonInfo(_User.PersonID);
            
            lblUserID.Text = _UserID.ToString();
            lblUserName.Text = _User.UserName.ToString();
            lblIsActive.Text = _User.IsActive == true? "Yes": "No";
 
        }

        public void LoadUserInfoUsingUserID(int UserID)
        {
            _User = clsUser.FindByUserID(UserID);
            if (_User == null)
            {
                MessageBox.Show("There is no user with this -> " + UserID + " UserID", "User Doesn't Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            _FillUserInfo();
        }

    }
}
