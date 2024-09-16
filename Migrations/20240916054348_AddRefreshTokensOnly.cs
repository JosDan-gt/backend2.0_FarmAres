using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GranjaLosAres_API.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokensOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    ClienteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCliente = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Direccion = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Telefono = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Clientes__71ABD0A7E7862C51", x => x.ClienteID);
                });

            migrationBuilder.CreateTable(
                name: "Corral",
                columns: table => new
                {
                    IdCorral = table.Column<int>(type: "int", nullable: false),
                    NumCorral = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Capacidad = table.Column<int>(type: "int", nullable: false),
                    Alto = table.Column<double>(type: "float", nullable: false),
                    Ancho = table.Column<double>(type: "float", nullable: false),
                    Largo = table.Column<double>(type: "float", nullable: false),
                    Agua = table.Column<bool>(type: "bit", nullable: false),
                    Comederos = table.Column<int>(type: "int", nullable: false),
                    Bebederos = table.Column<int>(type: "int", nullable: false),
                    Ponederos = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corral", x => x.IdCorral);
                });

            migrationBuilder.CreateTable(
                name: "Etapas",
                columns: table => new
                {
                    IdEtapa = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etapas", x => x.IdEtapa);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    ProductoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreProducto = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Producto__A430AE83E9DF4B6A", x => x.ProductoID);
                });

            migrationBuilder.CreateTable(
                name: "RazaGallina",
                columns: table => new
                {
                    IdRaza = table.Column<int>(type: "int", nullable: false),
                    Raza = table.Column<string>(type: "varchar(90)", unicode: false, maxLength: 90, nullable: false),
                    Origen = table.Column<string>(type: "varchar(90)", unicode: false, maxLength: 90, nullable: false),
                    Color = table.Column<string>(type: "varchar(90)", unicode: false, maxLength: 90, nullable: false),
                    ColorH = table.Column<string>(type: "varchar(90)", unicode: false, maxLength: 90, nullable: false),
                    CaractEspec = table.Column<string>(type: "varchar(5000)", unicode: false, maxLength: 5000, nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RazaGallina", x => x.IdRaza);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles__3214EC071BC0321D", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockHuevos",
                columns: table => new
                {
                    Tamano = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cajas = table.Column<int>(type: "int", nullable: false),
                    CartonesExtras = table.Column<int>(type: "int", nullable: false),
                    HuevosSueltos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StockHue__799ADF87B8CE711C", x => x.Tamano);
                });

            migrationBuilder.CreateTable(
                name: "TriggerDebugLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    OldCartonesExtras = table.Column<int>(type: "int", nullable: true),
                    NewCartonesExtras = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    VentaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaVenta = table.Column<DateOnly>(type: "date", nullable: false),
                    ClienteID = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    TotalVenta = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ventas__5B41514CA5140F29", x => x.VentaID);
                    table.ForeignKey(
                        name: "FK__Ventas__ClienteI__6477ECF3",
                        column: x => x.ClienteID,
                        principalTable: "Clientes",
                        principalColumn: "ClienteID");
                });

            migrationBuilder.CreateTable(
                name: "Lote",
                columns: table => new
                {
                    IdLote = table.Column<int>(type: "int", nullable: false),
                    NumLote = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    CantidadG = table.Column<int>(type: "int", nullable: false),
                    IdRaza = table.Column<int>(type: "int", nullable: false),
                    FechaAdq = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdCorral = table.Column<int>(type: "int", nullable: false),
                    CantidadGctual = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true),
                    EstadoBaja = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table1", x => x.IdLote);
                    table.ForeignKey(
                        name: "FK_Table1_Corral",
                        column: x => x.IdCorral,
                        principalTable: "Corral",
                        principalColumn: "IdCorral");
                    table.ForeignKey(
                        name: "FK_Table1_RazaGallina",
                        column: x => x.IdRaza,
                        principalTable: "RazaGallina",
                        principalColumn: "IdRaza");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Contrasena = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    FechaDeRegistro = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuarios__3214EC07059D3025", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Usuarios__RoleId__628FA481",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DetallesVenta",
                columns: table => new
                {
                    DetalleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VentaID = table.Column<int>(type: "int", nullable: true),
                    ProductoID = table.Column<int>(type: "int", nullable: true),
                    TipoEmpaque = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    TamanoHuevo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CantidadVendida = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(10,2)", nullable: true, defaultValue: 0m),
                    TotalHuevos = table.Column<int>(type: "int", nullable: true, computedColumnSql: "(case when [TipoEmpaque]='Caja' then [CantidadVendida]*(360) when [TipoEmpaque]='Cartón' then [CantidadVendida]*(30) else [CantidadVendida] end)", stored: true),
                    Total = table.Column<decimal>(type: "decimal(21,2)", nullable: true, computedColumnSql: "([CantidadVendida]*[PrecioUnitario])", stored: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Detalles__6E19D6FA2008A5B1", x => x.DetalleID);
                    table.ForeignKey(
                        name: "FK__DetallesV__Produ__59FA5E80",
                        column: x => x.ProductoID,
                        principalTable: "Productos",
                        principalColumn: "ProductoID");
                    table.ForeignKey(
                        name: "FK__DetallesV__Venta__5BE2A6F2",
                        column: x => x.VentaID,
                        principalTable: "Ventas",
                        principalColumn: "VentaID");
                });

            migrationBuilder.CreateTable(
                name: "EstadoLote",
                columns: table => new
                {
                    Id_Estado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad_G = table.Column<int>(type: "int", nullable: false),
                    Bajas = table.Column<int>(type: "int", nullable: false),
                    Fecha_Registro = table.Column<DateTime>(type: "datetime", nullable: false),
                    Semana = table.Column<int>(type: "int", nullable: true),
                    Id_Etapa = table.Column<int>(type: "int", nullable: false),
                    IdLote = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoLote_G", x => x.Id_Estado);
                    table.ForeignKey(
                        name: "FK_EstadoLote_Etapas",
                        column: x => x.Id_Etapa,
                        principalTable: "Etapas",
                        principalColumn: "IdEtapa");
                    table.ForeignKey(
                        name: "FK_EstadoLote_Lote",
                        column: x => x.IdLote,
                        principalTable: "Lote",
                        principalColumn: "IdLote");
                });

            migrationBuilder.CreateTable(
                name: "ProduccionGallinas",
                columns: table => new
                {
                    IdProd = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CantCajas = table.Column<int>(type: "int", nullable: false),
                    CantCartones = table.Column<int>(type: "int", nullable: false),
                    CantSueltos = table.Column<int>(type: "int", nullable: false),
                    IdLote = table.Column<int>(type: "int", nullable: false),
                    Defectuosos = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    FechaRegistroP = table.Column<DateTime>(type: "datetime", nullable: true),
                    CantTotal = table.Column<int>(type: "int", nullable: true, computedColumnSql: "([dbo].[CalcularCantidadTotalProduccion]([CantCajas],[CantCartones],[CantSueltos]))", stored: false),
                    Estado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Producci__E40D971DC3C087DC", x => x.IdProd);
                    table.ForeignKey(
                        name: "FK_ProduccionGallinas_Lote",
                        column: x => x.IdLote,
                        principalTable: "Lote",
                        principalColumn: "IdLote");
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RefreshToken__3214EC07", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_Usuario",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClasificacionHuevos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tamano = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Cajas = table.Column<int>(type: "int", nullable: false),
                    CartonesExtras = table.Column<int>(type: "int", nullable: false),
                    HuevosSueltos = table.Column<int>(type: "int", nullable: false),
                    TotalUnitaria = table.Column<int>(type: "int", nullable: true, computedColumnSql: "([dbo].[CalcularCantidadTotalUnitaria]([Cajas],[CartonesExtras],[HuevosSueltos]))", stored: false),
                    IdProd = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: true),
                    FechaClaS = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Clasific__3214EC27BBDCC8F0", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClasificacionHuevos_ProduccionGallinas",
                        column: x => x.IdProd,
                        principalTable: "ProduccionGallinas",
                        principalColumn: "IdProd");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClasificacionHuevos_IdProd",
                table: "ClasificacionHuevos",
                column: "IdProd");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesVenta_ProductoID",
                table: "DetallesVenta",
                column: "ProductoID");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesVenta_VentaID",
                table: "DetallesVenta",
                column: "VentaID");

            migrationBuilder.CreateIndex(
                name: "IX_EstadoLote_Id_Etapa",
                table: "EstadoLote",
                column: "Id_Etapa");

            migrationBuilder.CreateIndex(
                name: "IX_EstadoLote_IdLote",
                table: "EstadoLote",
                column: "IdLote");

            migrationBuilder.CreateIndex(
                name: "IX_Lote_IdCorral",
                table: "Lote",
                column: "IdCorral");

            migrationBuilder.CreateIndex(
                name: "IX_Lote_IdRaza",
                table: "Lote",
                column: "IdRaza");

            migrationBuilder.CreateIndex(
                name: "IX_ProduccionGallinas_IdLote",
                table: "ProduccionGallinas",
                column: "IdLote");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RoleId",
                table: "Usuarios",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_ClienteID",
                table: "Ventas",
                column: "ClienteID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClasificacionHuevos");

            migrationBuilder.DropTable(
                name: "DetallesVenta");

            migrationBuilder.DropTable(
                name: "EstadoLote");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "StockHuevos");

            migrationBuilder.DropTable(
                name: "TriggerDebugLog");

            migrationBuilder.DropTable(
                name: "ProduccionGallinas");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Ventas");

            migrationBuilder.DropTable(
                name: "Etapas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Lote");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Corral");

            migrationBuilder.DropTable(
                name: "RazaGallina");
        }
    }
}
