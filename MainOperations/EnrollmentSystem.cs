using EDUMETRICS_FINAL_.FileOperations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDUMETRICS_FINAL_.MainOperations
{
    abstract class EnrollmentSystem
    {
        protected string CourseCode { get; set; } 
        protected string ID { get; set; } 

        protected EnrollmentCatalog catalog = new EnrollmentCatalog();
        protected InputFormat input = new InputFormat();

        //Enrollment menu. Returns the Course Code/Class entered by the user (teacher or student)
        public virtual string Menu(string id, string prompt)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine($"[1] {prompt} class");
                    Console.WriteLine("[2] Enter class");
                    Console.WriteLine("[3] Exit");
                    Console.Write("Enter choice: ");
                    int choice = int.Parse(Console.ReadLine());
                    Console.Clear();

                    switch (choice)
                    {
                        case 1:
                            if (!EnrollClass(id))
                            {
                                break;
                            }
                            else
                            {
                                break;
                            }
                        case 2:
                            if (!EnterClass(id))
                            {
                                break;
                            }
                            else
                            {
                                return CourseCode;
                            }
                        case 3:
                            return "exit";
                        default:
                            Console.WriteLine("Invalid choice. Please choose 1-3 only. Try again.");
                            break;
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
        }


        //Abstract method to be overriden by the subclasses
        protected abstract bool EnrollClass(string id);


        //Virtual method to be overriden by the subclasses. 
        protected virtual bool EnterClass(string id)
        {
            Console.WriteLine("YOUR CLASSES");
            if (catalog.AllClasses(id))
            {
                while (true)
                {
                    try
                    {
                        Console.Write("Choose class you want to enter: ");
                        CourseCode = Console.ReadLine();

                        Console.Clear();
                        input.CourseCodeFormat(CourseCode);
                        if (catalog.CheckCourse(CourseCode, id))
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Class not enrolled.");
                            return false;
                        }
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine("You have no classes yet.");
                return false;
            }
        }
    }

    class TeacherClass: EnrollmentSystem
    {
        private UserFile userfile = new UserFile();

        private int Quiz = 0;
        private int Activity = 0;
        private int Exam = 0;
        private int Weight = 0;

        //Overrides method Menu and returns the result of the method
        public override string Menu(string id, string prompt)
        {
            return base.Menu(id, prompt);
        }

        //Overrides method EnterClass and returns the result of the method
        protected override bool EnterClass(string id)
        {
            return base.EnterClass(id);
        }

        //Overrides and implements the body of the method EnrollClass for the teacher
        protected override bool EnrollClass(string id)
        {
            Console.WriteLine("Creating class...");
            while (true)
            {
                try
                {
                    Console.Write("Enter course code [COURSE-000]: ");
                    CourseCode = Console.ReadLine();
                    input.CourseCodeFormat(CourseCode);

                    if (!catalog.CheckCourse(CourseCode))
                    {
                        Console.WriteLine($"{CourseCode} GRADING SYSTEM");
                        while (true)
                        {
                            try
                            {
                                Console.Write("Activity: ");
                                Activity = int.Parse(Console.ReadLine());
                                Console.Write("Quiz: ");
                                Quiz = int.Parse(Console.ReadLine());
                                Console.Write("Exam: ");
                                Exam = int.Parse(Console.ReadLine());
                                Weight = Activity + Quiz + Exam;
                                if (Weight > 100)
                                {
                                    Console.WriteLine("Invalid grading system. Total grade weight is greater than 100%. Try again.");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Invalid input. Please enter an integer value.");
                            }
                        }
                        CourseManagement courseActivity = new CourseManagement(CourseCode, "ACTIVITY");
                        courseActivity.CreateAssessmentFile(CourseCode, "ACTIVITY", Activity);
                        CourseManagement courseQuiz = new CourseManagement(CourseCode, "QUIZ");
                        courseQuiz.CreateAssessmentFile(CourseCode, "QUIZ", Quiz);
                        CourseManagement courseExam = new CourseManagement(CourseCode, "EXAM");
                        courseExam.CreateAssessmentFile(CourseCode, "EXAM", Exam);

                        catalog.SaveClass(CourseCode, id);
                        Console.Clear();
                        Console.WriteLine("Class created successfully!");


                        EnrollStudents(CourseCode);
                        return true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"Class with course code {CourseCode} already exists.");
                        return false;
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //Method to prompt the user to enroll registered students in the newly-created class
        public void EnrollStudents(string course)
        {
            while (true) {
                try
                {
                    Console.Write("Enroll students? (y)");
                    string choice = Console.ReadLine().Trim().ToLower();
                    if (choice == "y")
                    {
                        Console.Write("Enter student ID [S00-000]: ");
                        ID = Console.ReadLine().Trim();
                        input.StudentIDFormat(ID);
                        if (userfile.CheckID(ID) == true)
                        {
                            catalog.EnrollStudent(course, ID, "1");
                        }
                        else
                        {
                            Console.WriteLine("Student does not exist.");
                        }
                    }
                    else
                    {
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

    class StudentClass: EnrollmentSystem
    {

        //Overrides method Menu and returns the result of the method
        public override string Menu(string id, string prompt)
        {
            return base.Menu(id, prompt);
        }

        //Overrides method EnterClass and returns the result of the method
        protected override bool EnterClass(string id)
        {
            return base.EnterClass(id);
        }

        //Overrides and implements the body of the method EnrollClass for the student
        protected override bool EnrollClass(string id)
        {
            Console.WriteLine("Enrolling class...");
            Console.WriteLine("AVAILABLE CLASSES");
            if (catalog.AvailableClasses())
            {
                while (true)
                {
                    try
                    {
                        Console.Write("Enter course code: ");
                        CourseCode = Console.ReadLine();
                        input.CourseCodeFormat(CourseCode);

                        if (catalog.CheckCourse(CourseCode))
                        {
                            catalog.SaveClass(CourseCode, id);
                            Console.WriteLine($"Enrollment to class {CourseCode} requested.");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Class does not exist.");
                            return false;
                        }
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine("No available classes.");
                return false;
            }
        }
    }
}
