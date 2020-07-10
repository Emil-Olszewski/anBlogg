using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace anBlogg.IDP.Data.Migrations.IdentityServer.ConfigurationDb
{
    public partial class RepairSeedConfigurationMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApiResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 7, 10, 8, 4, 19, 967, DateTimeKind.Utc).AddTicks(7684));

            migrationBuilder.UpdateData(
                table: "ClientCorsOrigins",
                keyColumn: "Id",
                keyValue: 1,
                column: "Origin",
                value: "http://localhost:4200");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 7, 10, 8, 4, 19, 969, DateTimeKind.Utc).AddTicks(7617));

            migrationBuilder.UpdateData(
                table: "IdentityResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 7, 10, 8, 4, 19, 969, DateTimeKind.Utc).AddTicks(4213));

            migrationBuilder.UpdateData(
                table: "IdentityResources",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2020, 7, 10, 8, 4, 19, 969, DateTimeKind.Utc).AddTicks(5189));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApiResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 7, 10, 7, 51, 4, 863, DateTimeKind.Utc).AddTicks(8731));

            migrationBuilder.UpdateData(
                table: "ClientCorsOrigins",
                keyColumn: "Id",
                keyValue: 1,
                column: "Origin",
                value: "http://localhost:4200/");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 7, 10, 7, 51, 4, 866, DateTimeKind.Utc).AddTicks(1591));

            migrationBuilder.UpdateData(
                table: "IdentityResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 7, 10, 7, 51, 4, 865, DateTimeKind.Utc).AddTicks(7797));

            migrationBuilder.UpdateData(
                table: "IdentityResources",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2020, 7, 10, 7, 51, 4, 865, DateTimeKind.Utc).AddTicks(8838));
        }
    }
}
