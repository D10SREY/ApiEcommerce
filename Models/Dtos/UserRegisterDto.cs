using System;

namespace ApiEcommerce.Models.Dtos;

public class UserRegisterDto
{

     public String? Id { get; set; }
    public string? Name { get; set; } 
    public  required String UserName { get; set; }
    public required string Password { get; set; }
    public String?  Role { get; set; } 
}
