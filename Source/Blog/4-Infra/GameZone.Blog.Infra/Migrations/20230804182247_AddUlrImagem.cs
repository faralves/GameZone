using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameZone.Blog.Infra.Migrations
{
    public partial class AddUlrImagem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlImagem",
                table: "Noticia",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlImagem",
                table: "Noticia");
        }
    }
}
