using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Misaka.Sample.Web.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_msg_consumed",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Topic = table.Column<string>(nullable: true),
                    MessageBody = table.Column<string>(nullable: true),
                    ConsumeTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__msg_consumed", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "_msg_published",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Producer = table.Column<string>(nullable: true),
                    Topic = table.Column<string>(nullable: true),
                    MessageBody = table.Column<string>(nullable: true),
                    PublishTime = table.Column<DateTime>(nullable: false),
                    PublishError = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__msg_published", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "_msg_handles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HandleName = table.Column<string>(nullable: true),
                    Exception = table.Column<string>(nullable: true),
                    ConsumeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__msg_handles", x => x.Id);
                    table.ForeignKey(
                        name: "FK__msg_handles__msg_consumed_ConsumeId",
                        column: x => x.ConsumeId,
                        principalTable: "_msg_consumed",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX__msg_handles_ConsumeId",
                table: "_msg_handles",
                column: "ConsumeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_msg_handles");

            migrationBuilder.DropTable(
                name: "_msg_published");

            migrationBuilder.DropTable(
                name: "_msg_consumed");
        }
    }
}
