using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthCustom.Migrations
{
    /// <inheritdoc />
    public partial class ModUsuarioPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHashed",
                table: "Usuarios",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Usuarios",
                newName: "PasswordHashed");
        }
    }
}
