using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tasken2.Migrations
{
    public partial class Context1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AreaName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurroundingArea = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "persons",
                columns: table => new
                {
                    personID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    nationalID = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    confirmPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nationalIdImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persons", x => x.personID);
                });

            migrationBuilder.CreateTable(
                name: "searchHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KeyWord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoomFilter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceFilter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_searchHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "properties",
                columns: table => new
                {
                    propertyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    propPrice = table.Column<decimal>(type: "money", nullable: false),
                    propArea = table.Column<float>(type: "real", nullable: false),
                    probNumberOfRooms = table.Column<int>(type: "int", nullable: false),
                    propRegion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    propStreet = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    propFloorNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    propImage1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    propImage2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    propImage3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    propImage4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    propImage5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedIDBy = table.Column<int>(type: "int", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false),
                    HireStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_properties", x => x.propertyID);
                    table.ForeignKey(
                        name: "FK_properties_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_properties_persons_CreatedIDBy",
                        column: x => x.CreatedIDBy,
                        principalTable: "persons",
                        principalColumn: "personID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    commentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    commentText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    commentTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    propID = table.Column<int>(type: "int", nullable: false),
                    personID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.commentID);
                    table.ForeignKey(
                        name: "FK_Comments_persons_personID",
                        column: x => x.personID,
                        principalTable: "persons",
                        principalColumn: "personID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Comments_properties_propID",
                        column: x => x.propID,
                        principalTable: "properties",
                        principalColumn: "propertyID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PropertyRatings",
                columns: table => new
                {
                    propertyRatingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    propID = table.Column<int>(type: "int", nullable: false),
                    personID = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyRatings", x => x.propertyRatingID);
                    table.ForeignKey(
                        name: "FK_PropertyRatings_persons_personID",
                        column: x => x.personID,
                        principalTable: "persons",
                        principalColumn: "personID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PropertyRatings_properties_propID",
                        column: x => x.propID,
                        principalTable: "properties",
                        principalColumn: "propertyID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_personID",
                table: "Comments",
                column: "personID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_propID",
                table: "Comments",
                column: "propID");

            migrationBuilder.CreateIndex(
                name: "IX_properties_AreaId",
                table: "properties",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_properties_CreatedIDBy",
                table: "properties",
                column: "CreatedIDBy");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyRatings_personID",
                table: "PropertyRatings",
                column: "personID");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyRatings_propID",
                table: "PropertyRatings",
                column: "propID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "PropertyRatings");

            migrationBuilder.DropTable(
                name: "searchHistories");

            migrationBuilder.DropTable(
                name: "properties");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "persons");
        }
    }
}
