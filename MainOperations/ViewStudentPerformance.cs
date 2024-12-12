using EDUMETRICS_FINAL_.FileOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDUMETRICS_FINAL_.MainOperations
{

    internal class ViewStudentPerformance
    {
        public virtual void StudentPerformance(string course, string id)
        {
            CourseManagement courseActivity = new CourseManagement(course, "ACTIVITY");
            CourseManagement courseQuiz = new CourseManagement(course, "QUIZ");
            CourseManagement courseExam = new CourseManagement(course, "EXAM");

            List<(string Taskname, double Perfectscore, double Score)> activities = courseActivity.GetTasks(course, id);
            List<(string Taskname, double Perfectscore, double Score)> quizzes = courseQuiz.GetTasks(course, id);
            List<(string Taskname, double Perfectscore, double Score)> exams = courseExam.GetTasks(course, id);

            if (activities.Count > 0 || quizzes.Count > 0 || exams.Count > 0)
            {
                double activityScore = 0;
                double activityTotal = 0;
                double gradeActivity = 0;
                double modeActivity = double.NaN;
                string lowestActivity = string.Empty;

                if (activities.Count > 0)
                {
                    Console.WriteLine("ACTIVITIES");
                    List<double> activityScores = new List<double>();
                    foreach (var activity in activities)
                    {
                        Console.WriteLine($"{activity.Taskname}: {activity.Score}/{activity.Perfectscore}");
                        activityScore += activity.Score;
                        activityTotal += activity.Perfectscore;
                        activityScores.Add(activity.Score);
                        if (lowestActivity == string.Empty || activity.Score < activities.First(x => x.Taskname == lowestActivity).Score)
                        {
                            lowestActivity = activity.Taskname;
                        }
                    }
                    modeActivity = GetMode(activityScores);
                    if (courseActivity.GetAssessmentWeight(course) != 0)
                    {
                        gradeActivity = (activityScore / activityTotal) * courseActivity.GetAssessmentWeight(course);
                    }
                }

                double quizScore = 0;
                double quizTotal = 0;
                double gradeQuiz = 0;
                double modeQuiz = double.NaN;
                string lowestQuiz = string.Empty;

                if (quizzes.Count > 0)
                {
                    Console.WriteLine("QUIZZES");
                    List<double> quizScores = new List<double>();
                    foreach (var quiz in quizzes)
                    {
                        Console.WriteLine($"{quiz.Taskname}: {quiz.Score}/{quiz.Perfectscore}");
                        quizScore += quiz.Score;
                        quizTotal += quiz.Perfectscore;
                        quizScores.Add(quiz.Score);
                        if (lowestQuiz == string.Empty || quiz.Score < quizzes.First(x => x.Taskname == lowestQuiz).Score)
                        {
                            lowestQuiz = quiz.Taskname;
                        }
                    }
                    modeQuiz = GetMode(quizScores);
                    if (courseQuiz.GetAssessmentWeight(course) != 0)
                    {
                        gradeQuiz = (quizScore / quizTotal) * courseQuiz.GetAssessmentWeight(course);
                    }
                }

                double examScore = 0;
                double examTotal = 0;
                double gradeExam = 0;
                double modeExam = double.NaN;
                string lowestExam = string.Empty;

                if (exams.Count > 0)
                {
                    Console.WriteLine("EXAMS");
                    List<double> examScores = new List<double>();
                    foreach (var exam in exams)
                    {
                        Console.WriteLine($"{exam.Taskname}: {exam.Score}/{exam.Perfectscore}");
                        examScore += exam.Score;
                        examTotal += exam.Perfectscore;
                        examScores.Add(exam.Score);
                        if (lowestExam == string.Empty || exam.Score < exams.First(x => x.Taskname == lowestExam).Score)
                        {
                            lowestExam = exam.Taskname;
                        }
                    }
                    modeExam = GetMode(examScores);
                    if (courseExam.GetAssessmentWeight(course) != 0)
                    {
                        gradeExam = (examScore / examTotal) * courseExam.GetAssessmentWeight(course);
                    }
                }

                double average = (gradeActivity + gradeQuiz + gradeExam) * 100;
                Console.WriteLine($"Average: {average:F2}");
                Console.WriteLine($"Classification: {GetClassification(average)}");

                if (!string.IsNullOrEmpty(lowestActivity))
                {
                    Console.WriteLine($"Struggling with Activity: {lowestActivity}");
                }
                if (!string.IsNullOrEmpty(lowestQuiz))
                {
                    Console.WriteLine($"Struggling with Quiz: {lowestQuiz}");
                }
                if (!string.IsNullOrEmpty(lowestExam))
                {
                    Console.WriteLine($"Struggling with Exam: {lowestExam}");
                }

                Console.WriteLine("Press any key to go back to Menu.");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            else
            {
                Console.WriteLine("No tasks.");
                return;
            }
        }

        protected string GetClassification(double average)
        {
            if (average >= 93 && average < 100)
            {
                return "Excellent";
            }
            if (average >= 86 && average < 93)
            {
                return "Very Good";
            }
            if (average >= 80 && average < 86)
            {
                return "Good";
            }
            if (average >= 75 && average < 80)
            {
                return "Fair";
            }
            else
            {
                return "Poor";
            }
        }

        protected double GetMode(List<double> scores)
        {
            if (scores == null || scores.Count == 0)
            {
                return double.NaN;
            }

            List<double> uniqueScores = new List<double>();
            List<int> scoreCounts = new List<int>();

            foreach (var score in scores)
            {
                int index = uniqueScores.IndexOf(score);
                if (index == -1)
                {
                    uniqueScores.Add(score);
                    scoreCounts.Add(1);
                }
                else
                {
                    scoreCounts[index]++;
                }
            }

            int maxCount = 0;
            foreach (var count in scoreCounts)
            {
                if (count > maxCount)
                {
                    maxCount = count;
                }
            }

            double mode = double.NaN;
            bool isMultipleModes = false;

            for (int i = 0; i < scoreCounts.Count; i++)
            {
                if (scoreCounts[i] == maxCount)
                {
                    if (mode == double.NaN)
                    {
                        mode = uniqueScores[i];
                    }
                    else
                    {
                        isMultipleModes = true;
                    }
                }
            }
            return isMultipleModes ? double.NaN : mode;
        }
    }


    class StudentView : ViewStudentPerformance
    {
        public override void StudentPerformance(string course, string id)
        {
            base.StudentPerformance(course, id);
        }
    }

    class TeacherView : ViewStudentPerformance
    {
        private InputFormat input = new InputFormat();
        private EnrollmentCatalog catalog = new EnrollmentCatalog();
        public override void StudentPerformance(string course, string id)
        {
            var enrolledstudents = catalog.GetEnrolledStudents(course);
            foreach(var student in  enrolledstudents)
            {
                Console.WriteLine($"{student.ID}: {student.Firstname} {student.Lastname}");
            }
            while (true)
            {
                try
                {
                    Console.Write("Enter Student ID [S00-000]: ");
                    id = Console.ReadLine();
                    input.StudentIDFormat(id);
                    if (catalog.CheckUser(course, id) == true)
                    {
                        base.StudentPerformance(course, id);
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
