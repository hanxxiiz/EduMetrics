using EDUMETRICS_FINAL_.MainOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace EDUMETRICS_FINAL_.FileOperations
{
    class CourseManagement
    {
        private const string directoryPath = @"C:\Users\user1\source\vsstudio\CPE261\EduMetricsFile";
        private string courseDirectoryPath;
        private string assessmentFilePath;
        private string messageFilePath;
        private string announcementFilePath;

        private bool messagesViewed = false;

        //Constructor to create directory (if it does not exist)
        //Create a unique directory for a class
        //Create assessment filepaths to store class assessment information
        public CourseManagement(string course, string assessment)
        {
            try
            {
                Directory.CreateDirectory(directoryPath);
                courseDirectoryPath = Path.Combine(directoryPath, course);
                Directory.CreateDirectory(courseDirectoryPath);
                assessmentFilePath = Path.Combine(courseDirectoryPath, $"{course}_{assessment}.txt");

                if (!File.Exists(assessmentFilePath))
                {
                    using (FileStream fs = File.Create(assessmentFilePath)) { }
                }
            }
            catch (IOException)
            {
                Console.WriteLine($"ERROR: File operation failed.");
            }
        }


        //Constructor to create filepath to store messages exchanged within a course/class
        public CourseManagement(string course)
        {
            try
            {
                Directory.CreateDirectory(directoryPath);
                courseDirectoryPath = Path.Combine(directoryPath, course);
                Directory.CreateDirectory(courseDirectoryPath);
                messageFilePath = Path.Combine(courseDirectoryPath, $"{course}_MESSAGES.txt");

                if (!File.Exists(messageFilePath))
                {
                    using (FileStream fs = File.Create(messageFilePath)) { }
                }
            }
            catch (IOException)
            {
                Console.WriteLine($"ERROR: File operation failed.");
            }
        }


        //Create assessment file with first line as the grading system info
        public void CreateAssessmentFile(string course, string assessment, double weight)
        {
            Directory.CreateDirectory(directoryPath);
            courseDirectoryPath = Path.Combine(directoryPath, course);
            Directory.CreateDirectory(courseDirectoryPath);
            assessmentFilePath = Path.Combine(courseDirectoryPath, $"{course}_{assessment}.txt");
            try
            {
                using (StreamWriter write = new StreamWriter(assessmentFilePath, true))
                {
                    write.WriteLine($"weight,{weight / 100}");
                }
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: Credentials not saved.");
            }
        }


        //Method to save assessment created by the teacher to the appropriate assessment file
        //Initializes the value of score with negative 1 to indicate the task is ungraded
        public void SaveAssessment(string course, string taskname, double perfectscore, double score)
        {
            try
            {
                EnrollmentCatalog catalog = new EnrollmentCatalog();
                string classfilepath = catalog.GetFilePath();

                using (StreamReader read = new StreamReader(classfilepath))
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
                                using (StreamWriter write = new StreamWriter(assessmentFilePath, true))
                                {
                                    write.WriteLine($"{storedID},{taskname},{perfectscore},-1");
                                }
                            }
                        }
                    }
                }

            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: Assessment not saved.");
            }
        }
                

        public void SaveAssessment (string id, string taskname, double perfectscore)
        {
            try
            {
                using (StreamWriter write = new StreamWriter(assessmentFilePath, true))
                {
                    write.WriteLine($"{id},{taskname},{perfectscore},-1");
                }
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: Assessment not saved.");
            }
        }


        //Method to grade all ungraded students 
        //The score stored in the appropriate assessment file is updated into the new score given by the teacher
        public void GradeStudents(string course)
        {
            try
            {
                UserFile user = new UserFile();

                string tempFilePath = Path.GetTempFileName();
                bool updated = false;

                using (StreamReader read = new StreamReader(assessmentFilePath))
                using (StreamWriter writer = new StreamWriter(tempFilePath))
                {
                    string firstLine = read.ReadLine();
                    writer.WriteLine(firstLine);
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 4)
                        {
                            string storedStudentID = parts[0].Trim();
                            string storedTaskname = parts[1].Trim();
                            int storedPerfectscore = int.Parse(parts[2].Trim());
                            string storedScore = parts[3].Trim();

                            if (storedScore == "-1")
                            {
                                (string firstname, string lastname) = user.GetName(storedStudentID);
                                int score;

                                while (true)
                                {
                                    try
                                    {
                                        Console.Write($"{storedTaskname}");
                                        Console.Write($"{storedStudentID} {firstname} {lastname} score: ");
                                        score = int.Parse(Console.ReadLine());
                                        if (score < 0 || score > storedPerfectscore)
                                        {
                                            Console.WriteLine($"Invalid score. Please enter a score between 0 and {storedPerfectscore}.");
                                        }
                                        else
                                        {
                                            line = line.Replace($"{storedStudentID},{storedTaskname},{storedPerfectscore},{storedScore}", $"{storedStudentID},{storedTaskname},{storedPerfectscore},{score}");
                                            updated = true;
                                            break;
                                        }
                                    }
                                    catch (FormatException)
                                    {
                                        Console.WriteLine("Invalid score. Please try again.");
                                    }
                                }
                            }
                            writer.WriteLine(line);
                        }
                    }
                }
                if (updated)
                {
                    File.Delete(assessmentFilePath);
                    File.Move(tempFilePath, assessmentFilePath);
                    Console.Clear();
                    Console.WriteLine("Grading completed and file updated successfully.");
                }
                else
                {
                    File.Delete(tempFilePath);
                    Console.Clear();
                    Console.WriteLine("No ungraded students were found.");
                }
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: File not found.");
            }
        }


        //Returns a list of graded task of the student from a category 
        public List<(string TaskName, double PerfectScore, double Score)> GetTasks(string course, string id)
        {
            List<(string TaskName, double PerfectScore, double Score)> taskList = new List<(string TaskName, double PerfectScore, double Score)>();
            try
            {
                using (StreamReader read = new StreamReader(assessmentFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 4)
                        {
                            string storedID = parts[0].Trim();
                            string storedTaskName = parts[1].Trim();
                            double storedPerfectScore = double.Parse(parts[2].Trim());
                            double storedScore = double.Parse(parts[3].Trim());

                            if (storedID == id && storedScore != -1)
                            {
                                taskList.Add((storedTaskName, storedPerfectScore, storedScore));
                            }
                        }
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: File could not be read.");
            }
            return taskList;
        }

        //Returns a list of all tasks in a class/course
        public List<(string TaskName, double PerfectScore, double Score)> GetTasks()
        {
            List<(string TaskName, double PerfectScore, double Score)> taskList = new List<(string TaskName, double PerfectScore, double Score)>();
            try
            {
                using (StreamReader read = new StreamReader(assessmentFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 4)
                        {
                            string storedID = parts[0].Trim();
                            string storedTaskName = parts[1].Trim();
                            double storedPerfectScore = double.Parse(parts[2].Trim());
                            double storedScore = double.Parse(parts[3].Trim());

                            if (storedScore != -1)
                            {
                                taskList.Add((storedTaskName, storedPerfectScore, storedScore));
                            }
                        }
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: File could not be read.");
            }
            return taskList;
        }

        //Reads the first line of the assessment file and returns the recorded weight
        public double GetAssessmentWeight(string course)
        {
            try
            {
                using (StreamReader read = new StreamReader(assessmentFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        if (line.StartsWith("weight,"))
                        {
                            string[] parts = line.Split(',');
                            if (parts.Length == 2 && double.TryParse(parts[1], out double weight))
                            {
                                return weight;
                            }
                        }
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: Unable to read the file.");
            }
            return 0.0;
        }


        //Returns the number of ungraded Assessment
        public int GetUngradedAssessmentCount(string assessment)
        {
            int count = 0;
            try
            {
                if (!File.Exists(assessmentFilePath))
                {
                    Console.WriteLine($"No ungraded {assessment} found.");
                    return count;
                }

                using (StreamReader read = new StreamReader(assessmentFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 4)
                        {
                            string storedID = parts[0].Trim();
                            string storedTaskname = parts[1].Trim();
                            string storedPerfectscore = parts[2];
                            string storedScore = parts[3].Trim();
                            if (storedScore == "-1")
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


        //Method to display messages received by the user
        //Updates message status to viewed
        public void ViewMessage(string course, string receiver)
        {
            UserFile userfile = new UserFile();
            string tempFilePath = Path.GetTempFileName();
            bool hasMessages = false;
            bool updated = false;

            try
            {
                if (!File.Exists(messageFilePath))
                {
                    Console.WriteLine("No messages found.");
                    return;
                }

                using (StreamReader read = new StreamReader(messageFilePath))
                {
                    using (StreamWriter write = new StreamWriter(tempFilePath, true))
                    {
                        string line;
                        Console.WriteLine("INBOX");

                        while ((line = read.ReadLine()) != null)
                        {
                            string[] parts = line.Split(',');
                            if (parts.Length == 4)
                            {
                                string storedSender = parts[0].Trim();
                                string storedReceiver = parts[1].Trim();
                                string status = parts[2].Trim();
                                string storedMessage = parts[3];

                                if (storedReceiver == receiver)
                                {
                                    (string firstname, string lastname) = userfile.GetName(storedSender);
                                    firstname = char.ToUpper(firstname[0]) + firstname.Substring(1).ToLower();
                                    lastname = char.ToLower(lastname[0]) + lastname.Substring(1).ToLower();

                                    Console.WriteLine($"FROM: {storedSender} - {firstname} {lastname}");
                                    Console.WriteLine($"MESSAGE: {storedMessage}");
                                    hasMessages = true;

                                    if (status == "unviewed")
                                    {
                                        string newStatus = "viewed";
                                        line = line.Replace(status, newStatus);
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
                            File.Delete(messageFilePath);
                            File.Move(tempFilePath, messageFilePath);
                            Console.WriteLine("Messages viewed.");
                        }

                    }
                }
                if (!hasMessages)
                {
                    Console.WriteLine("No new messages.");
                }

                messagesViewed = true;
                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: Trouble checking messages.");
            }
        }


        //Method to save the sender and receiver info, and message to the messageFilePath
        public void SaveMessage(string course, string sender, string receiver, string message)
        {
            Directory.CreateDirectory(directoryPath);
            courseDirectoryPath = Path.Combine(directoryPath, course);
            Directory.CreateDirectory(courseDirectoryPath);
            messageFilePath = Path.Combine(courseDirectoryPath, $"{course}_MESSAGES.txt");
            try
            {
                using (StreamWriter write = new StreamWriter(messageFilePath, true))
                {
                    write.WriteLine($"{sender},{receiver},unviewed,{message}");
                }
                messagesViewed = false;
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: Credentials not saved.");
            }
        }


        //Returns the number of GetNewMessageCount
        public int GetNewMessageCount(string course, string receiver)
        {
            int count = 0;
            try
            {
                if (!File.Exists(messageFilePath))
                {
                    return count;
                }

                using (StreamReader read = new StreamReader(messageFilePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 4)
                        {
                            string storedReceiver = parts[1].Trim();
                            string status = parts[2].Trim();
                            if (storedReceiver == receiver && status == "unviewed")
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
    }
}
