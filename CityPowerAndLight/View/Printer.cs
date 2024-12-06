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
            Console.WriteLine($"{title}:\n\tName={account.Name}\n\tTelephone={account.Telephone}\n\tCity={account.City}");
        }

        public static void PrintModel(string title, Contact contact)
        {
            Console.WriteLine($"{title}:\n\tName={contact.FirstName} {contact.Lastname}\n\tAccount Id={contact.AccountId}\n");
        }

        public static void PrintModel(string title, Incident incident)
        {
            Console.WriteLine($"{title}:\n\tTitle={incident.Title}\n\tDescription={incident.Description}\n\tCustomer Id={incident.CustomerId}\n");
        }

        public static void PromptContinue(string text)
        {
            Console.Write(text);
            Console.ReadLine();
            Console.WriteLine();
        }
    }
}
