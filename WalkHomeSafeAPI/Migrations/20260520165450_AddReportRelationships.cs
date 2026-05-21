using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalkHomeSafeAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddReportRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ReportVotes_ReportId",
                table: "ReportVotes",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserId",
                table: "Reports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportRatings_CategoryId",
                table: "ReportRatings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportRatings_ReportId",
                table: "ReportRatings",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportRatings_ReportCategories_CategoryId",
                table: "ReportRatings",
                column: "CategoryId",
                principalTable: "ReportCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportRatings_Reports_ReportId",
                table: "ReportRatings",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_UserId",
                table: "Reports",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportVotes_Reports_ReportId",
                table: "ReportVotes",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportRatings_ReportCategories_CategoryId",
                table: "ReportRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportRatings_Reports_ReportId",
                table: "ReportRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_UserId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportVotes_Reports_ReportId",
                table: "ReportVotes");

            migrationBuilder.DropIndex(
                name: "IX_ReportVotes_ReportId",
                table: "ReportVotes");

            migrationBuilder.DropIndex(
                name: "IX_Reports_UserId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_ReportRatings_CategoryId",
                table: "ReportRatings");

            migrationBuilder.DropIndex(
                name: "IX_ReportRatings_ReportId",
                table: "ReportRatings");
        }
    }
}
