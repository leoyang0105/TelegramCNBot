using Microsoft.EntityFrameworkCore.Migrations;

namespace CNBot.Infrastructure.Migrations
{
    public partial class AddCommandText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TGMessageId",
                table: "user_command",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TGMessageId",
                table: "user_command");
        }
    }
}
