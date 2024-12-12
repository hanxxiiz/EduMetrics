using EDUMETRICS_FINAL_.MainOperations;
using Spectre.Console;

namespace EDUMETRICS_FINAL_
{
    class EduMetrics
    {
        static void Main()
        {
            Teacher teacher = new Teacher();
            Student student = new Student();

            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("=======================================");
                    Console.WriteLine("          Welcome to EduMetrics        ");
                    Console.WriteLine("=======================================");

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\nAre you a...");
                    Console.ResetColor();

                    Console.WriteLine("[1] Teacher");
                    Console.WriteLine("[2] Student");
                    Console.WriteLine("[3] Exit");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\nEnter choice: ");
                    Console.ResetColor();

                    int choice = int.Parse(Console.ReadLine());
                    Console.Clear();

                    switch (choice)
                    {
                        case 1:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Navigating to Teacher Menu...\n");
                            Console.ResetColor();
                            teacher.Menu();
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Navigating to Student Menu...\n");
                            Console.ResetColor();
                            student.Menu();
                            break;
                        case 3:
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("Exiting EduMetrics. Goodbye!");
                            Console.ResetColor();
                            return;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid choice. Please choose 1-3 only. Try again.\n");
                            Console.ResetColor();
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid choice. Please enter a number (1-3) only. Try again.\n");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Oops...Something went wrong. Details: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }
    }

}
