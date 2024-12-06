using CityPowerAndLight.App;
using CityPowerAndLight.Services;
using CityPowerAndLight.Models;
using CityPowerAndLight.Utilities;
using DotNetEnv;
using System.Net.Http.Headers;

namespace CityPowerAndLight;

class Program
{
    public static async Task Main(string[] args)
    {
        if (!Env.Load().Any())
            throw new Exception("No environment file was found. It should go in the CityPowerAndLight folder");

        HttpClient client = new HttpClient();
        var accounts = new Service<Account>(client, "accounts", "?$select=name,telephone1,address1_city,accountid");
        var contacts = new Service<Contact>(client, "contacts", "?$select=firstname,lastname,emailaddress1,telephone1,_parentcustomerid_value,contactid");
        var incidents = new Service<Incident>(client, "incidents", "?$select=title,description,prioritycode,_customerid_value,incidentid");

        //Give http client token from authorised user to allow modification to the customer service hub app
        string authToken = await Authorisation.Authorise(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        //Instruct api to return data after post and patch calls
        client.DefaultRequestHeaders.Add("Prefer", "return=representation");

        await Demo.Run(accounts, contacts, incidents);
    }
}