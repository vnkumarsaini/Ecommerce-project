using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecomm_project_01.DataAccess.Data.Migrations
{
    public partial class addresstoorderheader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SelectedAddress",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedAddress",
                table: "OrderHeaders");
        }
    }
}
