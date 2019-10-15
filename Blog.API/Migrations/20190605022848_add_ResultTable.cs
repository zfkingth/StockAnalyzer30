using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.API.Migrations
{
    public partial class add_ResultTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchResult",
                columns: table => new
                {
                    ActionName = table.Column<string>(maxLength: 32, nullable: false),
                    ActionParams = table.Column<string>(maxLength: 128, nullable: false),
                    ActionDate = table.Column<DateTime>(nullable: false),
                    ActionReslut = table.Column<string>(maxLength: 4096, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchResult", x => new { x.ActionName, x.ActionParams, x.ActionDate });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchResult");
        }
    }
}
