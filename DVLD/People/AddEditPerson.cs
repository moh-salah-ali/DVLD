using DVLD.Properties;
using DVLD_BusinessLayer;
using System.IO;
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
    public partial class frmAddEditPerson : Form
    {
       
        public delegate void DataBackEventHandler(object sender, int PersonID);
       
        public event DataBackEventHandler DataBack;

        private enum enMode { AddNew = 0, Update = 1}
        
        private enMode _Mode = enMode.AddNew;

        private clsPerson _Person;

        private int _PersonID = 0;

        private enum enGendor {Male = 0, Female = 1}
       

        public frmAddEditPerson()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddEditPerson(int PersonID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _PersonID = PersonID;
        }


        private void _FillCountriesInComboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();

            foreach (DataRow row in dtCountries.Rows)
            {
                cbCountry.Items.Add(row["CountryName"]);
            }
        }

        private void _ResetDefaultInfo()
        {
            _FillCountriesInComboBox();

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Person";
                _Person = new clsPerson();
            }
            else
            {
                lblTitle.Text = "Update Person";
            }

            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtNationalNo.Text = "";

            dtpDateOfBirth.MaxDate = DateTime.Today.AddYears(-18);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;

            dtpDateOfBirth.MinDate = DateTime.Today.AddYears(-100);


            rbMale.Checked = true;
            pbPerson.Image = Resources.Male_512;

            txtPhone.Text = "";
            txtEmail.Text = "";

            cbCountry.SelectedIndex = cbCountry.FindString("Sudan");

            txtAddress.Text = "";

            llRemoveImage.Visible = (pbPerson.ImageLocation != null);

        }

        private void _LoadData()
        {
            _Person = clsPerson.Find(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("No Person with ID = " + _PersonID, "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            lblPersonID.Text = _PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalNo;

            
            dtpDateOfBirth.Value = _Person.DateOfBirth;

            if(_Person.Gendor == 0)
            {
                rbMale.Checked = true;
                pbPerson.Image = Resources.Male_512;
            }
            else
            {
                rbFemale.Checked = true;
                pbPerson.Image = Resources.Female_512;
            }

            txtPhone.Text = _Person.Phone;
            txtEmail.Text = _Person.Email;

            cbCountry.SelectedIndex = cbCountry.FindString(_Person.CountryInfo.CountryName);

            txtAddress.Text = _Person.Address;

            if (_Person.ImagePath != "")
            {
                pbPerson.ImageLocation = _Person.ImagePath;

            }

            llRemoveImage.Visible = (_Person.ImagePath != "");



        }

        private void AddEditPerson_Load(object sender, EventArgs e)
        {
            _ResetDefaultInfo();
            if (_Mode == enMode.Update)
            {
                _LoadData();
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ValidateEmptyTextBoxes(object sender, CancelEventArgs e)
        {
            TextBox TextBoxToValidate = (TextBox)sender;

            if (string.IsNullOrEmpty(TextBoxToValidate.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(TextBoxToValidate, "This can not be empty");
            }
            else
            {
                errorProvider1.SetError(TextBoxToValidate, null);
            }
        }

        private void ValidateEmail(object sender, CancelEventArgs e)
        {
            if (txtEmail.Text.Trim() == "")
                return;

            if (!clsValidation.ValidateEmail(txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Wrong Email Format");
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
            }
        }

        private void ValidateNationalNo(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

            //Make sure the national number is not used by another person
            if (txtNationalNo.Text.Trim() != _Person.NationalNo && clsPerson.isPersonExist(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");

            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if(pbPerson.ImageLocation == null)
                pbPerson.Image = Resources.Male_512;

        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (pbPerson.ImageLocation == null)
                pbPerson.Image = Resources.Female_512;
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            openFileDialog1.Title = "Setting Person Image";
            openFileDialog1.InitialDirectory = @"D:\";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbPerson.ImageLocation = openFileDialog1.FileName;
                llRemoveImage.Visible = true;
            }

           
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPerson.ImageLocation = null;

            if (rbMale.Checked)
                pbPerson.Image = Resources.Male_512;
            else
                pbPerson.Image = Resources.Female_512;

            llRemoveImage.Visible = false;
        }

        private bool _HandlePersonImage()
        {
            if(_Person.ImagePath != pbPerson.ImageLocation)
            {
                if (_Person.ImagePath != "")
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch (IOException)
                    {

                    }
                }

                if (pbPerson.ImageLocation != null)
                {
                    string SourceImageFile = pbPerson.ImageLocation;

                    if (clsUtil.CopyPersonImageToImagesFolder(ref SourceImageFile))
                    {
                        pbPerson.ImageLocation = SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }


            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fields are not valid!, put the mouse over the red icon(s) to see the error(s)",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            if (!_HandlePersonImage())
                return;

            int NationalityCountryID = clsCountry.Find(cbCountry.Text.Trim()).ID;

            _Person.NationalNo = txtNationalNo.Text;
            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();

            _Person.NationalityCountryID = NationalityCountryID;

            if (rbMale.Checked)
                _Person.Gendor = (short)enGendor.Male;
            else
                _Person.Gendor = (short)enGendor.Female;

            if (pbPerson.ImageLocation != null)
                _Person.ImagePath = pbPerson.ImageLocation;
            else
                _Person.ImagePath = "";


            if (_Person.Save())
            {
                lblPersonID.Text = _Person.ID.ToString();
                
                _Mode = enMode.Update;
                lblTitle.Text = "Update Person";
                MessageBox.Show("Person Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DataBack?.Invoke(this, _Person.ID);
            }
            else
                MessageBox.Show("An error occured during saving", "Saving Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);

            
        }
    }

}
