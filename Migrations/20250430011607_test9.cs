using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tasken2.Migrations
{
    public partial class test9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_persons_personID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_properties_propID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_properties_Areas_AreaId",
                table: "properties");

            migrationBuilder.DropForeignKey(
                name: "FK_properties_persons_CreatedIDBy",
                table: "properties");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyRatings_persons_personID",
                table: "PropertyRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyRatings_properties_propID",
                table: "PropertyRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_searchHistories",
                table: "searchHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_properties",
                table: "properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_persons",
                table: "persons");

            migrationBuilder.DropColumn(
                name: "confirmPassword",
                table: "persons");

            migrationBuilder.DropColumn(
                name: "firstName",
                table: "persons");

            migrationBuilder.DropColumn(
                name: "lastName",
                table: "persons");

            migrationBuilder.RenameTable(
                name: "searchHistories",
                newName: "SearchHistories");

            migrationBuilder.RenameTable(
                name: "properties",
                newName: "Properties");

            migrationBuilder.RenameTable(
                name: "persons",
                newName: "Persons");

            migrationBuilder.RenameColumn(
                name: "KeyWord",
                table: "SearchHistories",
                newName: "Keyword");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "SearchHistories",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "personID",
                table: "PropertyRatings",
                newName: "PersonId");

            migrationBuilder.RenameColumn(
                name: "propertyRatingID",
                table: "PropertyRatings",
                newName: "PropertyRatingId");

            migrationBuilder.RenameColumn(
                name: "propID",
                table: "PropertyRatings",
                newName: "PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyRatings_personID",
                table: "PropertyRatings",
                newName: "IX_PropertyRatings_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyRatings_propID",
                table: "PropertyRatings",
                newName: "IX_PropertyRatings_PropertyId");

            migrationBuilder.RenameColumn(
                name: "propertyID",
                table: "Properties",
                newName: "PropertyId");

            migrationBuilder.RenameColumn(
                name: "propStreet",
                table: "Properties",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "propRegion",
                table: "Properties",
                newName: "Region");

            migrationBuilder.RenameColumn(
                name: "propPrice",
                table: "Properties",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "propImage5",
                table: "Properties",
                newName: "Image5");

            migrationBuilder.RenameColumn(
                name: "propImage4",
                table: "Properties",
                newName: "Image4");

            migrationBuilder.RenameColumn(
                name: "propImage3",
                table: "Properties",
                newName: "Image3");

            migrationBuilder.RenameColumn(
                name: "propImage2",
                table: "Properties",
                newName: "Image2");

            migrationBuilder.RenameColumn(
                name: "propImage1",
                table: "Properties",
                newName: "Image1");

            migrationBuilder.RenameColumn(
                name: "propFloorNumber",
                table: "Properties",
                newName: "NumberOfRooms");

            migrationBuilder.RenameColumn(
                name: "propArea",
                table: "Properties",
                newName: "Area");

            migrationBuilder.RenameColumn(
                name: "probNumberOfRooms",
                table: "Properties",
                newName: "FloorNumber");

            migrationBuilder.RenameColumn(
                name: "CreatedIDBy",
                table: "Properties",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_properties_AreaId",
                table: "Properties",
                newName: "IX_Properties_AreaId");

            migrationBuilder.RenameIndex(
                name: "IX_properties_CreatedIDBy",
                table: "Properties",
                newName: "IX_Properties_CreatedById");

            migrationBuilder.RenameColumn(
                name: "nationalIdImage",
                table: "Persons",
                newName: "NationalIdImage");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Persons",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "accountType",
                table: "Persons",
                newName: "AccountType");

            migrationBuilder.RenameColumn(
                name: "personID",
                table: "Persons",
                newName: "PersonID");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Persons",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "personID",
                table: "Comments",
                newName: "PersonId");

            migrationBuilder.RenameColumn(
                name: "commentTime",
                table: "Comments",
                newName: "CommentTime");

            migrationBuilder.RenameColumn(
                name: "commentText",
                table: "Comments",
                newName: "CommentText");

            migrationBuilder.RenameColumn(
                name: "commentID",
                table: "Comments",
                newName: "CommentId");

            migrationBuilder.RenameColumn(
                name: "propID",
                table: "Comments",
                newName: "PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_personID",
                table: "Comments",
                newName: "IX_Comments_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_propID",
                table: "Comments",
                newName: "IX_Comments_PropertyId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Properties",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Persons",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "AccountType",
                table: "Persons",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Persons",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "SurroundingArea",
                table: "Areas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AreaName",
                table: "Areas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SearchHistories",
                table: "SearchHistories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Properties",
                table: "Properties",
                column: "PropertyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Persons",
                table: "Persons",
                column: "PersonID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Persons_PersonId",
                table: "Comments",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "PersonID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Properties_PropertyId",
                table: "Comments",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "PropertyId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Areas_AreaId",
                table: "Properties",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Persons_CreatedById",
                table: "Properties",
                column: "CreatedById",
                principalTable: "Persons",
                principalColumn: "PersonID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyRatings_Persons_PersonId",
                table: "PropertyRatings",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "PersonID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyRatings_Properties_PropertyId",
                table: "PropertyRatings",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "PropertyId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Persons_PersonId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Properties_PropertyId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Areas_AreaId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Persons_CreatedById",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyRatings_Persons_PersonId",
                table: "PropertyRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyRatings_Properties_PropertyId",
                table: "PropertyRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SearchHistories",
                table: "SearchHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Properties",
                table: "Properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Persons",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Persons");

            migrationBuilder.RenameTable(
                name: "SearchHistories",
                newName: "searchHistories");

            migrationBuilder.RenameTable(
                name: "Properties",
                newName: "properties");

            migrationBuilder.RenameTable(
                name: "Persons",
                newName: "persons");

            migrationBuilder.RenameColumn(
                name: "Keyword",
                table: "searchHistories",
                newName: "KeyWord");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "searchHistories",
                newName: "DateTime");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "PropertyRatings",
                newName: "personID");

            migrationBuilder.RenameColumn(
                name: "PropertyRatingId",
                table: "PropertyRatings",
                newName: "propertyRatingID");

            migrationBuilder.RenameColumn(
                name: "PropertyId",
                table: "PropertyRatings",
                newName: "propID");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyRatings_PersonId",
                table: "PropertyRatings",
                newName: "IX_PropertyRatings_personID");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyRatings_PropertyId",
                table: "PropertyRatings",
                newName: "IX_PropertyRatings_propID");

            migrationBuilder.RenameColumn(
                name: "PropertyId",
                table: "properties",
                newName: "propertyID");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "properties",
                newName: "propStreet");

            migrationBuilder.RenameColumn(
                name: "Region",
                table: "properties",
                newName: "propRegion");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "properties",
                newName: "propPrice");

            migrationBuilder.RenameColumn(
                name: "NumberOfRooms",
                table: "properties",
                newName: "propFloorNumber");

            migrationBuilder.RenameColumn(
                name: "Image5",
                table: "properties",
                newName: "propImage5");

            migrationBuilder.RenameColumn(
                name: "Image4",
                table: "properties",
                newName: "propImage4");

            migrationBuilder.RenameColumn(
                name: "Image3",
                table: "properties",
                newName: "propImage3");

            migrationBuilder.RenameColumn(
                name: "Image2",
                table: "properties",
                newName: "propImage2");

            migrationBuilder.RenameColumn(
                name: "Image1",
                table: "properties",
                newName: "propImage1");

            migrationBuilder.RenameColumn(
                name: "FloorNumber",
                table: "properties",
                newName: "probNumberOfRooms");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "properties",
                newName: "CreatedIDBy");

            migrationBuilder.RenameColumn(
                name: "Area",
                table: "properties",
                newName: "propArea");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_AreaId",
                table: "properties",
                newName: "IX_properties_AreaId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_CreatedById",
                table: "properties",
                newName: "IX_properties_CreatedIDBy");

            migrationBuilder.RenameColumn(
                name: "NationalIdImage",
                table: "persons",
                newName: "nationalIdImage");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "persons",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "AccountType",
                table: "persons",
                newName: "accountType");

            migrationBuilder.RenameColumn(
                name: "PersonID",
                table: "persons",
                newName: "personID");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "persons",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Comments",
                newName: "personID");

            migrationBuilder.RenameColumn(
                name: "CommentTime",
                table: "Comments",
                newName: "commentTime");

            migrationBuilder.RenameColumn(
                name: "CommentText",
                table: "Comments",
                newName: "commentText");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Comments",
                newName: "commentID");

            migrationBuilder.RenameColumn(
                name: "PropertyId",
                table: "Comments",
                newName: "propID");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PersonId",
                table: "Comments",
                newName: "IX_Comments_personID");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PropertyId",
                table: "Comments",
                newName: "IX_Comments_propID");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "properties",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "persons",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.AlterColumn<string>(
                name: "accountType",
                table: "persons",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "confirmPassword",
                table: "persons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "firstName",
                table: "persons",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "lastName",
                table: "persons",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "SurroundingArea",
                table: "Areas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "AreaName",
                table: "Areas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_searchHistories",
                table: "searchHistories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_properties",
                table: "properties",
                column: "propertyID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_persons",
                table: "persons",
                column: "personID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_persons_personID",
                table: "Comments",
                column: "personID",
                principalTable: "persons",
                principalColumn: "personID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_properties_propID",
                table: "Comments",
                column: "propID",
                principalTable: "properties",
                principalColumn: "propertyID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_properties_Areas_AreaId",
                table: "properties",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_properties_persons_CreatedIDBy",
                table: "properties",
                column: "CreatedIDBy",
                principalTable: "persons",
                principalColumn: "personID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyRatings_persons_personID",
                table: "PropertyRatings",
                column: "personID",
                principalTable: "persons",
                principalColumn: "personID");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyRatings_properties_propID",
                table: "PropertyRatings",
                column: "propID",
                principalTable: "properties",
                principalColumn: "propertyID");
        }
    }
}
