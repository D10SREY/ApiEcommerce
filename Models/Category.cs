using System.ComponentModel.DataAnnotations;

public class Category
{
    [Key]
        public int Id { get; set; }
    [Required]    
        
        public String Name { get; set; } = String.Empty;
    [Required]    
        public DateTime CreationDate { get; set; }
}