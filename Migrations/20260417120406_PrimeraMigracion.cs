using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirTicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class PrimeraMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clases_servicio",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo = table.Column<string>(type: "char(3)", fixedLength: true, maxLength: 3, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clases_servicio", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "continentes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo = table.Column<string>(type: "char(2)", fixedLength: true, maxLength: 2, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_continentes", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "generos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_generos", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metodos_pago",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metodos_pago", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "relaciones_contacto",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    descripcion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_relaciones_contacto", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tipos_direccion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    descripcion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipos_direccion", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tipos_documento",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    descripcion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipos_documento", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tipos_email",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    descripcion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipos_email", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tipos_equipaje",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipos_equipaje", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tipos_telefono",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    descripcion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipos_telefono", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tipos_trabajador",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipos_trabajador", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "paises",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    continente_id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo_iso2 = table.Column<string>(type: "char(2)", fixedLength: true, maxLength: 2, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo_iso3 = table.Column<string>(type: "char(3)", fixedLength: true, maxLength: 3, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paises", x => x.id);
                    table.ForeignKey(
                        name: "FK_paises_continentes_continente_id",
                        column: x => x.continente_id,
                        principalTable: "continentes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "especialidades",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_trabajador_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_especialidades", x => x.id);
                    table.ForeignKey(
                        name: "FK_especialidades_tipos_trabajador_tipo_trabajador_id",
                        column: x => x.tipo_trabajador_id,
                        principalTable: "tipos_trabajador",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "aerolineas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    codigo_iata = table.Column<string>(type: "char(2)", fixedLength: true, maxLength: 2, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo_icao = table.Column<string>(type: "char(3)", fixedLength: true, maxLength: 3, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nombre_comercial = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    pais_id = table.Column<int>(type: "int", nullable: false),
                    sitio_web = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    activa = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aerolineas", x => x.id);
                    table.ForeignKey(
                        name: "FK_aerolineas_paises_pais_id",
                        column: x => x.pais_id,
                        principalTable: "paises",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "fabricantes_avion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    pais_id = table.Column<int>(type: "int", nullable: false),
                    sitio_web = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fabricantes_avion", x => x.id);
                    table.ForeignKey(
                        name: "FK_fabricantes_avion_paises_pais_id",
                        column: x => x.pais_id,
                        principalTable: "paises",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "personas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tipo_doc_id = table.Column<int>(type: "int", nullable: false),
                    numero_doc = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nombres = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    apellidos = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_nacimiento = table.Column<DateOnly>(type: "date", nullable: true),
                    genero_id = table.Column<int>(type: "int", nullable: true),
                    nacionalidad_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personas", x => x.id);
                    table.ForeignKey(
                        name: "FK_personas_generos_genero_id",
                        column: x => x.genero_id,
                        principalTable: "generos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personas_paises_nacionalidad_id",
                        column: x => x.nacionalidad_id,
                        principalTable: "paises",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personas_tipos_documento_tipo_doc_id",
                        column: x => x.tipo_doc_id,
                        principalTable: "tipos_documento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "regiones",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pais_id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_regiones", x => x.id);
                    table.ForeignKey(
                        name: "FK_regiones_paises_pais_id",
                        column: x => x.pais_id,
                        principalTable: "paises",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "emails_aerolinea",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    aerolinea_id = table.Column<int>(type: "int", nullable: false),
                    tipo_email_id = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    es_principal = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emails_aerolinea", x => x.id);
                    table.ForeignKey(
                        name: "FK_emails_aerolinea_aerolineas_aerolinea_id",
                        column: x => x.aerolinea_id,
                        principalTable: "aerolineas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_emails_aerolinea_tipos_email_tipo_email_id",
                        column: x => x.tipo_email_id,
                        principalTable: "tipos_email",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "telefonos_aerolinea",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    aerolinea_id = table.Column<int>(type: "int", nullable: false),
                    tipo_telefono_id = table.Column<int>(type: "int", nullable: false),
                    numero = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    indicativo_pais = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    es_principal = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_telefonos_aerolinea", x => x.id);
                    table.ForeignKey(
                        name: "FK_telefonos_aerolinea_aerolineas_aerolinea_id",
                        column: x => x.aerolinea_id,
                        principalTable: "aerolineas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_telefonos_aerolinea_tipos_telefono_tipo_telefono_id",
                        column: x => x.tipo_telefono_id,
                        principalTable: "tipos_telefono",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "modelos_avion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fabricante_id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo_modelo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    autonomia_km = table.Column<int>(type: "int", nullable: true),
                    velocidad_crucero_kmh = table.Column<int>(type: "int", nullable: true),
                    descripcion = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_modelos_avion", x => x.id);
                    table.ForeignKey(
                        name: "FK_modelos_avion_fabricantes_avion_fabricante_id",
                        column: x => x.fabricante_id,
                        principalTable: "fabricantes_avion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "emails_persona",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    persona_id = table.Column<int>(type: "int", nullable: false),
                    tipo_email_id = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    es_principal = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emails_persona", x => x.id);
                    table.ForeignKey(
                        name: "FK_emails_persona_personas_persona_id",
                        column: x => x.persona_id,
                        principalTable: "personas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_emails_persona_tipos_email_tipo_email_id",
                        column: x => x.tipo_email_id,
                        principalTable: "tipos_email",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "telefonos_persona",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    persona_id = table.Column<int>(type: "int", nullable: false),
                    tipo_telefono_id = table.Column<int>(type: "int", nullable: false),
                    numero = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    indicativo_pais = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    es_principal = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_telefonos_persona", x => x.id);
                    table.ForeignKey(
                        name: "FK_telefonos_persona_personas_persona_id",
                        column: x => x.persona_id,
                        principalTable: "personas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_telefonos_persona_tipos_telefono_tipo_telefono_id",
                        column: x => x.tipo_telefono_id,
                        principalTable: "tipos_telefono",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    persona_id = table.Column<int>(type: "int", nullable: false),
                    rol_id = table.Column<int>(type: "int", nullable: false),
                    username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    fecha_registro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ultimo_login = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    intentos_fallidos = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id);
                    table.ForeignKey(
                        name: "FK_usuarios_personas_persona_id",
                        column: x => x.persona_id,
                        principalTable: "personas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_usuarios_roles_rol_id",
                        column: x => x.rol_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "departamentos_estados",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    region_id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departamentos_estados", x => x.id);
                    table.ForeignKey(
                        name: "FK_departamentos_estados_regiones_region_id",
                        column: x => x.region_id,
                        principalTable: "regiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "aviones",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    matricula = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    modelo_avion_id = table.Column<int>(type: "int", nullable: false),
                    aerolinea_id = table.Column<int>(type: "int", nullable: false),
                    fecha_fabricacion = table.Column<DateOnly>(type: "date", nullable: true),
                    fecha_ultimo_mantenimiento = table.Column<DateOnly>(type: "date", nullable: true),
                    fecha_proximo_mantenimiento = table.Column<DateOnly>(type: "date", nullable: true),
                    total_horas_vuelo = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    estado = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "DISPONIBLE")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aviones", x => x.id);
                    table.CheckConstraint("chk_estado_avion", "estado IN ('DISPONIBLE','EN_VUELO','MANTENIMIENTO','FUERA_DE_SERVICIO')");
                    table.ForeignKey(
                        name: "FK_aviones_aerolineas_aerolinea_id",
                        column: x => x.aerolinea_id,
                        principalTable: "aerolineas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_aviones_modelos_avion_modelo_avion_id",
                        column: x => x.modelo_avion_id,
                        principalTable: "modelos_avion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clientes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    persona_id = table.Column<int>(type: "int", nullable: false),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    fecha_registro = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.id);
                    table.ForeignKey(
                        name: "FK_clientes_personas_persona_id",
                        column: x => x.persona_id,
                        principalTable: "personas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_clientes_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "log_acceso",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    fecha_acceso = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    tipo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ip_address = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_acceso", x => x.id);
                    table.ForeignKey(
                        name: "FK_log_acceso_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ciudades",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    departamento_id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo_postal = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ciudades", x => x.id);
                    table.ForeignKey(
                        name: "FK_ciudades_departamentos_estados_departamento_id",
                        column: x => x.departamento_id,
                        principalTable: "departamentos_estados",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "asientos_avion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    avion_id = table.Column<int>(type: "int", nullable: false),
                    clase_servicio_id = table.Column<int>(type: "int", nullable: false),
                    codigo_asiento = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fila = table.Column<int>(type: "int", nullable: false),
                    columna = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    es_ventana = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    es_pasillo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asientos_avion", x => x.id);
                    table.ForeignKey(
                        name: "FK_asientos_avion_aviones_avion_id",
                        column: x => x.avion_id,
                        principalTable: "aviones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_asientos_avion_clases_servicio_clase_servicio_id",
                        column: x => x.clase_servicio_id,
                        principalTable: "clases_servicio",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "contactos_emergencia",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cliente_id = table.Column<int>(type: "int", nullable: false),
                    persona_id = table.Column<int>(type: "int", nullable: false),
                    relacion_id = table.Column<int>(type: "int", nullable: false),
                    es_principal = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contactos_emergencia", x => x.id);
                    table.ForeignKey(
                        name: "FK_contactos_emergencia_clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_contactos_emergencia_personas_persona_id",
                        column: x => x.persona_id,
                        principalTable: "personas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_contactos_emergencia_relaciones_contacto_relacion_id",
                        column: x => x.relacion_id,
                        principalTable: "relaciones_contacto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "aeropuertos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    codigo_iata = table.Column<string>(type: "char(3)", fixedLength: true, maxLength: 3, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo_icao = table.Column<string>(type: "char(4)", fixedLength: true, maxLength: 4, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nombre = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ciudad_id = table.Column<int>(type: "int", nullable: false),
                    direccion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aeropuertos", x => x.id);
                    table.ForeignKey(
                        name: "FK_aeropuertos_ciudades_ciudad_id",
                        column: x => x.ciudad_id,
                        principalTable: "ciudades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "direcciones_persona",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    persona_id = table.Column<int>(type: "int", nullable: false),
                    tipo_direccion_id = table.Column<int>(type: "int", nullable: false),
                    ciudad_id = table.Column<int>(type: "int", nullable: false),
                    direccion_linea1 = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    direccion_linea2 = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo_postal = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    es_principal = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_direcciones_persona", x => x.id);
                    table.ForeignKey(
                        name: "FK_direcciones_persona_ciudades_ciudad_id",
                        column: x => x.ciudad_id,
                        principalTable: "ciudades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_direcciones_persona_personas_persona_id",
                        column: x => x.persona_id,
                        principalTable: "personas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_direcciones_persona_tipos_direccion_tipo_direccion_id",
                        column: x => x.tipo_direccion_id,
                        principalTable: "tipos_direccion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rutas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    aerolinea_id = table.Column<int>(type: "int", nullable: false),
                    origen_id = table.Column<int>(type: "int", nullable: false),
                    destino_id = table.Column<int>(type: "int", nullable: false),
                    distancia_km = table.Column<int>(type: "int", nullable: true),
                    duracion_estimada_min = table.Column<int>(type: "int", nullable: true),
                    activa = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rutas", x => x.id);
                    table.CheckConstraint("chk_origen_destino_ruta", "origen_id <> destino_id");
                    table.ForeignKey(
                        name: "FK_rutas_aerolineas_aerolinea_id",
                        column: x => x.aerolinea_id,
                        principalTable: "aerolineas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_rutas_aeropuertos_destino_id",
                        column: x => x.destino_id,
                        principalTable: "aeropuertos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_rutas_aeropuertos_origen_id",
                        column: x => x.origen_id,
                        principalTable: "aeropuertos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "terminales",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    aeropuerto_id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_terminales", x => x.id);
                    table.ForeignKey(
                        name: "FK_terminales_aeropuertos_aeropuerto_id",
                        column: x => x.aeropuerto_id,
                        principalTable: "aeropuertos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "trabajadores",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    persona_id = table.Column<int>(type: "int", nullable: false),
                    tipo_trabajador_id = table.Column<int>(type: "int", nullable: false),
                    aerolinea_id = table.Column<int>(type: "int", nullable: true),
                    aeropuerto_base_id = table.Column<int>(type: "int", nullable: false),
                    fecha_contratacion = table.Column<DateOnly>(type: "date", nullable: false),
                    salario = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    usuario_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trabajadores", x => x.id);
                    table.ForeignKey(
                        name: "FK_trabajadores_aerolineas_aerolinea_id",
                        column: x => x.aerolinea_id,
                        principalTable: "aerolineas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trabajadores_aeropuertos_aeropuerto_base_id",
                        column: x => x.aeropuerto_base_id,
                        principalTable: "aeropuertos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trabajadores_personas_persona_id",
                        column: x => x.persona_id,
                        principalTable: "personas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trabajadores_tipos_trabajador_tipo_trabajador_id",
                        column: x => x.tipo_trabajador_id,
                        principalTable: "tipos_trabajador",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trabajadores_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tarifas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ruta_id = table.Column<int>(type: "int", nullable: false),
                    clase_servicio_id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    precio_base = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    impuestos = table.Column<decimal>(type: "decimal(12,2)", nullable: false, defaultValue: 0m),
                    precio_total = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    permite_cambios = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    permite_reembolso = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    activa = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    vigente_hasta = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tarifas", x => x.id);
                    table.ForeignKey(
                        name: "FK_tarifas_clases_servicio_clase_servicio_id",
                        column: x => x.clase_servicio_id,
                        principalTable: "clases_servicio",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tarifas_rutas_ruta_id",
                        column: x => x.ruta_id,
                        principalTable: "rutas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "puertas_embarque",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    terminal_id = table.Column<int>(type: "int", nullable: false),
                    codigo = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    activa = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_puertas_embarque", x => x.id);
                    table.ForeignKey(
                        name: "FK_puertas_embarque_terminales_terminal_id",
                        column: x => x.terminal_id,
                        principalTable: "terminales",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "licencias_piloto",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    trabajador_id = table.Column<int>(type: "int", nullable: false),
                    numero_licencia = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_licencia = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_expedicion = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_vencimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    autoridad_emisora = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    activa = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_licencias_piloto", x => x.id);
                    table.CheckConstraint("chk_tipo_licencia", "tipo_licencia IN ('PPL','CPL','ATPL')");
                    table.ForeignKey(
                        name: "FK_licencias_piloto_trabajadores_trabajador_id",
                        column: x => x.trabajador_id,
                        principalTable: "trabajadores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "trabajador_especialidades",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    trabajador_id = table.Column<int>(type: "int", nullable: false),
                    especialidad_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trabajador_especialidades", x => x.id);
                    table.ForeignKey(
                        name: "FK_trabajador_especialidades_especialidades_especialidad_id",
                        column: x => x.especialidad_id,
                        principalTable: "especialidades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trabajador_especialidades_trabajadores_trabajador_id",
                        column: x => x.trabajador_id,
                        principalTable: "trabajadores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "restricciones_equipaje",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tarifa_id = table.Column<int>(type: "int", nullable: false),
                    tipo_equipaje_id = table.Column<int>(type: "int", nullable: false),
                    piezas_incluidas = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    peso_maximo_kg = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    largo_max_cm = table.Column<int>(type: "int", nullable: true),
                    ancho_max_cm = table.Column<int>(type: "int", nullable: true),
                    alto_max_cm = table.Column<int>(type: "int", nullable: true),
                    costo_exceso_kg = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_restricciones_equipaje", x => x.id);
                    table.ForeignKey(
                        name: "FK_restricciones_equipaje_tarifas_tarifa_id",
                        column: x => x.tarifa_id,
                        principalTable: "tarifas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_restricciones_equipaje_tipos_equipaje_tipo_equipaje_id",
                        column: x => x.tipo_equipaje_id,
                        principalTable: "tipos_equipaje",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vuelos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    numero_vuelo = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ruta_id = table.Column<int>(type: "int", nullable: false),
                    avion_id = table.Column<int>(type: "int", nullable: false),
                    puerta_embarque_id = table.Column<int>(type: "int", nullable: true),
                    fecha_salida = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_llegada_estimada = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_llegada_real = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    estado = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "PROGRAMADO")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    motivo_cambio_estado = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    checkin_apertura = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    checkin_cierre = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vuelos", x => x.id);
                    table.CheckConstraint("chk_estado_vuelo", "estado IN ('PROGRAMADO','ABORDANDO','EN_VUELO','ATERRIZADO','CANCELADO','DEMORADO','DESVIADO')");
                    table.ForeignKey(
                        name: "FK_vuelos_aviones_avion_id",
                        column: x => x.avion_id,
                        principalTable: "aviones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vuelos_puertas_embarque_puerta_embarque_id",
                        column: x => x.puerta_embarque_id,
                        principalTable: "puertas_embarque",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_vuelos_rutas_ruta_id",
                        column: x => x.ruta_id,
                        principalTable: "rutas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "habilitaciones_piloto",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    licencia_id = table.Column<int>(type: "int", nullable: false),
                    modelo_avion_id = table.Column<int>(type: "int", nullable: false),
                    fecha_habilitacion = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_vencimiento = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_habilitaciones_piloto", x => x.id);
                    table.ForeignKey(
                        name: "FK_habilitaciones_piloto_licencias_piloto_licencia_id",
                        column: x => x.licencia_id,
                        principalTable: "licencias_piloto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_habilitaciones_piloto_modelos_avion_modelo_avion_id",
                        column: x => x.modelo_avion_id,
                        principalTable: "modelos_avion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "disponibilidad_asientos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    vuelo_id = table.Column<int>(type: "int", nullable: false),
                    asiento_id = table.Column<int>(type: "int", nullable: false),
                    estado = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, defaultValue: "DISPONIBLE")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_disponibilidad_asientos", x => x.id);
                    table.CheckConstraint("chk_estado_asiento", "estado IN ('DISPONIBLE','RESERVADO','OCUPADO','BLOQUEADO')");
                    table.ForeignKey(
                        name: "FK_disponibilidad_asientos_asientos_avion_asiento_id",
                        column: x => x.asiento_id,
                        principalTable: "asientos_avion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_disponibilidad_asientos_vuelos_vuelo_id",
                        column: x => x.vuelo_id,
                        principalTable: "vuelos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "historial_vuelo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    vuelo_id = table.Column<int>(type: "int", nullable: false),
                    estado_anterior = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estado_nuevo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_cambio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    usuario_id = table.Column<int>(type: "int", nullable: true),
                    motivo = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historial_vuelo", x => x.id);
                    table.ForeignKey(
                        name: "FK_historial_vuelo_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_historial_vuelo_vuelos_vuelo_id",
                        column: x => x.vuelo_id,
                        principalTable: "vuelos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "reservas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cliente_id = table.Column<int>(type: "int", nullable: false),
                    vuelo_id = table.Column<int>(type: "int", nullable: false),
                    tarifa_id = table.Column<int>(type: "int", nullable: false),
                    codigo_reserva = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_reserva = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_expiracion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    estado = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, defaultValue: "PENDIENTE")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    valor_total = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    observaciones = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservas", x => x.id);
                    table.CheckConstraint("chk_estado_reserva", "estado IN ('PENDIENTE','CONFIRMADA','CANCELADA','EXPIRADA')");
                    table.ForeignKey(
                        name: "FK_reservas_clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservas_tarifas_tarifa_id",
                        column: x => x.tarifa_id,
                        principalTable: "tarifas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservas_vuelos_vuelo_id",
                        column: x => x.vuelo_id,
                        principalTable: "vuelos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tripulacion_vuelo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    vuelo_id = table.Column<int>(type: "int", nullable: false),
                    trabajador_id = table.Column<int>(type: "int", nullable: false),
                    rol_en_vuelo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tripulacion_vuelo", x => x.id);
                    table.CheckConstraint("chk_rol_vuelo", "rol_en_vuelo IN ('PILOTO','COPILOTO','SOBRECARGO','AUXILIAR_VUELO','AUXILIAR_SEGURIDAD')");
                    table.ForeignKey(
                        name: "FK_tripulacion_vuelo_trabajadores_trabajador_id",
                        column: x => x.trabajador_id,
                        principalTable: "trabajadores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tripulacion_vuelo_vuelos_vuelo_id",
                        column: x => x.vuelo_id,
                        principalTable: "vuelos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cargos_adicionales",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    reserva_id = table.Column<int>(type: "int", nullable: false),
                    concepto = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    monto = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cargos_adicionales", x => x.id);
                    table.ForeignKey(
                        name: "FK_cargos_adicionales_reservas_reserva_id",
                        column: x => x.reserva_id,
                        principalTable: "reservas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "facturas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    reserva_id = table.Column<int>(type: "int", nullable: false),
                    numero_factura = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_emision = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    direccion_facturacion_id = table.Column<int>(type: "int", nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    impuestos = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    total = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_facturas", x => x.id);
                    table.ForeignKey(
                        name: "FK_facturas_direcciones_persona_direccion_facturacion_id",
                        column: x => x.direccion_facturacion_id,
                        principalTable: "direcciones_persona",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_facturas_reservas_reserva_id",
                        column: x => x.reserva_id,
                        principalTable: "reservas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "historial_reserva",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    reserva_id = table.Column<int>(type: "int", nullable: false),
                    estado_anterior = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estado_nuevo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_cambio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    usuario_id = table.Column<int>(type: "int", nullable: true),
                    motivo = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historial_reserva", x => x.id);
                    table.ForeignKey(
                        name: "FK_historial_reserva_reservas_reserva_id",
                        column: x => x.reserva_id,
                        principalTable: "reservas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_historial_reserva_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pagos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    reserva_id = table.Column<int>(type: "int", nullable: false),
                    metodo_pago_id = table.Column<int>(type: "int", nullable: false),
                    monto = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    estado = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false, defaultValue: "PENDIENTE")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    referencia_pago = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_pago = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    fecha_vencimiento = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pagos", x => x.id);
                    table.CheckConstraint("chk_estado_pago", "estado IN ('PENDIENTE','APROBADO','RECHAZADO','REEMBOLSADO')");
                    table.ForeignKey(
                        name: "FK_pagos_metodos_pago_metodo_pago_id",
                        column: x => x.metodo_pago_id,
                        principalTable: "metodos_pago",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pagos_reservas_reserva_id",
                        column: x => x.reserva_id,
                        principalTable: "reservas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pasajeros_reserva",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    reserva_id = table.Column<int>(type: "int", nullable: false),
                    persona_id = table.Column<int>(type: "int", nullable: false),
                    tipo_pasajero = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, defaultValue: "ADULTO")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    asiento_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pasajeros_reserva", x => x.id);
                    table.CheckConstraint("chk_tipo_pasajero", "tipo_pasajero IN ('ADULTO','MENOR','INFANTE')");
                    table.ForeignKey(
                        name: "FK_pasajeros_reserva_disponibilidad_asientos_asiento_id",
                        column: x => x.asiento_id,
                        principalTable: "disponibilidad_asientos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_pasajeros_reserva_personas_persona_id",
                        column: x => x.persona_id,
                        principalTable: "personas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pasajeros_reserva_reservas_reserva_id",
                        column: x => x.reserva_id,
                        principalTable: "reservas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "checkin",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pasajero_reserva_id = table.Column<int>(type: "int", nullable: false),
                    tipo = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_checkin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    trabajador_id = table.Column<int>(type: "int", nullable: true),
                    estado = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, defaultValue: "PENDIENTE")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkin", x => x.id);
                    table.CheckConstraint("chk_estado_checkin", "estado IN ('COMPLETADO','PENDIENTE','CANCELADO')");
                    table.CheckConstraint("chk_tipo_checkin", "tipo IN ('VIRTUAL','PRESENCIAL')");
                    table.ForeignKey(
                        name: "FK_checkin_pasajeros_reserva_pasajero_reserva_id",
                        column: x => x.pasajero_reserva_id,
                        principalTable: "pasajeros_reserva",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_checkin_trabajadores_trabajador_id",
                        column: x => x.trabajador_id,
                        principalTable: "trabajadores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "equipaje_registrado",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pasajero_reserva_id = table.Column<int>(type: "int", nullable: false),
                    vuelo_id = table.Column<int>(type: "int", nullable: false),
                    tipo_equipaje_id = table.Column<int>(type: "int", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    peso_declarado_kg = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    largo_declarado_cm = table.Column<int>(type: "int", nullable: true),
                    ancho_declarado_cm = table.Column<int>(type: "int", nullable: true),
                    alto_declarado_cm = table.Column<int>(type: "int", nullable: true),
                    peso_real_kg = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    largo_real_cm = table.Column<int>(type: "int", nullable: true),
                    ancho_real_cm = table.Column<int>(type: "int", nullable: true),
                    alto_real_cm = table.Column<int>(type: "int", nullable: true),
                    codigo_equipaje = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    costo_adicional = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    estado = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, defaultValue: "DECLARADO")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipaje_registrado", x => x.id);
                    table.CheckConstraint("chk_estado_equipaje", "estado IN ('DECLARADO','REGISTRADO','EN_BODEGA','EN_DESTINO','ENTREGADO','PERDIDO','DAÑADO')");
                    table.ForeignKey(
                        name: "FK_equipaje_registrado_pasajeros_reserva_pasajero_reserva_id",
                        column: x => x.pasajero_reserva_id,
                        principalTable: "pasajeros_reserva",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_equipaje_registrado_tipos_equipaje_tipo_equipaje_id",
                        column: x => x.tipo_equipaje_id,
                        principalTable: "tipos_equipaje",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_equipaje_registrado_vuelos_vuelo_id",
                        column: x => x.vuelo_id,
                        principalTable: "vuelos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tiquetes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pasajero_reserva_id = table.Column<int>(type: "int", nullable: false),
                    codigo_tiquete = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    asiento_confirmado_id = table.Column<int>(type: "int", nullable: true),
                    fecha_emision = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    estado = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, defaultValue: "EMITIDO")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tiquetes", x => x.id);
                    table.CheckConstraint("chk_estado_tiquete", "estado IN ('EMITIDO','CHECKIN_HECHO','ABORDADO','USADO','ANULADO')");
                    table.ForeignKey(
                        name: "FK_tiquetes_asientos_avion_asiento_confirmado_id",
                        column: x => x.asiento_confirmado_id,
                        principalTable: "asientos_avion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_tiquetes_pasajeros_reserva_pasajero_reserva_id",
                        column: x => x.pasajero_reserva_id,
                        principalTable: "pasajeros_reserva",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pases_abordar",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    checkin_id = table.Column<int>(type: "int", nullable: false),
                    codigo_pase = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo_qr = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    puerta_embarque_id = table.Column<int>(type: "int", nullable: true),
                    hora_embarque = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    fecha_emision = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pases_abordar", x => x.id);
                    table.ForeignKey(
                        name: "FK_pases_abordar_checkin_checkin_id",
                        column: x => x.checkin_id,
                        principalTable: "checkin",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pases_abordar_puertas_embarque_puerta_embarque_id",
                        column: x => x.puerta_embarque_id,
                        principalTable: "puertas_embarque",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_aerolineas_codigo_iata",
                table: "aerolineas",
                column: "codigo_iata",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_aerolineas_codigo_icao",
                table: "aerolineas",
                column: "codigo_icao",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_aerolineas_pais_id",
                table: "aerolineas",
                column: "pais_id");

            migrationBuilder.CreateIndex(
                name: "IX_aeropuertos_ciudad_id",
                table: "aeropuertos",
                column: "ciudad_id");

            migrationBuilder.CreateIndex(
                name: "IX_aeropuertos_codigo_iata",
                table: "aeropuertos",
                column: "codigo_iata",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_aeropuertos_codigo_icao",
                table: "aeropuertos",
                column: "codigo_icao",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_asientos_avion_avion_id_codigo_asiento",
                table: "asientos_avion",
                columns: new[] { "avion_id", "codigo_asiento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_asientos_avion_clase_servicio_id",
                table: "asientos_avion",
                column: "clase_servicio_id");

            migrationBuilder.CreateIndex(
                name: "IX_aviones_aerolinea_id",
                table: "aviones",
                column: "aerolinea_id");

            migrationBuilder.CreateIndex(
                name: "IX_aviones_matricula",
                table: "aviones",
                column: "matricula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_aviones_modelo_avion_id",
                table: "aviones",
                column: "modelo_avion_id");

            migrationBuilder.CreateIndex(
                name: "IX_cargos_adicionales_reserva_id",
                table: "cargos_adicionales",
                column: "reserva_id");

            migrationBuilder.CreateIndex(
                name: "IX_checkin_pasajero_reserva_id",
                table: "checkin",
                column: "pasajero_reserva_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_checkin_trabajador_id",
                table: "checkin",
                column: "trabajador_id");

            migrationBuilder.CreateIndex(
                name: "IX_ciudades_departamento_id_nombre",
                table: "ciudades",
                columns: new[] { "departamento_id", "nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clases_servicio_codigo",
                table: "clases_servicio",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clases_servicio_nombre",
                table: "clases_servicio",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clientes_persona_id",
                table: "clientes",
                column: "persona_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clientes_usuario_id",
                table: "clientes",
                column: "usuario_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_contactos_emergencia_cliente_id",
                table: "contactos_emergencia",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_contactos_emergencia_persona_id",
                table: "contactos_emergencia",
                column: "persona_id");

            migrationBuilder.CreateIndex(
                name: "IX_contactos_emergencia_relacion_id",
                table: "contactos_emergencia",
                column: "relacion_id");

            migrationBuilder.CreateIndex(
                name: "IX_continentes_codigo",
                table: "continentes",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_continentes_nombre",
                table: "continentes",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_departamentos_estados_region_id_nombre",
                table: "departamentos_estados",
                columns: new[] { "region_id", "nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_direcciones_persona_ciudad_id",
                table: "direcciones_persona",
                column: "ciudad_id");

            migrationBuilder.CreateIndex(
                name: "IX_direcciones_persona_persona_id",
                table: "direcciones_persona",
                column: "persona_id");

            migrationBuilder.CreateIndex(
                name: "IX_direcciones_persona_tipo_direccion_id",
                table: "direcciones_persona",
                column: "tipo_direccion_id");

            migrationBuilder.CreateIndex(
                name: "IX_disponibilidad_asientos_asiento_id",
                table: "disponibilidad_asientos",
                column: "asiento_id");

            migrationBuilder.CreateIndex(
                name: "IX_disponibilidad_asientos_vuelo_id_asiento_id",
                table: "disponibilidad_asientos",
                columns: new[] { "vuelo_id", "asiento_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_emails_aerolinea_aerolinea_id",
                table: "emails_aerolinea",
                column: "aerolinea_id");

            migrationBuilder.CreateIndex(
                name: "IX_emails_aerolinea_email",
                table: "emails_aerolinea",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_emails_aerolinea_tipo_email_id",
                table: "emails_aerolinea",
                column: "tipo_email_id");

            migrationBuilder.CreateIndex(
                name: "IX_emails_persona_email",
                table: "emails_persona",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_emails_persona_persona_id",
                table: "emails_persona",
                column: "persona_id");

            migrationBuilder.CreateIndex(
                name: "IX_emails_persona_tipo_email_id",
                table: "emails_persona",
                column: "tipo_email_id");

            migrationBuilder.CreateIndex(
                name: "IX_equipaje_registrado_codigo_equipaje",
                table: "equipaje_registrado",
                column: "codigo_equipaje",
                unique: true,
                filter: "codigo_equipaje IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_equipaje_registrado_pasajero_reserva_id",
                table: "equipaje_registrado",
                column: "pasajero_reserva_id");

            migrationBuilder.CreateIndex(
                name: "IX_equipaje_registrado_tipo_equipaje_id",
                table: "equipaje_registrado",
                column: "tipo_equipaje_id");

            migrationBuilder.CreateIndex(
                name: "IX_equipaje_registrado_vuelo_id",
                table: "equipaje_registrado",
                column: "vuelo_id");

            migrationBuilder.CreateIndex(
                name: "IX_especialidades_nombre",
                table: "especialidades",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_especialidades_tipo_trabajador_id",
                table: "especialidades",
                column: "tipo_trabajador_id");

            migrationBuilder.CreateIndex(
                name: "IX_fabricantes_avion_nombre",
                table: "fabricantes_avion",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fabricantes_avion_pais_id",
                table: "fabricantes_avion",
                column: "pais_id");

            migrationBuilder.CreateIndex(
                name: "IX_facturas_direccion_facturacion_id",
                table: "facturas",
                column: "direccion_facturacion_id");

            migrationBuilder.CreateIndex(
                name: "IX_facturas_numero_factura",
                table: "facturas",
                column: "numero_factura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_facturas_reserva_id",
                table: "facturas",
                column: "reserva_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_generos_nombre",
                table: "generos",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_habilitaciones_piloto_licencia_id_modelo_avion_id",
                table: "habilitaciones_piloto",
                columns: new[] { "licencia_id", "modelo_avion_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_habilitaciones_piloto_modelo_avion_id",
                table: "habilitaciones_piloto",
                column: "modelo_avion_id");

            migrationBuilder.CreateIndex(
                name: "IX_historial_reserva_reserva_id",
                table: "historial_reserva",
                column: "reserva_id");

            migrationBuilder.CreateIndex(
                name: "IX_historial_reserva_usuario_id",
                table: "historial_reserva",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_historial_vuelo_usuario_id",
                table: "historial_vuelo",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_historial_vuelo_vuelo_id",
                table: "historial_vuelo",
                column: "vuelo_id");

            migrationBuilder.CreateIndex(
                name: "IX_licencias_piloto_numero_licencia",
                table: "licencias_piloto",
                column: "numero_licencia",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_licencias_piloto_trabajador_id",
                table: "licencias_piloto",
                column: "trabajador_id");

            migrationBuilder.CreateIndex(
                name: "IX_log_acceso_usuario_id",
                table: "log_acceso",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_metodos_pago_nombre",
                table: "metodos_pago",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_modelos_avion_codigo_modelo",
                table: "modelos_avion",
                column: "codigo_modelo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_modelos_avion_fabricante_id_nombre",
                table: "modelos_avion",
                columns: new[] { "fabricante_id", "nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pagos_metodo_pago_id",
                table: "pagos",
                column: "metodo_pago_id");

            migrationBuilder.CreateIndex(
                name: "IX_pagos_reserva_id",
                table: "pagos",
                column: "reserva_id");

            migrationBuilder.CreateIndex(
                name: "IX_paises_codigo_iso2",
                table: "paises",
                column: "codigo_iso2",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_paises_codigo_iso3",
                table: "paises",
                column: "codigo_iso3",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_paises_continente_id",
                table: "paises",
                column: "continente_id");

            migrationBuilder.CreateIndex(
                name: "IX_paises_nombre",
                table: "paises",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pasajeros_reserva_asiento_id",
                table: "pasajeros_reserva",
                column: "asiento_id");

            migrationBuilder.CreateIndex(
                name: "IX_pasajeros_reserva_persona_id",
                table: "pasajeros_reserva",
                column: "persona_id");

            migrationBuilder.CreateIndex(
                name: "IX_pasajeros_reserva_reserva_id_persona_id",
                table: "pasajeros_reserva",
                columns: new[] { "reserva_id", "persona_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pases_abordar_checkin_id",
                table: "pases_abordar",
                column: "checkin_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pases_abordar_codigo_pase",
                table: "pases_abordar",
                column: "codigo_pase",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pases_abordar_puerta_embarque_id",
                table: "pases_abordar",
                column: "puerta_embarque_id");

            migrationBuilder.CreateIndex(
                name: "IX_personas_genero_id",
                table: "personas",
                column: "genero_id");

            migrationBuilder.CreateIndex(
                name: "IX_personas_nacionalidad_id",
                table: "personas",
                column: "nacionalidad_id");

            migrationBuilder.CreateIndex(
                name: "IX_personas_tipo_doc_id_numero_doc",
                table: "personas",
                columns: new[] { "tipo_doc_id", "numero_doc" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_puertas_embarque_terminal_id_codigo",
                table: "puertas_embarque",
                columns: new[] { "terminal_id", "codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_regiones_pais_id_nombre",
                table: "regiones",
                columns: new[] { "pais_id", "nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_relaciones_contacto_descripcion",
                table: "relaciones_contacto",
                column: "descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reservas_cliente_id",
                table: "reservas",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_codigo_reserva",
                table: "reservas",
                column: "codigo_reserva",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reservas_tarifa_id",
                table: "reservas",
                column: "tarifa_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_vuelo_id",
                table: "reservas",
                column: "vuelo_id");

            migrationBuilder.CreateIndex(
                name: "IX_restricciones_equipaje_tarifa_id_tipo_equipaje_id",
                table: "restricciones_equipaje",
                columns: new[] { "tarifa_id", "tipo_equipaje_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_restricciones_equipaje_tipo_equipaje_id",
                table: "restricciones_equipaje",
                column: "tipo_equipaje_id");

            migrationBuilder.CreateIndex(
                name: "IX_roles_nombre",
                table: "roles",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rutas_aerolinea_id_origen_id_destino_id",
                table: "rutas",
                columns: new[] { "aerolinea_id", "origen_id", "destino_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rutas_destino_id",
                table: "rutas",
                column: "destino_id");

            migrationBuilder.CreateIndex(
                name: "IX_rutas_origen_id",
                table: "rutas",
                column: "origen_id");

            migrationBuilder.CreateIndex(
                name: "IX_tarifas_clase_servicio_id",
                table: "tarifas",
                column: "clase_servicio_id");

            migrationBuilder.CreateIndex(
                name: "IX_tarifas_ruta_id_clase_servicio_id_nombre",
                table: "tarifas",
                columns: new[] { "ruta_id", "clase_servicio_id", "nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_telefonos_aerolinea_aerolinea_id_numero",
                table: "telefonos_aerolinea",
                columns: new[] { "aerolinea_id", "numero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_telefonos_aerolinea_tipo_telefono_id",
                table: "telefonos_aerolinea",
                column: "tipo_telefono_id");

            migrationBuilder.CreateIndex(
                name: "IX_telefonos_persona_persona_id_numero",
                table: "telefonos_persona",
                columns: new[] { "persona_id", "numero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_telefonos_persona_tipo_telefono_id",
                table: "telefonos_persona",
                column: "tipo_telefono_id");

            migrationBuilder.CreateIndex(
                name: "IX_terminales_aeropuerto_id_nombre",
                table: "terminales",
                columns: new[] { "aeropuerto_id", "nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tipos_direccion_descripcion",
                table: "tipos_direccion",
                column: "descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tipos_documento_descripcion",
                table: "tipos_documento",
                column: "descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tipos_email_descripcion",
                table: "tipos_email",
                column: "descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tipos_equipaje_nombre",
                table: "tipos_equipaje",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tipos_telefono_descripcion",
                table: "tipos_telefono",
                column: "descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tipos_trabajador_nombre",
                table: "tipos_trabajador",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tiquetes_asiento_confirmado_id",
                table: "tiquetes",
                column: "asiento_confirmado_id");

            migrationBuilder.CreateIndex(
                name: "IX_tiquetes_codigo_tiquete",
                table: "tiquetes",
                column: "codigo_tiquete",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tiquetes_pasajero_reserva_id",
                table: "tiquetes",
                column: "pasajero_reserva_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trabajador_especialidades_especialidad_id",
                table: "trabajador_especialidades",
                column: "especialidad_id");

            migrationBuilder.CreateIndex(
                name: "IX_trabajador_especialidades_trabajador_id_especialidad_id",
                table: "trabajador_especialidades",
                columns: new[] { "trabajador_id", "especialidad_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trabajadores_aerolinea_id",
                table: "trabajadores",
                column: "aerolinea_id");

            migrationBuilder.CreateIndex(
                name: "IX_trabajadores_aeropuerto_base_id",
                table: "trabajadores",
                column: "aeropuerto_base_id");

            migrationBuilder.CreateIndex(
                name: "IX_trabajadores_persona_id",
                table: "trabajadores",
                column: "persona_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trabajadores_tipo_trabajador_id",
                table: "trabajadores",
                column: "tipo_trabajador_id");

            migrationBuilder.CreateIndex(
                name: "IX_trabajadores_usuario_id",
                table: "trabajadores",
                column: "usuario_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tripulacion_vuelo_trabajador_id",
                table: "tripulacion_vuelo",
                column: "trabajador_id");

            migrationBuilder.CreateIndex(
                name: "IX_tripulacion_vuelo_vuelo_id_rol_en_vuelo",
                table: "tripulacion_vuelo",
                columns: new[] { "vuelo_id", "rol_en_vuelo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tripulacion_vuelo_vuelo_id_trabajador_id",
                table: "tripulacion_vuelo",
                columns: new[] { "vuelo_id", "trabajador_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_persona_id",
                table: "usuarios",
                column: "persona_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_rol_id",
                table: "usuarios",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_username",
                table: "usuarios",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vuelos_avion_id",
                table: "vuelos",
                column: "avion_id");

            migrationBuilder.CreateIndex(
                name: "IX_vuelos_numero_vuelo_fecha_salida",
                table: "vuelos",
                columns: new[] { "numero_vuelo", "fecha_salida" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vuelos_puerta_embarque_id",
                table: "vuelos",
                column: "puerta_embarque_id");

            migrationBuilder.CreateIndex(
                name: "IX_vuelos_ruta_id",
                table: "vuelos",
                column: "ruta_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cargos_adicionales");

            migrationBuilder.DropTable(
                name: "contactos_emergencia");

            migrationBuilder.DropTable(
                name: "emails_aerolinea");

            migrationBuilder.DropTable(
                name: "emails_persona");

            migrationBuilder.DropTable(
                name: "equipaje_registrado");

            migrationBuilder.DropTable(
                name: "facturas");

            migrationBuilder.DropTable(
                name: "habilitaciones_piloto");

            migrationBuilder.DropTable(
                name: "historial_reserva");

            migrationBuilder.DropTable(
                name: "historial_vuelo");

            migrationBuilder.DropTable(
                name: "log_acceso");

            migrationBuilder.DropTable(
                name: "pagos");

            migrationBuilder.DropTable(
                name: "pases_abordar");

            migrationBuilder.DropTable(
                name: "restricciones_equipaje");

            migrationBuilder.DropTable(
                name: "telefonos_aerolinea");

            migrationBuilder.DropTable(
                name: "telefonos_persona");

            migrationBuilder.DropTable(
                name: "tiquetes");

            migrationBuilder.DropTable(
                name: "trabajador_especialidades");

            migrationBuilder.DropTable(
                name: "tripulacion_vuelo");

            migrationBuilder.DropTable(
                name: "relaciones_contacto");

            migrationBuilder.DropTable(
                name: "tipos_email");

            migrationBuilder.DropTable(
                name: "direcciones_persona");

            migrationBuilder.DropTable(
                name: "licencias_piloto");

            migrationBuilder.DropTable(
                name: "metodos_pago");

            migrationBuilder.DropTable(
                name: "checkin");

            migrationBuilder.DropTable(
                name: "tipos_equipaje");

            migrationBuilder.DropTable(
                name: "tipos_telefono");

            migrationBuilder.DropTable(
                name: "especialidades");

            migrationBuilder.DropTable(
                name: "tipos_direccion");

            migrationBuilder.DropTable(
                name: "pasajeros_reserva");

            migrationBuilder.DropTable(
                name: "trabajadores");

            migrationBuilder.DropTable(
                name: "disponibilidad_asientos");

            migrationBuilder.DropTable(
                name: "reservas");

            migrationBuilder.DropTable(
                name: "tipos_trabajador");

            migrationBuilder.DropTable(
                name: "asientos_avion");

            migrationBuilder.DropTable(
                name: "clientes");

            migrationBuilder.DropTable(
                name: "tarifas");

            migrationBuilder.DropTable(
                name: "vuelos");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "clases_servicio");

            migrationBuilder.DropTable(
                name: "aviones");

            migrationBuilder.DropTable(
                name: "puertas_embarque");

            migrationBuilder.DropTable(
                name: "rutas");

            migrationBuilder.DropTable(
                name: "personas");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "modelos_avion");

            migrationBuilder.DropTable(
                name: "terminales");

            migrationBuilder.DropTable(
                name: "aerolineas");

            migrationBuilder.DropTable(
                name: "generos");

            migrationBuilder.DropTable(
                name: "tipos_documento");

            migrationBuilder.DropTable(
                name: "fabricantes_avion");

            migrationBuilder.DropTable(
                name: "aeropuertos");

            migrationBuilder.DropTable(
                name: "ciudades");

            migrationBuilder.DropTable(
                name: "departamentos_estados");

            migrationBuilder.DropTable(
                name: "regiones");

            migrationBuilder.DropTable(
                name: "paises");

            migrationBuilder.DropTable(
                name: "continentes");
        }
    }
}
