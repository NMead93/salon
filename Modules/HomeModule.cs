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

    }
  }
}
