using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecomm_project_01.DataAccess.Data.Migrations
{
    public partial class addSPforCoverTypemodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE CreateCoverType
	                                @Name varchar(50)
                                   AS
	                                insert CoverTypes values(@Name)
                                 ");
            migrationBuilder.Sql(@"CREATE PROCEDURE UpdateCoverType
                                    @id int,	                                
                                    @Name varchar(50)
                                   AS
	                                update CoverTypes set Name = @Name where id = @id ");
            migrationBuilder.Sql(@"CREATE PROCEDURE DeleteCoverType
	                                @id int
                                   AS
	                                Delete from CoverTypes where id = @id");
            migrationBuilder.Sql(@"CREATE PROCEDURE GetCoverTypes
                                   AS
	                                select * from CoverTypes");
            migrationBuilder.Sql(@"CREATE PROCEDURE GetCoverType
                                    @id int                                  
                                  AS
	                                select * from CoverTypes where id = @id ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
