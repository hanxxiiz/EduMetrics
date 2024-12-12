using EDUMETRICS_FINAL_.FileOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDUMETRICS_FINAL_.MainOperations
{
    class MessageService
    {
        protected string Message { get; set; }
        protected string ID { get; set; }

        protected InputFormat input = new InputFormat();
        protected EnrollmentCatalog catalog = new EnrollmentCatalog();
        protected UserFile userfile = new UserFile();
        
        public virtual void Messages(string course, string id)
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("[1] Send message");
                    Console.WriteLine("[2] View messages");
                    Console.WriteLine("[3] Exit");
                    Console.Write("Enter choice: ");
                    int choice = int.Parse(Console.ReadLine());
                    Console.Clear();
                    switch (choice)
                    {
                        case 1:
                            SendMessage(course, id);
                            break;
                        case 2:
                            ViewMessage(course, id);
                            break;
                        case 3:
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please choose 1-3 only. Try again.");
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

        protected virtual void SendMessage(string course, string id)
        {
            CourseManagement courseMessage = new CourseManagement(course);

            (string firstname, string lastname) = userfile.GetName(ID);
            firstname = char.ToUpper(firstname[0]) + firstname.Substring(1).ToLower();
            lastname = char.ToLower(lastname[0]) + lastname.Substring(1).ToLower();

            Console.WriteLine($"TO: {ID} - {firstname} {lastname}");
            Console.WriteLine("MESSAGE:");
            Message = Console.ReadLine();
            Console.Write("Send message? (y)");
            string send = Console.ReadLine().Trim().ToLower();
            if (send == "y")
            {
                courseMessage.SaveMessage(course, id, ID, Message);
                Console.WriteLine("Message sent!");
            }
        }

        protected virtual void ViewMessage(string course, string id)
        {
            CourseManagement courseMessage = new CourseManagement(course);
            courseMessage.ViewMessage(course, id);
        }
    }

    class TeacherMessages: MessageService
    {
        public override void Messages(string course, string id)
        {
            base.Messages(course, id);
        }

        protected override void SendMessage(string course, string id)
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter Student ID [S00-000]: ");
                    ID = Console.ReadLine();
                    input.StudentIDFormat(ID);
                    if (catalog.CheckUser(course, ID) == true)
                    {
                        base.SendMessage(course, id);
                    }
                    else
                    {
                        Console.WriteLine("Student does not exist.");
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        protected override void ViewMessage(string course, string id)
        {
            base.ViewMessage(course, id);
        }
    }

    class StudentMessages: MessageService
    {
        public override void Messages(string course, string id)
        {
            base.Messages(course, id);
        }

        protected override void SendMessage(string course, string id)
        {
            ID = catalog.GetTeacherID(course);
            base.SendMessage(course, id);
        }
        protected override void ViewMessage(string course, string id)
        {
            base.ViewMessage(course, id);
        }
    }
}
