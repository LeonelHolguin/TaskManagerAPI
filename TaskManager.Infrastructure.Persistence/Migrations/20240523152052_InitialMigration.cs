using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "NVARCHAR2(25)", maxLength: 25, nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR2(60)", maxLength: 60, nullable: false),
                    Created = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR2(60)", maxLength: 60, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    SerialNumber = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR2(60)", maxLength: 60, nullable: false),
                    Created = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR2(60)", maxLength: 60, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserId",
                table: "Tasks",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
