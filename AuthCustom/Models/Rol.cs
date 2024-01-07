using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthCustom.Models
{
    public class Rol
    {
        [Key]
        [Column("PkRoles")]
        public int IdRoles { get; set; }
        [Required]
        public required string Nombre { get; set; }

        // NAVIGATION PROPERTIES
        public List<UsuarioRol> UsuariosRoles { get; set; } = new();
        public List<RolPermiso> RolesPermisos { get; set; } = new();

        // NOT MAPPED
        [NotMapped]
        public bool Asignado { get; set; }
    }
}

