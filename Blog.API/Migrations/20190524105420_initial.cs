using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MesTime = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MesTime);
                });

            migrationBuilder.CreateTable(
                name: "StockEvents",
                columns: table => new
                {
                    EventName = table.Column<string>(maxLength: 30, nullable: false),
                    LastAriseStartDate = table.Column<DateTime>(nullable: false),
                    LastAriseEndDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockEvents", x => x.EventName);
                });

            migrationBuilder.CreateTable(
                name: "StockSet",
                columns: table => new
                {
                    StockId = table.Column<string>(maxLength: 10, nullable: false),
                    StockName = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockSet", x => x.StockId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 40, nullable: false),
                    Username = table.Column<string>(maxLength: 20, nullable: false),
                    RoleName = table.Column<string>(maxLength: 10, nullable: false, defaultValue: "user"),
                    ExpiredDate = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    Email = table.Column<string>(maxLength: 60, nullable: false),
                    Password = table.Column<string>(maxLength: 90, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DayDataSet",
                columns: table => new
                {
                    StockId = table.Column<string>(maxLength: 10, nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Open = table.Column<float>(nullable: false),
                    Low = table.Column<float>(nullable: false),
                    High = table.Column<float>(nullable: false),
                    Close = table.Column<float>(nullable: false),
                    Volume = table.Column<float>(nullable: false),
                    Amount = table.Column<float>(nullable: false),
                    ZhangDieFu = table.Column<float>(nullable: true),
                    ZongShiZhi = table.Column<float>(nullable: true),
                    LiuTongShiZhi = table.Column<float>(nullable: true),
                    HuanShouLiu = table.Column<float>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayDataSet", x => new { x.StockId, x.Date });
                    table.ForeignKey(
                        name: "FK_DayDataSet_StockSet_StockId",
                        column: x => x.StockId,
                        principalTable: "StockSet",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RealTimeDataSet",
                columns: table => new
                {
                    StockId = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(maxLength: 20, nullable: false),
                    StockName = table.Column<string>(maxLength: 10, nullable: true),
                    Open = table.Column<float>(nullable: false),
                    Low = table.Column<float>(nullable: false),
                    High = table.Column<float>(nullable: false),
                    Close = table.Column<float>(nullable: false),
                    Volume = table.Column<float>(nullable: false),
                    Amount = table.Column<float>(nullable: false),
                    ZhangDieFu = table.Column<float>(nullable: true),
                    ZongShiZhi = table.Column<float>(nullable: true),
                    LiuTongShiZhi = table.Column<float>(nullable: true),
                    HuanShouLiu = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealTimeDataSet", x => new { x.StockId, x.Date });
                    table.ForeignKey(
                        name: "FK_RealTimeDataSet_StockSet_StockId",
                        column: x => x.StockId,
                        principalTable: "StockSet",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SharingSet",
                columns: table => new
                {
                    StockId = table.Column<string>(maxLength: 10, nullable: false),
                    DateGongGao = table.Column<DateTime>(nullable: false),
                    DateChuXi = table.Column<DateTime>(nullable: true),
                    DateDengJi = table.Column<DateTime>(nullable: true),
                    SongGu = table.Column<float>(nullable: false),
                    ZhuanZeng = table.Column<float>(nullable: false),
                    PaiXi = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharingSet", x => new { x.StockId, x.DateGongGao });
                    table.ForeignKey(
                        name: "FK_SharingSet_StockSet_StockId",
                        column: x => x.StockId,
                        principalTable: "StockSet",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockNumSet",
                columns: table => new
                {
                    StockId = table.Column<string>(maxLength: 10, nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    All = table.Column<double>(nullable: false),
                    LiuTongA = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockNumSet", x => new { x.StockId, x.Date });
                    table.ForeignKey(
                        name: "FK_StockNumSet_StockSet_StockId",
                        column: x => x.StockId,
                        principalTable: "StockSet",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Story",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    CreationTime = table.Column<long>(nullable: false),
                    LastEditTime = table.Column<long>(nullable: false),
                    PublishTime = table.Column<long>(nullable: false),
                    Draft = table.Column<bool>(nullable: false),
                    OwnerId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Story", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Story_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Like",
                columns: table => new
                {
                    StoryId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Like", x => new { x.StoryId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Like_Story_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Story",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Like_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Share",
                columns: table => new
                {
                    StoryId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Share", x => new { x.StoryId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Share_Story_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Story",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Share_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Like_UserId",
                table: "Like",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Share_UserId",
                table: "Share",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Story_OwnerId",
                table: "Story",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayDataSet");

            migrationBuilder.DropTable(
                name: "Like");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "RealTimeDataSet");

            migrationBuilder.DropTable(
                name: "Share");

            migrationBuilder.DropTable(
                name: "SharingSet");

            migrationBuilder.DropTable(
                name: "StockEvents");

            migrationBuilder.DropTable(
                name: "StockNumSet");

            migrationBuilder.DropTable(
                name: "Story");

            migrationBuilder.DropTable(
                name: "StockSet");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
