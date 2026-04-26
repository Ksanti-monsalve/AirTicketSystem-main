using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirTicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddFkClaseVueloIdEnDisponibilidadAsientos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_disponibilidad_asientos_clase_vuelo_id",
                table: "disponibilidad_asientos",
                column: "clase_vuelo_id");

            migrationBuilder.AddForeignKey(
                name: "FK_disponibilidad_asientos_clases_servicio_clase_vuelo_id",
                table: "disponibilidad_asientos",
                column: "clase_vuelo_id",
                principalTable: "clases_servicio",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_disponibilidad_asientos_clases_servicio_clase_vuelo_id",
                table: "disponibilidad_asientos");

            migrationBuilder.DropIndex(
                name: "IX_disponibilidad_asientos_clase_vuelo_id",
                table: "disponibilidad_asientos");
        }
    }
}
