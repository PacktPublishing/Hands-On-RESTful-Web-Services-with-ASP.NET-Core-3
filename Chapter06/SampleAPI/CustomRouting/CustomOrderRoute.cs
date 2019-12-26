using System;
using Microsoft.AspNetCore.Mvc.Routing;

namespace SampleAPI.CustomRouting
{
    public class CustomOrdersRoute : Attribute, IRouteTemplateProvider
    {
        public string Template => "api/orders";

        public int? Order { get; set; }

        public string Name => "Orders_route";
    }
}