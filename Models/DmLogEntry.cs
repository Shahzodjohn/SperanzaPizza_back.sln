﻿using System;
using System.Collections.Generic;

#nullable disable

namespace SperanzaPizzaApi.Models
{
    public partial class DmLogEntry
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public DateTime TimeFinish { get; set; }
        public int AuthorizedUser { get; set; }
        public string EntityName { get; set; }
        public int? EntityId { get; set; }
        public string ProxyId { get; set; }
        public string MessageId { get; set; }
        public byte ActionType { get; set; }
        public bool Success { get; set; }
        public string Info { get; set; }
    }
}
