using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirTicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class AgregarReservaYTiqueteEnDisponibilidadAsientos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReservaId1",
                table: "disponibilidad_asientos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TiqueteId1",
                table: "disponibilidad_asientos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "reserva_id",
                table: "disponibilidad_asientos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tiquete_id",
                table: "disponibilidad_asientos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "precio_base",
                table: "clases_servicio",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_disponibilidad_asientos_reserva_id",
                table: "disponibilidad_asientos",
                column: "reserva_id");

            migrationBuilder.CreateIndex(
                name: "IX_disponibilidad_asientos_ReservaId1",
                table: "disponibilidad_asientos",
                column: "ReservaId1");

            migrationBuilder.CreateIndex(
                name: "IX_disponibilidad_asientos_tiquete_id",
                table: "disponibilidad_asientos",
                column: "tiquete_id");

            migrationBuilder.CreateIndex(
                name: "IX_disponibilidad_asientos_TiqueteId1",
                table: "disponibilidad_asientos",
                column: "TiqueteId1");

            migrationBuilder.AddForeignKey(
                name: "FK_disponibilidad_asientos_reservas_ReservaId1",
                table: "disponibilidad_asientos",
                column: "ReservaId1",
                principalTable: "reservas",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_disponibilidad_asientos_reservas_reserva_id",
                table: "disponibilidad_asientos",
                column: "reserva_id",
                principalTable: "reservas",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_disponibilidad_asientos_tiquetes_TiqueteId1",
                table: "disponibilidad_asientos",
                column: "TiqueteId1",
                principalTable: "tiquetes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_disponibilidad_asientos_tiquetes_tiquete_id",
                table: "disponibilidad_asientos",
                column: "tiquete_id",
                principalTable: "tiquetes",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_disponibilidad_asientos_reservas_ReservaId1",
                table: "disponibilidad_asientos");

            migrationBuilder.DropForeignKey(
                name: "FK_disponibilidad_asientos_reservas_reserva_id",
                table: "disponibilidad_asientos");

            migrationBuilder.DropForeignKey(
                name: "FK_disponibilidad_asientos_tiquetes_TiqueteId1",
                table: "disponibilidad_asientos");

            migrationBuilder.DropForeignKey(
                name: "FK_disponibilidad_asientos_tiquetes_tiquete_id",
                table: "disponibilidad_asientos");

            migrationBuilder.DropIndex(
                name: "IX_disponibilidad_asientos_reserva_id",
                table: "disponibilidad_asientos");

            migrationBuilder.DropIndex(
                name: "IX_disponibilidad_asientos_ReservaId1",
                table: "disponibilidad_asientos");

            migrationBuilder.DropIndex(
                name: "IX_disponibilidad_asientos_tiquete_id",
                table: "disponibilidad_asientos");

            migrationBuilder.DropIndex(
                name: "IX_disponibilidad_asientos_TiqueteId1",
                table: "disponibilidad_asientos");

            migrationBuilder.DropColumn(
                name: "ReservaId1",
                table: "disponibilidad_asientos");

            migrationBuilder.DropColumn(
                name: "TiqueteId1",
                table: "disponibilidad_asientos");

            migrationBuilder.DropColumn(
                name: "reserva_id",
                table: "disponibilidad_asientos");

            migrationBuilder.DropColumn(
                name: "tiquete_id",
                table: "disponibilidad_asientos");

            migrationBuilder.DropColumn(
                name: "precio_base",
                table: "clases_servicio");
        }
    }
}
