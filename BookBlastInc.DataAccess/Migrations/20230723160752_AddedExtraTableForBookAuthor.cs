using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBlastInc.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedExtraTableForBookAuthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authors_MedicationId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_MedicationId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "MedicationId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Authors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MedicationId",
                table: "Books",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BookId",
                table: "Authors",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Books_MedicationId",
                table: "Books",
                column: "MedicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authors_MedicationId",
                table: "Books",
                column: "MedicationId",
                principalTable: "Authors",
                principalColumn: "Id");
        }
    }
}
