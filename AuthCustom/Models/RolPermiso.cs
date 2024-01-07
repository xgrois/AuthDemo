using System.ComponentModel.DataAnnotations.Schema;

namespace AuthCustom.Models
{
    public class RolPermiso
    {
        [Column("PkFkRoles")]
        public int IdRoles { get; set; }
        [Column("PkFkPermisos")]
        public int IdPermisos { get; set; }

        // NAVIGATION PROPERTIES
        public Rol Rol { get; set; }
        public Permiso Permiso { get; set; }
    }
}
