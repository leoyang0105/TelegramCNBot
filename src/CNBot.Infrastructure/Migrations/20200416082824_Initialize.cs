using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CNBot.Infrastructure.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    Description = table.Column<string>(maxLength: 128, nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    Published = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "chat",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TGChatId = table.Column<long>(nullable: false),
                    Title = table.Column<string>(maxLength: 256, nullable: true),
                    UserName = table.Column<string>(maxLength: 128, nullable: true),
                    Description = table.Column<string>(maxLength: 4000, nullable: true),
                    ChatType = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    MembersCount = table.Column<int>(nullable: false),
                    CreatorId = table.Column<long>(nullable: false),
                    InviteLink = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "message",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(maxLength: 4096, nullable: true),
                    TGMessageId = table.Column<long>(nullable: false),
                    TGChatId = table.Column<long>(nullable: false),
                    FromTGUserId = table.Column<long>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TGUserId = table.Column<long>(nullable: false),
                    UserName = table.Column<string>(maxLength: 128, nullable: true),
                    FirstName = table.Column<string>(maxLength: 256, nullable: true),
                    LastName = table.Column<string>(maxLength: 256, nullable: true),
                    IsBot = table.Column<bool>(nullable: false),
                    LanguageCode = table.Column<string>(maxLength: 4000, nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "chat_category",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChatId = table.Column<long>(nullable: false),
                    CategoryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_chat_category_category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chat_category_chat_ChatId",
                        column: x => x.ChatId,
                        principalTable: "chat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chat_member",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChatId = table.Column<long>(nullable: false),
                    TGUserId = table.Column<long>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Permissions = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_member", x => x.Id);
                    table.ForeignKey(
                        name: "FK_chat_member_chat_ChatId",
                        column: x => x.ChatId,
                        principalTable: "chat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "message_entity",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MessageId = table.Column<long>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Offset = table.Column<int>(nullable: false),
                    Length = table.Column<int>(nullable: false),
                    Url = table.Column<string>(maxLength: 1024, nullable: true),
                    TGUserId = table.Column<long>(nullable: true),
                    Language = table.Column<string>(maxLength: 4096, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message_entity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_message_entity_message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_command",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Completed = table.Column<bool>(nullable: false),
                    Text = table.Column<string>(maxLength: 4096, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_command", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_command_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_chat_TGChatId",
                table: "chat",
                column: "TGChatId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chat_category_CategoryId",
                table: "chat_category",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_chat_category_ChatId",
                table: "chat_category",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_chat_member_ChatId",
                table: "chat_member",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_message_entity_MessageId",
                table: "message_entity",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_user_TGUserId",
                table: "user",
                column: "TGUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_command_UserId",
                table: "user_command",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_category");

            migrationBuilder.DropTable(
                name: "chat_member");

            migrationBuilder.DropTable(
                name: "message_entity");

            migrationBuilder.DropTable(
                name: "user_command");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "chat");

            migrationBuilder.DropTable(
                name: "message");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
