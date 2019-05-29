using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Session9.DAL.Migrations
{
    public partial class RemoveOldFiledsFromProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Products");
            migrationBuilder.Sql("Drop Trigger If  EXISTS [dbo].[TRGInsertProducts]");
            migrationBuilder.Sql("Drop Trigger If  EXISTS [dbo].[TRGUpdateProducts]");
            migrationBuilder.Sql("Drop Trigger If  EXISTS [dbo].[TRGDeleteProducts]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Products",
                nullable: true);
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
        }
    }
}
