using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Session9.DAL.Migrations
{
    public partial class AddCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });
            migrationBuilder.Sql("INSERT INTO[dbo].[Categories]([Name]) SELECT distinct [Category] FROM[BackwardDB].[dbo].[Products]");

            migrationBuilder.Sql("UPDATE [dbo].[Products] SET[CategoryId] = Result.CategoryId " +
                "From( Select CategoryId As CategoryId,[Name] As[Name] From[Categories]) As Result " +
                "WHERE[dbo].[Products].[Category] = Result.[Name]");

            migrationBuilder.Sql("Create Trigger [dbo].[TRGInsertProducts] On [dbo].[Products] After Insert As declare @Name nvarchar(MAX);" +
                " declare @ProductId nvarchar(MAX); Select @Name = Category From Inserted " +
                " Select @ProductId = ProductId From Inserted " +
                " If @Name not in(Select[Name] From Categories) Insert into[dbo].[Categories] ([Name]) Values(@Name)" +
                " UPDATE[dbo].[Products] SET[CategoryId] = Result.CategoryId" +
                " From(Select CategoryId As CategoryId, [Name] As[Name] From[Categories]) As Result" +
                " WHERE[dbo].[Products].[Category] = Result.[Name] And[dbo].[Products].[ProductId]=@ProductId");

            migrationBuilder.Sql("Create Trigger [dbo].[TRGDeleteProducts] On [dbo].[Products] After Delete As" +
                " declare @Name nvarchar(MAX); Select @Name = Category From Deleted If @Name not in(Select[Category] From Products)" +
                " Delete From Categories Where[Name] = @Name");

            migrationBuilder.Sql("Create Trigger [dbo].[TRGUpdateProducts] On [dbo].[Products] After Update As" +
                " declare @Name nvarchar(MAX); declare @OldName nvarchar(MAX); declare @ProductId nvarchar(MAX);" +
                " Select @OldName = Category From Deleted Select @Name = Category From Inserted Select @ProductId = ProductId From Inserted" +
                " If @Name not in(Select[Name] From Categories) Insert into[dbo].[Categories] ([Name]) Values(@Name) " +
                " else If @OldName not in(Select[Category] From Products) Delete From Categories Where[Name] = @OldName " +
                " UPDATE[dbo].[Products] SET[CategoryId] = Result.CategoryId" +
                " From(Select CategoryId As CategoryId, [Name] As[Name] From[Categories]) As Result" +
                " WHERE[dbo].[Products].[Category] = Result.[Name] And[dbo].[Products].[ProductId]=@ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Products");

            migrationBuilder.Sql("Drop Trigger If  EXISTS [dbo].[TRGInsertProducts]");
            migrationBuilder.Sql("Drop Trigger If  EXISTS [dbo].[TRGUpdateProducts]");
            migrationBuilder.Sql("Drop Trigger If  EXISTS [dbo].[TRGDeleteProducts]");
        }
    }
}
