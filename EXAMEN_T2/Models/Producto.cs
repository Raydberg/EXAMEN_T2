using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EXAMEN_T2.Models
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }
        [MaxLength(100)]
        public string Nombre { get; set; }
        [MaxLength(100)]
        public string Marca { get; set; }
        [MaxLength(100)]
        public string Categoria { get; set; }
        [Precision(9, 2)]
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        [MaxLength(100)]
        public string Imagen { get; set; }
        public DateTime FechaCreacion { get; set; }

    }
}
