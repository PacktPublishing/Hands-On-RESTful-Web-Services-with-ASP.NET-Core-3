using System;
using System.Collections.Generic;

namespace SampleAPI.Models
 {
  public class Order
     {
         public Guid Id { get; set; }
         public IList<string> ItemsIds { get; set; }

         public string Currency { get; set; }
         
         public bool IsInactive { get; set; }
     }
 }