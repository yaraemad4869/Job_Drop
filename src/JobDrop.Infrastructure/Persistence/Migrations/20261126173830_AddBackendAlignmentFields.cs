using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobDrop.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBackendAlignmentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AcceptedAtUtc",
                table: "Conversations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CandidateId",
                table: "Conversations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeclinedAtUtc",
                table: "Conversations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExclusivityExpiresAtUtc",
                table: "Conversations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiresAtUtc",
                table: "Conversations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExclusivityActive",
                table: "Conversations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "JobId",
                table: "Conversations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "MessagingEnabled",
                table: "Conversations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Conversations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CoverImageStorageKey",
                table: "Companies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EligibleCompanyCount",
                table: "Companies",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FollowUpRate",
                table: "Companies",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "HasCoverImage",
                table: "Companies",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OrdinalRank",
                table: "Companies",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Percentile",
                table: "Companies",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExperienceYears",
                table: "candidate_profiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailableImmediately",
                table: "candidate_profiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MatchBand",
                table: "candidate_profiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MatchScore",
                table: "candidate_profiles",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DueAtUtc",
                table: "Applications",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsQueued",
                table: "Applications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "QueuedAtUtc",
                table: "Applications",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReviewedAtUtc",
                table: "Applications",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReviewedByUserId",
                table: "Applications",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SentAtUtc",
                table: "Applications",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SlaPolicyId",
                table: "Applications",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SlaPolicyVersion",
                table: "Applications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptedAtUtc",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "CandidateId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "DeclinedAtUtc",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "ExclusivityExpiresAtUtc",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "ExpiresAtUtc",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "IsExclusivityActive",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "MessagingEnabled",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "CoverImageStorageKey",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "EligibleCompanyCount",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "FollowUpRate",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "HasCoverImage",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "OrdinalRank",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Percentile",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ExperienceYears",
                table: "candidate_profiles");

            migrationBuilder.DropColumn(
                name: "IsAvailableImmediately",
                table: "candidate_profiles");

            migrationBuilder.DropColumn(
                name: "MatchBand",
                table: "candidate_profiles");

            migrationBuilder.DropColumn(
                name: "MatchScore",
                table: "candidate_profiles");

            migrationBuilder.DropColumn(
                name: "DueAtUtc",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "IsQueued",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "QueuedAtUtc",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ReviewedAtUtc",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ReviewedByUserId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "SentAtUtc",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "SlaPolicyId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "SlaPolicyVersion",
                table: "Applications");
        }
    }
}
