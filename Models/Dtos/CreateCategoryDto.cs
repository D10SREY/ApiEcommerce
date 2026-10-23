using System;
using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models.Dtos;

public class CreateCategoryDto
{
    [Required(ErrorMessage = ("el Nombre es obligatorio."))]
    [MaxLength(50,ErrorMessage ="El nombre no puede tener mas de 50 caracteres")]
      [MinLength(5,ErrorMessage ="El nombre no puede tener menos de 5 caracteres")]
        public String Name { get; set; } = String.Empty;   
       

}
