using System;
using System.Collections.Generic;

#nullable disable

namespace SperanzaPizzaApi.Models
{
    public partial class MessagesToSend
    {
        public int Id { get; set; }
        public string Destination { get; set; }
        public string Payload { get; set; }
    }
}
