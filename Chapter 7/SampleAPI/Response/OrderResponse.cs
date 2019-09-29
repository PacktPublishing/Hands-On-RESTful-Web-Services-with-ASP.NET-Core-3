using System;
using System.Collections.Generic;

namespace SampleAPI
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public IList<string> ItemsIds { get; set; }
        public string Currency { get; set; }
    }
}