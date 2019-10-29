using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Simulacao.Migrations
{
    public partial class OrdemMaterialEstudo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "message",
                table: "Posting",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ordem",
                table: "MaterialEstudo",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ordem",
                table: "MaterialEstudo");

            migrationBuilder.AlterColumn<string>(
                name: "message",
                table: "Posting",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
