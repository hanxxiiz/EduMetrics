using EDUMETRICS_FINAL_.Authentication;
using EDUMETRICS_FINAL_.FileOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDUMETRICS_FINAL_.MainOperations
{
    class Teacher
    {
        private TeacherAuthentication teacherAuth = new TeacherAuthentication();
        private TeacherClass teacherClass = new TeacherClass();

        public void Menu()
        {
            string teacherID = teacherAuth.Authentication();
            string course = teacherClass.Menu(teacherID, "Create");

            TeacherMessages messageservice = new TeacherMessages();
            StudentRequests requests = new StudentRequests();
            AddTask addTask = new AddTask();
            GradeStudents gradestudents = new GradeStudents();
            TeacherView teacherview = new TeacherView();
            ViewClassPerformance classperformance = new ViewClassPerformance();

            EnrollmentCatalog catalog = new EnrollmentCatalog();

            var enrolledStudents = catalog.GetEnrolledStudents(course);

            while (course != "exit")
            {
                CourseManagement courseMessages = new CourseManagement(course);
                int messageCount = courseMessages.GetNewMessageCount(course, teacherID);
                int requestCount = catalog.GetRequestCount(course);
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

                        if (requestCount > 0)
                        {
                            Console.WriteLine($"[2] Student Requests");
                        }
                        else
                        {
                            Console.WriteLine("[2] Student Requests");
                        }
                        Console.WriteLine("[3] Add task");
                        Console.WriteLine("[4] Grade students");
                        Console.WriteLine("[5] View student performance");
                        Console.WriteLine("[6] View class performance");
                        Console.WriteLine("[7] Exit");

                        Console.Write("Enter choice: ");
                        int choice = int.Parse(Console.ReadLine());
                        Console.Clear();

                        switch (choice)
                        {
                            case 1:
                                messageservice.Messages(course, teacherID);
                                break;
                            case 2:
                                requests.ViewStudentRequests(course, teacherID);
                                break;
                            case 3:
                                addTask.AddingTask(course);
                                break;
                            case 4:
                                gradestudents.GradingStudents(course);
                                break;
                            case 5:
                                string id = null;
                                teacherview.StudentPerformance(course, id);
                                break;
                            case 6:
                                classperformance.ClassPerformance(course);
                                break;
                            case 7:
                                return;
                            default:
                                Console.WriteLine("Invalid choice. Please try again");
                                break;
                        }
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid choice. Please choose 1-3 only. Try again.");
                }
                catch (Exception)
                {
                    Console.WriteLine("Oops...Something went wrong.");
                }
            }

            Console.WriteLine("No enrolled students yet");
        }
    }
}
