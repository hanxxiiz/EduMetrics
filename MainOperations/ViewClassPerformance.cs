using EDUMETRICS_FINAL_.FileOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDUMETRICS_FINAL_.MainOperations
{
    class ViewClassPerformance
    {
        public void ClassPerformance(string course)
        {
            List<(string StudentID, double ActivityGrade, double QuizGrade, double ExamGrade)> classScores = ClassScores(course);
            List<double> totalScores = new List<double>();
            List<double> activityScores = new List<double>();
            List<double> quizScores = new List<double>();
            List<double> examScores = new List<double>();

            foreach (var score in classScores)
            {
                double totalScore = score.ActivityGrade + score.QuizGrade + score.ExamGrade;
                totalScores.Add(totalScore);

                activityScores.Add(score.ActivityGrade);
                quizScores.Add(score.QuizGrade);
                examScores.Add(score.ExamGrade);
            }

            double mean = (CalculateMean(totalScores)) * 100;
            double median = (CalculateMedian(totalScores)) * 100;

            Console.WriteLine($"Mean: {mean:F2}%");
            Console.WriteLine($"Median: {median:F2}%");

            Console.WriteLine("\nGrade Distribution:");
            var gradeDistribution = GetGradeDistribution(totalScores);
            foreach (var grade in gradeDistribution)
            {
                Console.WriteLine($"{grade.Key}: {grade.Value} students");
            }

            Console.WriteLine("\nTask Performance");
            GetTaskPerformance(course);
            Console.WriteLine("Press any key to go back to Menu.");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        public List<(string StudentID, double ActivityGrade, double QuizGrade, double ExamGrade)> ClassScores(string course)
        {
            CourseManagement courseActivity = new CourseManagement(course, "ACTIVITY");
            CourseManagement courseQuiz = new CourseManagement(course, "QUIZ");
            CourseManagement courseExam = new CourseManagement(course, "EXAM");

            EnrollmentCatalog catalog = new EnrollmentCatalog();
            List<(string ID, string Firstname, string Lastname)> StudentID = catalog.GetEnrolledStudents(course);
            List<(string StudentID, double ActivityGrade, double QuizGrade, double ExamGrade)> classScores = new List<(string, double, double, double)>();

            foreach (var student in StudentID)
            {
                string id = student.ID;

                List<(string Taskname, double Perfectscore, double Score)> activities = courseActivity.GetTasks(course, id);
                List<(string Taskname, double Perfectscore, double Score)> quizzes = courseQuiz.GetTasks(course, id);
                List<(string Taskname, double Perfectscore, double Score)> exams = courseExam.GetTasks(course, id);

                double activityScore = 0;
                double activityTotal = 0;
                foreach (var a in activities)
                {
                    activityScore += a.Score;
                    activityTotal += a.Perfectscore;
                }

                double quizScore = 0;
                double quizTotal = 0;
                foreach (var q in quizzes)
                {
                    quizScore += q.Score;
                    quizTotal += q.Perfectscore;
                }

                double examScore = 0;
                double examTotal = 0;
                foreach (var e in exams)
                {
                    examScore += e.Score;
                    examTotal += e.Perfectscore;
                }


                double activityGrade;
                if (activityTotal > 0)
                {
                    activityGrade = (activityScore / activityTotal) * courseActivity.GetAssessmentWeight(course);
                }
                else
                {
                    activityGrade = 0;
                }

                double quizGrade;
                if (quizTotal > 0)
                {
                    quizGrade = (quizScore / quizTotal) * courseQuiz.GetAssessmentWeight(course);
                }
                else
                {
                    quizGrade = 0;
                }

                double examGrade;
                if (examTotal > 0)
                {
                    examGrade = (examScore / examTotal) * courseExam.GetAssessmentWeight(course);
                }
                else
                {
                    examGrade = 0;
                }

                classScores.Add((id, activityGrade, quizGrade, examGrade));
            }
            return classScores;
        }

        public double CalculateMean(List<double> scores)
        {
            if (scores == null || scores.Count == 0)
            {
                return 0;
            }
            return scores.Average();
        }

        public double CalculateMedian(List<double> scores)
        {
            if (scores == null || scores.Count == 0)
            {
                return 0;
            }

            scores.Sort();
            int count = scores.Count;
            if (count % 2 == 0)
            {
                return (scores[count / 2 - 1] + scores[count / 2]) / 2.0;
            }
            else
            {
                return scores[count / 2];
            }
        }

        public Dictionary<string, int> GetGradeDistribution(List<double> scores)
        {
            Dictionary<string, int> distribution = new Dictionary<string, int>();
            distribution["Excellent (93-100%)"] = 0;
            distribution["Very Good (86-92%)"] = 0;
            distribution["Good (80-85%)"] = 0;
            distribution["Fair (75-79%)"] = 0;
            distribution["Poor (<75%)"] = 0;

            foreach (var score in scores)
            {
                if (score >= 93)
                {
                    distribution["Excellent (93-100%)"] = distribution["Excellent (93-100%)"] + 1;
                }
                else if (score >= 86)
                {
                    distribution["Very Good (86-92%)"] = distribution["Very Good (86-92%)"] + 1;
                }
                else if (score >= 80)
                {
                    distribution["Good (80-85%)"] = distribution["Good (80-85%)"] + 1;
                }
                else if (score >= 75)
                {
                    distribution["Fair (75-79%)"] = distribution["Fair (75-79%)"] + 1;
                }
                else
                {
                    distribution["Poor (<75%)"] = distribution["Poor (<75%)"] + 1;
                }
            }
            return distribution;
        }

        public void GetTaskPerformance(string course)
        {
            CourseManagement courseActivity = new CourseManagement(course, "ACTIVITY");
            CourseManagement courseQuiz = new CourseManagement(course, "QUIZ");
            CourseManagement courseExam = new CourseManagement(course, "EXAM");

            List<(string Taskname, double Score, double Perfectscore)> activities = courseActivity.GetTasks();
            List<(string Taskname, double Score, double Perfectscore)> quizzes = courseQuiz.GetTasks();
            List<(string Taskname, double Score, double Perfectscore)> exams = courseExam.GetTasks();

            Console.WriteLine("Task Performance Analysis:");

            (string HighActivity, string LowActivity) = GetHighLowScoreTasks(activities);
            Console.WriteLine($"Activity\n\tHigh Scoring Task: {HighActivity}\n\tLow Scoring Task: {LowActivity}");

            (string HighQuiz, string LowQuiz) = GetHighLowScoreTasks(quizzes);
            Console.WriteLine($"Quiz\n\tHigh Scoring Task: {HighQuiz}\n\tLow Scoring Task: {LowQuiz}");

            (string HighExam, string LowExam) = GetHighLowScoreTasks(exams);
            Console.WriteLine($"Exam\n\tHigh Scoring Task: {HighExam}\n\tLow Scoring Task: {LowExam}");
        }

        private (string, string) GetHighLowScoreTasks(List<(string Taskname, double Score, double Perfectscore)> tasks)
        {
            double highScoreThreshold = 0.92;
            double lowScoreThreshold = 0.74;

            List<string> taskNames = new List<string>();
            List<int> highCounts = new List<int>();
            List<int> lowCounts = new List<int>();

            foreach (var task in tasks)
            {
                if (!taskNames.Contains(task.Taskname))
                {
                    taskNames.Add(task.Taskname);
                    highCounts.Add(0);
                    lowCounts.Add(0);
                }
            }

            for (int i = 0; i < taskNames.Count; i++)
            {
                string currentTask = taskNames[i];
                foreach (var task in tasks)
                {
                    if (task.Taskname == currentTask)
                    {
                        double taskScoreRatio = task.Score / task.Perfectscore;
                        if (taskScoreRatio >= highScoreThreshold)
                        {
                            highCounts[i]++;
                        }
                        else if (taskScoreRatio <= lowScoreThreshold)
                        {
                            lowCounts[i]++;
                        }
                    }
                }
            }

            int maxHighCount = 0;
            int maxLowCount = 0;
            string highScoringTask = "No high-scoring task";
            string lowScoringTask = "No low-scoring task";

            for (int i = 0; i < taskNames.Count; i++)
            {
                if (highCounts[i] > maxHighCount)
                {
                    maxHighCount = highCounts[i];
                    highScoringTask = taskNames[i];
                }
                if (lowCounts[i] > maxLowCount)
                {
                    maxLowCount = lowCounts[i];
                    lowScoringTask = taskNames[i];
                }
            }

            return (highScoringTask, lowScoringTask);
        }
    }
}