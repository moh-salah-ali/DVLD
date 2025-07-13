using DVLD_BusinessLayer;
using System;
using System.Data;

namespace TestConsoleApp
{
    internal class Program
    {


        static void testFindPersonByID(int ID)

        {
            clsPerson Person1 = clsPerson.Find(ID);

            if (Person1 != null)
            {
                Console.WriteLine(Person1.NationalNo);
                Console.WriteLine(Person1.FirstName + " " + Person1.SecondName 
                    + " " + Person1.ThirdName + " " + Person1.LastName);
                Console.WriteLine(Person1.DateOfBirth);
                Console.WriteLine(Person1.Gendor);
                Console.WriteLine(Person1.Address);
                Console.WriteLine(Person1.Phone);
                Console.WriteLine(Person1.Email);
                Console.WriteLine(Person1.NationalityCountryID);
                Console.WriteLine(Person1.ImagePath);
            }
            else
            {
                Console.WriteLine("Contact [" + ID + "] Not found!");
            }
        }

        static void testFindPersonByNationalNo(string NationalNo)

        {
            clsPerson Person1 = clsPerson.Find(NationalNo);

            if (Person1 != null)
            {
                Console.WriteLine(Person1.ID);
                Console.WriteLine(Person1.FirstName + " " + Person1.SecondName
                    + " " + Person1.ThirdName + " " + Person1.LastName);
                Console.WriteLine(Person1.DateOfBirth);
                Console.WriteLine(Person1.Gendor);
                Console.WriteLine(Person1.Address);
                Console.WriteLine(Person1.Phone);
                Console.WriteLine(Person1.Email);
                Console.WriteLine(Person1.NationalityCountryID);
                Console.WriteLine(Person1.ImagePath);
            }
            else
            {
                Console.WriteLine("Contact [" + NationalNo + "] Not found!");
            }
        }


        static void testAddNewContact()


        {
            clsPerson Contact1 = new clsPerson();
            Contact1.NationalNo = "N11";
            Contact1.FirstName = "Mohamed";
            Contact1.SecondName = "Salah";
            Contact1.SecondName = "Ali";
            Contact1.LastName = "Alfadul";
            Contact1.Email = "M@a.com";
            Contact1.Phone = "123456789";
            Contact1.Address = "address12";
            Contact1.DateOfBirth = new DateTime(1978, 11, 6, 10, 30, 0);
            Contact1.NationalityCountryID = 8;
            Contact1.ImagePath = "";

            if (Contact1.Save())
            {

                Console.WriteLine("Contact Added Successfully with id=" + Contact1.ID);
            }

        }

        static void testUpdateContact(int ID)

        {
            clsPerson Contact1 = clsPerson.Find(ID);

            if (Contact1 != null)
            {
                //update whatever info you want
                Contact1.NationalNo = "N13";
                Contact1.FirstName = "Tamer";
                Contact1.SecondName = "Ahmed";
                Contact1.ThirdName = "Ali";
                Contact1.LastName = "Ahmed";
                Contact1.Email = "Mka@a.com";
                Contact1.Phone = "123456789";
                Contact1.Address = "address13";
                Contact1.DateOfBirth = new DateTime(1998, 11, 7, 10, 30, 0);
                Contact1.NationalityCountryID = 13;
                Contact1.ImagePath = "";

                if (Contact1.Save())
                {

                    Console.WriteLine("Contact updated Successfully");
                }

            }
            else
            {
                Console.WriteLine("Not found!");
            }
        }

        static void testDeleteContact(int ID)

        {

            if (clsPerson.isPersonExist(ID))

                if (clsPerson.DeletePerson(ID))

                    Console.WriteLine("Contact Deleted Successfully.");
                else
                    Console.WriteLine("Faild to delete contact.");

            else
                Console.WriteLine("The contact with id = " + ID + " is not found");

        }

        static void ListContacts()
        {

            DataTable dataTable = clsPerson.GetAllPeople();

            Console.WriteLine("People Data:");

            foreach (DataRow row in dataTable.Rows)
            {
                Console.WriteLine($"{row["PersonID"]},  {row["FirstName"]} {row["LastName"]}");
            }

        }

