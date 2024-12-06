using CityPowerAndLight.Services;
using CityPowerAndLight.Models;
using CityPowerAndLight.View;

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

                Printer.PromptContinue("An error occurred: " + ex.Message);
            }
        }

        private static async Task CreateModels()
        {
            Printer.PrintHeading("Creating Account, Contact and Case (Incident)");

            account = await _accounts.Create(Account.GeneratePayload("Bad Company", "111-222-3333", "Edinburgh"));
            Printer.PrintModel("Account Created", account);

            contact1 = await _contacts.Create(Contact.GeneratePayload("John", "Badman", "johnbadman@badcompany.com", "444-555-6666", account.Id));
            Printer.PrintModel("Contact created", contact1);

            contact2 = await _contacts.Create(Contact.GeneratePayload("Jane", "Goodman", "janegoodman@badcompany.com", "777-888-9999", account.Id));
            Printer.PrintModel("Contact created", contact2);

            incident = await _incidents.Create(Incident.GeneratePayload("Company is bad", "I don't like that the company is bad :(", Priority.High, contact1.Id));
            Printer.PrintModel("Incident created", incident);

            Printer.PromptContinue("Press enter to continue");
        }

        private static async Task ReadAllIncidents()
        {
            Printer.PrintHeading("Reading All Incidents Sample");

            Incident[] allIncidents = await _incidents.ReadAll();
            allIncidents.Take(5).Select(i => 
            {
                Printer.PrintModel("Found Incident", i); return i; 
            }).ToArray();

            Printer.PromptContinue("Press enter to continue");
        }

        private static async Task ReadOneIncident()
        {
            Printer.PrintHeading("Reading Specific Incident by Id");

            Incident readIncident = await _incidents.ReadOne(incident.Id);
            Printer.PrintModel("Found Incident", readIncident);

            Printer.PromptContinue("Press enter to continue");
        }

        private static async Task UpdateIncident()
        {
            Printer.PrintHeading("Updating Incident");

            incident.Title = "Company is good";
            incident.Description = "I like that the company is good :)";
            incident.Priority = Priority.Low;
            incident.CustomerId = contact2.Id;
            incident = await _incidents.Update(incident.Id, incident.GetPayload());
            Printer.PrintModel("Updated Incident", incident);

            Printer.PromptContinue("Press enter to continue");
        }

        private static async Task DeleteIncident()
        {
            Printer.PrintHeading("Deleting Incident");
            await _incidents.Delete(incident.Id);
            Console.WriteLine("Deleted Incident");

            Printer.PromptContinue("Press enter to cleanup and finish");
        }

        private static async Task Cleanup()
        {
            await _contacts.Delete(contact1.Id);
            await _contacts.Delete(contact2.Id);
            await _accounts.Delete(account.Id);

            Printer.PromptContinue("Finished. Press enter to complete program");
        }
    }
}
