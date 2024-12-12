using EDUMETRICS_FINAL_.FileOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDUMETRICS_FINAL_.MainOperations
{
    class StudentRequests
    {
        private EnrollmentCatalog catalog = new EnrollmentCatalog();

        private InputFormat input = new InputFormat();
        private string ID { get; set; } = string.Empty;
        public void ViewStudentRequests(string course, string id)
        {
            AddTask addTask = new AddTask();
            List<(string course, string Category, string TaskName, double PerfectScore)> tasks = addTask.GetTasks();

            if (!catalog.GetStudentRequests(course))
            {
                Console.WriteLine("No student requests.");
                return;
            }

            else 
            { 
                try
                {
                    while (true)
                    {
                        Console.WriteLine("[1] Accept students");
                        Console.WriteLine("[2] Exit");
                        Console.Write("Enter choice: ");
                        int choice = int.Parse(Console.ReadLine());
                        switch (choice)
                        {
                            case 1:
                                AcceptStudents(course);
                                break;
                            case 2:
                                return;
                            default:
                                Console.WriteLine("Invalid choice. Please choose 1-2 only. Try again.");
                                break;
                        }
                    }
                }
                catch (Exception EX)
                {
                    Console.WriteLine($"ERROR: Student requests not found.{EX}");
                }
            }
        }

        public void AcceptStudents(string course)
        {
            AddTask addTask = new AddTask();
            List<(string course, string Category, string TaskName, double PerfectScore)> tasks = addTask.GetTasks();

            CourseManagement courseActivity = new CourseManagement(course, "ACTIVITY");
            CourseManagement courseQuiz = new CourseManagement(course, "QUIZ");
            CourseManagement courseExam = new CourseManagement(course, "EXAM");

            while (true)
            {
                try
                {
                    Console.Write("Enter Student ID [S00-000]: ");
                    ID = Console.ReadLine();
                    input.StudentIDFormat(ID);
                    if (catalog.CheckUser(course, ID) == true)
                    {
                        catalog.EnrollStudent(course, ID);
                        foreach (var task in tasks)
                        {
                            if(task.course == course)
                            {
                                if (task.Category == "activity")
                                {
                                    courseActivity.SaveAssessment(ID, task.TaskName, task.PerfectScore);
                                }
                                else if (task.Category == "quiz")
                                {
                                    courseExam.SaveAssessment(ID, task.TaskName, task.PerfectScore);
                                }
                                else if(task.Category == "exam")
                                {
                                    courseExam.SaveAssessment(ID, task.TaskName, task.PerfectScore);
                                }
                            }
                        }
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Student does not exist.");
                        return;
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
