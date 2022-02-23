using System;
using System.Collections.Generic;

#nullable disable

namespace SperanzaPizzaApi.Models
{
    public partial class Structure
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public bool? IsNullable { get; set; }
    }
}
