using DVLD_BusinessLayer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    internal class clsGlobal
    {
        public static clsUser CurrentUser;


        public static void RememberUsernameAndPassword(string username, string password)
        {
            string keyPath = @"HKEY_CURRENT_USER\Software\DVLD";

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                username = "";
                password = "";
            }

            try
            {
                Registry.SetValue(keyPath, "UserName", username, RegistryValueKind.String);
                Registry.SetValue(keyPath, "Password", password, RegistryValueKind.String);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        public static bool GetStoredLoginData(ref string username, ref string password)
        {
            string keyPath = @"HKEY_CURRENT_USER\Software\DVLD";

            try
            {
                string storedUsername = Registry.GetValue(keyPath, "UserName", null) as string;
                string storedPassword = Registry.GetValue(keyPath, "Password", null) as string;

                if (string.IsNullOrEmpty(storedUsername) || string.IsNullOrEmpty(storedPassword))
                {
                    return false;
                }

                username = storedUsername;
                password = storedPassword;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }


    }
}
