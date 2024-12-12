using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EDUMETRICS_FINAL_.FileOperations
{
    class EnrollmentCatalog
    {
        private const string directoryPath = @"C:\Users\user1\source\vsstudio\CPE261\EduMetricsFile";
        private string classFilePath;

        //Constructor to create directory (if it does not exist) and filepath to store class information
        public EnrollmentCatalog()
        {
            try
            {
                Directory.CreateDirectory(directoryPath);
                classFilePath = Path.Combine(directoryPath, "ClassInfoFile.txt");

                if (!File.Exists(classFilePath))
                {
                    using (FileStream fs = File.Create(classFilePath)) { }
                }
            }
            catch (IOException)
            {
                Console.WriteLine($"ERROR: File operation failed.");
            }
        }

        //Returns the classFilePath
        public string GetFilePath()
        {
            return classFilePath;
        }

        //Method to save class created by the teacher
        //Method to save class enrolled by the student. Default as zero to indicate that the student is not accepted by the teacher
        public void SaveClass(string course, string id)
        {
            string pattern = @"^T\d{2}-\d{3}$";
            try
            {
                using (StreamWriter write = new StreamWriter(classFilePath, true))
                {
                    if (Regex.IsMatch(id, pattern))
                    {
                        write.WriteLine($"{course},{id},1");
                    }
                    else
                    {
                        write.WriteLine($"{course},{id},0");
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: Course not saved.");
            }
        }


        //Method to enroll chosen registered students 
        public void EnrollStudent(string course, string id, string approval)
        {
            try
            {
                using (StreamWriter write = new StreamWriter(classFilePath, true))
                {
                    write.WriteLine($"{course},{id},{approval}");
                    Console.WriteLine($"Student {id} enrolled successfully.");
                }
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: Student not enrolled.");
            }
        }


        //Method to accept or enroll student requests
        public void EnrollStudent(string course, string id)
        {
            string tempFilePath = Path.GetTempFileName();
            try
            {
                bool updated = false;
                using (StreamReader read = new StreamReader(classFilePath))
                using (StreamWriter write = new StreamWriter(tempFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] parts = line.Split(",");
                        if (parts.Length == 3)
                        {
                            string storedCourse = parts[0];
                            string storedID = parts[1];
                            string approval = parts[2];
                            if (storedCourse == course && storedID == id)
                            {
                                if (approval == "0")
                                {
                                    string approved = "1";
                                    line = line.Replace($"{storedCourse},{storedID},{approval}",$"{storedCourse},{storedID},{approved}");
                                    updated = true;
                                }
                            }
                        }
                        write.WriteLine(line);
                    }
                    read.Close();
                    write.Close();

                    if (updated)
                    {
                        File.Delete(classFilePath);
                        File.Move(tempFilePath, classFilePath);
                        Console.WriteLine($"{id} enrolled successfully.");
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"ERROR: Enrollment unsucessful.");
            }
            finally
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }


        //Method to print all student requests. Returns the value of found. 
        public bool GetStudentRequests(string course)
        {
            try
            {
                bool found = false;
                using (StreamReader read = new StreamReader(classFilePath))
                {
                    string line;
                    Console.WriteLine("STUDENT REQUESTS");
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 3)
                        {
                            string storedCourse = parts[0];
                            string storedID = parts[1];
                            string approval = parts[2];
                            if (storedCourse == course && approval == "0")
                            {
                                UserFile userfile = new UserFile();
                                (string firstname, string lastname) = userfile.GetName(storedID);
                                Console.WriteLine($"{storedID} - {firstname} {lastname}");
                                found = true;
                            }
                        }
                    }
                }
                return found;
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: File not read.");
                return false;
            }
        }


        //Method to print classes created by the teacher. Returns the value of found.
        public bool AvailableClasses()
        {
            try
            {
                if (!File.Exists(classFilePath))
                {
                    Console.WriteLine("ERROR: User file not found.");
                    return false;
                }

                bool found = false;
                using (StreamReader read = new StreamReader(classFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 3)
                        {
                            string storedCourse = parts[0];
                            string storedId = parts[1];
                            string pattern = @"^T\d{2}-\d{3}$";
                            if (Regex.IsMatch(storedId, pattern))
                            {
                                Console.WriteLine($"{storedCourse}");
                                found = true;
                            }
                        }
                    }
                }
                return found;
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: File not read");
                return false;
            }
        }


        //Print all classes enrolled by the user (teacher or student)
        public bool AllClasses(string id)
        {
            try
            {
                bool found = false;
                using (StreamReader read = new StreamReader(classFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 3)
                        {
                            string storedCourse = parts[0];
                            string storedID = parts[1];
                            string approval = parts[2];
                            if (storedID == id && approval == "1")
                            {
                                Console.WriteLine($"{storedCourse}");
                                found = true;
                            }
                        }
                    }
                }
                return found;
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: File not read.");
                return false;
            }
        }


        //Checks if the course/class is enrolled by the user (teacher or student)
        public bool CheckCourse(string course, string id)
        {
            try
            {
                if (!File.Exists(classFilePath))
                {
                    Console.WriteLine("ERROR: course file not found.");
                    return false;
                }

                using (StreamReader read = new StreamReader(classFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 3)
                        {
                            string storedCourse = parts[0];
                            string storedID = parts[1];
                            if (storedCourse == course && storedID == id)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: File not read.");
            }
            return false;
        }


        //Checks if the course/class exists
        public bool CheckCourse(string course)
        {
            try
            {
                if (!File.Exists(classFilePath))
                {
                    Console.WriteLine("ERROR: course file not found.");
                    return false;
                }

                using (StreamReader read = new StreamReader(classFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 3)
                        {
                            string storedCourse = parts[0];
                            if (storedCourse == course)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: File not read.");
            }
            return false;
        }


        //Checks if the user exists in a course/class
        public bool CheckUser(string course, string id)
        {
            try
            {
                if (!File.Exists(classFilePath))
                {
                    Console.WriteLine("ERROR: User file not found.");
                    return false;
                }

                using (StreamReader read = new StreamReader(classFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 3)
                        {
                            string storedCourse = parts[0].Trim();
                            string storedID = parts[1].Trim();
                            if (storedCourse == course && storedID == id)
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


        //Returns the ID of the teacher
        public string GetTeacherID(string course)
        {
            try
            {
                if (!File.Exists(classFilePath))
                {
                    Console.WriteLine("ERROR: User file not found.");
                    return null;
                }

                using (StreamReader read = new StreamReader(classFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 3)
                        {
                            string storedCourse = parts[0].Trim();
                            string storedID = parts[1].Trim();
                            if (storedCourse == course)
                            {
                                return storedID;
                            }
                        }
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: Trouble checking ID.");
            }
            return null;
        }


        //Returns the number of student requests
        public int GetRequestCount(string course)
        {
            int count = 0;
            try
            {
                if (!File.Exists(classFilePath))
                {
                    Console.WriteLine("No messages found.");
                    return count;
                }

                using (StreamReader read = new StreamReader(classFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 3)
                        {
                            string storedCourse = parts[0].Trim();
                            string storedID = parts[1].Trim();
                            string approval = parts[2].Trim();
                            if (storedCourse == course && approval == "0")
                            {
                                count++;
                            }
                        }
                    }
                }
                return count;
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: File not read.");
                return count;
            }
        }

        public List<(string ID, string Firstname, string Lastname)> GetEnrolledStudents(string course)
        {
            UserFile user = new UserFile();
            List<(string ID, string Firstname, string Lastname)> enrolledStudents = new List<(string ID, string Firstname, string Lastname)>();

            try
            {
                using (StreamReader read = new StreamReader(classFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 3)
                        {
                            string storedCourse = parts[0].Trim();
                            string storedID = parts[1].Trim();
                            string approval = parts[2].Trim();
                            string pattern = @"^S\d{2}-\d{3}$";

                            if (storedCourse == course && Regex.IsMatch(storedID, pattern) && approval == "1")
                            {
                                (string firstname, string lastname) = user.GetName(storedID);
                                firstname = char.ToUpper(firstname[0]) + firstname.Substring(1).ToLower();
                                lastname = char.ToLower(lastname[0]) + lastname.Substring(1).ToLower();
                                enrolledStudents.Add((storedID, firstname, lastname));
                            }
                        }
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: File could not be read.");
            }
            return enrolledStudents;
        }
    }
}
