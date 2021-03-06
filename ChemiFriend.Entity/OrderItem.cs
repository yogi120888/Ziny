﻿using ChemiFriend.ENTITY;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Entity
{
    public class OrderItem : BaseEntity
    {
        [Key]
        public Int64 OrderItemId { get; set; }
        public Int64 OrderId { get; set; }
        //[ForeignKey("OrderId")]
        //public OrderItem Orders { get; set; }
        public Int64 UserId { get; set; } // Customer Id
        public Int64 DealId { get; set; }
        public Int64 ProductId { get; set; }
        public Int64 SchemeId { get; set; }
        public int OrderItemQuantity { get; set; }
        public decimal OrderItemUnitPrice { get; set; }
        public string OrderItemSchemes { get; set; }
    }
}
