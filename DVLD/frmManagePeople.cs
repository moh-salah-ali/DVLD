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
    public partial class frmManagePeople : Form
    {

        private static DataTable _dtAllPeople = clsPerson.GetAllPeople();
        private DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GendorCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");
        
        public frmManagePeople()
        {
            InitializeComponent();
        }

       private void _RefreshPeopleList()
        {


        _dtAllPeople = clsPerson.GetAllPeople();
        _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GendorCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");
            dgvPeopleList.DataSource = _dtPeople;

            lblPeopleRecordsNum.Text = dgvPeopleList.Rows.Count.ToString();
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            dgvPeopleList.DataSource = _dtPeople;
            cbFilterBy.SelectedIndex = 0;
            lblPeopleRecordsNum.Text = dgvPeopleList.Rows.Count.ToString();
            if (dgvPeopleList.Rows.Count > 0)
            {

                dgvPeopleList.Columns[0].HeaderText = "Person ID";
                dgvPeopleList.Columns[0].Width = 110;

                dgvPeopleList.Columns[1].HeaderText = "National No.";
                dgvPeopleList.Columns[1].Width = 120;

                    
                dgvPeopleList.Columns[2].HeaderText = "First Name";
                dgvPeopleList.Columns[2].Width = 120;

                dgvPeopleList.Columns[3].HeaderText = "Second Name";
                dgvPeopleList.Columns[3].Width = 140;


                dgvPeopleList.Columns[4].HeaderText = "Third Name";
                dgvPeopleList.Columns[4].Width = 120;

                dgvPeopleList.Columns[5].HeaderText = "Last Name";
                dgvPeopleList.Columns[5].Width = 120;

                dgvPeopleList.Columns[6].HeaderText = "Gendor";
                dgvPeopleList.Columns[6].Width = 120;

                dgvPeopleList.Columns[7].HeaderText = "Date Of Birth";
                dgvPeopleList.Columns[7].Width = 140;

                dgvPeopleList.Columns[8].HeaderText = "Nationality";
                dgvPeopleList.Columns[8].Width = 120;


                dgvPeopleList.Columns[9].HeaderText = "Phone";
                dgvPeopleList.Columns[9].Width = 120;


                dgvPeopleList.Columns[10].HeaderText = "Email";
                dgvPeopleList.Columns[10].Width = 170;
            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterString = "";

            switch (cbFilterBy.Text)
            {
                case "Person ID":
                    FilterString = "PersonID";
                    break;
                case "National No.":
                    FilterString = "NationalNo";
                    break;
                case "First Name":
                    FilterString = "FirstName";
                    break;
                case "Second Name":
                    FilterString = "SecondName";
                    break;
                case "Third Name":
                    FilterString = "ThirdName";
                    break;
                case "Last Name":
                    FilterString = "LastName";
                    break;
                case "Gendor":
                    FilterString = "GendorCaption";
                    break;
                case "Date Of Birth":
                    FilterString = "DateOfBirth";
                    break;
                case "Nationality":
                    FilterString = "CountryName";
                    break;
                case "Phone":
                    FilterString = "Phone";
                    break;
                case "Email":
                    FilterString = "Email";
                    break;
                default:
                    FilterString = "None";
                    break;
            }

            if (txtFilterValue.Text.Trim() == "" || FilterString == "None")
            {
                _dtPeople.DefaultView.RowFilter = "";
                lblPeopleRecordsNum.Text = dgvPeopleList.Rows.Count.ToString();
                return;
            }


            if (FilterString == "PersonID")
            {
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterString, txtFilterValue.Text.Trim());
            }
            else
            {
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterString, txtFilterValue.Text.Trim());
            }

            lblPeopleRecordsNum.Text = dgvPeopleList.Rows.Count.ToString();


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        //private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    //we allow number incase person id is selected.
        //    if (cbFilterBy.Text == "Person ID")
        //        e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        //}
    }
}
