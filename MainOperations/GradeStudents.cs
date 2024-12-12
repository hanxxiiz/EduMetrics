using EDUMETRICS_FINAL_.FileOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDUMETRICS_FINAL_.MainOperations
{
    class GradeStudents
    {
        public void GradingStudents(string course)
        {
            CourseManagement courseActivity = new CourseManagement(course, "ACTIVITY");
            CourseManagement courseQuiz = new CourseManagement(course, "QUIZ");
            CourseManagement courseExam = new CourseManagement(course, "EXAM");

            int activityCount = courseActivity.GetUngradedAssessmentCount("Activity");
            int quizCount = courseQuiz.GetUngradedAssessmentCount("Quiz");
            int examCount = courseExam.GetUngradedAssessmentCount("Exam");

            try
            {
                while (true)
                {
                    if (activityCount > 0)
                    {
                        Console.WriteLine($"[1] Activity");
                    }
                    else
                    {
                        Console.WriteLine("[1] Activity");
                    }

                    if (quizCount > 0)
                    {
                        Console.WriteLine($"[2] Quiz");
                    }
                    else
                    {
                        Console.WriteLine("[2] Quiz");
                    }

                    if (examCount > 0)
                    {
                        Console.WriteLine($"[3] Exam");
                    }
                    else
                    {
                        Console.WriteLine("[3] Exam");
                    }

                    Console.WriteLine("[4] Exit");
                    Console.Write("Enter choice: ");
                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            courseActivity.GradeStudents(course);
                            break;
                        case 2:
                            courseQuiz.GradeStudents(course);
                            break;
                        case 3:
                            courseExam.GradeStudents(course);
                            break;
                        case 4:
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please choose 1-4 only. Try again.");
                            break;
                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid choice. Please choose 1-4 only. Try again.");
            }
            catch (Exception)
            {
                Console.WriteLine("Oops...Something went wrong.");
            }
        }
    }
}
