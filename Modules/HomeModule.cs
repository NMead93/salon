using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using Salon.Objects;
using System.Linq;

namespace Salon
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {

        Get["/"] =_=>
        {
            return View["index.cshtml"];
        };

        Get["/stylists"] =_=>
        {
            List<Stylist> stylistList = Stylist.GetAll();
            return View["stylist-home.cshtml", stylistList];
        };

        Post["/stylists"] =_=>
        {
            Stylist newStylist = new Stylist(Request.Form["name"]);
            newStylist.Save();
            List<Stylist> stylistList = Stylist.GetAll();
            return View["stylist-home.cshtml", stylistList];
        };

        Delete["/stylists/{id}/delete"] =parameter=>
        {
            Stylist foundStylist = Stylist.Find(parameter.id);
            List<Client> clientList = foundStylist.GetClients();
            foreach (var client in clientList)
            {
                client.Delete();
            }
            foundStylist.Delete();
            return View["delete-confirmation.cshtml", foundStylist];
        };

        Get["/stylists/{id}/clients"] =parameter=>
        {
            Stylist foundStylist = Stylist.Find(parameter.id);
            Client id = new Client("id", parameter.id);
            List<Client> idList = new List<Client>{id};
            List<Client> clientList = foundStylist.GetClients();
            Dictionary<string, List<Client>> listAndId = new Dictionary<string, List<Client>>{};
            listAndId.Add("id", idList);
            listAndId.Add("clients", clientList);
            return View["clients-of-stylist.cshtml", listAndId];
        };

        Post["/stylists/{id}/clients"] =parameter=>
        {
            Client newClient = new Client(Request.Form["name"], parameter.id);
            newClient.Save();
            Stylist foundStylist = Stylist.Find(parameter.id);
            Client id = new Client("id", parameter.id);
            List<Client> idList = new List<Client>{id};
            List<Client> clientList = foundStylist.GetClients();
            Dictionary<string, List<Client>> listAndId = new Dictionary<string, List<Client>>{};
            listAndId.Add("id", idList);
            listAndId.Add("clients", clientList);
            return View["clients-of-stylist.cshtml", listAndId];
        };

        Patch["/stylists/{id}/clients/{clientId}/update"] =parameter=>
        {
            Client foundClient = Client.Find(parameter.clientId);
            foundClient.Update(Request.Form["new-name"]);
            return View["update-confirmation-client", foundClient];
        };

        Delete["/stylists/{id}/clients/{clientId}/delete"] =parameter=>
        {
            Client foundClient = Client.Find(parameter.clientId);
            foundClient.Delete();
            return View["delete-confirmation-client.cshtml", foundClient];
        };

    }
  }
}
