using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace web.cvetbenavente.Data.Migrations
{
    public partial class EventosForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Eventos_IdAnimal",
                table: "Eventos",
                column: "IdAnimal");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_IdCliente",
                table: "Eventos",
                column: "IdCliente");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Animais_IdAnimal",
                table: "Eventos",
                column: "IdAnimal",
                principalTable: "Animais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Clientes_IdCliente",
                table: "Eventos",
                column: "IdCliente",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Animais_IdAnimal",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Clientes_IdCliente",
                table: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_IdAnimal",
                table: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_IdCliente",
                table: "Eventos");
        }
    }
}
