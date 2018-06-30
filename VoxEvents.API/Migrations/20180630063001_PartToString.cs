using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VoxEvents.API.Migrations
{
    public partial class PartToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Part",
                table: "Members",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Part",
                table: "Members",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
