using CityPowerAndLight.Services;
using CityPowerAndLight.Models;

namespace CityPowerAndLight.App
{
    internal static class Demo
    {
        private static Service<Account> _accounts;
        private static Service<Contact> _contacts;
        private static Service<Incident> _incidents;

        private static Account account;
        private static Contact contact1, contact2;
        private static Incident incident;

        public static async Task Run(Service<Account> accounts, Service<Contact> contacts, Service<Incident> incidents)
        {
            _accounts = accounts;
            _contacts = contacts;
            _incidents = incidents;

            try
            {
                await CreateModels();
                await ReadAllIncidents();
                await ReadOneIncident();
                await UpdateIncident();
                await DeleteIncident();
                await Cleanup();
            }
            catch (Exception ex) 
            { 

                if (incident != null)
                    await incidents.Delete(incident.Id);

                if (contact1 != null)
                    await contacts.Delete(contact1.Id);

                if (contact2 != null)
                    await contacts.Delete(contact2.Id);

                if (account != null)
                    await accounts.Delete(account.Id);

                Console.WriteLine("An error occurred: " + ex.Message);
                Console.ReadLine();
            }
        }

        private static async Task CreateModels()
        {
            Console.WriteLine("--- Creating Account, Contact and Case (Incident) ---");

            account = await _accounts.Create(new Account("Bad Company"));
            Console.WriteLine($"Account created:\n\tName={account.Name}\n");

            contact1 = await _contacts.Create(new Contact("John", "Badman", account.Id));
            Console.WriteLine($"Contact created:\n\tName={contact1.FirstName} {contact1.Lastname}\n\tAccount Id={contact1.AccountId}\n");

            contact2 = await _contacts.Create(new Contact("Jane", "Goodman", account.Id));
            Console.WriteLine($"Contact created:\n\tName={contact2.FirstName} {contact2.Lastname}\n\tAccount Id={contact2.AccountId}\n");

            incident = await _incidents.Create(new Incident("Company is bad", "I don't like that the company is bad :(", contact1.Id));
            Console.WriteLine($"Incident created:\n\tTitle={incident.Title}\n\tDescription={incident.Description}\n\tCustomer Id={incident.CustomerId}\n");

            Console.Write("Press enter to continue");
            Console.ReadLine();
        }

        private static async Task ReadAllIncidents()
        {
            Console.WriteLine("\n--- Reading All Incidents Sample ---");

            Incident[] allIncidents = await _incidents.ReadAll();
            allIncidents.Take(5).Select(i => 
            { 
                Console.WriteLine($"Found Incident:\n\tTitle={i.Title}\n\tDescription={i.Description}\n\tCustomer Id={i.CustomerId}\n"); return i; 
            }).ToArray();

            Console.Write("Press enter to continue");
            Console.ReadLine();
        }

        private static async Task ReadOneIncident()
        {
            Console.WriteLine("\n--- Reading Specific Incident by Id ---");

            Incident readIncident = await _incidents.ReadOne(incident.Id);
            Console.WriteLine($"Found Incident:\n\tTitle={readIncident.Title}\n\tDescription={readIncident.Description}\n\tCustomer Id={readIncident.CustomerId}\n");

            Console.Write("Press enter to continue");
            Console.ReadLine();
        }

        private static async Task UpdateIncident()
        {
            Console.WriteLine("\n--- Updating Incident ---");

            incident.Title = "Company is good";
            incident.Description = "I like that the company is good :)";
            incident = await _incidents.Update(incident.Id, incident);
            Console.WriteLine($"Updated Incident:\n\tTitle={incident.Title}\n\tDescription={incident.Description}\n\tCustomer Id={incident.CustomerId}\n");

            Console.Write("Press enter to continue");
            Console.ReadLine();
        }

        private static async Task DeleteIncident()
        {
            Console.WriteLine("\n--- Deleting Incident ---");

            await _incidents.Delete(incident.Id);
            Console.WriteLine("Deleted Incident");

            Console.Write("Press enter to cleanup and finish");
            Console.ReadLine();
        }

        private static async Task Cleanup()
        {
            await _contacts.Delete(contact1.Id);
            await _contacts.Delete(contact2.Id);
            await _accounts.Delete(account.Id);

            Console.WriteLine("\nFinished");
            Console.ReadLine();
        }
    }
}
