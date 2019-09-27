using System.Collections.Generic;

namespace SampleAPI.Requests
{
    public class OrderRequest
    {
        public IList<string> ItemsIds { get; set; }
        public string Currency { get; set; }
    }
}