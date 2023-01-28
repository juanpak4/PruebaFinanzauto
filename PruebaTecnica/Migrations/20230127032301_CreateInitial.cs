using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PruebaTecnica.Migrations
{
    /// <inheritdoc />
    public partial class CreateInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COURSE",
                columns: table => new
                {
                    IdCourse = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Course = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COURSE", x => x.IdCourse);
                });

            migrationBuilder.CreateTable(
                name: "STUDENT",
                columns: table => new
                {
                    IdStudent = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentifactionNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STUDENT", x => x.IdStudent);
                });

            migrationBuilder.CreateTable(
                name: "TEACHER",
                columns: table => new
                {
                    IdTeacher = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdIdentificationNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEACHER", x => x.IdTeacher);
                });

            migrationBuilder.CreateTable(
                name: "RATING",
                columns: table => new
                {
                    IdRating = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCourse = table.Column<int>(type: "int", nullable: false),
                    IdTeacher = table.Column<int>(type: "int", nullable: false),
                    IdStudent = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RATING", x => x.IdRating);
                    table.ForeignKey(
                        name: "FK_RATING_COURSE",
                        column: x => x.IdCourse,
                        principalTable: "COURSE",
                        principalColumn: "IdCourse");
                    table.ForeignKey(
                        name: "FK_RATING_STUDENT",
                        column: x => x.IdStudent,
                        principalTable: "STUDENT",
                        principalColumn: "IdStudent");
                    table.ForeignKey(
                        name: "FK_RATING_TEACHER",
                        column: x => x.IdTeacher,
                        principalTable: "TEACHER",
                        principalColumn: "IdTeacher");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RATING_IdCourse",
                table: "RATING",
                column: "IdCourse");

            migrationBuilder.CreateIndex(
                name: "IX_RATING_IdStudent",
                table: "RATING",
                column: "IdStudent");

            migrationBuilder.CreateIndex(
                name: "IX_RATING_IdTeacher",
                table: "RATING",
                column: "IdTeacher");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RATING");

            migrationBuilder.DropTable(
                name: "COURSE");

            migrationBuilder.DropTable(
                name: "STUDENT");

            migrationBuilder.DropTable(
                name: "TEACHER");
        }
    }
}
