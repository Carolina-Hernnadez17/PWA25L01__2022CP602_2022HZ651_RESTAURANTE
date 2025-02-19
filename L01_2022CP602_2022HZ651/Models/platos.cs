using System.ComponentModel.DataAnnotations;

namespace L01_2022CP602_2022HZ651.Models
{
    public class platos
    {

        
        [Key]
        public int platoId { get; set; }
        public string nombrePlato { get; set; }
        public decimal precio { get; set; }
    }
}
