using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace web.cvetbenavente.Data.Migrations
{
    public partial class CodPostalLocalidade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodPostal",
                table: "Clientes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Localidade",
                table: "Clientes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodPostal",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Localidade",
                table: "Clientes");
        }
    }
}
