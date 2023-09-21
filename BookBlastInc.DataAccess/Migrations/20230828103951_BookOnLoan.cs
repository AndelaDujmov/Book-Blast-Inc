using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBlastInc.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class BookOnLoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookOnLoans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    BookId = table.Column<Guid>(type: "char(36)", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    DateOfBorrowing = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LoanStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookOnLoans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookOnLoans_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookOnLoans_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BookOnLoans_BookId",
                table: "BookOnLoans",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookOnLoans_UserId",
                table: "BookOnLoans",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookOnLoans");
        }
    }
}
