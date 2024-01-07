using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthCustom.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    PkPermisos = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisos", x => x.PkPermisos);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    PkRoles = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.PkRoles);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    PkUsuarios = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.PkUsuarios);
                });

            migrationBuilder.CreateTable(
                name: "RolesPermisos",
                columns: table => new
                {
                    PkFkRoles = table.Column<int>(type: "int", nullable: false),
                    PkFkPermisos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesPermisos", x => new { x.PkFkRoles, x.PkFkPermisos });
                    table.ForeignKey(
                        name: "FK_RolesPermisos_Permisos_PkFkPermisos",
                        column: x => x.PkFkPermisos,
                        principalTable: "Permisos",
                        principalColumn: "PkPermisos",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolesPermisos_Roles_PkFkRoles",
                        column: x => x.PkFkRoles,
                        principalTable: "Roles",
                        principalColumn: "PkRoles",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosRoles",
                columns: table => new
                {
                    PkFkUsuarios = table.Column<int>(type: "int", nullable: false),
                    PkFkRoles = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosRoles", x => new { x.PkFkUsuarios, x.PkFkRoles });
                    table.ForeignKey(
                        name: "FK_UsuariosRoles_Roles_PkFkRoles",
                        column: x => x.PkFkRoles,
                        principalTable: "Roles",
                        principalColumn: "PkRoles",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosRoles_Usuarios_PkFkUsuarios",
                        column: x => x.PkFkUsuarios,
                        principalTable: "Usuarios",
                        principalColumn: "PkUsuarios",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolesPermisos_PkFkPermisos",
                table: "RolesPermisos",
                column: "PkFkPermisos");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosRoles_PkFkRoles",
                table: "UsuariosRoles",
                column: "PkFkRoles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolesPermisos");

            migrationBuilder.DropTable(
                name: "UsuariosRoles");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
