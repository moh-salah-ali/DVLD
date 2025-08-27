using System;
using System.Data;
using System.Data.SqlClient;
using DVLD_DataLayer;

namespace DVLD_BusinessLayer
{
    public class clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 };

        public enMode Mode = enMode.Update;

        public int ID { set; get; }
        public string NationalNo { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string LastName { set; get; }


        public string FullName 
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; } 
        }
        
        public DateTime DateOfBirth { set; get; }
        public short Gendor { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public int NationalityCountryID { set; get; }
        public clsCountry CountryInfo;
        public string ImagePath { set; get; }

        

        public clsPerson()

        {
            this.ID = -1;
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Gendor = 2;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.NationalityCountryID = -1;
            this.ImagePath = "";

            Mode = enMode.AddNew;

        }

        private clsPerson(int ID, string NationalNo, string FirstName,
            string SecondName, string ThirdName, string LastName, DateTime DateOfBirth,
            short Gendor, string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)

        {
            this.ID = ID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gendor = Gendor;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;
            this.CountryInfo = clsCountry.Find(NationalityCountryID);
            Mode = enMode.Update;

        }

        private bool _AddNewPerson()
        {
            

            this.ID = clsPeopleDataAccess.AddNewPerson(this.NationalNo, this.FirstName,
            this.SecondName, this.ThirdName, this.LastName, this.DateOfBirth,
            this.Gendor, this.Address, this.Phone, this.Email,
            this.NationalityCountryID, this.ImagePath);

            return (this.ID != -1);
        }

        private bool _UpdatePerson()
        {
             

            return clsPeopleDataAccess.UpdatePerson(this.ID, this.NationalNo, this.FirstName,
            this.SecondName, this.ThirdName, this.LastName, this.DateOfBirth,
            this.Gendor, this.Address, this.Phone, this.Email,
            this.NationalityCountryID, this.ImagePath);

        }

        public static clsPerson Find(int ID)
        {

            string NationalNo = "", FirstName = "", SecondName = "", ThirdName = "", LastName = "",
                Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1;
            short Gendor = 2;

            if (clsPeopleDataAccess.GetPersonInfoByID(ID, ref NationalNo, ref FirstName,
            ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth,
            ref Gendor, ref Address, ref Phone, ref Email,
            ref NationalityCountryID, ref ImagePath))
            {
                return new clsPerson(ID, NationalNo, FirstName,
                SecondName, ThirdName, LastName, DateOfBirth,
                Gendor, Address, Phone, Email,
                NationalityCountryID, ImagePath);
            }
            else
            {
                return null;
            }
        }

        public static clsPerson Find(string NationalNo)
        {

            int PersonID = 0;
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "",
                Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1;
            short Gendor = 0;

            if (clsPeopleDataAccess.GetPersonInfoByNationalNo(NationalNo, ref PersonID, ref FirstName,
            ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth,
            ref Gendor, ref Address, ref Phone, ref Email,
            ref NationalityCountryID, ref ImagePath))
            {
                return new clsPerson(PersonID, NationalNo, FirstName,
                SecondName, ThirdName, LastName, DateOfBirth,
                Gendor, Address, Phone, Email,
                NationalityCountryID, ImagePath);
            }
            else
            {
                return null;
            }
        }

        public bool Save()
        {


            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdatePerson();

            }




            return false;
        }

        public static DataTable GetAllPeople()
        {
            return clsPeopleDataAccess.GetAllPeople();

        }

        public static bool DeletePerson(int ID)
        {
            return clsPeopleDataAccess.DeletePerson(ID);
        }

        public static bool isPersonExist(int ID)
        {
            return clsPeopleDataAccess.IsPersonExist(ID);
        }

        public static bool isPersonExist(string NationalNo)
        {
            return clsPeopleDataAccess.IsPersonExist(NationalNo);
        }
    }
}
