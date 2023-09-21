using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBlastInc.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class PaymentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "BookOnLoans",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "BookOnLoans");
        }
    }
}