        static void testIsPersonExist(int ID)

        {

            if (clsPerson.isPersonExist(ID))

                Console.WriteLine("Yes, Contact is there.");

            else
                Console.WriteLine("No, Contact Is not there.");

        }

        static void testIsPersonExist(string NationalNo)

        {

            if (clsPerson.isPersonExist(NationalNo))

                Console.WriteLine("Yes, Person is there.");

            else
                Console.WriteLine("No, Person Is not there.");

        }

        ////---Test Country Business

        static void testFindCountryByID(int ID)

        {
            clsCountry Country1 = clsCountry.Find(ID);

            if (Country1 != null)
            {
                Console.WriteLine("Name: " + Country1.CountryName);
                

            }

            else
            {
                Console.WriteLine("Country [" + ID + "] Not found!");
            }
        }


        static void testFindCountryByName(string CountryName)

        {
            clsCountry Country1 = clsCountry.Find(CountryName);

            if (Country1 != null)
            {
                Console.WriteLine("Country [" + CountryName + "] isFound with ID = " + Country1.ID);
                Console.WriteLine("Name: " + Country1.CountryName);
                
            }

            else
            {
                Console.WriteLine("Country [" + CountryName + "] Is Not found!");
            }
        }


        //static void testIsCountryExistByID(int ID)

        //{

        //    if (clsCountry.isCountryExist(ID))

        //        Console.WriteLine("Yes, Country is there.");

        //    else
        //        Console.WriteLine("No, Country Is not there.");

        //}

        //static void testIsCountryExistByName(string CountryName)

        //{

        //    if (clsCountry.isCountryExist(CountryName))

        //        Console.WriteLine("Yes, Country is there.");

        //    else
        //        Console.WriteLine("No, Country Is not there.");

        //}


        //static void testAddNewCountry()


        //{
        //    clsCountry Country1 = new clsCountry();

        //    Country1.CountryName = "Eygpt";
        //    Country1.Code = "222";
        //    Country1.PhoneCode = "001";


        //    if (Country1.Save())
        //    {

        //        Console.WriteLine("Country Added Successfully with id=" + Country1.ID);
        //    }

        //}

        //static void testUpdateCountry(int ID)

        //{
        //    clsCountry Country1 = clsCountry.Find(ID);

        //    if (Country1 != null)
        //    {
        //        //update whatever info you want
        //        Country1.CountryName = "Egypt";
        //        Country1.Code = "111";
        //        Country1.PhoneCode = "555";


        //        if (Country1.Save())
        //        {

        //            Console.WriteLine("Country updated Successfully ");
        //        }

        //    }
        //    else
        //    {
        //        Console.WriteLine("Country is you want to update is Not found!");
        //    }
        //}

        //static void testDeleteCountry(int ID)

        //{

        //    if (clsCountry.isCountryExist(ID))

        //        if (clsCountry.DeleteCountry(ID))

        //            Console.WriteLine("Country Deleted Successfully.");
        //        else
        //            Console.WriteLine("Faild to delete Country.");

        //    else
        //        Console.WriteLine("Faild to delete: The Country with id = " + ID + " is not found");

        //}

        //static void ListCountries()
        //{

        //    DataTable dataTable = clsCountry.GetAllCountries();

        //    Console.WriteLine("Coutries Data:");

        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        Console.WriteLine($"{row["CountryID"]},  {row["CountryName"]} , {row["Code"]}, {row["PhoneCode"]}");
        //    }

        //}

        static void Main(string[] args)
        {
            
            
            //testFindPersonByID(1);
            //testFindPersonByNationalNo("N3");
            //testAddNewContact();
            //testUpdateContact(1030);
            //testDeleteContact(1027);
            //ListContacts();
            //testIsPersonExist(1);
            testIsPersonExist("N3");

            //testFindCountryByID(6);
            //testFindCountryByName("UK");
            //testIsCountryExistByID(1);
            //testIsCountryExistByName("United States");
            //testAddNewCountry();
            //testUpdateCountry(6);
            //testDeleteCountry(6);
            //ListCountries();
        }
    }
}
