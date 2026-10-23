using System;
using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models.Dtos;

public class CreateProductDto
{
    [Required(ErrorMessage =("El nombre es obligatorio"))]
    [MaxLength(50,ErrorMessage =("EL nombre no debe de tener mas de 50 caracteres"))]
    [MinLength(3,ErrorMessage = ("El nombre debed de tener mas de 3 caracteres"))]
    public String Name { get; set; } = string.Empty;

    // Agregamos la descripción (no es obligatoria, así que no lleva [Required])
    public string Description { get; set; } = string.Empty;

    [Range(0,double.MaxValue,ErrorMessage =("EL valor debe de ser positivo sapo"))]
    public decimal Price { get; set; }

    // Agregamos la URL de la imagen
    public string ImageUrl { get; set; } = string.Empty;

    // El SKU (Código de barras/almacén) sí suele ser obligatorio
    [Required(ErrorMessage = "El código SKU es obligatorio")]
    public string Sku { get; set; } = string.Empty;

    // Protegemos el stock para que no te envíen inventario negativo
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; set; }

    // ¡SÚPER IMPORTANTE! Necesitamos saber a qué categoría pertenece
    public int CategoryId { get; set; }
}