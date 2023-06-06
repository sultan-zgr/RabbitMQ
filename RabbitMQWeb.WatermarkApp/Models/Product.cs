﻿using System.ComponentModel.DataAnnotations;

namespace RabbitMQWeb.WatermarkApp.Models
{
    public class Product
    {
     [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageName { get; set; }
    }
}
