﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstProject_Praksa.Migrations
{
    public partial class CreateTableUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_UserId",
                table: "Characters",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Users_UserId",
                table: "Characters",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Users_UserId",
                table: "Characters");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Characters_UserId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Characters");
        }
    }
}