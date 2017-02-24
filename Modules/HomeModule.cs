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
        }

    }
  }
}
