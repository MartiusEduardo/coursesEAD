using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Simulacao.Migrations
{
    public partial class MigManagement3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Identity",
                table: "AspNetUsers",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(int),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldMaxLength: 10,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Identity",
                table: "AspNetUsers",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);
        }
    }
}
