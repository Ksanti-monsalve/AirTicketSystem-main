using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirTicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixModeloAsientoExamen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_disponibilidad_asientos_reservas_ReservaId1",
                table: "disponibilidad_asientos");

            migrationBuilder.DropForeignKey(
                name: "FK_disponibilidad_asientos_tiquetes_TiqueteId1",
                table: "disponibilidad_asientos");

            migrationBuilder.DropIndex(
                name: "IX_disponibilidad_asientos_ReservaId1",
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

            // Agregar columnas como NULL primero para poder backfill sin romper datos existentes
            migrationBuilder.AddColumn<int>(
                name: "clase_vuelo_id",
                table: "disponibilidad_asientos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "numero_asiento",
                table: "disponibilidad_asientos",
                type: "varchar(5)",
                maxLength: 5,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            // Backfill: tomar el número de asiento y clase desde asientos_avion
            migrationBuilder.Sql(@"
UPDATE disponibilidad_asientos d
JOIN asientos_avion a ON a.id = d.asiento_id
SET d.numero_asiento = a.codigo_asiento,
    d.clase_vuelo_id = a.clase_servicio_id
WHERE d.numero_asiento IS NULL OR d.numero_asiento = '' OR d.clase_vuelo_id IS NULL OR d.clase_vuelo_id = 0;
");

            // Convertir a NOT NULL ya con datos consistentes
            migrationBuilder.AlterColumn<int>(
                name: "clase_vuelo_id",
                table: "disponibilidad_asientos",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "numero_asiento",
                table: "disponibilidad_asientos",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldMaxLength: 5,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_disponibilidad_asientos_vuelo_id_numero_asiento",
                table: "disponibilidad_asientos",
                columns: new[] { "vuelo_id", "numero_asiento" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_disponibilidad_asientos_vuelo_id_numero_asiento",
                table: "disponibilidad_asientos");

            migrationBuilder.DropColumn(
                name: "clase_vuelo_id",
                table: "disponibilidad_asientos");

            migrationBuilder.DropColumn(
                name: "numero_asiento",
                table: "disponibilidad_asientos");

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

            migrationBuilder.CreateIndex(
                name: "IX_disponibilidad_asientos_ReservaId1",
                table: "disponibilidad_asientos",
                column: "ReservaId1");

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
                name: "FK_disponibilidad_asientos_tiquetes_TiqueteId1",
                table: "disponibilidad_asientos",
                column: "TiqueteId1",
                principalTable: "tiquetes",
                principalColumn: "id");
        }
    }
}
