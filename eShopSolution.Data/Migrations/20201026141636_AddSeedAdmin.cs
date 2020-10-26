using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class AddSeedAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("6ace6592-a25f-484e-b109-dcbd8cffdc79"), "f79c7a3a-2a20-4332-9756-48174d5f86ed", null, "admin", "admin" });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("eaa6fdc7-f015-4f38-b3eb-c0732f47c439"), new Guid("6ace6592-a25f-484e-b109-dcbd8cffdc79") });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Dob", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("eaa6fdc7-f015-4f38-b3eb-c0732f47c439"), 0, "da198f22-acfb-4ad7-a6a0-9c8fc632a03d", new DateTime(1998, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", true, "Dao", "Hieu", false, null, null, "admin", "AQAAAAEAACcQAAAAEBr/W32JKFBN4H32M/3gCz//nLP5SEExnOrzL7VzGymWp1TptqbQwFubwpWhpTZwmw==", null, false, "", false, "admin" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "OriginalPrice" },
                values: new object[] { new DateTime(2020, 10, 26, 21, 16, 35, 873, DateTimeKind.Local).AddTicks(5307), 100000m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "OriginalPrice" },
                values: new object[] { new DateTime(2020, 10, 26, 21, 16, 35, 874, DateTimeKind.Local).AddTicks(6547), 100000m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("6ace6592-a25f-484e-b109-dcbd8cffdc79"));

            migrationBuilder.DeleteData(
                table: "AppUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("eaa6fdc7-f015-4f38-b3eb-c0732f47c439"), new Guid("6ace6592-a25f-484e-b109-dcbd8cffdc79") });

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("eaa6fdc7-f015-4f38-b3eb-c0732f47c439"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "OriginalPrice" },
                values: new object[] { new DateTime(2020, 10, 26, 20, 54, 8, 521, DateTimeKind.Local).AddTicks(2992), 100000m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "OriginalPrice" },
                values: new object[] { new DateTime(2020, 10, 26, 20, 54, 8, 522, DateTimeKind.Local).AddTicks(3566), 100000m });
        }
    }
}
