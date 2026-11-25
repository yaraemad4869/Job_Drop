using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobDrop.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Final_CleanSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "drop_wallet_balances");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "application_slas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Jobs",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "drop_wallet_balances",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "application_slas",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
