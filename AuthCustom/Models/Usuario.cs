using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthCustom.Models
{
    public class Usuario
    {
        [Key]
        [Column("PkUsuarios")]
        public int IdUsuarios { get; set; }
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Password { get; set; }

        // NAVIGATION PROPERTIES
        public List<UsuarioRol> UsuariosRoles { get; set; } = new();
    }
}
