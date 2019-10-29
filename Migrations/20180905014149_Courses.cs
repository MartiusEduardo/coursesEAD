using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Simulacao.Migrations
{
    public partial class Courses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<long>(
                name: "idarea",
                table: "area",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<long>(
                name: "idCourse",
                table: "area",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    idCourse = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    About = table.Column<string>(nullable: false),
                    CourseName = table.Column<string>(maxLength: 200, nullable: false),
                    Image = table.Column<byte[]>(nullable: false),
                    Price = table.Column<string>(nullable: false),
                    ProfessorName = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.idCourse);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropColumn(
                name: "idCourse",
                table: "area");

            migrationBuilder.AlterColumn<int>(
                name: "idarea",
                table: "area",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "about",
                table: "area",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "image",
                table: "area",
                nullable: false,
                defaultValue: new byte[] {  });

            migrationBuilder.AddColumn<string>(
                name: "professorname",
                table: "area",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
