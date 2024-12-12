using EDUMETRICS_FINAL_.FileOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace EDUMETRICS_FINAL_.Authentication
{
    abstract class UserAuthentication
    {
        protected string Firstname { get; set; } 
        protected string Lastname { get; set; } 
        protected string ID { get; set; }
        protected string Password { get; set; }

        protected UserFile userfile = new UserFile();
        protected InputFormat input = new InputFormat();

        //Authentication menu. Returns the ID of the user if the authentication is completed
        public virtual string Authentication()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("[1] Register");
                    Console.WriteLine("[2] Login");
                    Console.Write("Enter choice: ");
                    int choice = int.Parse(Console.ReadLine());
                    Console.Clear();

                    switch (choice)
                    {
                        case 1:
                            if (!Registration())
                            {
                                Console.Clear();
                                Console.WriteLine("Registration Unsuccessful. Try again.");
                            }
                            else 
                            {
                                Console.Clear();
                                Console.WriteLine("Registered successfully. Try logging in."); 
                            }
                            break;
                        case 2:
                            if (!Login())
                            {
                                Console.Clear();
                                Console.WriteLine("Login Unsuccessful. Try again.");
                                break;
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Login successful.");
                                return ID;
                            }
                        default:
                            Console.Clear();
                            Console.WriteLine("Invalid choice. Please choose 1-2 only. Try again");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid choice. Please choose 1-2 only. Try again");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Oops...Something went wrong.");
                }
            }
        }

        //A method to be overriden by the subclasses. 
        protected abstract bool Registration();

        //Virtual method to be overriden by the subclasses.  
        //Checks the ID existence. Returns false if the ID is not found.
        //If the ID is found, the user's password is asked and then authenticated. 
        protected virtual bool Login()
        {
            if (userfile.CheckID(ID))
            {
                Console.Write("Enter password: ");
                Password = input.HidePassword();
                return userfile.AuthenticateUser(ID, Password);
            }
            else
            {
                Console.WriteLine("ID does not exist.");
                return false;
            }
        }
    }

    class TeacherAuthentication : UserAuthentication
    {
        //Overrides method Authentication from the superclass
        public override string Authentication()
        {
            Console.WriteLine("Hi Teacher!");
            return base.Authentication();
        }

        //Overrides method Login from the superclass
        protected override bool Login()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter teacher ID [T00-000]: ");
                    ID = Console.ReadLine().Trim();
                    input.TeacherIDFormat(ID);
                    return base.Login();
                }
                catch (FormatException ex)
                {
                    Console.Clear();
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //Implements the body for the abstract method Registration
        protected override bool Registration()
        {
            string password;
            while (true)
            {
                try
                {
                    Console.Write("Enter teacher ID [T00-000]: ");
                    ID = Console.ReadLine().Trim();
                    input.TeacherIDFormat(ID);

                    if (!userfile.CheckID(ID))
                    {
                        do
                        {
                            Console.Write("Enter password: ");
                            Password = input.HidePassword();
                            Console.WriteLine();
                            Console.Write("Confirm password: ");
                            password = input.HidePassword();
                            if (password != Password)
                            {
                                Console.WriteLine("Password does not match. Try again.");
                            }
                        } while (password != Password);

                        Console.WriteLine();
                        Console.Write("Enter firstname: ");
                        Firstname = Console.ReadLine().Trim().ToLower();
                        Console.Write("Enter lastname: ");
                        Lastname = Console.ReadLine().Trim().ToLower();
                        Console.Clear();
                        userfile.SaveCredentials(ID, Password, Firstname, Lastname);
                        return true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Teacher ID already exists.");
                    }
                }
                catch (FormatException ex)
                {
                    Console.Clear();
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }

    class StudentAuthentication: UserAuthentication
    {
        //Overrides method Authentication from the superclass
        public override string Authentication()
        {
            Console.WriteLine("Hi Student!");
            return base.Authentication();
        }

        //Overrides method Login from the superclass
        protected override bool Login()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter student ID [S00-000]: ");
                    ID = Console.ReadLine().Trim();
                    input.StudentIDFormat(ID);
                    return base.Login();
                }
                catch (FormatException ex)
                {
                    Console.Clear();
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //Implements the body for the abstract method Registration
        protected override bool Registration()
        {
            string password;
            while (true)
            {
                try
                {
                    Console.Write("Enter student ID [S00-000]: ");
                    ID = Console.ReadLine().Trim();
                    input.StudentIDFormat(ID);

                    if (!userfile.CheckID(ID))
                    {
                        do
                        {
                            Console.Write("Enter password: ");
                            Password = input.HidePassword();
                            Console.WriteLine();
                            Console.Write("Confirm password: ");
                            password = input.HidePassword();
                            if (password != Password)
                            {
                                Console.WriteLine("Password does not match. Try again.");
                            }
                        } while (password != Password);

                        Console.WriteLine();
                        Console.Write("Enter firstname: ");
                        Firstname = Console.ReadLine().Trim().ToLower();
                        Console.Write("Enter lastname: ");
                        Lastname = Console.ReadLine().Trim().ToLower();
                        Console.Clear();
                        userfile.SaveCredentials(ID, Password, Firstname, Lastname);
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Student ID already exists.");
                    }
                }
                catch (FormatException ex)
                {
                    Console.Clear();
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}



