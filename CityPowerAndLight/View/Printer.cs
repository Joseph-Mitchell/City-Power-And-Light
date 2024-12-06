using CityPowerAndLight.Models;

namespace CityPowerAndLight.View
{
    internal static class Printer
    {
        public static void PrintHeading(string text)
        {
            Console.WriteLine($"--- {text} ---");
        }

        public static void PrintModel(string title, Account account) 
        {
            Console.WriteLine($"{title}:\n\tName={account.Name}\n\tTelephone={account.Phone}\n\tCity={account.City}\n");
        }

        public static void PrintModel(string title, Contact contact)
        {
            Console.WriteLine($"{title}:\n\tName={contact.FirstName} {contact.Lastname}\n\tEmail={contact.Email}\n\tPhone={contact.Phone}\n\tAccount Id={contact.AccountId}\n");
        }

        public static void PrintModel(string title, Incident incident)
        {
            Console.WriteLine($"{title}:\n\tTitle={incident.Title}\n\tPriority={incident.Priority}\n\tDescription={incident.Description}\n\tCustomer Id={incident.CustomerId}\n");
        }

        public static void PromptContinue(string text)
        {
            Console.Write(text);
            Console.ReadLine();
            Console.WriteLine();
        }
    }
}
