using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models
{
    public class User
    { 
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }
         
        [Required]
        public string Correo { get; set; }

        [Required]
        public string Contrasena { get; set; }

        public string Rol { get; set; } = "cliente";
    }
}
   