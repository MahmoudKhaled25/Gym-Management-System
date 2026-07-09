using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSuperAdminRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "019d5f90-527e-7234-8876-456086c0ef0f", "019d5f90-527e-7234-8876-455efcfd9243" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[] { "019e95fd-0895-787c-8a4b-0eec89dc2a1d", "019e95fd-0895-787c-8a4b-0eed79601c15", false, false, "SuperAdmin", "SUPERADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "019e95fd-0895-787c-8a4b-0eec89dc2a1d", "019d5f90-527e-7234-8876-455efcfd9243" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "019e95fd-0895-787c-8a4b-0eec89dc2a1d", "019d5f90-527e-7234-8876-455efcfd9243" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019e95fd-0895-787c-8a4b-0eec89dc2a1d");

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "019d5f90-527e-7234-8876-456086c0ef0f", "019d5f90-527e-7234-8876-455efcfd9243" });
        }
    }
}
