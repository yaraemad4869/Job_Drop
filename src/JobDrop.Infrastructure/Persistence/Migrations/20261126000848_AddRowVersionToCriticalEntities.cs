using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobDrop.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersionToCriticalEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_ReverseDrops_ReverseDropId",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_ReverseDropPools_Companies_CompanyId",
                table: "ReverseDropPools");

            migrationBuilder.DropForeignKey(
                name: "FK_ReverseDrops_Companies_CompanyId",
                table: "ReverseDrops");

            migrationBuilder.DropForeignKey(
                name: "FK_ReverseDrops_Jobs_JobId",
                table: "ReverseDrops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReverseDrops",
                table: "ReverseDrops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReverseDropPools",
                table: "ReverseDropPools");

            migrationBuilder.DropIndex(
                name: "IX_ReverseDropPools_CompanyId",
                table: "ReverseDropPools");

            migrationBuilder.RenameTable(
                name: "ReverseDrops",
                newName: "reverse_drops");

            migrationBuilder.RenameTable(
                name: "ReverseDropPools",
                newName: "reverse_drop_pools");

            migrationBuilder.RenameIndex(
                name: "IX_ReverseDrops_JobId",
                table: "reverse_drops",
                newName: "IX_reverse_drops_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_ReverseDrops_CompanyId",
                table: "reverse_drops",
                newName: "IX_reverse_drops_CompanyId");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "drop_wallet_balances",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "candidate_profiles",
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

            migrationBuilder.AddPrimaryKey(
                name: "PK_reverse_drops",
                table: "reverse_drops",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_reverse_drop_pools",
                table: "reverse_drop_pools",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_reverse_drops_CandidateProfileId",
                table: "reverse_drops",
                column: "CandidateProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_reverse_drops_ExpiresAtUtc",
                table: "reverse_drops",
                column: "ExpiresAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_reverse_drops_Status",
                table: "reverse_drops",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_reverse_drop_pools_CompanyId_CycleStartUtc",
                table: "reverse_drop_pools",
                columns: new[] { "CompanyId", "CycleStartUtc" });

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_reverse_drops_ReverseDropId",
                table: "Conversations",
                column: "ReverseDropId",
                principalTable: "reverse_drops",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_reverse_drop_pools_Companies_CompanyId",
                table: "reverse_drop_pools",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reverse_drops_Companies_CompanyId",
                table: "reverse_drops",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reverse_drops_Jobs_JobId",
                table: "reverse_drops",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_reverse_drops_ReverseDropId",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_reverse_drop_pools_Companies_CompanyId",
                table: "reverse_drop_pools");

            migrationBuilder.DropForeignKey(
                name: "FK_reverse_drops_Companies_CompanyId",
                table: "reverse_drops");

            migrationBuilder.DropForeignKey(
                name: "FK_reverse_drops_Jobs_JobId",
                table: "reverse_drops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reverse_drops",
                table: "reverse_drops");

            migrationBuilder.DropIndex(
                name: "IX_reverse_drops_CandidateProfileId",
                table: "reverse_drops");

            migrationBuilder.DropIndex(
                name: "IX_reverse_drops_ExpiresAtUtc",
                table: "reverse_drops");

            migrationBuilder.DropIndex(
                name: "IX_reverse_drops_Status",
                table: "reverse_drops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reverse_drop_pools",
                table: "reverse_drop_pools");

            migrationBuilder.DropIndex(
                name: "IX_reverse_drop_pools_CompanyId_CycleStartUtc",
                table: "reverse_drop_pools");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "drop_wallet_balances");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "candidate_profiles");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "application_slas");

            migrationBuilder.RenameTable(
                name: "reverse_drops",
                newName: "ReverseDrops");

            migrationBuilder.RenameTable(
                name: "reverse_drop_pools",
                newName: "ReverseDropPools");

            migrationBuilder.RenameIndex(
                name: "IX_reverse_drops_JobId",
                table: "ReverseDrops",
                newName: "IX_ReverseDrops_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_reverse_drops_CompanyId",
                table: "ReverseDrops",
                newName: "IX_ReverseDrops_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReverseDrops",
                table: "ReverseDrops",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReverseDropPools",
                table: "ReverseDropPools",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ReverseDropPools_CompanyId",
                table: "ReverseDropPools",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_ReverseDrops_ReverseDropId",
                table: "Conversations",
                column: "ReverseDropId",
                principalTable: "ReverseDrops",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReverseDropPools_Companies_CompanyId",
                table: "ReverseDropPools",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReverseDrops_Companies_CompanyId",
                table: "ReverseDrops",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReverseDrops_Jobs_JobId",
                table: "ReverseDrops",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
