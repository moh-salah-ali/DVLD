﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataLayer;

namespace DVLD_BusinessLayer
{
    public class clsUser
    {
        public enum enMode { AddNew = 0, Update = 1 };

        public enMode Mode = enMode.Update;

        public int UserID { set; get; }
        public int PersonID { set; get; }
        public clsPerson PersonInfo { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public bool IsActive { set; get; }


        public clsUser()

        {
            this.UserID = -1;
            this.PersonID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = false;

            Mode = enMode.AddNew;

        }

        private clsUser(int UserID, int PersonID, string UserName,
            string Password, bool IsActive)

        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;
           
            this.PersonInfo = clsPerson.Find(PersonID);
            Mode = enMode.Update;

        }

        private bool _AddNewUser()
        {
            this.UserID = clsUserData.AddNewUser(this.PersonID, this.UserName,
            this.Password, this.IsActive);

            return (this.UserID != -1);

        }

        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(this.UserID, this.PersonID, this.UserName,
            this.Password, this.IsActive);

        }

        public static clsUser FindByUserID(int UserID)
        {

            string Password = "", UserName = "";
            int PersonID = -1;
            bool IsActive = false;

            if (clsUserData.GetUserInfoByID(UserID, ref PersonID, ref UserName,
                ref Password, ref IsActive))
                return new clsUser(UserID, PersonID, UserName,
                Password, IsActive);
            else
                return null;
        }

        public static clsUser FindByPersonID(int PersonID)
        {
            int UserID = -1;
            string UserName = "", Password = "";
            bool IsActive = false;

            bool IsFound = clsUserData.GetUserInfoByPersonID
                                (PersonID, ref UserID, ref UserName, ref Password, ref IsActive);

            if (IsFound)
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            else
                return null;
        }
        public static clsUser FindByUsernameAndPassword(string UserName, string Password)
        {
            int UserID = -1;
            int PersonID = -1;

            bool IsActive = false;

            bool IsFound = clsUserData.GetUserInfoByUsernameAndPassword
                                (UserName, Password, ref UserID, ref PersonID, ref IsActive);

            if (IsFound)
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateUser();


            }

            return false;

        }

        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();

        }

        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }

        public static bool isUserExist(int UserID)
        {
            return clsUserData.IsUserExist(UserID);
        }

        public static bool isUserExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName);
        }

        public static bool isUserExistForPersonID(int PersonID)
        {
            return clsUserData.IsUserExistForPersonID(PersonID);
        }

        public static bool ChangePassword(int UserID, string NewPassword)
        {
            return clsUserData.ChangePassword(UserID, NewPassword);
        }

    }
}
