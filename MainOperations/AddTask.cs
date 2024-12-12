using EDUMETRICS_FINAL_.FileOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDUMETRICS_FINAL_.MainOperations
{
    public class AddTask
    {
        private string Category { get; set; } = string.Empty;
        private string Taskname { get; set; } = string.Empty;
        private int Perfectscore { get; set; } = 0;
        private int Score { get; set; } = 0;

        public static List<(string course, string Category, string TaskName, double PerfectScore)> Tasks { get; set; } = new List<(string course, string Category, string TaskName, double PerfectScore)>();


        //Method to add task within a course/class
        public void AddingTask(string course)
        {

            CourseManagement courseActivity = new CourseManagement(course, "ACTIVITY");
            CourseManagement courseQuiz = new CourseManagement(course, "QUIZ");
            CourseManagement courseExam = new CourseManagement(course, "EXAM");
            while (true)
            {
                try
                {
                    Console.WriteLine("Adding task...");
                    do
                    {
                        Console.Write("Enter category [ Quiz - Activity - Exam ]: ");
                        Category = Console.ReadLine().Trim().ToLower();
                        if (Category != "quiz" && Category != "activity" && Category != "exam")
                        {
                            Console.WriteLine("Invalid category. Try again.");
                        }
                    } while (Category != "quiz" && Category != "activity" && Category != "exam");

                    Console.Write("Enter taskname: ");
                    Taskname = Console.ReadLine();
                    Console.Write("Enter perfect score: ");
                    Perfectscore = int.Parse(Console.ReadLine());
                    Tasks.Add((course, Category, Taskname, Perfectscore));

                    switch (Category)
                    {
                        case "activity":
                            courseActivity.SaveAssessment(course, Taskname, Perfectscore, -1);
                            break;
                        case "quiz":
                            courseQuiz.SaveAssessment(course, Taskname, Perfectscore, -1);
                            break;
                        case "exam":
                            courseExam.SaveAssessment(course, Taskname, Perfectscore, -1);
                            break;
                    }
                    Console.Clear();
                    Console.WriteLine($"{Taskname} saved under category '{Category}'.");
                    return;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
            }
        }

        public List<(string course, string Category, string TaskName, double PerfectScore)> GetTasks()
        {
            return Tasks;
        }
    }
}
