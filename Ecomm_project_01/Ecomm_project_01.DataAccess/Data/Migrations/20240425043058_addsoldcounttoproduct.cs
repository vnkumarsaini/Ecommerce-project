using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecomm_project_01.DataAccess.Data.Migrations
{
    public partial class addsoldcounttoproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoldCount",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoldCount",
                table: "Product");
        }
    }
}
