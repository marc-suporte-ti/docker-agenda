using Microsoft.EntityFrameworkCore.Migrations;

namespace DockerAgenda.Migrations
{
    public partial class AplicandoCampoAgenda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomeResponsavel",
                table: "Agendas",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeResponsavel",
                table: "Agendas");
        }
    }
}
