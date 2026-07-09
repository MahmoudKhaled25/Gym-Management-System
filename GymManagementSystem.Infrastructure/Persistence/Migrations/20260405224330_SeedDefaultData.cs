using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gym_Management_System.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "LogDate",
                table: "ProgressLogs",
                type: "date",
                nullable: false,
                defaultValueSql: "CAST(GETUTCDATE() AS DATE)",
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "019d5f90-527e-7234-8876-456086c0ef0f", "019d5f90-527e-7234-8876-4561d8a8a2db", false, false, "Admin", "ADMIN" },
                    { "019d5f90-527e-7234-8876-456220b0d0d1", "019d5f90-527e-7234-8876-45635f3dcaa5", true, false, "Member", "MEMBER" },
                    { "019d5f90-527e-7234-8876-456442c7a04a", "019d5f90-527e-7234-8876-45650cededb1", false, false, "Trainer", "TRAINER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "Height", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "Weight" },
                values: new object[] { "019d5f90-527e-7234-8876-455efcfd9243", 0, "019d5f90-527e-7234-8876-455f638b36d4", new DateOnly(1, 1, 1), "Admin@Gym.com", true, "Admin", 0m, "Admin", false, null, "ADMIN@GYM.COM", "ADMIN@GYM.COM", "AQAAAAIAAYagAAAAEMPuZ1wcSazj8+BfO7cyIh0QS3FszFECEcdpRBDVddRnZp4vliw/94XqWHuBQU3gfg==", null, false, "68859EF0-2601-4D27-AB97-F81C02991505", false, "Admin@Gym.com", 0m });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "019d5f90-527e-7234-8876-456086c0ef0f", "019d5f90-527e-7234-8876-455efcfd9243" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019d5f90-527e-7234-8876-456220b0d0d1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019d5f90-527e-7234-8876-456442c7a04a");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "019d5f90-527e-7234-8876-456086c0ef0f", "019d5f90-527e-7234-8876-455efcfd9243" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019d5f90-527e-7234-8876-456086c0ef0f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019d5f90-527e-7234-8876-455efcfd9243");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetRoles");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "LogDate",
                table: "ProgressLogs",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValueSql: "CAST(GETUTCDATE() AS DATE)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");
        }
    }
}
