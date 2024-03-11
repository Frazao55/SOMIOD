using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Middleware.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Feature { get; set; }
        public int Parent { get; set; }
        public char Event { get; set; }
        public string Endpoint { get; set; }
        public DateTime CreationDT { get; set; }
    }
}