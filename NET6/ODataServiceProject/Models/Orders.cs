using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ODataServiceProject.Models
{
    public partial class Orders
    {
        [Key]
        public long OrderId { get; set; }
        public string CustomerId { get; set; }
        public int? Freight { get; set; }
        public DateTime? OrderDate { get; set; }
    }
}
