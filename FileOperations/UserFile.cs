using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDUMETRICS_FINAL_.FileOperations
{
    class UserFile
    {
        private const string directoryPath = @"C:\Users\user1\source\vsstudio\CPE261\EduMetricsFile";
        private string userFilePath;

        //Constructor to create directory (if it does not exist) and filepath to store user credentials
        public UserFile()
        {
            try
            {
                Directory.CreateDirectory(directoryPath);
                userFilePath = Path.Combine(directoryPath, "UserFile.txt");

                if (!File.Exists(userFilePath))
                {
                    using (FileStream fs = File.Create(userFilePath)) { }
                }
            }
            catch (IOException)
            {
                Console.WriteLine($"ERROR: File operation failed.");
            }
        }

        //Method to save credentials of the user (id,password,firstname,lastname}
        public void SaveCredentials(string id, string password, string firstname, string lastname)
        {
            Directory.CreateDirectory(directoryPath);
            userFilePath = Path.Combine(directoryPath, "UserFile.txt");
            try
            {
                using (StreamWriter write = new StreamWriter(userFilePath, true))
                {
                    write.WriteLine($"{id},{password},{firstname},{lastname}");
                }
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: Credentials not saved.");
            }
        }

        //Returns userFilePath
        public string GetFilePath()
        {
            return userFilePath;
        }

        //Method to check if the ID entered by the user exists
        public bool CheckID(string id)
        {
            try
            {
                if (!File.Exists(userFilePath))
                {
                    Console.WriteLine("ERROR: User file not found.");
                    return false;
                }

                using (StreamReader read = new StreamReader(userFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] credentials = line.Split(',');
                        if (credentials.Length == 4)
                        {
                            string storedID = credentials[0].Trim();
                            if (storedID == id)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: Trouble checking ID.");
            }
            return false;
        }


        //Method to authenticate user. 
        public bool AuthenticateUser(string id, string password)
        {
            try
            {
                if (!File.Exists(userFilePath))
                {
                    Console.WriteLine("ERROR: User file not found.");
                    return false;
                }

                using (StreamReader read = new StreamReader(userFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] credentials = line.Split(',');
                        if (credentials.Length == 4)
                        {
                            string storedID = credentials[0].Trim();
                            string storePassword = credentials[1].Trim();
                            if (storedID == id && storePassword == password)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: User not authenticated");
            }
            return false;
        }


        //Method to return firstname and lastname
        public (string, string) GetName(string id)
        {
            try
            {
                if (!File.Exists(userFilePath))
                {
                    Console.WriteLine("ERROR: User file not found.");
                    return (null, null);
                }

                using (StreamReader read = new StreamReader(userFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] credentials = line.Split(',');
                        if (credentials.Length == 4)
                        {
                            string storedID = credentials[0].Trim();
                            string storedFirstname = credentials[2].Trim();
                            string storedLastname = credentials[3].Trim();
                            if (storedID == id)
                            {
                                storedFirstname = char.ToUpper(storedFirstname[0]) + storedFirstname.Substring(1).ToLower();
                                storedLastname = char.ToLower(storedLastname[0]) + storedLastname.Substring(1).ToLower();
                                return (storedFirstname, storedLastname);
                            }
                        }
                    }
                }
                Console.WriteLine("ID not found.");
                return (null, null);
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: Trouble checking ID.");
                return (null, null);
            }
        }

    }
}
