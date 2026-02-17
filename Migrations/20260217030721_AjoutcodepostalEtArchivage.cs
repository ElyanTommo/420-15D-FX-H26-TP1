using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _420_15D_FX_H26_TP1.Migrations
{
    /// <inheritdoc />
    public partial class AjoutcodepostalEtArchivage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodePostal",
                table: "Evenements",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EstArchive",
                table: "Evenements",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodePostal",
                table: "Evenements");

            migrationBuilder.DropColumn(
                name: "EstArchive",
                table: "Evenements");
        }
    }
}
