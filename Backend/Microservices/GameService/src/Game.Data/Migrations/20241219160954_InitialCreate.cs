using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameRools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstPlayerMove = table.Column<int>(type: "int", nullable: false),
                    SecondPlayerMove = table.Column<int>(type: "int", nullable: false),
                    GameResults = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstPlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SecondPlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Tipe = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RoundNum = table.Column<int>(type: "int", nullable: false),
                    GameResult = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Users_FirstPlayerId",
                        column: x => x.FirstPlayerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rooms_Users_SecondPlayerId",
                        column: x => x.SecondPlayerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstPlayerMove = table.Column<int>(type: "int", nullable: true),
                    SecondPlayerMove = table.Column<int>(type: "int", nullable: true),
                    RoundResult = table.Column<int>(type: "int", nullable: true),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rounds_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "GameRools",
                columns: new[] { "Id", "FirstPlayerMove", "GameResults", "SecondPlayerMove" },
                values: new object[,]
                {
                    { new Guid("0a8f4450-6d8d-4b27-a27a-24a9a11ab42d"), 1, 0, 0 },
                    { new Guid("1ee056d4-62c1-4f7b-8d8f-b1cc79953def"), 0, 1, 0 },
                    { new Guid("2decef60-2a16-4953-9a88-4ddd42c3531c"), 1, 1, 1 },
                    { new Guid("3fcf738b-6f01-45fd-b1c3-870538aa0025"), 0, 2, 1 },
                    { new Guid("59982e94-bdab-44e8-8064-99e8bc048e94"), 2, 2, 0 },
                    { new Guid("79273bbd-5660-4f74-82c4-7e289df0ec30"), 1, 2, 2 },
                    { new Guid("ae09c9e4-16ae-410b-8b3b-3473bfca4ba1"), 0, 0, 2 },
                    { new Guid("da0b28a3-8b89-4ba7-8fb8-85dfbff2283d"), 2, 0, 1 },
                    { new Guid("e770cee5-7c6f-43fe-886d-066b9c90307d"), 2, 1, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_FirstPlayerId",
                table: "Rooms",
                column: "FirstPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_SecondPlayerId",
                table: "Rooms",
                column: "SecondPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_RoomId",
                table: "Rounds",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameRools");

            migrationBuilder.DropTable(
                name: "Rounds");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
