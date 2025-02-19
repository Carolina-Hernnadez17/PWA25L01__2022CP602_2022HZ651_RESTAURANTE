using System.ComponentModel.DataAnnotations;

namespace L01_2022CP602_2022HZ651.Models
{
    public class motoristas
    {
        [Key]
        public int motoristaId { get; set; }
        public string nombreMotorista { get; set; }

    }
}
