using Microsoft.EntityFrameworkCore.Migrations;

namespace DockerAgenda.Migrations
{
    public partial class AdicionandoDescricaoItemContato : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Observacao",
                table: "ItensContatos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observacao",
                table: "ItensContatos");
        }
    }
}
