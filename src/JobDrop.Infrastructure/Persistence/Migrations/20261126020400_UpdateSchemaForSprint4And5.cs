using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobDrop.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchemaForSprint4And5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Channel",
                table: "OutboxMessages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "OutboxMessages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "OutboxMessages",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DailyIntakeCap",
                table: "Jobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "candidate_profiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Channel",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "DailyIntakeCap",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "candidate_profiles");
        }
    }
}
