﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBlastInc.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDeposit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Deposit",
                table: "Books",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deposit",
                table: "Books");
        }
    }
}
