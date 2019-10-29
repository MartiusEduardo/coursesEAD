using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Simulacao.Migrations
{
    public partial class MigArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "about",
                table: "area",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "image",
                table: "area",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "professorname",
                table: "area",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "about",
                table: "area");

            migrationBuilder.DropColumn(
                name: "image",
                table: "area");

            migrationBuilder.DropColumn(
                name: "professorname",
                table: "area");
        }
    }
}
