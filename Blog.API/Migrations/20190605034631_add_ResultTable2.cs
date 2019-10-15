using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.API.Migrations
{
    public partial class add_ResultTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SearchResult",
                table: "SearchResult");

            migrationBuilder.RenameTable(
                name: "SearchResult",
                newName: "SearchResultSet");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SearchResultSet",
                table: "SearchResultSet",
                columns: new[] { "ActionName", "ActionParams", "ActionDate" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SearchResultSet",
                table: "SearchResultSet");

            migrationBuilder.RenameTable(
                name: "SearchResultSet",
                newName: "SearchResult");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SearchResult",
                table: "SearchResult",
                columns: new[] { "ActionName", "ActionParams", "ActionDate" });
        }
    }
}
