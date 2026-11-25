using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobDrop.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MasterSprint_Features : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobAssignments_Jobs_JobId",
                table: "JobAssignments");

            migrationBuilder.DropIndex(
                name: "IX_JobAssignments_JobId",
                table: "JobAssignments");

            migrationBuilder.DropColumn(
                name: "ClosedAtUtc",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "LastModifiedAtUtc",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "Jobs");

            migrationBuilder.RenameColumn(
                name: "IsSalaryNegotiable",
                table: "Jobs",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "DailyIntakeCap",
                table: "Jobs",
                newName: "DailyIntakeLimit");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Jobs",
                newName: "EmploymentType");

            migrationBuilder.AlterColumn<string>(
                name: "WorkMode",
                table: "Jobs",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Jobs",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "SalaryPeriod",
                table: "Jobs",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "Monthly",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Monthly");

            migrationBuilder.AlterColumn<string>(
                name: "SalaryCurrency",
                table: "Jobs",
                type: "character varying(8)",
                maxLength: 8,
                nullable: true,
                defaultValue: "USD",
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8,
                oldDefaultValue: "USD");

            migrationBuilder.AlterColumn<Guid>(
                name: "OfficeId",
                table: "Jobs",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<int>(
                name: "CurrentApplicantsCount",
                table: "Jobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_application_slas_Applications_ApplicationId",
                table: "application_slas",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_application_slas_Applications_ApplicationId",
                table: "application_slas");

            migrationBuilder.DropColumn(
                name: "CurrentApplicantsCount",
                table: "Jobs");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Jobs",
                newName: "IsSalaryNegotiable");

            migrationBuilder.RenameColumn(
                name: "EmploymentType",
                table: "Jobs",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "DailyIntakeLimit",
                table: "Jobs",
                newName: "DailyIntakeCap");

            migrationBuilder.AlterColumn<int>(
                name: "WorkMode",
                table: "Jobs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Jobs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "SalaryPeriod",
                table: "Jobs",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Monthly",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "Monthly");

            migrationBuilder.AlterColumn<string>(
                name: "SalaryCurrency",
                table: "Jobs",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "USD",
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValue: "USD");

            migrationBuilder.AlterColumn<Guid>(
                name: "OfficeId",
                table: "Jobs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ClosedAtUtc",
                table: "Jobs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "Jobs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedAtUtc",
                table: "Jobs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Jobs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerUserId",
                table: "Jobs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_JobAssignments_JobId",
                table: "JobAssignments",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobAssignments_Jobs_JobId",
                table: "JobAssignments",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
