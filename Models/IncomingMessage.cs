using System;
using System.Collections.Generic;

#nullable disable

namespace SperanzaPizzaApi.Models
{
    public partial class IncomingMessage
    {
        public int Id { get; set; }
        public string MessageId { get; set; }
        public string ParentId { get; set; }
        public string Author { get; set; }
        public string Payload { get; set; }
        public DateTime CreateDate { get; set; }
        public string Comment { get; set; }
        public bool Offline { get; set; }
        public byte DeliveryState { get; set; }
    }
}
