using CityPowerAndLight.Services;
using CityPowerAndLight.Models;
using CityPowerAndLight.View;

namespace CityPowerAndLight.App
{
    internal static class Demo
    {
        private static Service<Account> _accountService;
        private static Service<Contact> _contactService;
        private static Service<Incident> _incidentService;

        private static Account account;
        private static Contact contact1, contact2;
        private static Incident incident;

        public static async Task Run(Service<Account> accountService, Service<Contact> contactService, Service<Incident> incidentService)
        {
            _accountService = accountService;
            _contactService = contactService;
            _incidentService = incidentService;

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
                    await incidentService.Delete(incident.Id);

                if (contact1 != null)
                    await contactService.Delete(contact1.Id);

                if (contact2 != null)
                    await contactService.Delete(contact2.Id);

                if (account != null)
                    await accountService.Delete(account.Id);

                Printer.PromptContinue("An error occurred: " + ex.Message);
            }
        }

        private static async Task CreateModels()
        {
            Printer.PrintHeading("Creating Account, Contact and Case (Incident)");

            StringContent content;

            content = new StringContent(
                Account.GeneratePayload("Bad Company", "111-222-3333", "Edinburgh"), 
                System.Text.Encoding.UTF8, 
                "application/json"
            );
            account = await _accountService.Create(content);
            Printer.PrintModel("Account Created", account);

            content = new StringContent(
                Contact.GeneratePayload("John", "Badman", "johnbadman@badcompany.com", "444-555-6666", account.Id),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            contact1 = await _contactService.Create(content);
            Printer.PrintModel("Contact created", contact1);

            content = new StringContent(
                Contact.GeneratePayload("Jane", "Goodman", "janegoodman@badcompany.com", "777-888-9999", account.Id),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            contact2 = await _contactService.Create(content);
            Printer.PrintModel("Contact created", contact2);

            content = new StringContent(
                Incident.GeneratePayload("Company is bad", "I don't like that the company is bad :(", Priority.High, contact1.Id),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            incident = await _incidentService.Create(content);
            Printer.PrintModel("Incident created", incident);

            Printer.PromptContinue("Press enter to continue");
        }

        private static async Task ReadAllIncidents()
        {
            Printer.PrintHeading("Reading All Incidents Sample");

            Incident[] allIncidents = await _incidentService.ReadAll();
            allIncidents.Take(5).Select(i => 
            {
                Printer.PrintModel("Found Incident", i); return i; 
            }).ToArray();

            Printer.PromptContinue("Press enter to continue");
        }

        private static async Task ReadOneIncident()
        {
            Printer.PrintHeading("Reading Specific Incident by Id");

            Incident readIncident = await _incidentService.ReadOne(incident.Id);
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

            StringContent content = new StringContent(
                incident.GetPayload(),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            incident = await _incidentService.Update(incident.Id, content);
            Printer.PrintModel("Updated Incident", incident);

            Printer.PromptContinue("Press enter to continue");
        }

        private static async Task DeleteIncident()
        {
            Printer.PrintHeading("Deleting Incident");
            await _incidentService.Delete(incident.Id);
            Console.WriteLine("Deleted Incident");

            Printer.PromptContinue("Press enter to cleanup and finish");
        }

        private static async Task Cleanup()
        {
            await _contactService.Delete(contact1.Id);
            await _contactService.Delete(contact2.Id);
            await _accountService.Delete(account.Id);

            Printer.PromptContinue("Finished. Press enter to complete program");
        }
    }
}
