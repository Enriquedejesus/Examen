using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Roles
    {
        
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Rol { get; set; }

        public ICollection<Usuarios>? Usuarios { get; set; }
    }
}