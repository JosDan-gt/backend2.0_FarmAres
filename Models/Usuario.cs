using System;
using System.Collections.Generic;

namespace GranjaLosAres_API.Models
{
    public partial class Usuario
    {
        public int Id { get; set; }

        public string NombreUser { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Contrasena { get; set; } = null!;

        public int? RoleId { get; set; }

        public DateTime FechaDeRegistro { get; set; }

        public bool Estado { get; set; }

        // Relación con el modelo Role
        public virtual Role? Role { get; set; }

    }
}
