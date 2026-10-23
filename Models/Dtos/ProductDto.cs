using System;

namespace ApiEcommerce.Models.Dtos;

public class ProductDto
{
        public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
     public string Description  { get; set; } = string.Empty;
     
     
     public decimal Price { get; set; }
     public string ImageUrl { get; set; } = string.Empty;
     
     public string Sku { get; set; } = string.Empty;// product 001-BLK-M
     
     public int Stock{ get; set; }
     public DateTime CreationDate { get; set; } = DateTime.Now;
     public DateTime? UpdateDate { get; set; } = null;
     // la relacion con el modelo category

     public int CategoryId { get; set; }
     public string CategoryName { get; set; } = string.Empty;
}
