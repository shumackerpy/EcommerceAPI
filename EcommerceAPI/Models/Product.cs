using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public required string Nombre { get; set; }

        public required string Descripcion { get; set; }

        [Required]
        public decimal Precio { get; set; }

        public int Stock { get; set; }
    }
}