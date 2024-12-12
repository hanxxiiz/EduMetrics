using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EDUMETRICS_FINAL_
{
    class InputFormat
    {
        string pattern;

        //Method to check the format of Teacher ID. returns ID if the format is correct and throws an Exception if not.
        public string TeacherIDFormat(string id)
        {
            pattern = @"^T\d{2}-\d{3}$";
            while (true)
            {
                if (Regex.IsMatch(id, pattern))
                {
                    return id;
                }
                else
                {
                    throw new FormatException("Invalid ID. Follow this format: T00-000");
                }
            }
        }

        //Method to check the format of Student ID. returns ID if the format is correct and throws an Exception if not.
        public string StudentIDFormat(string id)
        {
            pattern = @"^S\d{2}-\d{3}$";
            while (true)
            {
                if (Regex.IsMatch(id, pattern))
                {
                    return id;
                }
                else
                {
                    throw new FormatException("Invalid ID. Follow this format: S00-000");
                }
            }
        }

        //Method to check the format of Course Code. returns course Code if the format is correct and throws an Exception if not.
        public string CourseCodeFormat(string course)
        {
            pattern = @"^[A-Z]+-\d{3}$";
            while (true)
            {
                if (Regex.IsMatch(course, pattern))
                {
                    return course;
                }
                else
                {
                    throw new FormatException("Invalid course code. Follow this format: COURSENAME-000");
                }
            }
        }

        //Method to hide input (password) 
        public string HidePassword()
        {
            string password = string.Empty;
            while (true)
            {
                var passwordKey = Console.ReadKey(intercept: true);
                if (passwordKey.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (passwordKey.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Remove(password.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    password += passwordKey.KeyChar;
                    Console.Write("*");
                }
            }
            return password;
        }
    }
}
