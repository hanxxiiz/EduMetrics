using EDUMETRICS_FINAL_.Authentication;
using EDUMETRICS_FINAL_.FileOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDUMETRICS_FINAL_.MainOperations
{
    class Student
    {
        protected StudentAuthentication studentAuth = new StudentAuthentication();
        protected StudentClass studentClass = new StudentClass();
        public void Menu()
        {
            while (true)
            {
                string studentID = studentAuth.Authentication();
                string course = studentClass.Menu(studentID, "Enroll");

                StudentMessages messageservice = new StudentMessages();
                StudentView studentview = new StudentView();
                CourseManagement courseMessages = new CourseManagement(course);

                if (course == "exit")
                {
                    return;
                }
                else 
                { 
                    int messageCount = courseMessages.GetNewMessageCount(course, studentID);
                    try
                    {
                        while (true)
                        {
                            if (messageCount > 0)
                            {
                                Console.WriteLine($"[1] Inbox({messageCount})");
                            }
                            else
                            {
                                Console.WriteLine("[1] Inbox");
                            }
                            Console.WriteLine("[2] View Student Performance");
                            Console.WriteLine("[3] Exit");
                            Console.Write("Enter choice: ");
                            int choice = int.Parse(Console.ReadLine());
                            Console.Clear();

                            switch (choice)
                            {
                                case 1:
                                    messageservice.Messages(course, studentID);
                                    break;
                                case 2:
                                    studentview.StudentPerformance(course, studentID);
                                    break;
                                case 3:
                                    return;
                                default:
                                    Console.WriteLine("Invalid choice. Please try again.");
                                    break;
                            }
                        }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid choice. Please try again.");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Oops...Something went wrong.");
                    }
                }
            }
        }
    }
}
