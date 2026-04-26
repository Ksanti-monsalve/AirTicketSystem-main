using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirTicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddFlightClassAndSeatTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "flight_classes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flight_classes", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seats",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    flight_id = table.Column<int>(type: "int", nullable: false),
                    seat_number = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    flight_class_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, defaultValue: "Available")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    booking_id = table.Column<int>(type: "int", nullable: true),
                    ticket_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seats", x => x.id);
                    table.CheckConstraint("chk_seat_status", "status IN ('Available','Reserved','Occupied','Blocked')");
                    table.ForeignKey(
                        name: "FK_seats_flight_classes_flight_class_id",
                        column: x => x.flight_class_id,
                        principalTable: "flight_classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_seats_reservas_booking_id",
                        column: x => x.booking_id,
                        principalTable: "reservas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_seats_tiquetes_ticket_id",
                        column: x => x.ticket_id,
                        principalTable: "tiquetes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_seats_vuelos_flight_id",
                        column: x => x.flight_id,
                        principalTable: "vuelos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // Backfill requerido por el examen: crear FlightClass (inglés) y Seats (por vuelo)
            migrationBuilder.Sql(@"
INSERT INTO flight_classes (name, code, description)
SELECT * FROM (
    SELECT 'Economy'    AS name, 'ECO' AS code, 'Economy class'      AS description
    UNION ALL
    SELECT 'Business'   AS name, 'BUS' AS code, 'Business class'     AS description
    UNION ALL
    SELECT 'FirstClass' AS name, 'FST' AS code, 'First class'        AS description
) x
WHERE NOT EXISTS (SELECT 1 FROM flight_classes fc WHERE fc.code = x.code);
");

            // Generar Seats desde disponibilidad_asientos existente (si ya hay datos)
            migrationBuilder.Sql(@"
INSERT IGNORE INTO seats (flight_id, seat_number, flight_class_id, status, booking_id, ticket_id)
SELECT
    d.vuelo_id AS flight_id,
    d.numero_asiento AS seat_number,
    (
        SELECT fc.id
        FROM flight_classes fc
        WHERE fc.code = (
            CASE cs.codigo
                WHEN 'ECO' THEN 'ECO'
                WHEN 'EJE' THEN 'BUS'
                WHEN 'PRC' THEN 'FST'
                ELSE 'ECO'
            END
        )
        LIMIT 1
    ) AS flight_class_id,
    (
        CASE d.estado
            WHEN 'DISPONIBLE' THEN 'Available'
            WHEN 'RESERVADO'  THEN 'Reserved'
            WHEN 'OCUPADO'    THEN 'Occupied'
            WHEN 'BLOQUEADO'  THEN 'Blocked'
            ELSE 'Available'
        END
    ) AS status,
    d.reserva_id AS booking_id,
    d.tiquete_id AS ticket_id
FROM disponibilidad_asientos d
LEFT JOIN clases_servicio cs ON cs.id = d.clase_vuelo_id
WHERE d.numero_asiento IS NOT NULL AND d.numero_asiento <> '';
");

            migrationBuilder.CreateIndex(
                name: "IX_flight_classes_code",
                table: "flight_classes",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_flight_classes_name",
                table: "flight_classes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_seats_booking_id",
                table: "seats",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_seats_flight_class_id",
                table: "seats",
                column: "flight_class_id");

            migrationBuilder.CreateIndex(
                name: "IX_seats_flight_id_seat_number",
                table: "seats",
                columns: new[] { "flight_id", "seat_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_seats_ticket_id",
                table: "seats",
                column: "ticket_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "seats");

            migrationBuilder.DropTable(
                name: "flight_classes");
        }
    }
}
