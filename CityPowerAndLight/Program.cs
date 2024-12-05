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
        var accounts = new Controller<Account>(client, "accounts");
        var contacts = new Controller<Contact>(client, "contacts");
        var incidents = new Controller<Incident>(client, "incidents");

        //Give http client token from authorised user to allow modification to the customer service hub app
        string authToken = await Authorisation.Authorise(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        //Instruct api to return data after post and patch calls
        client.DefaultRequestHeaders.Add("Prefer", "return=representation");
        #endregion

        #region Demo
        Console.WriteLine("--- Creating Account, Contact and Case (Incident) ---");
        Account account = await accounts.Create(new Account("Bad Company"));
        Console.WriteLine($"Account created:\n\tName={account.Name}\n");

        Contact contact1 = await contacts.Create(new Contact("John", "Badman", account.Id));
        Console.WriteLine($"Contact created:\n\tName={contact1.FirstName} {contact1.Lastname}\n\tAccount Id={contact1.AccountId}\n");
        Contact contact2 = await contacts.Create(new Contact("Jane", "Goodman", account.Id));
        Console.WriteLine($"Contact created:\n\tName={contact2.FirstName} {contact2.Lastname}\n\tAccount Id={contact2.AccountId}\n");

        Incident incident = await incidents.Create(new Incident("Company is bad", "I don't like that the company is bad :(", contact1.Id));
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

        incident.Title = "Company is good";
        incident.Description = "I like that the company is good :)";
        incident = await incidents.Update(incident.Id, incident);
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