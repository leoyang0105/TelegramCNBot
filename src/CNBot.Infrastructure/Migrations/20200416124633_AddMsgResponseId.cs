using Microsoft.EntityFrameworkCore.Migrations;

namespace CNBot.Infrastructure.Migrations
{
    public partial class AddMsgResponseId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TGMessageId",
                table: "user_command");

            migrationBuilder.AddColumn<long>(
                name: "ResponseId",
                table: "message",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseId",
                table: "message");

            migrationBuilder.AddColumn<long>(
                name: "TGMessageId",
                table: "user_command",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
