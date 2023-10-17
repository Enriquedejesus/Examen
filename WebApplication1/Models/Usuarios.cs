using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Usuarios
    {
        
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Nombre { get; set; }
        [MaxLength(100)]
        public string? Usuario { get; set; }
        [MaxLength(100)]
        public string? Password { get; set; }
        public string? Correo { get; set; }

        public int RolesId { get; set; }
        public Roles? Roles { get; set; }
    }
}