using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Simulacao.Migrations
{
    public partial class Pagamento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enrollment",
                columns: table => new
                {
                    idEnrollment = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    idCourse = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollment", x => x.idEnrollment);
                });

            migrationBuilder.CreateTable(
                name: "Pagamento",
                columns: table => new
                {
                    idPagamento = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Course = table.Column<bool>(nullable: false),
                    DataBaixa = table.Column<DateTime>(nullable: false),
                    DataEmissao = table.Column<DateTime>(nullable: false),
                    DataRegistro = table.Column<DateTime>(nullable: false),
                    DataVencimento = table.Column<DateTime>(nullable: false),
                    Prova = table.Column<bool>(nullable: false),
                    Simulado = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    ValorPagar = table.Column<double>(nullable: false),
                    ValorPago = table.Column<double>(nullable: false),
                    idCourse = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamento", x => x.idPagamento);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollment");

            migrationBuilder.DropTable(
                name: "Pagamento");
        }
    }
}
