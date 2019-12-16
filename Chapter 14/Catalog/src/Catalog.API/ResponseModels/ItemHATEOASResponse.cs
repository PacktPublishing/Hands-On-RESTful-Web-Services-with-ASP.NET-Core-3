using System.Collections.Generic;
using Catalog.Domain.Responses;
using Newtonsoft.Json;
using RiskFirst.Hateoas.Models;

namespace Catalog.API.ResponseModels
{
    public class ItemHateoasResponse : ILinkContainer
    {
        private Dictionary<string, Link> _links;
        public ItemResponse Data;

        [JsonProperty(PropertyName = "_links")]
        public Dictionary<string, Link> Links
        {
            get => _links ??= new Dictionary<string, Link>();
            set => _links = value;
        }

        public void AddLink(string id, Link link)
        {
            Links.Add(id, link);
        }
    }
}