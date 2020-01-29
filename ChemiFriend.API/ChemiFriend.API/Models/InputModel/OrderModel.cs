using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChemiFriend.API.Models.InputModel
{
    public class OrderModel
    {
        public Int64 OrderId { get; set; }
        [Required]
        public Int64 UserId { get; set; }
        [Required]
        public DateTime DateOrderPlaced { get; set; }
        public string OrderDetails { get; set; }
        public byte Status { get; set; } = 0;
        public bool IsDeleted { get; set; }
        public Int64 CreatedBy { get; set; } = 0;
        public DateTime CreatedDate { get; set; }
        public Int64? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}