using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthCustom.Models
{
    public class Permiso
    {
        [Key]
        [Column("PkPermisos")]
        public int IdPermisos { get; set; }
        [Required]
        public required string Nombre { get; set; }

        // NAVIGATION PROPERTIES
        public List<RolPermiso> RolesPermisos { get; set; } = new();

        // NOT MAPPED
        [NotMapped]
        public bool Asignado { get; set; }
    }
}
