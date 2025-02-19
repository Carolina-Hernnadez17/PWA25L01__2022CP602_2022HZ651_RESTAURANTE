using System.ComponentModel.DataAnnotations;

namespace L01_2022CP602_2022HZ651.Models
{
    public class clientes
    {
        [Key]
        public int clienteId { get; set; }
        public string nombreCliente { get; set; }
        public string direccion { get; set; }
    }
}
