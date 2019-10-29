using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Simulacao.Migrations
{
    public partial class MigManagement2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Identity",
                table: "AspNetUsers",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(int),
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldMaxLength: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Identity",
                table: "AspNetUsers",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldMaxLength: 10,
                oldNullable: true);
        }
    }
}
