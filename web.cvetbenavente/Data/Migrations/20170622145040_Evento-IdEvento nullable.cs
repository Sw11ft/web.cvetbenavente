using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace web.cvetbenavente.Data.Migrations
{
    public partial class EventoIdEventonullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdEvento",
                table: "Eventos",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdEvento",
                table: "Eventos",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
