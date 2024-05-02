using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecomm_project_01.DataAccess.Data.Migrations
{
    public partial class addspofproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //create
            migrationBuilder.Sql(@"CREATE PROCEDURE createProduct
                                      @Title varchar(50),
                                      @Description varchar(max),
                                      @Author varchar(50),
                                      @ISBN int,
                                      @ListPrice int,
                                      @Price int,
                                      @Price50 int,
                                      @Price100 int,
                                      @ImageUrl nvarchar(max),
                                      @categoryId int,
                                      @covertypeId int
                                   AS
                                    	 insert into Products values(@Title,@Description,@Author,@ISBN,@ListPrice
                                         ,@Price,@Price50,@Price100,@ImageUrl,@categoryId,@covertypeId)
                                    ");
            //update
            migrationBuilder.Sql(@"CREATE PROCEDURE updateProduct
                                        @id int,
                                        @Title varchar(50),
                                        @Description varchar(max),
                                        @Author varchar(50),
                                        @ISBN int,
                                        @ListPrice int,
                                        @Price int,
                                        @Price50 int,
                                        @Price100 int,
                                        @ImageUrl nvarchar(max),
                                        @categoryId int,
                                        @covertypeId int
                                    AS
                                            update Products set Title=@Title,Description=@Description,Author=@Author,ISBN=@ISBN
                                        	,ListPrice=@ListPrice,Price=@Price,Price50=@Price50,Price100=@Price100
                                        	,ImageUrl=@ImageUrl,CategoryId=@categoryId,CovertypeId=@covertypeId  where Id=@id                                                                            	 
                                   ");
            //delete
            migrationBuilder.Sql(@"CREATE PROCEDURE deleteProduct
                                    @id int
                                    AS
	                                    delete from Products where Id=@id ");
            //find all
            migrationBuilder.Sql(@"CREATE PROCEDURE getProducts
                                    AS
	                                    select * from Products");
            //find one
            migrationBuilder.Sql(@"CREATE PROCEDURE getProduct
                                    @id int
                                   AS
	                                    select * from Products where Id =@id");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
