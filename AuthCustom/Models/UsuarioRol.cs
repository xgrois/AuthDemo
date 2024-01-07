using System.ComponentModel.DataAnnotations.Schema;

namespace AuthCustom.Models
{
    public class UsuarioRol
    {
        [Column("PkFkUsuarios")]
        public int IdUsuarios { get; set; }
        [Column("PkFkRoles")]
        public int IdRoles { get; set; }

        // NAVIGATION PROPERTIES
        public Usuario Usuario { get; set; }
        public Rol Rol { get; set; }
    }
}
