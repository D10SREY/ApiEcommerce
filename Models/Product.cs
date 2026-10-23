using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace ApiEcommerce.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
     public string Description  { get; set; } = string.Empty;
     [Range(0,double.MaxValue)]
     [Column(TypeName = "decimal(18,    )")]
     public decimal Price { get; set; }
     public string ImageUrl { get; set; } = string.Empty;
     [Required]
     public string Sku { get; set; } = string.Empty;// product 001-BLK-M
     [Range(0,int.MaxValue)]
     public int Stock{ get; set; }
     public DateTime CreationDate { get; set; } = DateTime.Now;
     public DateTime? UpdateDate { get; set; } = null;
     // la relacion con el modelo category

     public int CategoryId { get; set; }
     [ForeignKey("CategoryId")]
     public  required Category category { get; set; }
}
