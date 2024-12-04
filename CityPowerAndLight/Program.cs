using CityPowerAndLight.Controllers;
using CityPowerAndLight.Models;
using DotNetEnv;
using System.Net.Http.Headers;

namespace CityPowerAndLight;

class Program
{
    public static async Task Main(string[] args)
    {
        #region Prep
        if (!Env.Load("../../../../.env").Any())
            throw new Exception("No environment file was found. It should go next to the solution file");

        HttpClient client = new HttpClient();
        AccountController accounts = new AccountController(client);
        ContactController contacts = new ContactController(client);
        IncidentController incidents = new IncidentController(client);

        //Give http client token from authorised user to allow modification to the customer service hub app
        string authToken = await Authorisation.Authorise(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        //Instruct api to return data after post and patch calls
        client.DefaultRequestHeaders.Add("Prefer", "return=representation");
        #endregion

        #region Demo
        Console.WriteLine("--- Creating Account, Contact and Case (Incident) ---");
        Account account = await accounts.Create("Bad Company");
        Console.WriteLine($"Account created:\n\tName={account.Name}\n");

        Contact contact1 = await contacts.Create("John", "Badman", account.Id);
        Console.WriteLine($"Contact created:\n\tName={contact1.FirstName} {contact1.Lastname}\n\tAccount Id={contact1.AccountId}\n");
        Contact contact2 = await contacts.Create("Jane", "Goodman", account.Id);
        Console.WriteLine($"Contact created:\n\tName={contact2.FirstName} {contact2.Lastname}\n\tAccount Id={contact2.AccountId}\n");

        Incident incident = await incidents.Create("Company is bad", "I don't like that the company is bad :(", contact1.Id);
        Console.WriteLine($"Incident created:\n\tTitle={incident.Title}\n\tDescription={incident.Description}\n\tCustomer Id={incident.CustomerId}\n");

        Console.Write("Press enter to continue");
        Console.ReadLine();

        Console.WriteLine("\n--- Reading All Incidents and Searching by Title ---");

        Incident[] allIncidents = await incidents.ReadAll();
        incident = allIncidents.Where(x => x.Title == "Company is bad").ToArray()[0];
        Console.WriteLine($"Found Incident:\n\tTitle={incident.Title}\n\tDescription={incident.Description}\n\tCustomer Id={incident.CustomerId}\n");

        Console.Write("Press enter to continue");
        Console.ReadLine();

        Console.WriteLine("\n--- Reading Specific Incident by Id ---");

        incident = await incidents.ReadOne(incident.Id);
        Console.WriteLine($"Found Incident:\n\tTitle={incident.Title}\n\tDescription={incident.Description}\n\tCustomer Id={incident.CustomerId}\n");

        Console.Write("Press enter to continue");
        Console.ReadLine();

        Console.WriteLine("\n--- Updating Incident ---");

        incident = await incidents.Update(incident.Id, "Company is good", "I like that the company is good :)", contact2.Id);
        Console.WriteLine($"Updated Incident:\n\tTitle={incident.Title}\n\tDescription={incident.Description}\n\tCustomer Id={incident.CustomerId}\n");

        Console.Write("Press enter to continue");
        Console.ReadLine();

        Console.WriteLine("\n--- Deleting Incident ---");

        await incidents.Delete(incident.Id);
        Console.WriteLine("Deleted Incident");

        Console.Write("Press enter to cleanup and finish");
        Console.ReadLine();

        await contacts.Delete(contact1.Id);
        await contacts.Delete(contact2.Id);
        await accounts.Delete(account.Id);

        Console.WriteLine("\nFinished");
        #endregion
    }
}