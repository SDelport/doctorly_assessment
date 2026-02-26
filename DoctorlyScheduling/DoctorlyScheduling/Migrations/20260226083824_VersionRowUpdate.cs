using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorlyScheduling.Migrations
{
    /// <inheritdoc />
    public partial class VersionRowUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Events");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Events",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
