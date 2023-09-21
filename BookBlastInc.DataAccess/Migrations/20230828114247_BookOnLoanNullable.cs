using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBlastInc.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class BookOnLoanNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookOnLoans_Books_BookId",
                table: "BookOnLoans");

            migrationBuilder.DropIndex(
                name: "IX_BookOnLoans_BookId",
                table: "BookOnLoans");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "BookOnLoans",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookOnLoans_OrderId",
                table: "BookOnLoans",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookOnLoans_Books_OrderId",
                table: "BookOnLoans",
                column: "OrderId",
                principalTable: "Books",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookOnLoans_Books_OrderId",
                table: "BookOnLoans");

            migrationBuilder.DropIndex(
                name: "IX_BookOnLoans_OrderId",
                table: "BookOnLoans");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "BookOnLoans");

            migrationBuilder.CreateIndex(
                name: "IX_BookOnLoans_BookId",
                table: "BookOnLoans",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookOnLoans_Books_BookId",
                table: "BookOnLoans",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");
        }
    }
}
